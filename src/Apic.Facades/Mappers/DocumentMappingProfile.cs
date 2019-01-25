using Apic.Contracts.Documents;
using Apic.Facades.Mappers.Resolvers;
using Apic.Services.AzureStorage;
using AutoMapper;
using DocumentDbo = Apic.Entities.Documents.Document;
using Document = Apic.Contracts.Documents.Document;

namespace Apic.Facades.Mappers
{
    public class DocumentMappingProfile : Profile
    {
        private readonly IAzureStorageService azureStorageService;

        public DocumentMappingProfile(IAzureStorageService azureStorageService)
        {
            this.azureStorageService = azureStorageService;
            CreateMap<Document, DocumentDbo>();

            CreateMap<DocumentDbo, Document>()
                //.ForMember(dest => dest.Url, opt => opt.MapFrom<BlobUrlValueResolver>())
                .ForMember(dest => dest.Url, opt => opt.MapFrom(new BlobUrlValueResolver(azureStorageService)))
                .AfterMap((s, d) => d.Availability = "OK");

            CreateMap<DocumentCreate, DocumentDbo>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Customer, x => x.Ignore())
                .ForMember(x => x.CustomerId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CustomerId"]));
        }
    }
}
