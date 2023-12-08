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
        Task<DistributorResponse> CreateDistributorWithAssociationsAsync(Distributor distributor);

        Task<DistributorResponse> DeleteDistributor(string distributorId);
    }
}
