using Kuafor.Core; // Modellerimizi (Randevu, Kullanici vb.) bilmesi için
using Kuafor.DataAccess; // DbContext'i bilmesi için
using Microsoft.EntityFrameworkCore; // .AnyAsync, .FindAsync vb. EF komutları için
using System; // Exception ve DateTime için
using System.Threading.Tasks; // async/await (Task) için

namespace Kuafor.Business
{
    public class RandevuService
    {
        // 1. Veritabanı context'ini tutacak özel, salt okunur bir alan
        private readonly ApplicationDbContext _context;

        // 2. Constructor (Yapıcı Metot)
        public RandevuService(ApplicationDbContext context)
        {
            _context = context;
        }

        // METOT 1: Randevu Oluşturma (Halka Açık Metot)
        // Dışarıdan (API'den) bu metot çağrılacak
        public async Task<Randevu> RandevuOlusturAsync(int musteriId, int calisanId, int hizmetId, DateTime baslangic)
        {
            // 1. Hizmet bilgilerini veritabanından al (Süre ve Ücret için)
            var hizmet = await _context.Hizmetler.FindAsync(hizmetId);
            if (hizmet == null)
            {
                throw new Exception("Geçersiz hizmet seçimi.");
            }

            // 2. ÇAKIŞMA KONTROLÜNÜ ÇAĞIR
            bool cakismaVar = await CakismaVarMiAsync(calisanId, baslangic, hizmet.SureDakika);
            if (cakismaVar)
            {
                throw new Exception("Seçilen tarih ve saatte çakışma mevcut. Lütfen başka bir zaman seçin.");
            }

            // 3. (Gelişmiş Kontrol) Çalışanın uygunluk zamanlarına bakılabilir
            // ... (Bu, ödevin sonraki aşaması olabilir)

            // 4. Tüm kontrollerden geçtiyse, Randevu nesnesini oluştur
            // NOT: HATA BÜYÜK İHTİMALLE BU BLOKTAYDI
            var yeniRandevu = new Randevu
            {
                MusteriId = musteriId,
                CalisanId = calisanId,
                HizmetId = hizmetId,
                BaslangicTarihSaati = baslangic,
                ToplamSureDakika = hizmet.SureDakika, // Hizmetten al
                ToplamUcret = hizmet.Ucret, // Hizmetten al
                Durum = RandevuDurumu.OnayBekliyor // Onay mekanizması için ilk durum
            };

            // 5. Veritabanına ekle ve kaydet
            await _context.Randevular.AddAsync(yeniRandevu);
            await _context.SaveChangesAsync();

            return yeniRandevu; // Oluşturulan randevuyu geri döndür
        }


        // METOT 2: Çakışma Kontrolü (Özel Metot - Sadece bu sınıf kullanabilir)
        private async Task<bool> CakismaVarMiAsync(int calisanId, DateTime yeniRandevuBaslangic, int yeniRandevuSuresi)
        {
            // 1. Yeni randevunun bitiş zamanını hesapla
            DateTime yeniRandevuBitis = yeniRandevuBaslangic.AddMinutes(yeniRandevuSuresi);

            // 2. Veritabanında çakışan bir kayıt var mı diye KONTROL ET
            // (EF Core bu sorguyu SQL'e çevirecek)
            var cakismaVar = await _context.Randevular
                .Where(r =>
                    r.CalisanId == calisanId && // Sadece bu çalışan için
                    r.Durum != RandevuDurumu.IptalEdildi && // İptal edilmemiş randevular arasında

                    // Klasik Çakışma Mantığı:
                    // (YeniBaşlangıç < MevcutBitiş) AND (YeniBitiş > MevcutBaşlangtç)
                    (yeniRandevuBaslangic < r.BaslangicTarihSaati.AddMinutes(r.ToplamSureDakika)) &&
                    (yeniRandevuBitis > r.BaslangicTarihSaati)
                )
                .AnyAsync(); // Bu koşula uyan EN AZ BİR kayıt varsa "true" döner

            return cakismaVar; // true (çakışma var) veya false (çakışma yok)
        }
    }
}