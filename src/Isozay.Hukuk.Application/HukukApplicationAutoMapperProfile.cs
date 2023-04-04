using AutoMapper;
using Isozay.Hukuk.Clients;
using Isozay.Hukuk.Safes;
using Isozay.Hukuk.Currencies;
using Isozay.Hukuk.Items;
using Isozay.Hukuk.Fiches;

namespace Isozay.Hukuk
{
    public class HukukApplicationAutoMapperProfile : Profile
    {
        public HukukApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
            * Alternatively, you can split your mapping configurations
            * into multiple profile classes for a better organization. */
            CreateMap<Client,ClientDto>().ReverseMap();
            CreateMap<CreateUpdateClientDto, Client>();

            CreateMap<Safe, SafeDto>().ReverseMap();
            CreateMap<CreateUpdateSafeDto, Safe>();

            CreateMap<Currency, CurrencyDto>().ReverseMap();

            CreateMap<Item, ItemDto>().ReverseMap();

            CreateMap<Fiche, FicheDto>().ReverseMap();
            CreateMap<CreateUpdateFicheDto, Fiche>();

            CreateMap<FicheLine, FicheLineDto>().ReverseMap();
                
	        CreateMap<SafeTran, SafeTranDto>().ReverseMap();
			CreateMap<CreateUpdateSafeTranDto, SafeTran> ();

            CreateMap<ClientTranDto, ClientTran>().ReverseMap();

        }
    }
}