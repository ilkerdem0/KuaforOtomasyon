using Kuafor.Core;
using Kuafor.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kuafor.Business
{
    public class YonetimService
    {
        private readonly ApplicationDbContext _context;

        public YonetimService(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- SALON İŞLEMLERİ ---

        public async Task<Salon> SalonEkleAsync(string ad, string adres, string saatler)
        {
            var yeniSalon = new Salon
            {
                Ad = ad,
                Adres = adres,
                CalismaSaatleriAciklamasi = saatler
            };

            await _context.Salonlar.AddAsync(yeniSalon);
            await _context.SaveChangesAsync();
            return yeniSalon;
        }

        public async Task<List<Salon>> TumSalonlariGetirAsync()
        {
            return await _context.Salonlar.ToListAsync();
        }

        // --- HİZMET İŞLEMLERİ ---

        public async Task<Hizmet> HizmetEkleAsync(string ad, int sure, decimal ucret)
        {
            var yeniHizmet = new Hizmet
            {
                Ad = ad,
                SureDakika = sure,
                Ucret = ucret
            };

            await _context.Hizmetler.AddAsync(yeniHizmet);
            await _context.SaveChangesAsync();
            return yeniHizmet;
        }

        public async Task<List<Hizmet>> TumHizmetleriGetirAsync()
        {
            return await _context.Hizmetler.ToListAsync();
        }
    }
}