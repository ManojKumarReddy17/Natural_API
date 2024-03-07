using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IDistributorSalesRepository : IRepository<DistributorSalesReport>
    {
        Task<IEnumerable<DistributorSalesReport>> GetById(DistributorSalesReportInput DSReport);
    }
}
