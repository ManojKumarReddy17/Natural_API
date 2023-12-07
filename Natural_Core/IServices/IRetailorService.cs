using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IRetailorService
    {
        Task<IEnumerable<Retailor>> GetAllRetailors();
        Task<Retailor> GetRetailorById(string distributorId);
        Task<RetailorResponce> CreateRetailorWithAssociationsAsync(Retailor distributor,
        string areaId, string cityId, string stateId);

<<<<<<< HEAD
        Task<RetailorResponce> DeleteRetailor(string retailorId);
        Task<Retailor> GetRetailorsById(string retailorId);


=======
>>>>>>> 7677d32a2ec34e478c20486a57ff8f5d9b7d2917

    }
}
