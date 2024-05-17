using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class ExecutiveGpsRepository : Repository<ExecutiveGp>, IExecutiveGpsRepository
    {
        public ExecutiveGpsRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<ExecutiveGp> GetByExeId(string executiveId)
        {
            var result = await NaturalDbContext.ExecutiveGps.FirstOrDefaultAsync(c => c.ExecutiveId == executiveId);
            return result;
        }
        //public async Task<IEnumerable<ExecutiveGp>> GetExecutiveByGpsIdAsync(string GpsId)
        //{

        //    var executives = await NaturalDbContext.ExecutiveGps
        //           .Include(c => c.ExecutiveNavi)
        //           .Where(c => c.ExecutiveId == GpsId)
        //           .Select(c => new ExecutiveGp
        //           {
        //               Executive = c.ExecutiveNavi.FirstName + " " + c.ExecutiveNavi.LastName,
        //               Id = c.Id
        //           }).ToListAsync();
        //    return executives;

        //}

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
