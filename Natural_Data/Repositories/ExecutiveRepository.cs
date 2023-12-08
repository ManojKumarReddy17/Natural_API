using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class ExecutiveRepository : Repository<Executive>, IExecutiveRepository
    {
        public ExecutiveRepository(NaturalsContext context) : base(context)
        {
        }

        public async Task<List<Executive>> GetAllExectivesAsync()
        {
            var exec = await NaturalDBContext.Executives
            .Include(c => c.AreaNavigation)
             .ThenInclude(a => a.City)
            .ThenInclude(ct => ct.State)
             .ToListAsync();
            
            var result = exec.Select(c => new Executive
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Area = c.AreaNavigation.AreaName,
                Email = c.Email,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName
            }).ToList();

            return result;
        }

        public async Task<Executive> GetWithExectiveByIdAsync(string execid)
        {
            var exec = await NaturalDBContext.Executives
                       .Include(c => c.AreaNavigation)
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == execid);

            if (exec != null)
            {
                var result = new Executive
                {
                    Id = exec.Id,
                    FirstName = exec.FirstName,
                    LastName = exec.LastName,
                    MobileNumber = exec.MobileNumber,
                    Address = exec.Address,
                    Area = exec.AreaNavigation.AreaName,
                    Email = exec.Email,
                    City = exec.AreaNavigation.City.CityName,
                    State = exec.AreaNavigation.City.State.StateName
                };

                return result;

            }
            else
            {
                return null;
            }
        }

        public Task<List<Executive>> GetAll()
        {
            throw new NotImplementedException();
        }

      

        private NaturalsContext NaturalDBContext
        {
            get { return Context as NaturalsContext; }
   }    }
}
