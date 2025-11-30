# ✂️ Kuaför & Berber Randevu Otomasyon Sistemi

![.NET 8](https://img.shields.io/badge/.NET-8.0-purple?style=flat&logo=dotnet)
![Bootstrap 5](https://img.shields.io/badge/Frontend-Bootstrap%205-blue?style=flat&logo=bootstrap)
![EF Core](https://img.shields.io/badge/ORM-EF%20Core%208-green?style=flat)
![Status](https://img.shields.io/badge/Durum-Aktif-success)

Bu proje, **Nesneye Dayalı Programlama (OOP)** prensipleri ve **N-Katmanlı Mimari (N-Layer Architecture)** kullanılarak geliştirilmiş, ölçeklenebilir bir randevu yönetim sistemidir. Müşterilerin modern bir arayüz ile kolayca randevu almasını, işletme sahiplerinin ise personel, hizmet ve gelir takibini yapmasını sağlar.

## 🚀 Proje Hakkında

Proje, **Code-First** yaklaşımı ile geliştirilmiş olup, arka planda güçlü bir mimari ile çalışırken, ön yüzde **Bootstrap 5** ile güçlendirilmiş kullanıcı dostu (UI/UX) bir deneyim sunar.

### 🌟 Sürüm 2.0 Yenilikleri
* **Modern Landing Page:** Kullanıcıları karşılayan şık, responsive ve profesyonel ana sayfa tasarımı.
* **Adım Adım Randevu (Wizard):** Karmaşadan uzak; Hizmet Seçimi -> Personel Seçimi -> Onay akışı.
* **Akıllı Görsel Kartlar:** Hizmet türüne göre (Saç, Sakal, Bakım vb.) otomatik değişen dinamik ikonlar.
* **Kullanıcı Dostu Seçimler:** Müşteriler personel ID'si girmek zorunda kalmadan, açılır listeden (Dropdown) isim ile uzman seçebilir.

---

## 🏗 Mimari ve Teknolojiler

Proje endüstri standartlarına uygun olarak **4 Ana Katman** üzerine inşa edilmiştir:

| Katman | Açıklama |
| :--- | :--- |
| **1. Kuafor.Core** | Varlıklar (Entities), DTO'lar ve Arayüzler (Interfaces). Bağımsız katmandır. |
| **2. Kuafor.DataAccess** | Veritabanı bağlamı (`DbContext`), Migrations ve Repository desenleri. |
| **3. Kuafor.Business** | İş kuralları (Validasyonlar, Çakışma kontrolleri, Servisler). |
| **4. Kuafor.Web** | Kullanıcı arayüzü (MVC), Controller yapıları ve View katmanı. |

### 🛠 Teknik Yığın (Tech Stack)
* **Backend:** C# .NET 8, ASP.NET Core MVC
* **Veritabanı:** MS SQL Server
* **ORM:** Entity Framework Core 8 (Code-First)
* **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript (jQuery), FontAwesome
* **Prensipler