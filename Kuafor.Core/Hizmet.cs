namespace Kuafor.Core
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty; // Düzeltildi

        // "her işlemin süresi ve ücreti detaylı şekilde tanımlanabilecek"
        public int SureDakika { get; set; }
        public decimal Ucret { get; set; }

        // --- İLİŞKİLER ---
        // 1. Bu hizmeti sunan salonlar
        public virtual ICollection<Salon> Salonlar { get; set; } = new List<Salon>();

        // 2. Bu hizmeti verebilen çalışanlar (Uzmanlık alanı)
        public virtual ICollection<Calisan> VerebilenCalisanlar { get; set; } = new List<Calisan>();
    }
}