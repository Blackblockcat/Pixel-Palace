using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        public DateTime Date { get; set; }

        public float Total_Price { get; set; }

        public virtual User? User { get; set; }
        public virtual IList<PaymentItems>? Items { get; set; }
    }
}
