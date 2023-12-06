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
        Task<DistributorResponse> CreateDistributorWithAssociationsAsync(Distributor distributor);
        //Task DeleteDistributor(string distributorId);

        Task<DistributorResponse> DeleteDistributor(string distributorId);
    }
}
