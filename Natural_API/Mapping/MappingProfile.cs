using AutoMapper;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.Models;

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
            CreateMap<Dsrdetail,DsrDetailPostResource>();
            CreateMap<InsertDEmapper, DistributorToExecutive>();
            CreateMap<DistributorToExecutive , DistributorToExecutiveResource>();


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
            CreateMap<DistributorToExecutiveResource, DistributorToExecutive>();




        }
    }
}
