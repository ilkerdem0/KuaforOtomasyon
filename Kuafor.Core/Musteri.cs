namespace Kuafor.Core
{
    // Musteri sınıfı, Kullanici sınıfından miras alır
    public class Musteri : Kullanici
    {
        // Musteri'nin kendine özel özellikleri buraya eklenebilir.
        // Şimdilik sadece aldığı randevuları tutalım.
        public virtual ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}