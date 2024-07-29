using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class DistributorSalesRepository : Repository<DistributorSalesReport>, IDistributorSalesRepository
    {
        public DistributorSalesRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        {
            var saleReport = await NaturalDbContext.DistributorSalesReports
                .FromSqlInterpolated($"CALL Naturals.AreaSales2({DSReport.Area},{DSReport.Executive},{DSReport.Distributor}, {DSReport.Retailor}, {DSReport.StartDate}, {DSReport.EndDate})")
                .ToListAsync();

            return saleReport;
        }

        public async Task<string> GetRetailorNameById(string retailorId)
        {
            return await NaturalDbContext.Retailors
                .Where(r => r.Id == retailorId)
        .Select(r => r.FirstName + " " + r.LastName)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetExecutiveNameById(string executiveId)
        {
            return await NaturalDbContext.Executives
                .Where(e => e.Id == executiveId)
        .Select(r => r.FirstName + " " + r.LastName)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetDistributorNameById(string distributorId)
        {
            return await NaturalDbContext.Distributors
                .Where(d => d.Id == distributorId)
        .Select(r => r.FirstName + " " + r.LastName)
                .FirstOrDefaultAsync();
        }




        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
