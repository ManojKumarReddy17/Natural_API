﻿using Microsoft.EntityFrameworkCore;
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
    public class DistributorRepository : Repository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(NaturalsContext context) : base(context)
        {

        }
        public async Task<List<Distributor>> GetAllDistributorstAsync()
        {

            var distributors = await NaturalDbContext.Distributors
            .Include(c => c.AreaNavigation)
             .ThenInclude(a => a.City)
            .ThenInclude(ct => ct.State)
             .ToListAsync();

            var result = distributors.Select(c => new Distributor
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Area = c.AreaNavigation.AreaName,
                Email = c.Email,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName
            }).ToList();

            return result;
        }

        public async ValueTask<Distributor> GetWithDistributorsByIdAsync(string distributorid)
        {
            var distributors = await NaturalDbContext.Distributors
                       .Include(c => c.AreaNavigation)
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == distributorid);

            if (distributors != null)
            {
                var result = new Distributor
                {
                    Id = distributors.Id,
                    FirstName = distributors.FirstName,
                    LastName = distributors.LastName,
                    MobileNumber = distributors.MobileNumber,
                    Address = distributors.Address,
                    Area = distributors.AreaNavigation.AreaName,
                    Email = distributors.Email,
                    City = distributors.AreaNavigation.City.CityName,
                    State = distributors.AreaNavigation.City.State.StateName
                };

                return result;

            }
            else
            {
                    return null;
            }
            
        }
    

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }


    }
}