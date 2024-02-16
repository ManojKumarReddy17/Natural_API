using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IAssignRetailorToDistributorService
    {
        Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorId(string distributorId);
        Task<IEnumerable<Retailor>> GetRetailorsDetailsByDistributorId(string distributorId);
        Task<ResultResponse> AssignRetailorsToDistributor(RetailorToDistributor retailorToDistributorlist);
        Task<ResultResponse> DeleteAssignedRetailotorByid(string retailorId, string distributorId);


    }


}
