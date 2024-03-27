using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data
{
    public class ProductReview
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string CustomerName { get; set; }
        [Range(1, 5)]
        [Required]
        public int Rate { get; set; }
        [MaxLength(500)]
        public string ReviewText { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Product Product { get; set; }
    }
}
