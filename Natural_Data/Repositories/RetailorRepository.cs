using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Data;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

#nullable disable
namespace Natural_Data.Repositories
{
    public class RetailorRepository : Repository<Retailor>, IRetailorRepository
    {
        public RetailorRepository(NaturalsContext context) : base(context)
        {

        }


        public async Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, string? NonAssign)
        {
            var retailors = await (from Retailor in NaturalDbContext.Retailors
                                   join area in NaturalDbContext.Areas on Retailor.Area equals area.Id
                                   join city in NaturalDbContext.Cities on area.CityId equals city.Id
                                   join state in NaturalDbContext.States on city.StateId equals state.Id
                                   where Retailor.IsDeleted != true
                                   select new
                                   {
                                       retailor = Retailor,
                                       Area = area,
                                       City = city,
                                       State = state
                                   }).ToListAsync();
            if(search != null)
            {
                
                retailors = retailors.Where(c =>
                       (c.retailor.IsDeleted != true) &&
        (string.IsNullOrEmpty(search.State) || c.retailor.State == search.State) &&
        (string.IsNullOrEmpty(search.City) || c.retailor.City == search.City) &&
        (string.IsNullOrEmpty(search.Area) || c.retailor.Area == search.Area) &&
        (string.IsNullOrEmpty(search.FullName) || c.retailor.FirstName.StartsWith(search.FullName) ||
        c.retailor.LastName.StartsWith(search.FullName) || (c.retailor.FirstName + c.retailor.LastName).StartsWith(search.FullName) ||
        (c.retailor.FirstName + " " + c.retailor.LastName).StartsWith(search.FullName))).ToList();
                if(NonAssign == "y")
                {
                    var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                                                .Select(de => de.RetailorId)
                                                    .ToListAsync();
                     retailors = retailors
                        .Where(c => !assignedRetailorIds.Contains(c.retailor.Id)).ToList();
                }
            }
            var result = retailors.
                Select(c => new Retailor
                {

                    Id = c.retailor.Id,
                    FirstName = c.retailor.FirstName,
                    LastName = c.retailor.LastName,
                    MobileNumber = c.retailor.MobileNumber,
                    Address = c.retailor.Address,
                    Area = c.Area.AreaName,
                    Email = c.retailor.Email,
                    City = c.City.CityName,
                    State = c.State.StateName,
                    Latitude = c.retailor.Latitude,
                    Longitude = c.retailor.Longitude,
                    Image = c.retailor.Image
                });

            return result;
        }

        public async Task<GetRetailor> GetRetailorDetailsByIdAsync(string id)
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
                var result = new GetRetailor
                {
                    Id = retailorDetails.Retailor.Id,
                    FirstName = retailorDetails.Retailor.FirstName,
                    LastName = retailorDetails.Retailor.LastName,
                    MobileNumber = retailorDetails.Retailor.MobileNumber,
                    Address = retailorDetails.Retailor.Address,
                    Email = retailorDetails.Retailor.Email,
                    Area = retailorDetails.Area.AreaName,
                    AreaId = retailorDetails.Area.Id,
                    City = retailorDetails.City.CityName,
                    CityId = retailorDetails.City.Id,
                    State = retailorDetails.State.StateName,
                    StateId = retailorDetails.State.Id,
                    Latitude = retailorDetails.Retailor.Latitude,
                    Longitude = retailorDetails.Retailor.Longitude,
                    Image = retailorDetails.Retailor.Image,
                };

                return result;
            }

            return null;
        }

        public async Task UpdateRetailorAsync(Retailor existingRetailor, Retailor retailor)
        {
            if (existingRetailor != null)
            {

                existingRetailor.FirstName = retailor.FirstName;
                existingRetailor.LastName = retailor.LastName;
                existingRetailor.Email = retailor.Email;
                existingRetailor.MobileNumber = retailor.MobileNumber;
                existingRetailor.Address = retailor.Address;
                existingRetailor.City = retailor.City;
                existingRetailor.State = retailor.State;
                existingRetailor.Area = retailor.Area;
                existingRetailor.Latitude = retailor.Latitude;
                existingRetailor.Longitude = retailor.Longitude;
                existingRetailor.Image = retailor.Image;
                await NaturalDbContext.SaveChangesAsync();


            }
        }

        public async Task<IEnumerable<Retailor>> SearchRetailorAsync(SearchModel search)
        {
            {

                var exec = await NaturalDbContext.Retailors
                       .Include(c => c.AreaNavigation)
                       .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                       .Where(c =>
                       (c.IsDeleted != true) &&
        (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
        (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
        (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
        (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
        c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName))).ToListAsync();
                var result = exec.Select(c => new Retailor
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Area = c.AreaNavigation.AreaName,
                    Email = c.Email,
                    City = c.AreaNavigation.City.CityName,
                    State = c.AreaNavigation.City.State.StateName,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude
                }).ToList();
                return result;
            }

        }

        public async Task<IEnumerable<Retailor>> GetNonAssignedRetailorsAsync()
        {

            var retailors = await NaturalDbContext.Retailors
                .Include(c => c.AreaNavigation)
                .ThenInclude(a => a.City)
                .ThenInclude(ct => ct.State)
                .ToListAsync();

            var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                .Select(de => de.RetailorId)
                .ToListAsync();

            var nonAssignedRetailors = retailors
             .Where(c => !assignedRetailorIds.Contains(c.Id))
             .Select(c => new Retailor
             {
                 Id = c.Id,
                 FirstName = c.FirstName,
                 LastName = c.LastName,
                 MobileNumber = c.MobileNumber,
                 Address = c.Address,
                 Email = c.Email,
                 Area = c.AreaNavigation.AreaName,
                 City = c.AreaNavigation.City.CityName,
                 State = c.AreaNavigation.City.State.StateName,
                 Latitude = c.Latitude,
                 Longitude = c.Longitude
             })
             .ToList();

            return nonAssignedRetailors;
        }

        public async Task<IEnumerable<Retailor>> SearchNonAssignedRetailorsAsync(SearchModel search)
        {
            var retailors = await NaturalDbContext.Retailors
                      .Include(c => c.AreaNavigation)
                       .ThenInclude(a => a.City)
                      .ThenInclude(ct => ct.State)
                      .Where(c =>
       (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
       (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
       (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
       (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
       c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName) ||
       (c.FirstName + " " + c.LastName).StartsWith(search.FullName)))
      .ToListAsync();

            var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                 .Select(de => de.RetailorId)
                 .ToListAsync();

            var nonAssignedRetailors = retailors
                .Where(c => !assignedRetailorIds.Contains(c.Id))
                .Select(c => new Retailor
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Email = c.Email,
                    Area = c.AreaNavigation.AreaName,
                    City = c.AreaNavigation.City.CityName,
                    State = c.AreaNavigation.City.State.StateName,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude
                })
                .ToList();

            return nonAssignedRetailors;
        }


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
