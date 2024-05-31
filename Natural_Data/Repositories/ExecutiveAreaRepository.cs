using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;

namespace Natural_Data.Repositories
{
    public class ExecutiveAreaRepository : Repository<ExecutiveArea>, IExecutiveAreaRepository
    {
        public ExecutiveAreaRepository(NaturalsContext context) : base(context)
        {
        }

        public async Task<List<ExecutiveArea>> GetExectiveAreaByIdAsync(string id)
        {
            var exec = await NaturalDbContext.ExecutiveAreas
                           .Include(c => c.AreaNavigation)
                            .Where(g => g.Executive == id)
                            .Select(x => new ExecutiveArea
                            {
                                Id = x.Id,
                                Area = x.AreaNavigation.AreaName
                            }).ToListAsync();

            return exec;

        }


        public async Task<List<ExecutiveArea>> GetExAreaByIdAsync(string id)
        {
            var exec = await NaturalDbContext.ExecutiveAreas
                            .Where(g => g.Executive == id)
                            .ToListAsync();


            return exec;

        }


        private NaturalsContext NaturalDbContext
        {
            get
            {
                return Context as NaturalsContext;
            }
        }
    }
}

