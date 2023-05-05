using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Clients
{
    public enum ClientIdentifier
    {
        [Display(Name = "Tanımsız")]
        Undefined,
        [Display(Name = "TC Kimlik Numarası")]
        TCKN,
        [Display(Name = "Vergi Kimlik Numarası")]
        VKN
    }
}
