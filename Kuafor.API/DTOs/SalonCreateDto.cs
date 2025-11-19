namespace Kuafor.API.DTOs
{
    public class SalonCreateDto
    {
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string CalismaSaatleri { get; set; } // Örn: "09:00 - 18:00"
    }
}