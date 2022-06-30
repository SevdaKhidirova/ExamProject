using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using examPro.DAL;
using examPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace examPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class FooterAreaController : Controller
    {
        private readonly AppDbContext _context;

        public FooterAreaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/FooterAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.footerArea.ToListAsync());
        }

      

        // GET: Admin/FooterAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerArea = await _context.footerArea.FindAsync(id);
            if (footerArea == null)
            {
                return NotFound();
            }
            return View(footerArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FooterArea footerArea)
        {
            if (id != footerArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footerArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FooterAreaExists(footerArea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(footerArea);
        }

        // GET: Admin/FooterAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var footerArea = await _context.footerArea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (footerArea == null)
            {
                return NotFound();
            }

            return View(footerArea);
        }

        
        private bool FooterAreaExists(int id)
        {
            return _context.footerArea.Any(e => e.Id == id);
        }
    }
}
