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
    public class AssignDistributorToExecutiveRepository : Repository<DistributorToExecutive>, IAssignDistributorToExecutiveRepository
    {
        public AssignDistributorToExecutiveRepository(NaturalsContext context) : base(context)
        {
        }
        public async Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {
            var AssignedList = await NaturalDbContext.DistributorToExecutives.
                Include(D => D.Distributor).
                Include(D => D.Executive).Where(c => c.ExecutiveId == ExecutiveId).ToListAsync();
            var result = AssignedList.Select(c => new DistributorToExecutive
            {
                Id = c.Id,
                ExecutiveId = string.Concat(c.Executive.FirstName, "", c.Executive.LastName),
                DistributorId = string.Concat(c.Distributor.FirstName, "", c.Distributor.LastName)
            }).ToList();

            return result;
        }

        public async Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorByExecutiveId(string ExecutiveId)
        {
            return await NaturalDbContext.DistributorToExecutives.
             Where(c => c.ExecutiveId == ExecutiveId).ToListAsync();
        }

        public async Task<bool> IsExecutiveAssignedToDistributor(List<string> distributorIds)

        {
            var existingAssignment = await NaturalDbContext.DistributorToExecutives
                .AnyAsync(d => distributorIds.Contains(d.DistributorId));

            return existingAssignment;
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}
