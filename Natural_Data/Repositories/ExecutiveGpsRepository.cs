using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
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


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
