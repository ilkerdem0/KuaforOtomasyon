using Kuafor.Core;
using Kuafor.Core.DTOs;
using Kuafor.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        // --- SALON İŞLEMLERİ ---

        public async Task<Salon> SalonEkleAsync(string ad, string adres, string saatler)
        {
            var yeniSalon = new Salon
            {
                Ad = ad,
                Adres = adres,
                CalismaSaatleriAciklamasi = saatler
            };

            await _context.Salonlar.AddAsync(yeniSalon);
            await _context.SaveChangesAsync();
            return yeniSalon;
        }

        public async Task<List<Salon>> TumSalonlariGetirAsync()
        {
            return await _context.Salonlar.ToListAsync();
        }

        // --- HİZMET İŞLEMLERİ ---

        public async Task<Hizmet> HizmetEkleAsync(string ad, int sure, decimal ucret)
        {
            var yeniHizmet = new Hizmet
            {
                Ad = ad,
                SureDakika = sure,
                Ucret = ucret
            };

            await _context.Hizmetler.AddAsync(yeniHizmet);
            await _context.SaveChangesAsync();
            return yeniHizmet;
        }

        public async Task<List<Hizmet>> TumHizmetleriGetirAsync()
        {
            return await _context.Hizmetler.ToListAsync();
        }
        // --- ÇALIŞAN İŞLEMLERİ ---

        public async Task<Calisan> CalisanEkleAsync(CalisanCreateDto dto)
        {
            // 1. Önce çalışanın temel bilgilerini oluştur
            var yeniCalisan = new Calisan
            {
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                Email = dto.Email,
                Telefon = dto.Telefon,
                SifreHash = "12345", // Varsayılan şifre (İleride değiştirilebilir)
                SalonId = dto.SalonId,
                AktifMi = true
            };

            // 2. Seçilen uzmanlık alanlarını (Hizmetleri) bul ve ekle
            if (dto.UzmanlikHizmetIdleri != null && dto.UzmanlikHizmetIdleri.Count > 0)
            {
                // Veritabanından bu ID'lere sahip hizmetleri bul
                var secilenHizmetler = await _context.Hizmetler
                    .Where(h => dto.UzmanlikHizmetIdleri.Contains(h.Id))
                    .ToListAsync();

                // Çalışanın uzmanlık listesine ekle
                yeniCalisan.Uzmanliklar = secilenHizmetler;
            }

            // 3. Kaydet
            await _context.Calisanlar.AddAsync(yeniCalisan);
            await _context.SaveChangesAsync();
            return yeniCalisan;
        }

        // Çalışanları getirirken uzmanlıklarını da (Include) getirelim
        public async Task<List<Calisan>> TumCalisanlariGetirAsync()
        {
            return await _context.Calisanlar
                .Include(c => c.Uzmanliklar) // Uzmanlık bilgisini de yükle
                .Include(c => c.Salon)       // Salon bilgisini de yükle
                .ToListAsync();
        }
        // --- UYGUNLUK (MESAİ) İŞLEMLERİ ---

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

        // Çalışanın uygunluklarını getiren metot (İsteğe bağlı kontrol için)
        public async Task<List<UygunlukZamani>> CalisanUygunluklariniGetirAsync(int calisanId)
        {
            return await _context.UygunlukZamanlari
                .Where(u => u.CalisanId == calisanId)
                .ToListAsync();
        }
        // --- SALON BAZLI ÇALIŞAN LİSTELEME ---

        public async Task<List<CalisanListDto>> SalonCalisanlariniGetirAsync(int salonId)
        {
            return await _context.Calisanlar
                .Where(c => c.SalonId == salonId) // Sadece seçilen salonun çalışanları
                .Include(c => c.Uzmanliklar)      // Uzmanlıklarını da yükle
                .Select(c => new CalisanListDto
                {
                    Id = c.Id,
                    AdSoyad = c.Ad + " " + c.Soyad,
                    // Uzmanlık isimlerini virgülle birleştirip tek string yapıyoruz:
                    Uzmanliklar = string.Join(", ", c.Uzmanliklar.Select(u => u.Ad))
                })
                .ToListAsync();
        }
    }
}