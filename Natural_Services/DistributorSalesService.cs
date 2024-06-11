using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Natural_Core.IServices;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Natural_Services
{
    public class DistributorSalesService : IDistributorSalesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DistributorSalesService> _logger;

        public DistributorSalesService(IUnitOfWork unitOfWork,ILogger<DistributorSalesService>logger)
        {
            _unitOfWork = unitOfWork;
            _logger=logger;
        }
        public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        {
            Log.Information("Starting GetById method for DistributorSalesReportInput: {DSReport}", DSReport);

            IEnumerable<DistributorSalesReport> salesReports;

            try
            {
                salesReports = await _unitOfWork.DistributorSalesRepositoryRepo.GetById(DSReport);
                Log.Information("Retrieved sales reports from repository for DistributorSalesReportInput: {DSReport}", DSReport);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving sales reports for DistributorSalesReportInput: {DSReport}", DSReport);
                throw;
            }

            foreach (var report in salesReports)
            {
                try
                {
                    report.Retailor = await _unitOfWork.DistributorSalesRepositoryRepo.GetRetailorNameById(report.Retailor);
                    Log.Information("Retrieved Retailor name for report ID: {ReportId}", report.Retailor);

                    report.Executive = await _unitOfWork.DistributorSalesRepositoryRepo.GetExecutiveNameById(report.Executive);
                    Log.Information("Retrieved Executive name for report ID: {ReportId}", report.Executive);

                    report.Distributor = await _unitOfWork.DistributorSalesRepositoryRepo.GetDistributorNameById(report.Distributor);
                    Log.Information("Retrieved Distributor name for report ID: {ReportId}", report.Distributor);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message,"DistributorSalesService"+ "Error occurred while retrieving names for report ID: {ReportId}", report);
                    throw;
                }
            }

            Log.Information("Completed GetById method for DistributorSalesReportInput: {DSReport}", DSReport);
            return salesReports;
            
        }

    }
}

