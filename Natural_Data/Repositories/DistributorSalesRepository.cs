using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            .FromSqlInterpolated($"CALL Naturals.AreaSales({DSReport.Area}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
          .ToListAsync();

            // Apply filtering based on the provided parameters
            //var filteredResults = saleReport
            //    .Where(report =>
                   
            //        (string.IsNullOrEmpty(DSReport.Area) || report.Distributor == DSReport.Area) &&
            //        (string.IsNullOrEmpty(DSReport.Retailor) || report.Retailor == DSReport.Retailor) &&
            //        (report.CreatedDate.Date >= DSReport.StartDate.Date && report.CreatedDate.Date <= DSReport.EndDate.Date)).ToList();

            return saleReport;
        }













     




        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}

