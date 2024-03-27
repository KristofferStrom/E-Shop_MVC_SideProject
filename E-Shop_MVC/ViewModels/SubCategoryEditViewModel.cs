using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class SubCategoryEditViewModel
    {
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [Range(1, 1000, ErrorMessage = "Du måste välja kategori")]
        [Required(ErrorMessage = "Du måste välja kategori")]
        public int SelectedCategoryId { get; set; }
        public int Id { get; set; }
        [MaxLength(30, ErrorMessage = "Titel får max innehålla 30 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public string Title { get; set; }
    }
}
