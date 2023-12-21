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
        Task<Retailor> GetRetailorDetailsById(string distributorId);
        Task<ResultRepsonse> CreateRetailorWithAssociationsAsync(Retailor distributor);
        Task<ResultRepsonse> UpdateRetailors(Retailor existingRetailor, Retailor retailor);

        Task<ResultRepsonse> DeleteRetailor(string retailorId);
        Task<Retailor> GetRetailorsById(string retailorId);
        Task<IEnumerable<Retailor>> SearcRetailors(SearchModel search);




    }
}
