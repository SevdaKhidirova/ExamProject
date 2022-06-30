using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using examPro.DAL;
using examPro.Models;
using Microsoft.AspNetCore.Hosting;
using examPro.Extensions;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace examPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles ="Admin,SuperAdmin")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public ServicesController(AppDbContext context, IWebHostEnvironment _env)
        {
            _context = context;
            env=_env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!service.Img.isImage())
            {
                ModelState.AddModelError("Img", "File is not an image");
                return View(service);
            }
            if (!service.Img.isSmallerThan(1048576))
            {
                ModelState.AddModelError("Img", "File is too big");
                return View(service);
            }

            string path = env.WebRootPath + @"\img";
            string fileName = Guid.NewGuid().ToString() + service.Img.FileName;
            string finalPath = Path.Combine(path, fileName);

            using(FileStream stream =new FileStream(finalPath, FileMode.Create))
            {
                await service.Img.CopyToAsync(stream);
            }
            service.Image = fileName;
            
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (service.Img != null)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (!service.Img.isImage())
                {
                    ModelState.AddModelError("Img", "File is not an image");
                    return View(service);
                }
                if (!service.Img.isSmallerThan(1048576))
                {
                    ModelState.AddModelError("Img", "File is too big");
                    return View(service);
                }

                string path = env.WebRootPath + @"\img";
                string fileName = Guid.NewGuid().ToString() + service.Img.FileName;
                string finalPath = Path.Combine(path, fileName);

                if (System.IO.File.Exists(Path.Combine(path, service.Image)))
                {
                    System.IO.File.Delete(Path.Combine(path, service.Image));
                }

                using (FileStream stream = new FileStream(finalPath, FileMode.Create))
                {
                    await service.Img.CopyToAsync(stream);
                }

                service.Image = fileName;
            }

            _context.Update(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (System.IO.File.Exists(Path.Combine(env.WebRootPath+@"\img", service.Image)))
            {
                System.IO.File.Delete(Path.Combine(env.WebRootPath + @"\img", service.Image));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
