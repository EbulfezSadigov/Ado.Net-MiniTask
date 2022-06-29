using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exercise.DAL;
using Exercise.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Exercise.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ElementsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ElementsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Elements.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: Admin/Elements/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Element element)
        {
            string path = _env.WebRootPath + @"\img";
            string filename = Guid.NewGuid().ToString() + element.Img.FileName;
            string final = Path.Combine(path,filename);

            using (FileStream fs =new FileStream(final,FileMode.Create))
            {
                await element.Img.CopyToAsync(fs);
            }

            element.Image = filename;


                if (ModelState.IsValid)
                {
                    _context.Add(element);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            return View(element);
        }

        // GET: Admin/Elements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        // POST: Admin/Elements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Element element)
        {
            if (element.Img != null)
            {
                if (!element.Img.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Img", "File is not an image.");
                    return View();
                }
                if (element.Img.Length/1024>1000)
                {
                    ModelState.AddModelError("Img", "File is too large to upload.");
                    return View();
                }

                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath + "/img", element.Image)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath + "/img", element.Image));
                }

                string path = _env.WebRootPath + @"\img";
                string filename = Guid.NewGuid().ToString() + element.Img.FileName;
                string final = Path.Combine(path, filename);

                using (FileStream fs = new FileStream(final, FileMode.Create))
                {
                    await element.Img.CopyToAsync(fs);
                }

                element.Image = filename;

            }
            if (id != element.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(element);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementExists(element.Id))
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
            return View(element);
        }

        // GET: Admin/Elements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Admin/Elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var element = await _context.Elements.FindAsync(id);

            string path = _env.WebRootPath + @"\img";
            string filename = element.Image;
            string final = Path.Combine(path, filename);


            if (System.IO.File.Exists(final))
            {
                System.IO.File.Delete(final);
            }
            _context.Elements.Remove(element);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementExists(int id)
        {
            return _context.Elements.Any(e => e.Id == id);
        }
    }
}
