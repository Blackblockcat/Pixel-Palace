using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pixel_Palace.Models
{
    public class PaymentItems
    {
        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }

        [ForeignKey("Payment")]
        public int Pid { get; set; }
        public Games Game { get; set; }
        
        public virtual Payment Payment { get; set; }
    }
}
