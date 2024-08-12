using Microsoft.CodeAnalysis;
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


        //public async Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, bool? NonAssign)
        //{
        //    var retailors = await NaturalDbContext.Retailors
        //    //.Include(c => c.AreaNavigation)
        //     .Include(a => a.CityNavigation)
        //    .ThenInclude(ct => ct.State)
        //    .Where(d => d.IsDeleted != true)
        //     .ToListAsync();


        //    if (search.City != null || search.State != null || search.FullName != null ||
        //        search.FirstName != null || search.LastName != null)
        //    {
        //        retailors = await SearchRetailors(retailors, search);

        //        if (NonAssign == true)
        //        {
        //            retailors = await SearchNonAssignedRetailors(retailors);
        //        }
        //    }
        //    if (NonAssign == true && (search.City == null || search.State == null || search.FullName == null ||
        //        search.FirstName == null || search.LastName == null))
        //    {
        //        retailors = await SearchNonAssignedRetailors(retailors);
        //    }
        //    var result = retailors.
        //        Select(c => new Retailor
        //        {

        //            Id = c.Id,
        //            FirstName = c.FirstName,
        //            LastName = c.LastName,

        //            MobileNumber = c.MobileNumber,
        //            Address = c.Address,
        //            Email = c.Email,
        //            //Area = c.AreaNavigation.AreaName,
        //            //City = c.AreaNavigation.City.CityName,
        //            //State = c.AreaNavigation.City.State.StateName,
        //            City = c.CityNavigation?.CityName,
        //            State = c.StateNavigation?.StateName,
        //            Latitude = c.Latitude,
        //            Longitude = c.Longitude,
        //            Image = c.Image
        //        });

        //    return result;
        //}
        public async Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, bool? NonAssign)
        {
            if (NonAssign == true)
            {
                var retailor = await NaturalDbContext.Retailors
             //.Include(c => c.AreaNavigation)
             .Include(a => a.CityNavigation)
            .ThenInclude(ct => ct.State)
            .Where(d => d.IsDeleted != true)
             .ToListAsync();
                if (search.City != null || search.State != null || search.FullName != null ||
                     search.FirstName != null || search.LastName != null)
                {
                    retailor = await SearchRetailors(retailor, search);

                    if (NonAssign == true)
                    {
                        retailor = await SearchNonAssignedRetailors(retailor);
                    }
                }
                else if (NonAssign == true && (search.City == null || search.State == null || search.FullName == null ||
                    search.FirstName == null || search.LastName == null))
                {
                    retailor = await SearchNonAssignedRetailors(retailor);
                }
                var result = retailor.
               Select(c => new Retailor
               {

                   Id = c.Id,
                   FirstName = c.FirstName,
                   LastName = c.LastName,

                   MobileNumber = c.MobileNumber,
                   Address = c.Address,
                   Email = c.Email,
                   //Area = c.AreaNavigation.AreaName,
                   //City = c.AreaNavigation.City.CityName,
                   //State = c.AreaNavigation.City.State.StateName,
                   City = c.CityNavigation?.CityName,
                   State = c.StateNavigation?.StateName,
                   Latitude = c.Latitude,
                   Longitude = c.Longitude,
                   Image = c.Image
               });

                return result;
            }
            else 
            {
                var query = from r in NaturalDbContext.Retailors
                            join rtd in NaturalDbContext.RetailorToDistributors
                            on r.Id equals rtd.RetailorId into rtdGroup
                            from rtd in rtdGroup.DefaultIfEmpty() // Left join to include retailors without distributors
                            join q in NaturalDbContext.Distributors
                            on rtd.DistributorId equals q.Id into dGroup
                            from q in dGroup.DefaultIfEmpty() // Left join to include distributors without retailors
                            where r.IsDeleted != true


                            select new
                            {
                                Id = r.Id,
                                Retailor = r,
                                //City = r.CityNavigation.CityName,
                                //State = r.StateNavigation.StateName,
                                Distributor = q == null ? null : $"{q.FirstName} {q.LastName}"


                            };

                // Apply search filters
                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.State))
                    {
                        query = query.Where(q => q.Retailor.StateNavigation.Id == search.State);
                    }

                    if (!string.IsNullOrEmpty(search.City))
                    {
                        query = query.Where(q => q.Retailor.CityNavigation.Id == search.City);
                    }

                    //if (!string.IsNullOrEmpty(search.FullName))
                    //{
                    //    query = query.Where(q =>
                    //        q.Retailor.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
                    //        (q.Retailor.LastName != null && q.Retailor.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase)) ||
                    //        (q.Retailor.FirstName + q.Retailor.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
                    //        (q.Retailor.FirstName + " " + q.Retailor.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase)
                    //    );
                    //}
                }



                // Execute the query and transform the results
                var retailors = await query
                    .Select(q => new Retailor
                    {
                        Id = q.Id,
                        FirstName = q.Retailor.FirstName,
                        LastName = q.Retailor.LastName,
                        MobileNumber = q.Retailor.MobileNumber,
                        Address = q.Retailor.Address,
                        Email = q.Retailor.Email,
                        City = q.Retailor.CityNavigation.CityName,
                        State = q.Retailor.StateNavigation.StateName,
                        Latitude = q.Retailor.Latitude,
                        Longitude = q.Retailor.Longitude,
                        Image = q.Retailor.Image,
                        // Optionally, include distributor information if needed
                        Distributor = q.Distributor
                    })
                    .ToListAsync();
                if ( search.FullName != null )
                {
                    retailors = await searchDistributorRecords(retailors, search);

                    
                }
                return retailors;
                
            }
         

        }
        public async Task<IEnumerable<RetailorDetailsByArea>> GetRetailorDetailsByAreaId(string areaId)
        {
            var retailorDetail = await NaturalDbContext.Retailors



                   .Where(d => d.Id == areaId && d.IsDeleted != true)

                   .Select(c => new RetailorDetailsByArea
                   {
                       Id = c.Id,
                       Retailor = string.Concat(c.FirstName, "", c.LastName),





                   }).ToListAsync();
            return retailorDetail;


        }
        private async Task<List<Retailor>> SearchNonAssignedRetailors(List<Retailor> retailors)
        {
            var assignedRetailorIds = await NaturalDbContext.RetailorToDistributors
                            .Select(de => de.RetailorId)
                                .ToListAsync();
            var nonAssignedRetailors = retailors
               .Where(c => !assignedRetailorIds.Contains(c.Id)).ToList();
            return nonAssignedRetailors;
        }

        private async Task<List<Retailor>> SearchRetailors(List<Retailor> retailors, SearchModel search)
        {
            var searchedRetailors = retailors.Where(c =>
       (c.IsDeleted != true) &&
       (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
         (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
        //(string.IsNullOrEmpty(search.Area) || c.Area == search.Area) && 
          (string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.LastName?.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ?? false) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))).ToList();
            return searchedRetailors;
        }
        private async Task<List<Retailor>> searchDistributorRecords(List<Retailor> retailors, SearchModel search)
        {
            var searchedRetailors = retailors.Where(c =>
       (c.IsDeleted != true) &&

         (string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.LastName?.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ?? false) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))).ToList();
            return searchedRetailors;
        }
        public async Task<GetRetailor> GetRetailorDetailsByIdAsync(string id)
        {
            var retailorDetails = await (from retailor in NaturalDbContext.Retailors
                                         //join area in NaturalDbContext.Areas on retailor.Area equals area.Id
                                         join city in NaturalDbContext.Cities on retailor.City equals city.Id
                                         join state in NaturalDbContext.States on city.StateId equals state.Id
                                         where retailor.Id == id
                                         select new
                                         {
                                             Retailor = retailor,
                                             //Area = area,
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
                    //Area = retailorDetails.Area.AreaName,
                    //AreaId = retailorDetails.Area.Id,
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
                //existingRetailor.Area = retailor.Area;
                existingRetailor.Latitude = retailor.Latitude;
                existingRetailor.Longitude = retailor.Longitude;
                existingRetailor.Image = retailor.Image;
                await NaturalDbContext.SaveChangesAsync();


            }
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
