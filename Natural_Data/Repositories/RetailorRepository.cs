using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

#nullable disable
namespace Natural_Data.Repositories
{
    public class RetailorRepository : Repository<Retailor>, IRetailorRepository
    {
        private readonly ILogger<RetailorRepository> _logger;
        public RetailorRepository(NaturalsContext context, ILogger<RetailorRepository> logger) : base(context)
        {
            _logger= logger;    

        }

        public RetailorRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, bool? NonAssign)
        {
            try
            {
                var retailors = await NaturalDbContext.Retailors
               .Include(c => c.AreaNavigation)
                .ThenInclude(a => a.City)
               .ThenInclude(ct => ct.State)
               .Where(d => d.IsDeleted != true)
                .ToListAsync();

                if (search.Area != null || search.City != null || search.State != null || search.FullName != null ||
                    search.FirstName != null || search.LastName != null)
                {
                    retailors = await SearchRetailors(retailors, search);

                    if (NonAssign == true)
                    {
                        retailors = await SearchNonAssignedRetailors(retailors);
                    }
                }
                if (NonAssign == true && (search.Area == null || search.City == null || search.State == null || search.FullName == null ||
                    search.FirstName == null || search.LastName == null))
                {
                    retailors = await SearchNonAssignedRetailors(retailors);
                }
                var result = retailors
         .Select(c => new Retailor
         {
             Id = c.Id,
             FirstName = c.FirstName + " " + c.LastName, // Combining FirstName and LastName
             MobileNumber = c.MobileNumber,
             Address = c.Address,
             Email = c.Email,
             Area = c.AreaNavigation.AreaName,
             City = c.AreaNavigation.City.CityName,
             State = c.AreaNavigation.City.State.StateName,
             Latitude = c.Latitude,
             Longitude = c.Longitude,
             Image = c.Image
         });
<<<<<<< Updated upstream

            return result;
        }
        public async Task<IEnumerable<RetailorDetailsByArea>> GetRetailorDetailsByAreaId(string areaId)
        {
            var retailorDetail = await NaturalDbContext.Retailors



                   .Where(d => d.Area == areaId && d.IsDeleted != true)

                   .Select(c => new RetailorDetailsByArea
                   {
                       Id = c.Id,
                       Retailor = string.Concat(c.FirstName, "", c.LastName),





                   }).ToListAsync();
            return retailorDetail;


        }
        private async Task<List<Retailor>> SearchNonAssignedRetailors(List<Retailor> retailors)
        {
            try
            {
                var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                          .Select(de => de.RetailorId)
                              .ToListAsync();
                var nonAssignedRetailors = retailors
                   .Where(c => !assignedRetailorIds.Contains(c.Id)).ToList();
                return nonAssignedRetailors;

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-SearchNonAssignedRetailors", ex.Message);
                return null;

            }


        }

        private async Task<List<Retailor>> SearchRetailors(List<Retailor> retailors, SearchModel search)
        {
            try
            {
                var searchedRetailors = retailors.Where(c =>
           (c.IsDeleted != true) &&
    (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
    (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
    (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) && (string.IsNullOrEmpty(search.FullName) ||
            c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            c.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
    ).ToList();
                return searchedRetailors;
            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository- SearchRetailors", ex.Message);
                return null;

            }
        }


        public async Task<GetRetailor> GetRetailorDetailsByIdAsync(string id)
        {
            try
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
                        FullName = retailorDetails.Retailor.FirstName,
                        //LastName = retailorDetails.Retailor.LastName,
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
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-GetRetailorDetailsByIdAsync", ex.Message);
                return null;
=======
>>>>>>> Stashed changes

            }

<<<<<<< Updated upstream
=======
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-GetAllRetailorsAsync", ex.Message);
                return null;

            }

        }

        private async Task<List<Retailor>> SearchNonAssignedRetailors(List<Retailor> retailors)
        {
            try
            {
                var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                          .Select(de => de.RetailorId)
                              .ToListAsync();
                var nonAssignedRetailors = retailors
                   .Where(c => !assignedRetailorIds.Contains(c.Id)).ToList();
                return nonAssignedRetailors;

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-SearchNonAssignedRetailors", ex.Message);
                return null;

            }


        }

        private async Task<List<Retailor>> SearchRetailors(List<Retailor> retailors, SearchModel search)
        {
            try
            {
                var searchedRetailors = retailors.Where(c =>
           (c.IsDeleted != true) &&
    (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
    (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
    (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) && (string.IsNullOrEmpty(search.FullName) ||
            c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            c.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
            (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
    ).ToList();
                return searchedRetailors;
            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository- SearchRetailors", ex.Message);
                return null;

            }
        }


        public async Task<GetRetailor> GetRetailorDetailsByIdAsync(string id)
        {
            try
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
                        FullName = retailorDetails.Retailor.FirstName,
                        //LastName = retailorDetails.Retailor.LastName,
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
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-GetRetailorDetailsByIdAsync", ex.Message);
                return null;

            }

>>>>>>> Stashed changes
        }

        public async Task UpdateRetailorAsync(Retailor existingRetailor, Retailor retailor)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("RetailorRepository-UpdateRetailorAsync", ex.Message);
                

            }
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
