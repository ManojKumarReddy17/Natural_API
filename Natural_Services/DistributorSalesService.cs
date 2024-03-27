using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Natural_Core.IServices;

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

            var result = await _unitOfWork.DistributorSalesRepositoryRepo.GetById(DSReport);

            return result;

            
        }
    }
}

