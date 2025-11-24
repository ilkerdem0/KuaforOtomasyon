using Kuafor.Core;
using Kuafor.Core.DTOs;
using Kuafor.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; // OfType için gerekli
using System.Threading.Tasks;

namespace Kuafor.Business
{
    public class YonetimService
    {
        private readonly ApplicationDbContext _context;

        public YonetimService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- EKLEME İŞLEMLERİ ---

        public async Task<Salon> SalonEkleAsync(string ad, string adres, string saatler)
        {
            var yeniSalon = new Salon { Ad = ad, Adres = adres, CalismaSaatleriAciklamasi = saatler };
            await _context.Salonlar.AddAsync(yeniSalon);
            await _context.SaveChangesAsync();
            return yeniSalon;
        }

        public async Task<Hizmet> HizmetEkleAsync(string ad, int sure, decimal ucret)
        {
            var yeniHizmet = new Hizmet { Ad = ad, SureDakika = sure, Ucret = ucret };
            await _context.Hizmetler.AddAsync(yeniHizmet);
            await _context.SaveChangesAsync();
            return yeniHizmet;
        }

        public async Task<Calisan> CalisanEkleAsync(CalisanCreateDto dto)
        {
            var yeniCalisan = new Calisan
            {
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                Email = dto.Email,
                Telefon = dto.Telefon,
                SifreHash = "12345",
                SalonId = dto.SalonId,
                AktifMi = true
            };

            if (dto.UzmanlikHizmetIdleri != null && dto.UzmanlikHizmetIdleri.Count > 0)
            {
                var secilenHizmetler = await _context.Hizmetler
                    .Where(h => dto.UzmanlikHizmetIdleri.Contains(h.Id))
                    .ToListAsync();
                yeniCalisan.Uzmanliklar = secilenHizmetler;
            }

            await _context.Calisanlar.AddAsync(yeniCalisan);
            await _context.SaveChangesAsync();
            return yeniCalisan;
        }

        public async Task<UygunlukZamani> UygunlukEkleAsync(UygunlukCreateDto dto)
        {
            var uygunluk = new UygunlukZamani
            {
                CalisanId = dto.CalisanId,
                Gun = dto.Gun,
                BaslangicSaati = dto.BaslangicSaati,
                BitisSaati = dto.BitisSaati
            };
            await _context.UygunlukZamanlari.AddAsync(uygunluk);
            await _context.SaveChangesAsync();
            return uygunluk;
        }

        // --- LİSTELEME METOTLARI (HATA VEREN YERLER DÜZELTİLDİ) ---

        // 1. Salonları Getir
        public async Task<List<Salon>> TumSalonlariGetirAsync()
        {
            return await _context.Salonlar.ToListAsync();
        }

        // 2. Hizmetleri Getir
        public async Task<List<Hizmet>> TumHizmetleriGetirAsync()
        {
            return await _context.Hizmetler.ToListAsync();
        }

        // 3. Müşterileri Getir (YENİ)
        public async Task<List<Musteri>> TumMusterileriGetirAsync()
        {
            return await _context.Kullanicilar.OfType<Musteri>().ToListAsync();
        }

        // 4. Çalışanları Detaylı Getir (YENİ)
        public async Task<List<Calisan>> TumCalisanlariDetayliGetirAsync()
        {
            return await _context.Calisanlar
                .Include(c => c.Salon)
                .Include(c => c.Uzmanliklar)
                .ToListAsync();
        }

        // 5. Belirli Salondaki Çalışanları Getir (DTO ile)
        public async Task<List<CalisanListDto>> SalonCalisanlariniGetirAsync(int salonId)
        {
            return await _context.Calisanlar
                .Where(c => c.SalonId == salonId)
                .Include(c => c.Uzmanliklar)
                .Select(c => new CalisanListDto
                {
                    Id = c.Id,
                    AdSoyad = c.Ad + " " + c.Soyad,
                    Uzmanliklar = string.Join(", ", c.Uzmanliklar.Select(u => u.Ad))
                })
                .ToListAsync();
        }

        // --- RAPORLAMA ---
        public async Task<decimal> ToplamGelirHesaplaAsync()
        {
            return await _context.Randevular
                .Where(r => r.Durum == RandevuDurumu.Onaylandi)
                .SumAsync(r => r.ToplamUcret);
        }
        // --- GÜNCELLEME İŞLEMLERİ ---

        // 1. ID'ye göre tek bir müşteri getir (Düzenleme sayfasına veriyi doldurmak için)
        public async Task<Musteri> MusteriGetirByIdAsync(int id)
        {
            return await _context.Kullanicilar.OfType<Musteri>().FirstOrDefaultAsync(m => m.Id == id);
        }

        
        // 2. Müşteriyi Güncelle (Şifre Desteğiyle)
        public async Task MusteriGuncelleAsync(int id, string ad, string soyad, string email, string telefon, string yeniSifre)
        {
            var musteri = await _context.Kullanicilar.FindAsync(id);
            if (musteri != null)
            {
                musteri.Ad = ad;
                musteri.Soyad = soyad;
                musteri.Email = email;
                musteri.Telefon = telefon;

                // Eğer yönetici yeni bir şifre yazdıysa onu da güncelle
                if (!string.IsNullOrEmpty(yeniSifre))
                {
                    musteri.SifreHash = yeniSifre;
                }

                await _context.SaveChangesAsync();
            }
        }

        // --- MÜŞTERİ EKLEME ---
        public async Task MusteriEkleAsync(MusteriCreateDto dto)
        {
            var yeniMusteri = new Musteri
            {
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                Email = dto.Email,
                Telefon = dto.Telefon,
                SifreHash = dto.Sifre // Admin belirlediği şifreyi atıyoruz
            };

            await _context.Kullanicilar.AddAsync(yeniMusteri);
            await _context.SaveChangesAsync();
        }
    }
}