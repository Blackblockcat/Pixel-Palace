using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Please Enter Your Username")]
        public string User_name { get; set; }

        [Required(ErrorMessage = "Please Enter Your Gender")]
        public bool Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Your region ")]
        public string Region { get; set; }

        [Required(ErrorMessage = "Please Enter Your Type ")]
        [ForeignKey("Category")]

        public int Category_id { get; set; }
    

        [Required(ErrorMessage = "Please Enter Your Email ")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "please enter correct email address")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Please Enter Your Password ")]
        [Display(Name = "Pass")]
        //[StringLength(50, ErrorMessage = "The Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [NotMapped]
        public IFormFile? ClientFile { get; set; }

        public byte[]? Photo { get; set; }
        public string? AdminRole { get; set; }
        public virtual Category Category { get; set; }

        public virtual IList<Cart>? Carts { get; set; }
        public virtual IList<Library>? Library { get; set; }
        public virtual IList<Rating>? Rating { get; set; }
        public virtual IList<Payment>? Payments { get; set; }


    }
}
