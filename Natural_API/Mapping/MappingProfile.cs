using System.Collections.Generic;
using AutoMapper;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.Models;
using Natural_Core.S3_Models;

#nullable disable
namespace Natural_API.Mapping
{
    public class MappingProfile : Profile
    {
      
            public MappingProfile()
            {
                // DOMAIN TO RESOURCE 

             CreateMap<Login, LoginResource>();
             CreateMap<Distributor, DistributorGetResource>();
             CreateMap<Executive, AngularLoginResourse>();
             CreateMap<Distributor, AngularLoginResourse>();
             CreateMap<Distributor, DistributorGetResource>();
             CreateMap<Distributor, InsertUpdateResource>();
             CreateMap<State, StateResource>();
             CreateMap<Area, AreaResource>();
             CreateMap<City, CityResource>();
             CreateMap<Category, CategoryResource>();
             CreateMap<Retailor , RetailorResource>();
             CreateMap<Retailor, RetailorPostResource>();
             CreateMap <Executive, ExecutiveGetResource>();
             CreateMap<Executive, InsertUpdateResource>();
             CreateMap<Category , CategoryInsertResource>();
             CreateMap<Dsr,DsrResource>();
             CreateMap<Dsr, DsrPostResource>();
             CreateMap<Dsrdetail, DsrDetailResource>();
             CreateMap<InsertDEmapper, DistributorToExecutive>();
             CreateMap<Distributor , DistributorToExecutiveResource>();
             CreateMap<Dsrdetail,DsrDetailPostResource>();
             CreateMap<Retailor, RetailorToDistributorResource>();
             CreateMap<RetailorToDistributor, AssignRetailorToDistributorResource>();
             CreateMap<RetailorToDistributor,InsertRTDResource>();
             CreateMap<Product,DsrProductResource>();
             CreateMap<Dsr,DsrDetailsByIdResource>();
             CreateMap<Product, ProductResource>();
             CreateMap<GetProduct, ProductResource>();
             CreateMap<DsrDistributor, DsrDistributorResource>();
             CreateMap<DsrRetailor, DsrRetailorResource>();
             CreateMap<Dsr,DsrInsertResource>();
             CreateMap<DsrProduct, DsrdetailProduct>();
             CreateMap<DsrProduct, Dsrdetail>();
             CreateMap<Dsr, DsrEditResource>();
             CreateMap<GetProduct, DsrProductResource>();
             CreateMap<DistributorSalesReport, DistributorSalesReportResuorce>();
             CreateMap<RetailorToDistributor, RetailorToDistributorResource>();
             CreateMap<DistributorToExecutive, DistributorToExecutiveResource>();
             CreateMap<Dsr,DsrResource>();
             CreateMap<Dsr, DsrPostResource>();
             CreateMap<Dsrdetail, DsrDetailResource>();
             CreateMap<InsertDEmapper, DistributorToExecutive>();
             CreateMap<Distributor , DistributorToExecutiveResource>();
             CreateMap<Dsrdetail,DsrDetailPostResource>();
             CreateMap<Retailor, RetailorToDistributorResource>();
             CreateMap<RetailorToDistributor, AssignRetailorToDistributorResource>();
             CreateMap<RetailorToDistributor,InsertRTDResource>();
             CreateMap<Product,DsrProductResource>();
             CreateMap<Dsr,DsrDetailsByIdResource>();
             CreateMap<Product, ProductResource>();
             CreateMap<GetProduct, ProductResource>();
             CreateMap<DsrDistributor, DsrDistributorResource>();
             CreateMap<DsrRetailor, DsrRetailorResource>();
             CreateMap<Dsr,DsrInsertResource>();
             CreateMap<Dsrdetail,DsrdetailProduct>();
             CreateMap<Dsr, DSRRetailorsListResource>();
             CreateMap< Notification, NotificationResource>();
             CreateMap<NotificationDistributor, NotificationDistributorResource>();
            CreateMap<DSRretailorDetails, DSRretailorDetailsResources>();



            //// RESOURCE TO DOMAIN

            CreateMap<LoginResource, Login>();
                CreateMap<DistributorGetResource, Distributor>();
                CreateMap<InsertUpdateResource, Distributor>();
                CreateMap<StateResource, State>();
                CreateMap<AreaResource, Area>();
                CreateMap<CityResource, City>();
                CreateMap<CategoryResource,Category>();
                CreateMap<RetailorPostResource, Retailor>();
                CreateMap<ExecutiveGetResource, Executive>();   
                CreateMap<InsertUpdateResource, Executive>();
                CreateMap<ExecutiveGetResource, Executive>();

                CreateMap<CategoryInsertResource, Category>();
                CreateMap<DsrResource, Dsr>();
                CreateMap<DsrPostResource, Dsr>();
            CreateMap<DsrDetailResource,Dsrdetail>();
            CreateMap<DsrDetailPostResource, Dsrdetail>();
            CreateMap<DistributorToExecutive, InsertDEmapper>();
            CreateMap<DistributorToExecutiveResource, Distributor>();
                CreateMap<DistributorToExecutive, InsertDEmapper>();
                CreateMap<RetailorToDistributorResource, Retailor>();
                CreateMap<AssignRetailorToDistributorResource, RetailorToDistributor>();
            CreateMap<InsertRTDResource, RetailorToDistributor>();
            CreateMap<DsrProductResource,Product>();
            CreateMap<DsrDetailsByIdResource, Dsr>();
            CreateMap<ProductResource, Product>();
            CreateMap<Product, GetProduct>();

            CreateMap<Executive, GetExecutive>();
            CreateMap<Executive, ExecutiveGetResource>();

            CreateMap<DsrdetailProduct, Dsrdetail>();
            CreateMap<DsrInsertResource, Dsr>();
            CreateMap<DsrDetailsByIdResource, Dsr>().ForMember(c=>c.CreatedDate,(obj)=>obj.MapFrom(s=>s.StartDate))
                                                    .ForMember(c=>c.ModifiedDate,(obj)=>obj.MapFrom(s=>s.EndDate));
            

             //CreateMap<DsrDetailsByIdResource, Dsr>().ForMember(c => c.CreatedDate, (obj) => obj.MapFrom(s => s.createdDate));
             //.ForMember(c=>c.ModifiedDate,(obj)=>obj.MapFrom(s=>s.EndDate));

             CreateMap<AngularLoginResourse,Executive>();
             CreateMap<AngularLoginResourse,Distributor>();
             CreateMap<DSRRetailorsListResource, Dsr>();
             CreateMap<NotificationResource, Notification>();
             CreateMap<NotificationDistributorResource, NotificationDistributor>();
             CreateMap<RetailorToDistributorResource, RetailorToDistributor>();
             CreateMap<DistributorToExecutiveResource, DistributorToExecutive>();
             CreateMap<DistributorSalesReportResuorce, DistributorSalesReport>();
            CreateMap< DSRretailorDetailsResources, DSRretailorDetails>();
        }
    }
}
