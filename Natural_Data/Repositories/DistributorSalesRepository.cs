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
        //public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        //{

        //    //// Construct the SQL query and retrieve the data
        //    var saleReport = await NaturalDbContext.DistributorSalesReports
        //        .FromSqlInterpolated($"CALL Naturals.SaleReportDS({DSReport.Executive}, {DSReport.Distributor}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
        //        .ToListAsync();

        //    // Apply filtering based on the provided parameters
        //    var filteredResults = saleReport
        //        .Where(report =>
        //            (string.IsNullOrEmpty(DSReport.Executive) || report.Executive == DSReport.Executive) &&
        //            (string.IsNullOrEmpty(DSReport.Distributor) || report.Distributor == DSReport.Distributor) &&
        //            (string.IsNullOrEmpty(DSReport.Retailor) || report.Retailor == DSReport.Retailor) &&
        //            (report.CreatedDate.Date >= DSReport.StartDate.Date && report.CreatedDate.Date <= DSReport.EndDate.Date)).ToList();

        //    return filteredResults;
        //}





        public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        {



            //// Construct the SQL query and retrieve the data
            var saleReport = await NaturalDbContext.DistributorSalesReports
                .FromSqlInterpolated($"CALL Naturals.SaleReportDS({DSReport.Executive}, {DSReport.Distributor}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
                .ToListAsync();

            // Apply filtering based on the provided parameters
            var filteredResults = saleReport
                .Where(report =>
                    (string.IsNullOrEmpty(DSReport.Executive) || report.Executive == DSReport.Executive) &&
                    (string.IsNullOrEmpty(DSReport.Distributor) || report.Distributor == DSReport.Distributor) &&
                    (string.IsNullOrEmpty(DSReport.Retailor) || report.Retailor == DSReport.Retailor) &&
                    (report.CreatedDate.Date >= DSReport.StartDate.Date && report.CreatedDate.Date <= DSReport.EndDate.Date)).ToList();

            return filteredResults;
        }













        //public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        //{
        // Check if any of the input parameters are null or empty strings
        //if (DSReport == null || (DSReport.StartDate == null && DSReport.EndDate == null))
        //{
        //    // Handle the case where only StartDate and EndDate are provided
        //    var defaultStartDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"); // Set default start date to one day before today
        //    var defaultEndDate = DateTime.Today.ToString("yyyy-MM-dd"); // Set default end date to today

        //    return await NaturalDbContext.DistributorSalesReports
        //        .FromSqlInterpolated($"CALL DSR('', '', '', {defaultStartDate}, {defaultEndDate})")
        //        .ToListAsync();
        //}


        ////Construct the SQL query and retrieve the data
        //var saleReport = await NaturalDbContext.DistributorSalesReports

        //    .FromSqlInterpolated($"CALL Naturals.DSR({DSReport.Executive}, {DSReport.Distributor}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
        //    .ToListAsync();

        ////Apply filtering based on the provided parameters
        //var filteredResults = saleReport.Where(report =>
        //    (string.IsNullOrEmpty(DSReport.Executive) || report.Executive == DSReport.Executive) &&
        //    (string.IsNullOrEmpty(DSReport.Distributor) || report.Distributor == DSReport.Distributor) &&
        //    (string.IsNullOrEmpty(DSReport.Retailor) || report.Retailor == DSReport.Retailor) &&
        // report.CreatedDate.Date >= DSReport.StartDate.Date && report.CreatedDate.Date <= DSReport.EndDate.Date);


        //return filteredResults;


        //}




        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }

    }
}

