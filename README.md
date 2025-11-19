# Kuaför / Berber Randevu Otomasyon Sistemi (Seçenek A)

Bu proje, **Nesneye Dayalı Programlama (OOP)** dersi kapsamında geliştirilmiş; çoklu şube desteğine sahip, ölçeklenebilir ve katmanlı mimariye uygun bir randevu yönetim sistemidir.

## 🎯 Proje Özeti
Proje, müşterilerin kuaför/berber salonlarından randevu almasını, yöneticilerin ise salon, çalışan ve hizmet yönetimini yapmasını sağlayan bir Backend API projesidir. "Code-First" yaklaşımı ile geliştirilmiş olup, karmaşık iş mantıklarını (çakışma kontrolü, mesai kontrolü) barındırır.

## 🛠 Kullanılan Teknolojiler ve Mimari
* **Platform:** .NET 8 (Core)
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core 8 (Code-First)
* **Mimari:** N-Katmanlı Mimari (N-Layer Architecture)
* **Dokümantasyon:** Swagger UI

### Mimari Yapı
Proje 4 ana katmandan oluşur:
1. **Core:** Varlıklar (Entities), DTO'lar ve Enum'lar. (Bağımsız Katman)
2. **DataAccess:** Veritabanı bağlantısı (`DbContext`) ve Migrations.
3. **Business:** İş mantığı, validasyonlar ve servisler (`RandevuService`, `YonetimService`).
4. **API:** Dış dünyaya açılan RESTful Endpoint'ler (Controllers).

## ⚙️ Temel Özellikler (Gereksinimler)

### 1. Nesneye Dayalı Tasarım (OOP)
* **Kalıtım (Inheritance):** `Musteri`, `Calisan` ve `Yonetici` sınıfları, ortak özellikleri barındıran soyut `Kullanici` sınıfından türetilmiştir.
* **Kapsülleme (Encapsulation):** Veritabanı erişimi ve iş mantığı, API katmanından gizlenerek Servis katmanında kapsüllenmiştir.
* **İlişkiler:** Tablolar arası Bire-Çok ve Çoka-Çok (Many-to-Many) ilişkiler kurulmuştur (Örn: Çalışan ve Uzmanlık Alanları).

### 2. Randevu Sistemi ve Algoritmalar
* **Çakışma Kontrolü:** Sistem, yeni randevu oluşturulurken çalışanın mevcut randevularını kontrol eder ve zaman çakışması varsa işlemi engeller.
* **Mesai/Uygunluk Kontrolü:** Çalışanların sadece tanımlı olduğu gün ve saatlerde randevu alması sağlanır.
* **Filtreleme:** Müşteriler kendi randevu geçmişlerini görüntüleyebilir.

### 3. Yönetim Paneli (Admin)
* **Salon Yönetimi:** Yeni şube/salon ekleme ve listeleme.
* **Hizmet Yönetimi:** İşlem (Saç Kesimi, Boya vb.), süre ve ücret tanımlama.
* **Personel Yönetimi:** Çalışan ekleme ve çalışana uzmanlık alanı (Hizmet) atama.
* **Raporlama:** Onaylanmış randevular üzerinden toplam gelir hesaplama.

## 🚀 Kurulum ve Çalıştırma

1. Projeyi klonlayın.
2. `Kuafor.API` içindeki `appsettings.json` dosyasındaki `ConnectionStrings` alanını kendi SQL Server bilginize göre düzenleyin.
3. **Package Manager Console** üzerinden veritabanını oluşturun:
   ```powershell
   Update-Database