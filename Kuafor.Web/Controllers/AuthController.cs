using Kuafor.Core; // Musteri sınıfı için
using Kuafor.DataAccess; // Veritabanı için
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // FirstOrDefaultAsync için
using System.Security.Claims;

namespace Kuafor.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GİRİŞ EKRANI (3 Sekmeli Sayfa)
        [HttpGet]
        public IActionResult Login()
        {
            // Eğer zaten giriş yapmışsa, rolüne göre yönlendir
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Yonetici")) return RedirectToAction("Index", "Yonetim");
                return RedirectToAction("Al", "Randevu");
            }
            return View();
        }

        // 2. GİRİŞ YAPMA İŞLEMİ (Müşteri ve Yönetici için ortak)
        [HttpPost]
        public async Task<IActionResult> GirisYap(string email, string sifre)
        {
            // A) YÖNETİCİ KONTROLÜ (Sabit şifre: admin / 123)
            if (email == "admin" && sifre == "123")
            {
                await OturumAc("Yönetici", "Yonetici", 0);
                return RedirectToAction("Index", "Yonetim"); // Yönetim Paneline Git
            }

            // B) MÜŞTERİ KONTROLÜ (Veritabanından)
            var musteri = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == email && x.SifreHash == sifre);

            if (musteri != null)
            {
                await OturumAc(musteri.Ad + " " + musteri.Soyad, "Musteri", musteri.Id);
                return RedirectToAction("Al", "Randevu"); // Randevu Alma Ekranına Git
            }

            // Başarısızsa hata ver
            ViewBag.Hata = "Hatalı kullanıcı adı veya şifre!";
            return View("Login");
        }

        // 3. KAYIT OLMA İŞLEMİ
        [HttpPost]
        public async Task<IActionResult> KayitOl(string ad, string soyad, string email, string telefon, string sifre)
        {
            // E-posta kontrolü
            if (await _context.Kullanicilar.AnyAsync(x => x.Email == email))
            {
                ViewBag.Hata = "Bu e-posta zaten kayıtlı!";
                return View("Login");
            }

            // Yeni Müşteri Kaydet
            var yeniMusteri = new Musteri
            {
                Ad = ad,
                Soyad = soyad,
                Email = email,
                Telefon = telefon,
                SifreHash = sifre
            };

            _context.Kullanicilar.Add(yeniMusteri);
            await _context.SaveChangesAsync();

            // Otomatik giriş yap ve yönlendir
            await OturumAc(ad + " " + soyad, "Musteri", yeniMusteri.Id);
            return RedirectToAction("Al", "Randevu");
        }

        // --- Çerez (Cookie) Oluşturma Yardımcısı ---
        private async Task OturumAc(string isim, string rol, int id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, isim),
                new Claim(ClaimTypes.Role, rol),
                new Claim("UserId", id.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        // 4. ÇIKIŞ YAP
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        // --- PROFİL SAYFASI (GET) ---
        [Authorize] // Sadece giriş yapanlar görebilir
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            // Çerezden (Cookie) kullanıcının ID'sini al
            var userIdString = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdString)) return RedirectToAction("Logout");

            int userId = int.Parse(userIdString);

            // Veritabanından kullanıcıyı bul
            var kullanici = await _context.Kullanicilar.FindAsync(userId);
            if (kullanici == null) return RedirectToAction("Logout");

            return View(kullanici);
        }

        // --- PROFİL GÜNCELLEME (POST) ---
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProfilGuncelle(string ad, string soyad, string telefon, string yeniSifre)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            var kullanici = await _context.Kullanicilar.FindAsync(userId);

            if (kullanici != null)
            {
                kullanici.Ad = ad;
                kullanici.Soyad = soyad;
                kullanici.Telefon = telefon;

                // Eğer yeni şifre alanına bir şey yazdıysa şifreyi de değiştir
                if (!string.IsNullOrEmpty(yeniSifre))
                {
                    kullanici.SifreHash = yeniSifre;
                }

                await _context.SaveChangesAsync();
                ViewBag.Mesaj = "Bilgileriniz başarıyla güncellendi!";
                return View("Profil", kullanici);
            }

            return RedirectToAction("Login");
        }// --- ŞİFREMİ UNUTTUM (GET) ---
        [HttpGet]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }

        // --- ŞİFRE SIFIRLAMA İŞLEMİ (POST) ---
        [HttpPost]
        public async Task<IActionResult> SifremiUnuttum(string email, string yeniSifre)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(x => x.Email == email);

            if (kullanici != null)
            {
                kullanici.SifreHash = yeniSifre;
                await _context.SaveChangesAsync();
                ViewBag.Mesaj = "Şifreniz başarıyla değiştirildi. Giriş yapabilirsiniz.";
                return View("Login"); // Login sayfasına geri gönder ama mesajı göster
            }

            ViewBag.Hata = "Bu e-posta adresi sistemde bulunamadı.";
            return View();
        }
    }
}