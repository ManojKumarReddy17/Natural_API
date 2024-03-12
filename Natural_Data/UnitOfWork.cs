using Amazon.S3;
using Microsoft.EntityFrameworkCore.Storage;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NaturalsContext _context;
        private readonly IAmazonS3 _S3Client;

        private ILoginRepository _loginRepository;
        private IDistributorRepository _distributorRepository;
        private ICityRepository _cityRepository;
        private IAreaRepository _areaRepository;
        private IStateRepository _stateRepository;
        private ICategoryRepository _categoryRepository;
        private IRetailorRepository _retailorRepository;
        private IExecutiveRepository _executiveRepository;
        private IProductRepository _ProductRepository;
        private IDsrRepository _dsrRepository;
        private IAssignDistributorToExecutiveRepository _dstributorToExecutiveRepository;
        
        private IAssignRetailorToDistributorRepository _retailorToDistributorRepository;
        private IDsrdetailRepository _dsrdetailRepository;
        private IDistributorSalesRepository _distributorSalesRepository;

        private INotificationRepository _NotificationRepository;
        private INotificationDistributorRepository _NotificationDistributorRepository;



        public UnitOfWork(NaturalsContext context, IAmazonS3 S3Client)
        {
            _context = context;
            _S3Client = S3Client;
        }

        public ILoginRepository Login => _loginRepository = _loginRepository ?? new LoginRepository(_context);
        public IDistributorRepository DistributorRepo => _distributorRepository = _distributorRepository ?? new DistributorRepository(_context);
        public IExecutiveRepository ExecutiveRepo => _executiveRepository = _executiveRepository ?? new ExecutiveRepository(_context);
        public ICityRepository CityRepo => _cityRepository = _cityRepository ?? new CityRepository(_context);
        public IStateRepository StateRepo => _stateRepository = _stateRepository ?? new StateRepository(_context);

        public IAreaRepository AreaRepo => _areaRepository= _areaRepository ?? new AreaRepository(_context);

        public ICategoryRepository CategoryRepo => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IRetailorRepository RetailorRepo  => _retailorRepository = _retailorRepository ?? new RetailorRepository(_context);


        public IDsrRepository dSRRepo => _dsrRepository  = _dsrRepository ?? new DsrRepository(_context);

        public IProductRepository ProductRepository => _ProductRepository = _ProductRepository ?? new ProductRepository(_context, _S3Client);

        public IAssignDistributorToExecutiveRepository distributorToExecutiveRepo => _dstributorToExecutiveRepository = _dstributorToExecutiveRepository ?? new AssignDistributorToExecutiveRepository(_context);

        public IAssignRetailorToDistributorRepository RetailorToDistributorRepositoryRepo => _retailorToDistributorRepository = _retailorToDistributorRepository ?? new AssignRetailorToDistributorRepository(_context);
        public IDsrdetailRepository DsrdetailRepository => _dsrdetailRepository = _dsrdetailRepository ?? new DsrdetailRepository(_context);

        public IDistributorSalesRepository DistributorSalesRepositoryRepo => _distributorSalesRepository = _distributorSalesRepository ?? new DistributorSalesRepository(_context);


        


        public INotificationRepository NotificationRepository => _NotificationRepository = _NotificationRepository ?? new NotificationRepository(_context);

        public INotificationDistributorRepository NotificationDistributorRepository => _NotificationDistributorRepository = _NotificationDistributorRepository ?? new NotificationDistributorRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }


        // For Transactions 

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


