using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
          private readonly ILogger<ExecutiveGpsRepository> _logger;

    
      
    

    
        public ExecutiveGpsRepository(NaturalsContext context, ILogger<ExecutiveGpsRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public ExecutiveGpsRepository(DbContext context) : base(context)
        {
        }

        public async Task<ExecutiveGp> GetByExeId(string executiveId)
        {
            try
            {


                var result = await NaturalDbContext.ExecutiveGps.FirstOrDefaultAsync(c => c.ExecutiveId == executiveId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGpsRepository-GetByExeId", ex.Message);
                return null;

            }
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
