using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moboguide.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Mobiles")]
        public string MobileName { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public virtual User Users { get; set; }  
        public virtual Mobile Mobiles { get; set; }
    }
}
