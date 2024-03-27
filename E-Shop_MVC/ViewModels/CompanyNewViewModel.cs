using E_Shop_MVC.Attributes;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class CompanyNewViewModel
    {
        [MaxLength(40, ErrorMessage = "Titel får max innehålla 40 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]




        [NoSwedish(ErrorMessage = "Tillåter inte å, ä eller ö")]
        public string Title { get; set; }
    }
}
