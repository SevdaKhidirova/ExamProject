using examPro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace examPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UsersController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            int skip = (page-1)*10;
            ViewBag.Count = Math.Ceiling((double)userManager.Users.Count()) / (double)2;
            ViewBag.CurrentPage = page;

            return View( await userManager.Users.Skip(skip).Take(10).ToListAsync());
        }




        public async Task<IActionResult> Block(string id)
        {
           AppUser user= await userManager.FindByIdAsync(id);
            user.Status = false;
            await userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UnBlock(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            user.Status = true;
            await userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
