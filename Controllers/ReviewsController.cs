using BobMart.Data;
using BobMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BobMart.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, int rating, string comment)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var review = new Review
            {
                ProductId = productId,
                UserId = user!.Id,
                Rating = rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            
            // Update product average rating
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.ReviewCount++;
                // Recalculate average (simplified logic for prototype)
                product.Rating = ((product.Rating * (product.ReviewCount - 1)) + rating) / product.ReviewCount;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Products", new { id = productId });
        }
    }
}
