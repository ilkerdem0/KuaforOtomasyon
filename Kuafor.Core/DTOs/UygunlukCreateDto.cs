using System;

namespace Kuafor.Core.DTOs
{
    public class UygunlukCreateDto
    {
        public int CalisanId { get; set; }

        // 0=Pazar, 1=Pazartesi, ..., 6=Cumartesi
        public DayOfWeek Gun { get; set; }

        // Örn: "09:00"
        public TimeSpan BaslangicSaati { get; set; }

        // Örn: "18:00"
        public TimeSpan BitisSaati { get; set; }
    }
}