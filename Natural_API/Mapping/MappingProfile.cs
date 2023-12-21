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
                CreateMap<Distributor, DEInsertUpdateResource>();
                CreateMap<State, StateResource>();
                CreateMap<Area, AreaResource>();
                CreateMap<City, CityResource>();
                CreateMap<Category, CategoryResource>();
                CreateMap<Retailor , RetailorResource>();
                CreateMap<Retailor, RetailorPostResource>();
                CreateMap <Executive, ExecutiveGetResource>();
                CreateMap<Executive, DEInsertUpdateResource>();
                CreateMap<Category , CategoryInsertResource>();
                CreateMap<DistributorToExecutive, DistributorToExecutiveResource>();
                CreateMap<DistributorToExecutive, AssignDistributorToExecutiveResource>();
                CreateMap<InsertDEmapper, DistributorToExecutive>();

    
            
                //// RESOURCE TO DOMAIN
                
                CreateMap<LoginResource, Login>();
                CreateMap<DistributorGetResource, Distributor>();
                CreateMap<DEInsertUpdateResource, Distributor>();
                CreateMap<StateResource, State>();
                CreateMap<AreaResource, Area>();
                CreateMap<CityResource, City>();
                CreateMap<CategoryResource,Category>();
                CreateMap<RetailorPostResource, Retailor>();
                CreateMap<ExecutiveGetResource, Executive>();   
                CreateMap<DEInsertUpdateResource, Executive>();
                CreateMap<ExecutiveGetResource, Executive>();
                CreateMap<CategoryInsertResource,Category>();
                CreateMap<DistributorToExecutiveResource, DistributorToExecutive>();
                CreateMap<AssignDistributorToExecutiveResource, DistributorToExecutive>();
                CreateMap<DistributorToExecutive, InsertDEmapper>();


        }
    }
}
