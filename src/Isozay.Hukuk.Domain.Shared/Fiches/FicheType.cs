using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Fiches
{
    public enum  FicheType
    {
        [Display(Name = "Tanımsız")]
        Undefined,
        [Display(Name = "Alış")]
        Buying,
        [Display(Name = "Satış")]
        Selling,
        [Display(Name = "Alış İade")]
        BuyReturn,
        [Display(Name = "Satış İade")]
        SelesReturn,
    }
}
