using Kuafor.Business;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Kuafor.Web.Controllers
{
    public class HomeController : Controller
    {
        // Buradaki servisleri sildik çünkü ana sayfada veri göstermiyoruz.

        public IActionResult Index()
        {
            return View();
        }

        // Onayla/Ýptal metotlarýný da siliyoruz çünkü onlarý YonetimController altýna 
        // veya oradaki listeye taþýdýk. Eðer butonlarýn linklerini güncellemediysen
        // ve hata alýrsan söyle, onlarý geri ekleyelim.
    }
}