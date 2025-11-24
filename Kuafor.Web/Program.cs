using Kuafor.Business;
using Kuafor.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// 1. Builder (Ýnþa edici) BURADA oluþturuluyor.
// Senin hatan muhtemelen bu satýrýn üstüne kod yazmandan kaynaklýydý.
var builder = WebApplication.CreateBuilder(args);
// Giriþ (Auth) Ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Giriþ yapmamýþsa buraya at
        options.AccessDeniedPath = "/Auth/Login";
    });

// MVC servislerini ekle (Web Uygulamasý olduðu için)
builder.Services.AddControllersWithViews();

// --- BÝZÝM EKLEDÝÐÝMÝZ AYARLAR BAÞLANGIÇ ---

// Veritabaný Baðlantýsý
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Servislerin Tanýmlanmasý (Dependency Injection)
builder.Services.AddScoped<RandevuService>();
builder.Services.AddScoped<YonetimService>();

// --- BÝZÝM EKLEDÝÐÝMÝZ AYARLAR BÝTÝÞ ---

var app = builder.Build();

// HTTP istek hattýný yapýlandýr (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doðrulama
app.UseAuthorization();  // Yetkilendirme

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"); // <-- Deðiþiklik Burada: Auth/Login

app.Run();