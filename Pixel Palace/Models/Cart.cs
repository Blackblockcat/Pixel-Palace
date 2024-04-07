using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int user_id {  get; set; }
        [ForeignKey("Game")]
        public int game_id {  get; set; }


        public virtual User? User { get; set; }
        public virtual Games? Game { get; set; }
    }
}
