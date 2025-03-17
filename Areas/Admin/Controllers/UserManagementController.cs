using IdentityDemo.Areas.Admin.Models;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        
        public async Task<IActionResult> IndexAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = userRoles.ToList(),
                    IsLockedOut = await userManager.IsLockedOutAsync(user)
                };

                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);

        }
    }
}
