using Microsoft.EntityFrameworkCore;

using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Data.Models;
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
            return await NaturalDbContext.Areas.Where(m => m.CityId == CityId && m.IsDeleted != true).ToListAsync();

        }


        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
        public async Task<Area> GetAreasId(string AreaId)
        {
            var result = await NaturalDbContext.Areas.Where(m => m.Id == AreaId && m.IsDeleted != true).FirstOrDefaultAsync();
            return result;
        }
    }
}
