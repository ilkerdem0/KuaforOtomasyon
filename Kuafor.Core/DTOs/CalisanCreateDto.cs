using System.Collections.Generic;

namespace Kuafor.Core.DTOs // Bak burası artık Core oldu
{
    public class CalisanCreateDto
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public int SalonId { get; set; }
        public List<int> UzmanlikHizmetIdleri { get; set; }
    }
}