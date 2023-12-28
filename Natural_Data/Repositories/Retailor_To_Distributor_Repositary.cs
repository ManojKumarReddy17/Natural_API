using Microsoft.EntityFrameworkCore;
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
    public class Retailor_To_Distributor_Repository : Repository<RetailorToDistributor>, IRetailor_To_Distributor_Repository
    {
        public Retailor_To_Distributor_Repository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorIdAsync(string distributorId)
        {
            return await Context.Set<RetailorToDistributor>()
                .Where(rt => rt.DistributorId == distributorId)
                .ToListAsync();
        }
        public async Task<IEnumerable<RetailorToDistributor>> GetAssignedRetailorDetailsByDistributorIdAsync(string distributorId)
        {
            var AssignedList = await NaturalDbContext.RetailorToDistributors.
                Include(D => D.Retailor).
                Include(D => D.Distributor).Where(rt=> rt.DistributorId == distributorId).ToListAsync();

            var result = AssignedList.Select(rt => new RetailorToDistributor
            {
                Id = rt.Id,
                DistributorId = string.Concat(rt.Distributor.FirstName, "", rt.Distributor.LastName),
                RetailorId = string.Concat(rt.Retailor.FirstName, "", rt.Retailor.LastName)
            }).ToList();
            return result;
        }

        public async Task<bool> DistributorAssignedToRetailor(List<string> retailorid)
        {
            var existingAssignment = await NaturalDbContext.RetailorToDistributors
                           .AnyAsync(d => retailorid.Contains(d.RetailorId));

            return existingAssignment;
        }

        private NaturalsContext  NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }


}
