
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IAssignDistributorToExecutiveService
    {
        Task<IEnumerable<DistributorToExecutive>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId);

        Task<IEnumerable<DistributorToExecutive>> AssignedDistributorsByExecutiveId(string ExecutiveId);

        Task<ResultResponse> AssignDistributorsToExecutive(DistributorToExecutive model);


    }
}
