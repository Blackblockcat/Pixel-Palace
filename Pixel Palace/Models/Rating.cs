using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Game")]
        public int Game_id { get; set; }
        [ForeignKey("User")]
        public int User_id { get; set; }

        public int Rate {  get; set; }

        public DateTime Date { get; set; }

        public virtual User? User { get; set; }
        public virtual Games? Game { get; set; }
    }
}
