using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IRetailorService
    {
        Task<GetRetailor> GetRetailorDetailsById(string distributorId);
        Task<ResultResponse> CreateRetailorWithAssociationsAsync(Retailor distributor);
        Task<ResultResponse> UpdateRetailors(Retailor existingRetailor, Retailor retailor);

        Task<Retailor> GetRetailorsById(string retailorId);

        Task<ResultResponse> SoftDelete(string retailorId);

        Task<IEnumerable<GetRetailor>> GetAllRetailorDetailsAsync(SearchModel? search, bool? NonAssign, string? prefix);
        Task<IEnumerable<RetailorDetailsByArea>> GetRetailordetailsByAreaId(string areaId);

    }
}
