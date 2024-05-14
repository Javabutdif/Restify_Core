using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcLandlord.Data;
using System.Security.Claims;

namespace Restify.Controllers
{
    public class UserController : Controller
    {
        private readonly MvcLandlordContext _context;

        public UserController(MvcLandlordContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {

            ViewBag.name = HttpContext.Session.GetString("name");

            return View();
        }
    }
}
