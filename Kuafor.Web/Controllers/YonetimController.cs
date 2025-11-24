using Kuafor.Business;
using Kuafor.Core;
using Kuafor.Core.DTOs; // Bunu eklemeyi unutma
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Kuafor.Web.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class YonetimController : Controller
    {
        private readonly YonetimService _yonetimService;
        private readonly RandevuService _randevuService; // 1. Bunu ekledik

        // 2. Constructor'a (Yapıcı Metoda) randevuService'i de ekledik
        public YonetimController(YonetimService yonetimService, RandevuService randevuService)
        {
            _yonetimService = yonetimService;
            _randevuService = randevuService; // 3. Değeri atadık
        }

        // --- ANA SAYFA (LİSTELEME) ---
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Artık _randevuService tanınıyor, hata vermez.
            var randevular = await _randevuService.RandevulariGetirAsync();
            return View(randevular);
        }

        // --- HİZMET EKLEME ---
        [HttpGet]
        public IActionResult HizmetEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HizmetEkle(string ad, int sure, decimal ucret)
        {
            await _yonetimService.HizmetEkleAsync(ad, sure, ucret);
            return RedirectToAction("Index");
        }

        // --- SALON EKLEME ---
        [HttpGet]
        public IActionResult SalonEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SalonEkle(SalonCreateDto dto)
        {
            await _yonetimService.SalonEkleAsync(dto.Ad, dto.Adres, dto.CalismaSaatleri);
            return RedirectToAction("Index");
        }

        // --- ÇALIŞAN EKLEME ---
        [HttpGet]
        public IActionResult CalisanEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalisanEkle(CalisanCreateDto dto)
        {
            await _yonetimService.CalisanEkleAsync(dto);
            return RedirectToAction("Index");
        }

        // --- UYGUNLUK (MESAİ) EKLEME ---
        [HttpGet]
        public IActionResult UygunlukEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UygunlukEkle(UygunlukCreateDto dto)
        {
            await _yonetimService.UygunlukEkleAsync(dto);
            return RedirectToAction("Index");
        }

        // --- GELİR RAPORU ---
        [HttpGet]
        public async Task<IActionResult> Rapor()
        {
            var gelir = await _yonetimService.ToplamGelirHesaplaAsync();
            ViewBag.Gelir = gelir;
            return View(); // Bunun View'ını (Rapor.cshtml) oluşturmayı unutma
        }
        // --- LİSTELEME SAYFALARI ---

        [HttpGet]
        public async Task<IActionResult> Musteriler()
        {
            var liste = await _yonetimService.TumMusterileriGetirAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> Calisanlar()
        {
            var liste = await _yonetimService.TumCalisanlariDetayliGetirAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> Salonlar()
        {
            var liste = await _yonetimService.TumSalonlariGetirAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> Hizmetler()
        {
            var liste = await _yonetimService.TumHizmetleriGetirAsync();
            return View(liste);
        }
        // --- MÜŞTERİ DÜZENLEME ---

        [HttpGet]
        public async Task<IActionResult> MusteriDuzenle(int id)
        {
            var musteri = await _yonetimService.MusteriGetirByIdAsync(id);
            if (musteri == null) return NotFound();
            return View(musteri);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> MusteriDuzenle(int id, string ad, string soyad, string email, string telefon, string yeniSifre)
        {
            // Yeni şifre parametresini de gönderiyoruz
            await _yonetimService.MusteriGuncelleAsync(id, ad, soyad, email, telefon, yeniSifre);
            return RedirectToAction("Musteriler");
        }
        // --- RANDEVU ONAY/İPTAL İŞLEMLERİ ---

        public async Task<IActionResult> Onayla(int id)
        {
            // RandevuService içindeki Onayla metodunu çağır
            await _randevuService.RandevuOnaylaAsync(id);

            // İşlem bitince tekrar Yönetim Paneli ana sayfasına dön
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Iptal(int id)
        {
            // RandevuService içindeki İptal metodunu çağır
            await _randevuService.RandevuIptalEtAsync(id);

            // İşlem bitince tekrar Yönetim Paneli ana sayfasına dön
            return RedirectToAction("Index");
        }
        // --- MÜŞTERİ EKLEME SAYFALARI ---

        [HttpGet]
        public IActionResult MusteriEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MusteriEkle(MusteriCreateDto dto)
        {
            await _yonetimService.MusteriEkleAsync(dto);
            return RedirectToAction("Musteriler");
        }
    }
}