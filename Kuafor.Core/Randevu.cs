using System; // DateTime için bu gerekli

namespace Kuafor.Core
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime BaslangicTarihSaati { get; set; }

        // "Randevu detayları (işlem, süre, ücret, çalışan, tarih/saat) kaydedilecek"
        public int ToplamSureDakika { get; set; }
        public decimal ToplamUcret { get; set; }

        // "Randevu için onay mekanizması olacak"
        public RandevuDurumu Durum { get; set; } // Bu enum, hata vermemişti

        // --- İLİŞKİLER (Randevunun "sahipleri") ---
        public int MusteriId { get; set; }
        public virtual Musteri? Musteri { get; set; } // Düzeltildi: 'Musteri' null olabilir (?)

        public int CalisanId { get; set; }
        public virtual Calisan? Calisan { get; set; } // Düzeltildi: 'Calisan' null olabilir (?)

        public int HizmetId { get; set; }
        public virtual Hizmet? Hizmet { get; set; } // Düzeltildi: 'Hizmet' null olabilir (?)
    }
}