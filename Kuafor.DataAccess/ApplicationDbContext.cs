// 1. Gerekli kütüphaneleri ekliyoruz
using Kuafor.Core; // Modellerimizin (Kullanici, Salon vb.) olduğu yer
using Microsoft.EntityFrameworkCore;

namespace Kuafor.DataAccess
{
    // 2. Sınıfımızın EF Core'un DbContext sınıfından miras almasını sağlıyoruz
    public class ApplicationDbContext : DbContext
    {
        // 3. Bu constructor (yapıcı metot), veritabanı bağlantı ayarlarını
        // dışarıdan (API katmanından) alabilmemiz için gerekli.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // 4. Hangi sınıflarımızın veritabanında bir tabloya dönüşeceğini
        // DbSet<> olarak buraya yazıyoruz.
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Yonetici> Yoneticiler { get; set; }
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<UygunlukZamani> UygunlukZamanlari { get; set; }


        // 5. (İsteğe bağlı ama önerilir) İlişkileri detaylı yapılandırmak
        // için bu metodu override ediyoruz.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- DECIMAL AYARLARI ---
            modelBuilder.Entity<Randevu>()
                .Property(r => r.ToplamUcret)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasColumnType("decimal(18, 2)");

            // --- İLİŞKİLERDE CASCADE (ZİNCİRLEME SİLME) KURALLARI ---
            // SQL Server "multiple cascade paths" hatasını önlemek için,
            // bir tablonun birden fazla yoldan cascade edilmesini engelliyoruz.

            // 1. Randevu İlişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Musteri)
                .WithMany(m => m.Randevular)
                .HasForeignKey(r => r.MusteriId)
                .OnDelete(DeleteBehavior.Restrict); // Müşteri silinirse randevu silinmesin

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany(c => c.Randevular)
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict); // Çalışan silinirse randevu silinmesin

            modelBuilder.Entity<Randevu>()
               .HasOne(r => r.Hizmet)
               .WithMany()
               .HasForeignKey(r => r.HizmetId)
               .OnDelete(DeleteBehavior.Restrict); // Hizmet silinirse randevu silinmesin

            // 2. Çalışan -> Salon İlişkisi (Bu da bir yoldur)
            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Salon)
                .WithMany(s => s.Calisanlar)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Restrict); // Salon silinirse çalışan silinmesin (veya AktifMi=false yap)


            // --- MEVCUT MANY-TO-MANY İLİŞKİLERİ ---
            // (Bunlar genellikle sorun çıkarmaz ama biz yine de tanımlayalım)

            modelBuilder.Entity<Calisan>()
                .HasMany(c => c.Uzmanliklar)
                .WithMany(h => h.VerebilenCalisanlar)
                .UsingEntity(j => j.ToTable("CalisanHizmet")); // Birleştirme tablosunun adı

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.SunulanHizmetler)
                .WithMany(h => h.Salonlar)
                .UsingEntity(j => j.ToTable("SalonHizmet")); // Birleştirme tablosunun adı

            modelBuilder.Entity<Salon>()
                .HasMany(s => s.Yoneticiler)
                .WithMany(y => y.YonetilenSalonlar)
                .UsingEntity(j => j.ToTable("SalonYonetici")); // Birleştirme tablosunun adı
        }
    }
}