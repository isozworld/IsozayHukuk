using AutoMapper;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Safes;
using Isozay.Hukuk.Currencies;
using Isozay.Hukuk.Fiches;

namespace Isozay.Hukuk.Blazor
{

    public class HukukBlazorAutoMapperProfile : Profile
    {
        public HukukBlazorAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Blazor project.
            CreateMap<ClientDto, CreateUpdateClientDto>();
            CreateMap<SafeDto, CreateUpdateSafeDto>();
            CreateMap<CreateUpdateSafeDto, SafeDto>();
            CreateMap<CreateUpdateFicheDto, FicheDto>();
            CreateMap<FicheDto, CreateUpdateFicheDto>();


        }
    }
}