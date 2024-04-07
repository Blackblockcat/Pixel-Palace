using System.ComponentModel.DataAnnotations;

namespace Pixel_Palace.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual IList<User>? Users { get; set; }

        public virtual IList<Games> Games { get; set; }


    }
}
