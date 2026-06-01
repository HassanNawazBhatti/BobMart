using System.ComponentModel.DataAnnotations.Schema;

namespace BobMart.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }

        public int WishlistId { get; set; }
        [ForeignKey("WishlistId")]
        public Wishlist? Wishlist { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
