using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pixel_Palace.Models;

namespace Pixel_Palace.Controllers
{
    public class CartsController : Controller
    {
        private readonly PixelEntity _context;

        public CartsController(PixelEntity context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var pixelEntity = _context.carts.Include(c => c.Game).Include(c => c.User);
            return View(await pixelEntity.ToListAsync());
        }


        public async Task<IActionResult> Create(int gameid)
        {
            Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");

            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {

                return RedirectToAction("Login", "Users");
            }
            var user = await _context.users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("Email"));
            var model = _context.carts.Where(p => p.user_id == user.Id && p.game_id == gameid);

            if (model != null)
            {
                RedirectToAction("Home", "Home");

            }

            var game = await _context.games.FirstOrDefaultAsync(m => m.id == gameid);
            var cat = await _context.categories.FirstOrDefaultAsync(m => m.Id == game.Category_id);
            game.Category = cat;

            var cart = new Cart
            {
                
                user_id = user.Id,
                game_id = game.id,
                User = user,
                Game = game,
            };

            _context.carts.Add(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Cart()
        {
            Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");

            var name = HttpContext.Session.GetString("Email");
            if (String.IsNullOrEmpty(name))
            {

                var returnUrl = Request.Path.Value;
                return RedirectToAction("Login", "Users", new { returnUrl });
            }
            dynamic viewmodel = new ExpandoObject();

            var categories = await _context.GetCategoriesAsync();
            viewmodel.Categories = categories;

            var user = await _context.users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("Email"));

            var model = await _context.carts.Where(p => p.user_id == user.Id).Include(p => p.Game).Include(p => p.User).ToListAsync();
            viewmodel.cart = model;

            return View(viewmodel);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var cartItem = await _context.carts.FindAsync(id);
            if (cartItem != null)
            {
                _context.carts.Remove(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("cart");
        }

        private bool CartExists(int id)
        {
            return (_context.carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
