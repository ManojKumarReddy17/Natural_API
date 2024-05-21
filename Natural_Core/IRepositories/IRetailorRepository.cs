using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IRetailorRepository : IRepository<Retailor>
    {

        Task<IEnumerable<Retailor>> GetAllRetailorsAsync(SearchModel? search, string? NonAssign);
        Task<GetRetailor> GetRetailorDetailsByIdAsync(string id);
        Task UpdateRetailorAsync(Retailor retailor, Retailor existingRetailor);
        Task<IEnumerable<Retailor>> SearchRetailorAsync(SearchModel search);
        Task<IEnumerable<Retailor>> GetNonAssignedRetailorsAsync();
        Task<IEnumerable<Retailor>> SearchNonAssignedRetailorsAsync(SearchModel search);
    }
}