using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IDistributorLoginReportsRepository : IRepository<DistributorReport>
    {
        Task<IEnumerable<DistributorReport>> GetById(DistributorLoginReports reports);
    }
}
