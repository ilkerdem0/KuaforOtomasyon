namespace Kuafor.Core
{
    public class Salon
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty; // Düzeltildi
        public string Adres { get; set; } = string.Empty; // Düzeltildi

        // "Her salonun çalışma saatleri tanımlanabilecek"
        public string CalismaSaatleriAciklamasi { get; set; } = string.Empty; // Düzeltildi

        // --- İLİŞKİLER ---
        // 1. Salondaki çalışanlar
        public virtual ICollection<Calisan> Calisanlar { get; set; } = new List<Calisan>();

        // 2. Salonda sunulan hizmetler
        public virtual ICollection<Hizmet> SunulanHizmetler { get; set; } = new List<Hizmet>();

        // 3. Bu salonu yöneten yönetici (veya yöneticiler)
        public virtual ICollection<Yonetici> Yoneticiler { get; set; } = new List<Yonetici>();
    }
}