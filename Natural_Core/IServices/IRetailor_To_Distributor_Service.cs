using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IRetailor_To_Distributor_Service
    {
        Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorId(string distributorId);
        Task<IEnumerable<RetailorToDistributor>> GetRetailorsDetailsByDistributorId(string distributorId);
        Task<ResultResponse> AssignRetailorsToDistributor(RetailorToDistributor retailorToDistributorlist);


    }


}
