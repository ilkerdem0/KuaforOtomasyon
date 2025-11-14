namespace Kuafor.Core
{
    // Calisan sınıfı da Kullanici'dan miras alır
    public class Calisan : Kullanici
    {
        // Çalışanın kendine özel özellikleri:
        public bool AktifMi { get; set; } = true;

        // --- İLİŞKİLER ---

        // 1. Çalışan bir salona bağlıdır
        public int SalonId { get; set; }
        public virtual Salon? Salon { get; set; } // Düzeltildi: 'Salon' null olabilir (?)

        // 2. Çalışanın uzmanlık alanları (Hizmetler)
        public virtual ICollection<Hizmet> Uzmanliklar { get; set; } = new List<Hizmet>();

        // 3. Çalışanın uygunluk zaman dilimleri
        public virtual ICollection<UygunlukZamani> UygunlukZamanlari { get; set; } = new List<UygunlukZamani>();

        // 4. Çalışanın verdiği randevular
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}