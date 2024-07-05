using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class DistributorLoginReportsService : IDistributorLoginReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DistributorLoginReportsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DistributorReport>> getbyId(DistributorLoginReports distributorLogin)
        {
            var salereport =  await _unitOfWork.DistributorLoginReportsRepo.GetById(distributorLogin);
            return salereport;
        }
    }
}
