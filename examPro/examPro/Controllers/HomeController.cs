using examPro.DAL;
using examPro.Models;
using examPro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace examPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db)
        {
            db = _db;
        }
        public async  Task<IActionResult> Index()
        {
            HomeViewModel hvm = new HomeViewModel
            {
                services=await db.Services.ToListAsync(),
                FooterArea=await db.footerArea.OrderByDescending(x=>x.Id).FirstAsync()

            };
            return View(hvm);
        }

     
    }
}
