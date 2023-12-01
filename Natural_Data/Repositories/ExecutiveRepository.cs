using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
=======
using Natural_Core;
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Text;
using System.Threading.Tasks;
using System.Linq;
=======
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa

namespace Natural_Data.Repositories
{
    public class ExecutiveRepository : Repository<Executive>, IExecutiveRepository
    {
        public ExecutiveRepository(NaturalsContext context) : base(context)
        {
        }

<<<<<<< HEAD
        public async Task<IEnumerable<Executive>> GetAllExecutiveAsync()
        {
            var executive = await(from executives in NaturalDbContext.Executives
                                  join area in NaturalDbContext.Areas on executives.Area equals area.Id
                                  join city in NaturalDbContext.Cities on area.CityId equals city.Id
                                  join state in NaturalDbContext.States on city.StateId equals state.Id
                                  select new
                                  {
                                      executive = executives,
                                      Area = area,
                                      City = city,
                                      State = state
                                  }).ToListAsync();
            var result = executive.Select(c => new Executive
            {

                FirstName = c.executive.FirstName,
                LastName = c.executive.LastName,
                MobileNumber = c.executive.MobileNumber,
                Address = c.executive.Address,
                Area = c.Area.AreaName,
                Email = c.executive.Email,
                City = c.City.CityName,
                State = c.State.StateName,
            });
=======
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
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa

            return result;
        }

<<<<<<< HEAD
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
=======
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

        private NaturalsContext NaturalDBContext
        {
            get { return Context as NaturalsContext; }
   }    }
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
}
