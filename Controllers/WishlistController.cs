using BobMart.Data;
using BobMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobMart.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(w => w.UserId == user!.Id);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = user!.Id };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var wishlist = await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == user!.Id);

            if (wishlist == null)
            {
                wishlist = new Wishlist { UserId = user!.Id };
                _context.Wishlists.Add(wishlist);
            }

            if (!wishlist.Items.Any(i => i.ProductId == productId))
            {
                wishlist.Items.Add(new WishlistItem { ProductId = productId });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.WishlistItems.FindAsync(id);
            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MoveToCart(int id)
        {
            var item = await _context.WishlistItems.Include(w => w.Wishlist).FirstOrDefaultAsync(i => i.Id == id);
            if (item == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == user!.Id);

            if (cart == null)
            {
                cart = new Cart { UserId = user!.Id };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (cartItem == null)
            {
                cart.Items.Add(new CartItem { ProductId = item.ProductId, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }
    }
}
