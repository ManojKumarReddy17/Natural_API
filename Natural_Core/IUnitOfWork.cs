using Natural_Core.IRepositories;
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
        IAssignDistributorToExecutiveRepository distributorToExecutiveRepo { get; }
        IRetailor_To_Distributor_Repository Retailor_To_Distributor_RepositoryRepo { get; }

        Task<int> CommitAsync();


    }
}
