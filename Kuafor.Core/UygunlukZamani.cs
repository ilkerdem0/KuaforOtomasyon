using System; // DayOfWeek ve TimeSpan için bu gerekli

namespace Kuafor.Core
{
    public class UygunlukZamani
    {
        public int Id { get; set; }

        // Örn: DayOfWeek.Monday (Pazartesi)
        public DayOfWeek Gun { get; set; }

        // Örn: 09:00:00
        public TimeSpan BaslangicSaati { get; set; }

        // Örn: 17:30:00
        public TimeSpan BitisSaati { get; set; }

        // --- İLİŞKİ ---
        // Bu uygunluk zamanının sahibi olan çalışan
        public int CalisanId { get; set; }
        public virtual Calisan? Calisan { get; set; } // Düzeltildi: 'Calisan' null olabilir (?)
    }
}