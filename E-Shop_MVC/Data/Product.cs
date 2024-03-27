using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Title { get; set; }
        [MaxLength(200)]
        [Required]
        public string ShortDescription { get; set; }
        [MaxLength(1000)]
        [Required]
        public string LongDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Range(0, 500)]
        [Required]
        public int InStock { get; set; }
       
        [MaxLength(300)]
        public string ?imgTitle { get; set; }
        [Required]
        public Company Company { get; set; }
        [Required]
        public SubProductCategory SubCategory { get; set; }
        [Required]
        public ProductColor Color { get; set; }

        [Required]
        public int Warranty { get; set; }
    }
}
