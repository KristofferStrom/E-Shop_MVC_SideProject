using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class UserNewViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Du har skrivit in två olika lösenord")]
        public string ConfirmPassword { get; set; }
        public List<RoleItem> Roles { get; set; } = new List<RoleItem>();
        public class RoleItem
        {
            [HiddenInput]
            public string RoleId { get; set; }
            [HiddenInput]
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
