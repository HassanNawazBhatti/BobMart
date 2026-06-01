using BobMart.Data;
using BobMart.Models;
using BobMart.Models.Enums;
using BobMart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BobMart.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user!.Id);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutViewModel
            {
                ShippingName = $"{user!.FirstName} {user.LastName}",
                ShippingAddress = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                Country = user.Country
            };

            ViewBag.CartTotal = cart.Items.Sum(i => i.Quantity * i.Product!.Price) + 150.00m;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user!.Id);

            if (cart == null || !cart.Items.Any()) return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                UserId = user!.Id,
                OrderDate = DateTime.UtcNow,
                ShippingName = model.ShippingName,
                ShippingAddress = model.ShippingAddress,
                ShippingCity = model.City,
                ShippingPostalCode = model.PostalCode,
                ShippingCountry = model.Country,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Items.Sum(i => i.Quantity * i.Product!.Price) + 15.00m
            };

            foreach (var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PriceAtPurchase = item.Product!.Price
                });
            }

            _context.Orders.Add(order);
            _context.Carts.Remove(cart); // Clear cart

            // Save info for future orders
            user.Address = model.ShippingAddress;
            user.City = model.City;
            user.PostalCode = model.PostalCode;
            user.Country = model.Country;
            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return RedirectToAction("Success", new { id = order.Id });
        }

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
