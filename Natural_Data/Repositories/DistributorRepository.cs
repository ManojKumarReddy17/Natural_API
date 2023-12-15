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
    public class DistributorRepository : Repository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<List<Distributor>> GetAllDistributorstAsync()
        {

            var distributors = await NaturalDbContext.Distributors
            .Include(c => c.AreaNavigation)
             .ThenInclude(a => a.City)
            .ThenInclude(ct => ct.State)
             .ToListAsync();

            var result = distributors.Select(c => new Distributor
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Email = c.Email,
                UserName = c.UserName,
                Password = c.Password,
                Area = c.AreaNavigation.AreaName,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName,
              
            }).ToList();

            return result;
        }

        public async Task<Distributor> GetDistributorDetailsByIdAsync(string distributorid)
        {
            var distributors = await NaturalDbContext.Distributors
                       .Include(c => c.AreaNavigation)
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == distributorid);

            if (distributors != null)
            {
                var result = new Distributor
                {
                    Id = distributors.Id,
                    FirstName = distributors.FirstName,
                    LastName = distributors.LastName,
                    MobileNumber = distributors.MobileNumber,
                    Address = distributors.Address,
                    Email = distributors.Email,
                    Area = distributors.AreaNavigation.AreaName,
                   
                    City = distributors.AreaNavigation.City.CityName,
                    State = distributors.AreaNavigation.City.State.StateName,
                    UserName = distributors.UserName,
                    Password = distributors.Password
                };

                return result;

            }
            else
            {
                    return null;
            }
            
        }

        public async Task<IEnumerable<Distributor>> SearchDistributorAsync(SearchModel search)
        {
            
                var exec = await NaturalDbContext.Distributors
                       .Include(c => c.AreaNavigation)
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                       .Where(c =>
        (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
        (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
        (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
        (string.IsNullOrEmpty(search.FirstName) || c.FirstName.StartsWith(search.FirstName)) &&
        (string.IsNullOrEmpty(search.LastName) || c.LastName.StartsWith(search.LastName)))
       .ToListAsync();
            var result = exec.Select(c => new Distributor
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Area = c.AreaNavigation.AreaName,
                Email = c.Email,
                UserName = c.UserName,
                Password = c.Password,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName
            }).ToList();
                return result;
            }
        

        private NaturalsContext NaturalDbContext
        {
            get 
            
            { return Context as NaturalsContext; }
        }


    }
}