using examPro.Models;
using examPro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examPro.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser
            {
                Name=viewModel.Name,
                Surname=viewModel.Surname,
                UserName=viewModel.Username,
                Email=viewModel.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(viewModel);
            }

            return RedirectToAction("Index","Home" );
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser loginUser = await userManager.FindByEmailAsync(viewModel.Email);
            if (loginUser == null)
            {
                ModelState.AddModelError("", "Password or Email is wrong");
                return View(viewModel);
            }

           Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(loginUser,viewModel.Password,viewModel.RememberMe,true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or Email is wrong");
                return View(viewModel);
            }

            if((await userManager.GetRolesAsync(loginUser)).Count>0 && ((await userManager.GetRolesAsync(loginUser))[0]=="Admin")|| (await userManager.GetRolesAsync(loginUser))[0] == "SuperAdmin"){

                return RedirectToAction("Index", "Home",new {area="Admin" });
            }

            return RedirectToAction("Home", "Index");
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


        //public async Task<IActionResult> CreateRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await roleManager.CreateAsync(new IdentityRole("Member"));

        //    return Content("Ok");
        //} 


        //public async Task<IActionResult> AddRolesToUser()
        //{
        //    AppUser user = await userManager.FindByNameAsync("Sevda571");
        //    await userManager.AddToRoleAsync(user, "Admin");

        //    AppUser user1 = await userManager.FindByNameAsync("AbulfazAga");
        //    await userManager.AddToRoleAsync(user1, "Member");

        //    AppUser user2 = await userManager.FindByNameAsync("Bashir");
        //    await userManager.AddToRoleAsync(user2, "SuperAdmin");

        //    return Content("Ok");
        //}
    }
}
