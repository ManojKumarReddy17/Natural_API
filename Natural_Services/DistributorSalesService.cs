using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Natural_Core.IServices;
using System.Composition;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natural_Services
{
    public class DistributorSalesService : IDistributorSalesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DistributorSalesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport)
        {

           
                var salesReports = await _unitOfWork.DistributorSalesRepositoryRepo.GetById(DSReport);

                foreach (var report in salesReports)
                {

                    report.Area = report.Area.ToString();
                    report.Retailor = await _unitOfWork.DistributorSalesRepositoryRepo.GetRetailorNameById(report.Retailor);
                    report.Executive = await _unitOfWork.DistributorSalesRepositoryRepo.GetExecutiveNameById(report.Executive);
                    report.Distributor = await _unitOfWork.DistributorSalesRepositoryRepo.GetDistributorNameById(report.Distributor);
                }
                return salesReports;
            

        }
            public async Task<IEnumerable<DistributorShopwiseResult>> GetDistributorShopwiseDetails(DistributorShopwiseReport DSReport)
            { 
               var salesReports = await _unitOfWork.DistributorSalesRepositoryRepo.GetShopWiseDetails(DSReport);
                foreach (var report in salesReports)
                {
                    report.FirstName = report.FirstName.ToString();
                    report.TotalSaleAmount = report.TotalSaleAmount;
                }
                return salesReports;
            }
        

    }
}

