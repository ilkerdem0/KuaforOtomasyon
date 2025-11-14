namespace Kuafor.Core
{
    // Yonetici sınıfı da Kullanici'dan miras alır
    public class Yonetici : Kullanici
    {
        // Yöneticinin yönettiği salonları tutacağız.
        public virtual ICollection<Salon> YonetilenSalonlar { get; set; } = new List<Salon>();
    }
}