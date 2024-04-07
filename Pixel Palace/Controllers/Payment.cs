using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel_Palace.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel_Palace.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PixelEntity _context;

        public PaymentController(PixelEntity context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Check()
        {
            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Users");
            }

            var user = await _context.users.FirstOrDefaultAsync(m => m.Email == name);
            var cartItems = await _context.carts
                .Where(c => c.user_id == user.Id)
                .Include(c => c.Game)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> check()
        {
            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {
                return RedirectToAction("Login", "Users");
            }

            var user = await _context.users.FirstOrDefaultAsync(m => m.Email == name);
            var cartItems = await _context.carts
                .Where(c => c.user_id == user.Id)
                .Include(c => c.Game)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Cart", "Carts");
            }

            var payment = new Payment
            {
                user_id = user.Id,
                Date = DateTime.UtcNow,
                Total_Price = cartItems.Sum(c => c.Game.price),
                Items = cartItems.Select(c => new PaymentItems
                {
                    GameId = c.game_id
                }).ToList()
            };

            _context.Payments.Add(payment);
            _context.carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return RedirectToAction("indx","Home");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
