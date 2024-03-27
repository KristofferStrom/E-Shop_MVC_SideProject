using E_Shop_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Shop_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller

    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            var viewModel = new UserIndexViewModel();

            viewModel.Users = IndexGetAllUsers();

            return View(viewModel);
        }


        public IActionResult New()
        {
            var viewModel = new UserNewViewModel();

            viewModel.Roles = NewGetAllRoles();

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> New(UserNewViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = CreateUser(viewModel);
                var result = await _userManager.CreateAsync(newUser, viewModel.Password);

                if (!result.Succeeded)
                {
                    AddModelError(result);

                    return View(viewModel);
                }

                result = await _userManager.AddToRolesAsync(newUser,
                   viewModel.Roles.Where(role => role.IsSelected).Select(role => role.RoleName));

                if (!result.Succeeded)
                {
                    AddModelError(result);

                    return View(viewModel);
                }

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = new UserEditViewModel();

            var user = await _userManager.FindByIdAsync(id);

            viewModel.UserId = user.Id;
            viewModel.UserName = user.UserName;
            foreach (var role in _roleManager.Roles)
            {
                var userRoles = CreateRole(role);

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRoles.IsSelected = true;
                else
                    userRoles.IsSelected = false;

                viewModel.Roles.Add(userRoles);
            }

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(viewModel.UserId);
                var userName = user.UserName;
                var userId = user.Id;
                var roles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    return View(viewModel);
                }

                result = await _userManager.AddToRolesAsync(user,
                    viewModel.Roles.Where(role => role.IsSelected).Select(role => role.RoleName));

                if (!result.Succeeded)
                {
                    AddModelError(result);

                    return View(viewModel);
                }

                user.UserName = viewModel.UserName;
                user.Email = viewModel.UserName;
                result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    viewModel.UserId = userId;
                    viewModel.UserName = userName;
                    AddModelError(result);

                    return View(viewModel);
                }

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }


        private List<UserIndexViewModel.UserItem> IndexGetAllUsers()
        {
            return _userManager.Users.Select(user => new UserIndexViewModel.UserItem
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToList();
        }
        private List<UserNewViewModel.RoleItem> NewGetAllRoles()
        {
            return _roleManager.Roles.Select(role => new UserNewViewModel.RoleItem
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = false
            }).ToList();
        }
        private IdentityUser CreateUser(UserNewViewModel viewModel)
        {
            return new IdentityUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email
            };
        }
        private void AddModelError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        private UserEditViewModel.RoleItem CreateRole(IdentityRole role)
        {
            return new UserEditViewModel.RoleItem
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
        }
    }
}
