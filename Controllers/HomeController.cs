using BobMart.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featured = await _context.Products.Where(p => p.IsFeatured).Take(4).ToListAsync();
            var latest = await _context.Products.OrderByDescending(p => p.DateAdded).Take(4).ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            ViewBag.FeaturedProducts = featured;
            ViewBag.LatestProducts = latest;
            ViewBag.Categories = categories;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
