using AutoMapper;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3Models;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Natural_Services
{

    public class DsrService : IDsrService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssignDistributorToExecutiveService _distributorToExecutiveService;
        private readonly IDsrdetailService _DsrdetailService;
        private readonly IMapper _mapper;

        public DsrService(IUnitOfWork unitOfWork, IAssignDistributorToExecutiveService distributorToExecutiveService, IDsrdetailService DsrdetailService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _distributorToExecutiveService = distributorToExecutiveService;
            _DsrdetailService = DsrdetailService;
            _mapper = mapper;
        }


        public async Task<ProductResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    dsr.Id = "DSR" + new Random().Next(10000, 99999).ToString();
                    dsr.ModifiedDate = dsr.CreatedDate;

                    await _unitOfWork.dSRRepo.AddAsync(dsr);



                    var commit = await _unitOfWork.CommitAsync();

                    var create = dsrdetails.Select(c => new Dsrdetail
                    {

                        Product = c.Product,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        Dsr = dsr.Id

                    }).ToList();

                    await _unitOfWork.DsrdetailRepository.AddRangeAsync(create);

                    var commit1 = await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = " Dsr and DsrdetailInsertion Successful";
                    response.StatusCode = 200;
                    response.Id = dsr.Id;


                }
                catch (Exception)
                {
                    transaction.Rollback();
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }

        }

        public async Task<IEnumerable<Dsr>> GetAllDsr(EdittDSR? search)
        {
            var result = await _unitOfWork.dSRRepo.GetAllDsrAsync(search);
            return result;
        }

        public async Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId)

        {

            var dsrdetails = await _unitOfWork.DsrdetailRepository.GetDsrDetailsByDsrIdAsync(dsrId);
            return dsrdetails;

        }



        public async Task<DsrResponse> DeleteDsr(Dsr dsr, List<Dsrdetail> dsrdetails, string dsrId)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new DsrResponse();

                try
                {

                    _unitOfWork.DsrdetailRepository.RemoveRange(dsrdetails);
                    var commit = await _unitOfWork.CommitAsync();
                    _unitOfWork.dSRRepo.Remove(dsr);
                    var commit1 = await _unitOfWork.CommitAsync();

                    transaction.Commit();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "delete Failed";
                    response.StatusCode = 401;
                }

                return response;
            }
        }



        public async Task<IEnumerable<DsrDistributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {

            var result = await _unitOfWork.dSRRepo.GetAssignedDistributorDetailsByExecutiveId(ExecutiveId);

            return result;


        }

        public async Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId)
        {

            var result = await _unitOfWork.dSRRepo.GetAssignedRetailorDetailsByDistributorId(DistributorId);

            return result;


        }


        public async Task<Dsr> GetDsrbyId(string dsrid)
        {
            var dsr = await _unitOfWork.dSRRepo.GetDsrbyId(dsrid);
            return dsr;
        }
        
        public async Task<IEnumerable<Dsr>> getRetailorListByDistributorId(string distributorId)
        {
            var retailorList = await _unitOfWork.dSRRepo.GetRetailorDetailsByDistributorId(distributorId);
            return retailorList;
        }
       

        public  async Task<IEnumerable<Dsr>> getRetailorListByExecutiveId(string executiveId)
        {
            var retailorList = await _unitOfWork.dSRRepo.GetRetailorDetailsByExecutiveId(executiveId);
            return retailorList;
        }
        public async Task<IEnumerable<Dsr>> GetRetailorListByDate(string Id, DateTime date)
        {
            return await _unitOfWork.dSRRepo.GetRetailorDetailsByDate(Id, date);
        }

        public async Task<Dsr> GetbyId(string dsrid)
        {
            var dsr = await _unitOfWork.dSRRepo.GetByIdAsync(dsrid);
            return dsr;
        }

        //dsrdetails as in table ids
        public async Task<IEnumerable<Dsrdetail>> GetDetailTableByDsrIdAsync(string dsrId)

        {
            var dsedetail = await _unitOfWork.DsrdetailRepository.GetDetailTableByDsrIdAsync(dsrId);
            return dsedetail;
        }

        public async Task<IEnumerable<GetProduct>> GetDetTableByDsrIdAsync(string dsrId)


        {
            var dsedetail = await _unitOfWork.DsrdetailRepository.GetDetailTableAsync(dsrId);
            return dsedetail;
        }

        public async Task<ProductResponse> UpdateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails)
        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {

                    var updatedsr = await _unitOfWork.dSRRepo.GetByIdAsync(dsr.Id);

                    updatedsr.TotalAmount = dsr.TotalAmount;
                    updatedsr.OrderBy = dsr.OrderBy;
                    updatedsr.Area = dsr.Area;
                    updatedsr.Executive = dsr.Executive;
                    updatedsr.Distributor=dsr.Distributor;
                    updatedsr.Retailor = dsr.Retailor;
                    updatedsr.CreatedDate = dsr.CreatedDate;
                    updatedsr.ModifiedDate = dsr.CreatedDate;
                    

                    _unitOfWork.dSRRepo.Update(updatedsr);


                    var commit = await _unitOfWork.CommitAsync();

                    var create = dsrdetails.Select(c => new Dsrdetail
                    {

                        Product = c.Product,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        
                        Dsr = dsr.Id

                    }).ToList();


             var deleteproducts = create.Where(s => s.Quantity == 0)
                              .Select(s => new Dsrdetail
                               {
                               Product = s.Product,
                               Quantity = s.Quantity,
                               Price = s.Price,
                                  Dsr =s.Dsr
                              })
                               .ToList();

                  var resultODelete = await GetDetailTableByDsrIdAsync(dsr.Id);

                  var  resulttodELETE = resultODelete
                            .Join(deleteproducts,
                             s1 => s1.Dsr,
                             s2 => s2.Dsr,
                            (s1, s2) => new { s1, s2 })
                            .OrderBy(c => c.s1.Product)
                           .Where(c => c.s1.Product == c.s2.Product)
                           .Select(c => new Dsrdetail
                           {
                               Id = c.s1.Id,
                               Product = c.s1.Product,
                               Quantity = c.s2.Quantity,
                               Price = c.s2.Price != null ? c.s2.Price : 0,
                               Dsr = c.s1.Dsr
                           })
                          .ToList();

                    resultODelete = resultODelete
.Join(resulttodELETE,
od => od.Id,
pd => pd.Id,
(od, pd) => {
 od.Quantity = pd != null ? pd.Quantity : od.Id; // Update Quantity
 od.Price = pd != null ? pd.Price : pd.Id;       // Update Price
 return od;
})
.ToList();

                    _unitOfWork.DsrdetailRepository.RemoveRange(resultODelete);
                    var deted = await _unitOfWork.CommitAsync();



                    List<Dsrdetail> result=  new List<Dsrdetail>();




                    var result1 = await GetDetailTableByDsrIdAsync(dsr.Id);
   
                    result = result1
                              .Join(create,
                               s1 => s1.Dsr,
                               s2 => s2.Dsr,
                              (s1, s2) => new { s1, s2 })
                              .OrderBy(c => c.s1.Product)
                             .Where(c => c.s1.Product == c.s2.Product)
                             .Select(c => new Dsrdetail
                             {
                              Id = c.s1.Id ,
                              Product = c.s1.Product,
                              Quantity = c.s2.Quantity,
                              Price = c.s2.Price != null ? c.s2.Price : 0,
                              Dsr = c.s1.Dsr
                              })
                            .ToList();


                    result1 = result1
    .Join(result,
        od => od.Id,
        pd => pd.Id,
        (od, pd) => {
            od.Quantity = pd != null ? pd.Quantity : od.Id; // Update Quantity
            od.Price = pd != null ? pd.Price : pd.Id;       // Update Price
            return od;
        })
    .ToList();

                    _unitOfWork.DsrdetailRepository.UpdateRange(result1);
                    
                    var updated = await _unitOfWork.CommitAsync();


                    var inserdsr = create
    .Where(s2 => !result1.Any(s1 => s1.Product == s2.Product))
    .Where(s2 => s2.Quantity != 0)
    .OrderBy(s2 => s2.Product)
    .Select(s2 => new Dsrdetail
    {
        Product = s2.Product,
        Quantity = s2.Quantity,
        Price = s2.Price,
        Dsr = s2.Dsr
    })
    .ToList();

                    await _unitOfWork.DsrdetailRepository.AddRangeAsync(inserdsr);

                    var created = await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = " Dsr and DsrdetailInsertion Successful";
                    response.StatusCode = 200;
                    response.Id = dsr.Id;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }


        }


        public async Task<IEnumerable<DSRretailorDetails>> GetDetailsByIdAsync(string Id)

        {
            var dsrRetilordetails = await _unitOfWork.dSRRepo.GetRetailorDetailsbyId(Id);


            return dsrRetilordetails;

        }
        public async Task<IEnumerable<DSRretailorDetails>> GetDetailsByIdAsyncdis(string DistributorId)

        {
            var dsrRetilordetails = await _unitOfWork.dSRRepo.GetRetailorDetailsbyDistributorId(DistributorId);


            return dsrRetilordetails;

        }

    }
}
