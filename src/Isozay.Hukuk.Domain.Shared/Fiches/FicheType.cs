using System.ComponentModel.DataAnnotations;

namespace Isozay.Hukuk.Fiches
{
    public enum  FicheType
    {
        [Display(Name = "Tanımsız")]
        Undefined,
	[Display (Name = "Müşavirlik")]
	Consultancy,
	[Display (Name = "Dava")]
	Case,
	[Display(Name = "İcra")]
        Executive,
        [Display(Name = "Diğer")]
        Other,
    }
}
