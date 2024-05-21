﻿using Microsoft.EntityFrameworkCore;
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
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await NaturalDbContext.Cities.ToListAsync();
        }

        public async Task<City> GetCityWithId(string CityId)
        {
            return await NaturalDbContext.Cities.Where(m => m.Id == CityId && m.IsDeleted == false).FirstOrDefaultAsync();
        }

        

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
