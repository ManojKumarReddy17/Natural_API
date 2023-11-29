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

        Task<Distributor> GetDistributorByonlId(string Id);
        Task<DistributorResponse> CreateDistributorWithAssociationsAsync(Distributor distributor,
        string areaId, string cityId, string stateId);

    }
}
