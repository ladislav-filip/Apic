using Apic.Contracts.Customers;
using AutoMapper;
using CustomerDbo = Apic.Entities.Customers.Customer;

namespace Apic.Facades.Mappers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDbo>();
            CreateMap<CustomerDbo, Customer>();

            CreateMap<CustomerUpdate, CustomerDbo>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<CustomerCreate, CustomerDbo>()
                .ForMember(x => x.Id, x => x.Ignore());
        }
    }
}
