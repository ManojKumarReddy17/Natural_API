using AutoMapper.Mappers;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IServices
{
    public interface IDistributorService
    {
        Task<IEnumerable<Distributor>> GetAllDistributors();
        Task<Distributor> GetDistributorById(string distributorId);
        Task<Distributor> GetDistributorDetailsById(string distributorId);
        Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor);

        Task<ResultResponse> DeleteDistributor(string distributorId);
        Task<ResultResponse> UpdateDistributor(Distributor distributor);
        Task<IEnumerable<Distributor>> SearcDistributors(SearchModel search);

        Task<IEnumerable<Distributor>> GetNonAssignedDistributors();


    }
}
