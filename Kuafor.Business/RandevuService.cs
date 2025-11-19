using Kuafor.Core; // Modeller (Randevu, Kullanici vb.)
using Kuafor.Core.DTOs; // DTO'lar (RandevuListDto) -> İşte bu eksikti!
using Kuafor.DataAccess; // DbContext
using Microsoft.EntityFrameworkCore; // .Include, .ToListAsync vb.
using System; // Exception, DateTime
using System.Collections.Generic; // List<>
using System.Linq; // .Select, .OrderBy vb.
using System.Threading.Tasks; // async/await

namespace Kuafor.Business
{
    public class RandevuService
    {
        private readonly ApplicationDbContext _context;

        public RandevuService(ApplicationDbContext context)
        {
            _context = context;
        }

        // METOT 1: Randevu Oluşturma
        public async Task<Randevu> RandevuOlusturAsync(int musteriId, int calisanId, int hizmetId, DateTime baslangic)
        {
            var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
            if (hizmet == null)
            {
                throw new Exception("Geçersiz hizmet seçimi.");
            }

            // --- YENİ EKLENEN MESAİ KONTROLÜ ---
            // 1.5. Çalışan o saatte çalışıyor mu?
            bool calisanMusait = await CalisanMusaitMiAsync(calisanId, baslangic, hizmet.SureDakika);
            if (!calisanMusait)
            {
                throw new Exception("Çalışan belirtilen tarih ve saatte hizmet vermemektedir (Mesai dışı).");
            }
            // -----------------------------------

          

            bool cakismaVar = await CakismaVarMiAsync(calisanId, baslangic, hizmet.SureDakika);
            if (cakismaVar)
            {
                throw new Exception("Seçilen tarih ve saatte çakışma mevcut. Lütfen başka bir zaman seçin.");
            }

            var yeniRandevu = new Randevu
            {
                MusteriId = musteriId,
                CalisanId = calisanId,
                HizmetId = hizmetId,
                BaslangicTarihSaati = baslangic,
                ToplamSureDakika = hizmet.SureDakika,
                ToplamUcret = hizmet.Ucret,
                Durum = RandevuDurumu.OnayBekliyor
            };

            await _context.Randevular.AddAsync(yeniRandevu);
            await _context.SaveChangesAsync();

            return yeniRandevu;
        }

        // METOT 2: Çakışma Kontrolü
        private async Task<bool> CakismaVarMiAsync(int calisanId, DateTime yeniRandevuBaslangic, int yeniRandevuSuresi)
        {
            DateTime yeniRandevuBitis = yeniRandevuBaslangic.AddMinutes(yeniRandevuSuresi);

            var cakismaVar = await _context.Randevular
                .Where(r =>
                    r.CalisanId == calisanId &&
                    r.Durum != RandevuDurumu.IptalEdildi &&
                    (yeniRandevuBaslangic < r.BaslangicTarihSaati.AddMinutes(r.ToplamSureDakika)) &&
                    (yeniRandevuBitis > r.BaslangicTarihSaati)
                )
                .AnyAsync();

            return cakismaVar;
        }

        // METOT 3: Randevuları Listeleme (YENİ EKLENEN KISIM)
        public async Task<List<RandevuListDto>> RandevulariGetirAsync()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Musteri)  // Müşteri tablosunu bağla
                .Include(r => r.Calisan)  // Çalışan tablosunu bağla
                .Include(r => r.Hizmet)   // Hizmet tablosunu bağla
                .OrderByDescending(r => r.BaslangicTarihSaati) // En yeni tarih en üstte
                .Select(r => new RandevuListDto
                {
                    Id = r.Id,
                    Baslangic = r.BaslangicTarihSaati,
                    Bitis = r.BaslangicTarihSaati.AddMinutes(r.ToplamSureDakika),
                    // Ad ve Soyad'ı birleştirip tek string yapıyoruz:
                    MusteriAdi = r.Musteri.Ad + " " + r.Musteri.Soyad,
                    CalisanAdi = r.Calisan.Ad + " " + r.Calisan.Soyad,
                    HizmetAdi = r.Hizmet.Ad,
                    Ucret = r.ToplamUcret,
                    Durum = r.Durum.ToString()
                })
                .ToListAsync();

            return randevular;
        }
        // METOT 4: Randevu Onaylama
        public async Task<bool> RandevuOnaylaAsync(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return false; // Randevu bulunamadı

            randevu.Durum = RandevuDurumu.Onaylandi;
            await _context.SaveChangesAsync();
            return true;
        }

        // METOT 5: Randevu İptal Etme
        public async Task<bool> RandevuIptalEtAsync(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return false;

            randevu.Durum = RandevuDurumu.IptalEdildi;
            await _context.SaveChangesAsync();
            return true;
        }
        // METOT 6: Mesai Kontrolü (Özel Metot)
        private async Task<bool> CalisanMusaitMiAsync(int calisanId, DateTime baslangic, int sureDakika)
        {
            // 1. İstenen randevunun gününü ve saat aralığını al
            var istenenGun = baslangic.DayOfWeek; // Örn: Pazartesi
            var istenenBaslangicSaati = baslangic.TimeOfDay; // Örn: 10:00
            var istenenBitisSaati = istenenBaslangicSaati.Add(TimeSpan.FromMinutes(sureDakika)); // Örn: 10:30

            // 2. Veritabanında bu kurala uyan bir "Uygunluk" kaydı var mı?
            // Kural: Çalışan o gün çalışıyor mu VE randevu saatleri mesai içine sığıyor mu?
            var uygunlukVar = await _context.UygunlukZamanlari
                .AnyAsync(u =>
                    u.CalisanId == calisanId &&
                    u.Gun == istenenGun &&
                    u.BaslangicSaati <= istenenBaslangicSaati && // Mesai randevudan önce (veya tam o an) başlamalı
                    u.BitisSaati >= istenenBitisSaati // Mesai randevudan sonra (veya tam o an) bitmeli
                );

            return uygunlukVar;
        }
    }
}