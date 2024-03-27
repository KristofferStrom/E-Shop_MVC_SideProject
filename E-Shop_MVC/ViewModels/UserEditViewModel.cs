using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace E_Shop_MVC.ViewModels
{
    public class UserEditViewModel
    {
        public string UserId { get; set; }
        [EmailAddress(ErrorMessage = "Användarnamnet måste vara en mejladress")]
        [Required(ErrorMessage = "Fyll i fältet")]
        [MaxLength(256)]
        public string UserName { get; set; }
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
