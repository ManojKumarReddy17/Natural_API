using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Natural_Data.Repositories
{
    public class RetailorRepository : Repository<Retailor>, IRetailorRepository
    {
        public RetailorRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Retailor>> GetAllRetailorsAsync()
        {
            var retailors = await (from Retailor in NaturalDbContext.Retailors
                                   join area in NaturalDbContext.Areas on Retailor.Area equals area.Id
                                   join city in NaturalDbContext.Cities on area.CityId equals city.Id
                                   join state in NaturalDbContext.States on city.StateId equals state.Id
                                   select new
                                   {
                                       retailor = Retailor,
                                       Area = area,
                                       City = city,
                                       State = state
                                   }).ToListAsync();
            var result = retailors.Select(c => new Retailor
            {

                FirstName = c.retailor.FirstName,
                LastName = c.retailor.LastName,
                MobileNumber = c.retailor.MobileNumber,
                Address = c.retailor.Address,
                Area = c.Area.AreaName,
                Email = c.retailor.Email,
                City = c.City.CityName,
                State = c.State.StateName,
            });

            return result;
        }

        public async Task<Retailor> GetWithRetailorsByIdAsync(string id)
        {
            var retailorDetails = await (from retailor in NaturalDbContext.Retailors
                                         join area in NaturalDbContext.Areas on retailor.Area equals area.Id
                                         join city in NaturalDbContext.Cities on area.CityId equals city.Id
                                         join state in NaturalDbContext.States on city.StateId equals state.Id
                                         where retailor.Id == id
                                         select new
                                         {
                                             Retailor = retailor,
                                             Area = area,
                                             City = city,
                                             State = state
                                         }).FirstOrDefaultAsync();

            if (retailorDetails != null)
            {
                var result = new Retailor
                {
                    Id = retailorDetails.Retailor.Id,
                    FirstName = retailorDetails.Retailor.FirstName,
                    LastName = retailorDetails.Retailor.LastName,
                    MobileNumber = retailorDetails.Retailor.MobileNumber,
                    Address = retailorDetails.Retailor.Address,
                    Email = retailorDetails.Retailor.Email,
                    Area = retailorDetails.Area.AreaName,
                    City = retailorDetails.City.CityName,
                    State = retailorDetails.State.StateName,
                };

                return result;
            }

            return null;
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }


    }
}
