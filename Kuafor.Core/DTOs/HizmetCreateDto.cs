namespace Kuafor.Core.DTOs
{
    public class HizmetCreateDto
    {
        // "= string.Empty;" ekliyoruz
        public string Ad { get; set; } = string.Empty;
        public int SureDakika { get; set; }
        public decimal Ucret { get; set; }
    }
}