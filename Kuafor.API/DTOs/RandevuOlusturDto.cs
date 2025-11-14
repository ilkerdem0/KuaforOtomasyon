using System; // DateTime için

namespace Kuafor.API.DTOs
{
    // Bu sınıf, dışarıdan (frontend'den) bir randevu talebi
    // geldiğinde hangi bilgileri beklediğimizi tanımlar.
    public class RandevuOlusturDto
    {
        public int MusteriId { get; set; }
        public int CalisanId { get; set; }
        public int HizmetId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
    }
}