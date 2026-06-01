using System.ComponentModel.DataAnnotations.Schema;

namespace BobMart.Models
{
    public class Cart
    {
        public int Id { get; set; }

        // Nullable because guest users might have a cart based on Session ID
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Session ID for guest users
        public string? SessionId { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
