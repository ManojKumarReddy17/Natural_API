using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Natural_Data.Repositories
{
    public class DistributorSalesRepository : Repository<DistributorSalesReport>, IDistributorSalesRepository
    {
        private readonly ILogger<DistributorSalesRepository> _logger;
        public DistributorSalesRepository(NaturalsContext context,ILogger<DistributorSalesRepository>logger) : base(context)
        {
            _logger = logger;
        }

        public DistributorSalesRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        {
            Log.Information("Starting GetById method for DistributorSalesReportInput: {DSReport}", DSReport);

            IEnumerable<DistributorSalesReport> saleReport;

            try
            {
                Log.Information("Executing SQL stored procedure with parameters: Area = {Area}, Retailor = {Retailor}, StartDate = {StartDate}, EndDate = {EndDate}",
                    DSReport.Area, DSReport.Retailor, DSReport.StartDate, DSReport.EndDate);

                saleReport = await NaturalDbContext.DistributorSalesReports
                    .FromSqlInterpolated($"CALL Naturals.AreaSales({DSReport.Area}, {DSReport.Retailor}, {DSReport.StartDate.Date}, {DSReport.EndDate.Date})")
                    .ToListAsync();

                Log.Information("Successfully executed SQL stored procedure and retrieved sales reports.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "DistributorSaleRepository" + "Error occurred while executing SQL stored procedure for DistributorSalesReportInput: {DSReport}", DSReport);
                throw;
            }

            Log.Information("Completed GetById method for DistributorSalesReportInput: {DSReport}", DSReport);
            return saleReport;
            
        }

        public async Task<string> GetRetailorNameById(string retailorId)
        {
            Log.Information("Starting GetRetailorNameById method for RetailorId: {RetailorId}", retailorId);

            string retailorName;

            try
            {
                retailorName = await NaturalDbContext.Retailors
                    .Where(r => r.Id == retailorId)
                    .Select(r => r.FirstName + " " + r.LastName)
                    .FirstOrDefaultAsync();

                if (retailorName == null)
                {
                    Log.Warning("No retailor found with RetailorId: {RetailorId}", retailorId);
                }
                else
                {
                    Log.Information("Successfully retrieved retailor name for RetailorId: {RetailorId}", retailorId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "DistributorSaleRepository" + "Error occurred while retrieving retailor name for RetailorId: {RetailorId}", retailorId);
                throw;
            }

            Log.Information("Completed GetRetailorNameById method for RetailorId: {RetailorId}", retailorId);
            return retailorName;
            
        }

        public async Task<string> GetExecutiveNameById(string executiveId)
        {
            Log.Information("Starting GetExecutiveNameById method for ExecutiveId: {ExecutiveId}", executiveId);

            string executiveName;

            try
            {
                executiveName = await NaturalDbContext.Executives
                    .Where(e => e.Id == executiveId)
                    .Select(e => e.FirstName + " " + e.LastName)
                    .FirstOrDefaultAsync();

                if (executiveName == null)
                {
                    Log.Warning("No executive found with ExecutiveId: {ExecutiveId}", executiveId);
                }
                else
                {
                    Log.Information("Successfully retrieved executive name for ExecutiveId: {ExecutiveId}", executiveId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "DistributorSaleRepository" + "Error occurred while retrieving executive name for ExecutiveId: {ExecutiveId}", executiveId);
                throw;
            }

            Log.Information("Completed GetExecutiveNameById method for ExecutiveId: {ExecutiveId}", executiveId);
            return executiveName;
            
            }

            public async Task<string> GetDistributorNameById(string distributorId)
            {
            Log.Information("Starting GetDistributorNameById method for DistributorId: {DistributorId}", distributorId);

            string distributorName;

            try
            {
                distributorName = await NaturalDbContext.Distributors
                    .Where(d => d.Id == distributorId)
                    .Select(d => d.FirstName + " " + d.LastName)
                    .FirstOrDefaultAsync();

                if (distributorName == null)
                {
                    Log.Warning("No distributor found with DistributorId: {DistributorId}", distributorId);
                }
                else
                {
                    Log.Information("Successfully retrieved distributor name for DistributorId: {DistributorId}", distributorId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,"DistributorSaleRepository"+ "Error occurred while retrieving distributor name for DistributorId: {DistributorId}", distributorId);
                throw;
            }

            Log.Information("Completed GetDistributorNameById method for DistributorId: {DistributorId}", distributorId);
            return distributorName;
           
        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
