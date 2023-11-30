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
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            return await NaturalDbContext.Areas.ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAreasWithCityID(string CityId)
        {
            return await NaturalDbContext.Areas.Where(m => m.CityId == CityId).ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
