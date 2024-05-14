using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApartment.Data;
using Restify.Models;

namespace Restify.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly MvcApartmentContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ApartmentsController(MvcApartmentContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            var apartments = await _context.Apartment.Where(m => m.landlord_id == HttpContext.Session.GetInt32("id")).ToListAsync();
  
            return View(apartments);
        }


        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .FirstOrDefaultAsync(m => m.apartment_id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? name, string? details, string? location, IFormFile? apartment_images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (apartment_images != null && apartment_images.Length > 0)
                    {
          
                        string uniqueFileName = name + "_" + apartment_images.FileName;

         
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

      
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await apartment_images.CopyToAsync(fileStream);
                        }


                        Apartment apartment = new Apartment
                        {
                            apartment_name = name,
                            apartment_details = details,
                            apartment_location = location,
                            apartment_image = "/uploads/" + uniqueFileName,
                            landlord_id = HttpContext.Session.GetInt32("id")
                        };

              
                        _context.Add(apartment);
          
                        await _context.SaveChangesAsync();
       
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
           
                        ModelState.AddModelError("image", "Please select an image file.");
                    }
                }
                catch (Exception ex)
                {
          
                    ModelState.AddModelError("", "An error occurred while saving the apartment information.");
     
                    return View();
                }
            }
   
            return View();
        }
    


    // GET: Apartments/Edit/5
    public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("apartment_id,apartment_name,apartment_details,apartment_location,apartment_image")] Apartment apartment)
        {
            if (id != apartment.apartment_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.apartment_id))
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
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .FirstOrDefaultAsync(m => m.apartment_id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var apartment = await _context.Apartment.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartment.Remove(apartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(int? id)
        {
            return _context.Apartment.Any(e => e.apartment_id == id);
        }
    }
}
