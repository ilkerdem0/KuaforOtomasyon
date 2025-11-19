using Kuafor.API.DTOs;
using Kuafor.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kuafor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YonetimController : ControllerBase
    {
        private readonly YonetimService _yonetimService;

        public YonetimController(YonetimService yonetimService)
        {
            _yonetimService = yonetimService;
        }

        // --- SALON ENDPOINTLERİ ---

        [HttpPost("salon-ekle")]
        public async Task<IActionResult> SalonEkle([FromBody] SalonCreateDto dto)
        {
            var salon = await _yonetimService.SalonEkleAsync(dto.Ad, dto.Adres, dto.CalismaSaatleri);
            return Ok(salon);
        }

        [HttpGet("salonlar")]
        public async Task<IActionResult> SalonlariGetir()
        {
            var salonlar = await _yonetimService.TumSalonlariGetirAsync();
            return Ok(salonlar);
        }

        // --- HİZMET ENDPOINTLERİ ---

        [HttpPost("hizmet-ekle")]
        public async Task<IActionResult> HizmetEkle([FromBody] HizmetCreateDto dto)
        {
            var hizmet = await _yonetimService.HizmetEkleAsync(dto.Ad, dto.SureDakika, dto.Ucret);
            return Ok(hizmet);
        }

        [HttpGet("hizmetler")]
        public async Task<IActionResult> HizmetleriGetir()
        {
            var hizmetler = await _yonetimService.TumHizmetleriGetirAsync();
            return Ok(hizmetler);
        }
    }
}