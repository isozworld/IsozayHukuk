using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Clients
{
    public enum ClientType
    {
        [Display(Name = "Tanımsız")]
        Undefined,
        [Display(Name = "Alıcı - Müvekkil")]
        Client,
        [Display(Name = "Satıcı")]
        Dealer

        

    }
}
