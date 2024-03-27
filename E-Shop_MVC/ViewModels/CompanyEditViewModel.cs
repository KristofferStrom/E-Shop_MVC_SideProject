using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class CompanyEditViewModel
    {
        public int Id { get; set; }
        [MaxLength(40, ErrorMessage = "Titel får max innehålla 40 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public string Title { get; set; }
    }
}
