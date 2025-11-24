using Kuafor.Business;
using Microsoft.AspNetCore.Mvc;

namespace Kuafor.Web.Controllers
{
    public class RandevuController : Controller
    {
        private readonly RandevuService _randevuService;

        public RandevuController(RandevuService randevuService)
        {
            _randevuService = randevuService;
        }

        // 1. Randevu Alma Sayfasını Göster
        [HttpGet]
        public IActionResult Al()
        {
            return View();
        }

        // 2. Randevuyu Kaydetmeyi Dene
        [HttpPost]
        public async Task<IActionResult> Al(int musteriId, int calisanId, int hizmetId, DateTime tarih)
        {
            try
            {
                // Backend'deki servisi çağırıyoruz
                await _randevuService.RandevuOlusturAsync(musteriId, calisanId, hizmetId, tarih);

                // Başarılıysa ana sayfaya dön
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // HATA VARSA (Çakışma vb.) sayfayı yeniden göster ve hatayı yaz
                ViewBag.HataMesaji = ex.Message;
                return View();
            }
        }
    }
}