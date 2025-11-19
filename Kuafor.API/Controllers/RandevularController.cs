using Kuafor.API.DTOs; // 1. Adımda oluşturduğumuz DTO sınıfı için
using Kuafor.Business; // RandevuService'i kullanabilmek için
using Microsoft.AspNetCore.Mvc; // [ApiController], [Route] vb. için
using System; // Exception sınıfı için
using System.Threading.Tasks; // async/await (Task) için

namespace Kuafor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Bu controller'a http://.../api/randevular adresinden erişilir
    public class RandevularController : ControllerBase
    {
        // 1. Business katmanındaki servisimizi tutacak alan
        private readonly RandevuService _randevuService;

        // 2. Constructor Injection:
        // Program.cs'e kaydettiğimiz RandevuService'in
        // bu controller'a otomatik olarak "enjekte edilmesini" sağlıyoruz.
        public RandevularController(RandevuService randevuService)
        {
            _randevuService = randevuService;
        }

        // 3. Randevu Oluşturma Endpoint'i (Kapısı)
        // Dışarıdan birisi HTTP POST isteği ile
        // http://.../api/randevular/olustur adresine JSON verisi gönderdiğinde
        // bu metot çalışacak.
        [HttpPost("olustur")]
        public async Task<IActionResult> RandevuOlustur([FromBody] RandevuOlusturDto randevuDto)
        {
            try
            {
                // 4. Gelen DTO verisini Business katmanındaki servise gönder
                var yeniRandevu = await _randevuService.RandevuOlusturAsync(
                    randevuDto.MusteriId,
                    randevuDto.CalisanId,
                    randevuDto.HizmetId,
                    randevuDto.BaslangicTarihi
                );

                // 5. Başarılı olursa:
                // HTTP 201 (Created) durum kodu ve
                // oluşturulan randevunun bilgilerini geri döndür.
                return CreatedAtAction(nameof(GetRandevuById), new { id = yeniRandevu.Id }, yeniRandevu);
            }
            catch (Exception ex)
            {
                // 6. Başarısız olursa (örn: çakışma varsa):
                // Business katmanından fırlatılan hatayı yakala
                // ve HTTP 400 (Bad Request) olarak hatayı döndür.
                return BadRequest(new { mesaj = ex.Message });
            }
        }

        // (Bu metot sadece 'CreatedAtAction' için bir yer tutucudur,
        // ödevin ilerleyen kısımlarında doldurulabilir)
        [HttpGet("{id}")]
        public IActionResult GetRandevuById(int id)
        {
            // İleride burası tek bir randevuyu getirmek için doldurulabilir
            return Ok(new { Mesaj = $"Randevu {id} getirildi (simülasyon)" });
        }
        [HttpGet("listele")]
        public async Task<IActionResult> TumRandevulariListele()
        {
            var randevular = await _randevuService.RandevulariGetirAsync();
            return Ok(randevular);
        }
    }
}