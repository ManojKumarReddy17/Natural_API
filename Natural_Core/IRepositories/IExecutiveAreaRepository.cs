using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;

namespace Natural_Core.IRepositories
{
    public interface IExecutiveAreaRepository : IRepository<ExecutiveArea>
    {
        Task<List<ExecutiveArea>> GetExectiveAreaByIdAsync(string id);

        Task<List<ExecutiveArea>> GetExAreaByIdAsync(string id);

    }
}
