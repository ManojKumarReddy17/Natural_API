using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data.Repositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            return await NaturalDbContext.Areas.ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
