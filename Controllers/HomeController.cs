using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApartment.Data;
using MvcLandlord.Data;
using Restify.Models;
using System.Diagnostics;

namespace Restify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MvcLandlordContext _context;
        private readonly MvcApartmentContext _contextApartment;


        public HomeController(ILogger<HomeController> logger, MvcLandlordContext context, MvcApartmentContext context1)
        {
            _logger = logger;
            _context = context;
            _contextApartment = context1;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Home()
        {
            
            return View(await _contextApartment.Apartment.ToListAsync());
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult RegisterButton(string? fname, string?lname,string? email, string? contact , string? pass)
        {
            Landlord land = new Landlord { landlord_firstname = fname, landlord_lastname = lname, landlord_email = email, landlord_contact = contact, landlord_password = pass };
            _context.Landlord.Add(land);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful. You can now login with your credentials.";

            return RedirectToAction("Login", "Home");
        }
        public async Task<IActionResult> LoginSubmit(string? email, string? pass)
        {
            if (email == "admin@gmail.com" && pass == "admin")
            {
                return RedirectToAction("Index", "Landlords");
            }
            else
            {

                var landlord = await _context.Landlord
                    .FirstOrDefaultAsync(m => m.landlord_email == email && m.landlord_password == pass);
                if (landlord == null)
                {
                    TempData["invalidLogin"] = "Incorrect email and password.";

                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    HttpContext.Session.SetString("name", landlord.landlord_firstname + " " + landlord.landlord_lastname);
                    HttpContext.Session.SetInt32("id",landlord.landlord_id);
                    return RedirectToAction("Dashboard", "User");
                }
            }
            
        }
        public void ViewData(string? image )
        {
      
            ViewBag.Image = image;
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
