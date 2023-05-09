using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Isozay.Hukuk.Safes
{
    public enum SafeType
    {
        [Display(Name = "Tanımsız")]
        Undefined,
        [Display(Name = "Büro")]
        Bureaue,
        [Display(Name = "Diğer")]
        Other
    }
}

