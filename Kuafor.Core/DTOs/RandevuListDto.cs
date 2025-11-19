using System;

namespace Kuafor.Core.DTOs
{
    public class RandevuListDto
    {
        public int Id { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string MusteriAdi { get; set; }
        public string CalisanAdi { get; set; }
        public string HizmetAdi { get; set; }
        public decimal Ucret { get; set; }
        public string Durum { get; set; }
    }
}