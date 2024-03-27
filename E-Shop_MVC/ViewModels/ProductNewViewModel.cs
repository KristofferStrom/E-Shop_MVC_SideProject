using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class ProductNewViewModel
    {
        [MaxLength(30, ErrorMessage = "Produktnamnet får max vara 30 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public string Title { get; set; }
        [MaxLength(200, ErrorMessage = "Kortare beskrivning får max vara 200 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public string ShortDescription { get; set; }
        [MaxLength(1000, ErrorMessage = "Längre beskrivning får max vara 1000 tecken")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public string LongDescription { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Fyll i fältet")]
        public decimal Price { get; set; }
        [Range(0, 500, ErrorMessage = "Det får inte finnas mer än 500 i lagret")]
        [Required(ErrorMessage = "Fyll i fältet")]
        public int InStock { get; set; }
        public List<SelectListItem> Companies { get; set; } = new List<SelectListItem>();
        [Range(1, 1000, ErrorMessage = "Du måste välja varumärke")]
        [Required(ErrorMessage = "Du måste välja varumärke")]
        public int SelectedCompanyId { get; set; }
        public List<SelectListItem> Colors { get; set; } = new List<SelectListItem>();
        [Range(1, 1000, ErrorMessage = "Du måste välja färg")]
        [Required(ErrorMessage = "Du måste välja färg")]
        public int SelectedColorId { get; set; }
        public List<SelectListItem> SubCategories { get; set; } = new List<SelectListItem>();
        [Range(1, 1000, ErrorMessage = "Du måste välja underkategori")]
        [Required(ErrorMessage = "Du måste välja underkategori")]
        public int SelectedSubCategoryId { get; set; }
        [Required(ErrorMessage = "Fyll i fältet")]
        [Range(0, 10, ErrorMessage = "Garanti måste vara mellan 0-10 år")]
        public int WarrantyYears { get; set; }
        public IFormFile Image { get; set; }




    }
}
