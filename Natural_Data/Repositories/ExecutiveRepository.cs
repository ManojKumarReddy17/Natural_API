using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;

namespace Natural_Data.Repositories
namespace Natural_Data.Repositories
{
    public class ExecutiveRepository : Repository<Executive>, IExecutiveRepository
    {
        public ExecutiveRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Executive>> GetAllExecutiveAsync()
        {
            var executive = await (from executives in NaturalDBContext.Executives
                                   join area in NaturalDBContext.Areas on executives.Area equals area.Id
                                   join city in NaturalDBContext.Cities on area.CityId equals city.Id
                                   join state in NaturalDBContext.States on city.StateId equals state.Id
                                   select new
                                   {
                                       executivee = executives,
                                       Area = area,
                                       City = city,
                                       State = state
                                   }).ToListAsync();

            var result = executive.Select(c => new Executive
            {

                FirstName = c.executive.FirstName,
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == execid);

                FirstName = c.executive.FirstName,
                LastName = c.executive.LastName,
                MobileNumber = c.executive.MobileNumber,
                Address = c.executive.Address,
                Area = c.Area.AreaName,
                Email = c.executive.Email,
                UserName= c.executivee.UserName,
                Password= c.executivee.Password,
                City = c.City.CityName,
                State = c.State.StateName,
           
            });

        public async Task<List<Executive>> GetAllExectivesAsync()


            else
            {
                return null;
            }
        }

        public Task<List<Executive>> GetAll()
        {
            throw new NotImplementedException();
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
   }    }

}
