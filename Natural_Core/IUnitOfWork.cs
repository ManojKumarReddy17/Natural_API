using Microsoft.EntityFrameworkCore.Storage;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

#nullable disable


namespace Natural_Core
{
    public interface IUnitOfWork : IDisposable
    {
        ILoginRepository Login { get; }


        IDistributorRepository DistributorRepo { get; }
        ICityRepository CityRepo { get; }
        IStateRepository StateRepo { get; }
        IAreaRepository AreaRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        IRetailorRepository  RetailorRepo { get; }
        IDsrRepository dSRRepo { get; }
        IExecutiveRepository ExecutiveRepo { get; }
        IProductRepository ProductRepository { get; }
        IAssignDistributorToExecutiveRepository distributorToExecutiveRepo { get; }
        IAssignRetailorToDistributorRepository RetailorToDistributorRepositoryRepo { get; }
        IDsrdetailRepository DsrdetailRepository { get; }
        INotificationRepository NotificationRepository { get; }
        INotificationDistributorRepository NotificationDistributorRepository { get; }

        Task<int> CommitAsync();

        IDbContextTransaction BeginTransaction(); // added for transaction
        IDistributorSalesRepository DistributorSalesRepositoryRepo { get; }
        IExecutiveGpsRepository executiveGpsRepo { get; }
    }
}
