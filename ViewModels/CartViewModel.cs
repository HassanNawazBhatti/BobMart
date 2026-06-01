using BobMart.Models;

namespace BobMart.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal SubTotal { get; set; }
        public decimal Shipping { get; set; } = 15.00m; // Flat rate for example
        public decimal Total => SubTotal + Shipping;
    }
}
