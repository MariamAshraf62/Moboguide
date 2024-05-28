using System.ComponentModel.DataAnnotations.Schema;

namespace Moboguide.Models
{
    public class MobileUser
    {
        [ForeignKey("Mobiles")]
        public string MobileName { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public bool IsFavorite { get; set; } = false;
        public float Rate { get; set; }
        public virtual Mobile Mobiles { get; set; }
        public virtual User Users { get; set; }

    }
}
