using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class Games
    {
        [Key]
        public int id {  get; set; }
        [Required]
        public string name { get; set; }

        [Required]
        public string description { get; set; }


        [Required]
        public float price { get; set; }

        [ForeignKey("Category")]
        public int Category_id { get; set; }

        [Required]
        public string Os_mode { get; set; }

        

        public int Total_rating { get; set; }

        public float Average_rating { get; set; }

        [NotMapped]
        public IFormFile ClientFile { get; set; }
        public byte[] Game_Images { get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<Cart> Carts { get; set; }
        public virtual IList<Library> Library { get; set; } 
        public virtual IList<Rating> Rating { get; set; }
        public virtual IList<PaymentItems> PaymentItems { get; set; }


    }
}
