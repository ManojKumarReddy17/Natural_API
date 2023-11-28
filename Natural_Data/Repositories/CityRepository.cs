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
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await NaturalDbContext.Cities.ToListAsync();
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
