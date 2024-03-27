using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Shop_MVC.Models.Data
{
    public class Company
    {
        public int Id { get; set; }
        [MaxLength(40)]
        [Required]
        public string Title { get; set; }
    }
}
