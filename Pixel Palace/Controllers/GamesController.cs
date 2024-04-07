using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pixel_Palace.Models;

namespace Pixel_Palace.Controllers
{
    public class GamesController : Controller
    {
        private readonly PixelEntity _context;

        public GamesController(PixelEntity context)
        {
            _context = context;
        }
        User us=new User();
        // GET: Games
        public async Task<IActionResult> Index()
        {
            var pixelEntity = _context.games.Include(g => g.Category);
            return View(await pixelEntity.ToListAsync());
        }
        public async Task<IActionResult> Games(int Cid)
        {
            dynamic viewmodel = new ExpandoObject();

            var categories = await _context.GetCategoriesAsync();
            viewmodel.Categories = categories;


            var games = await _context.games.Where(g => g.Category_id == Cid).Include(p => p.Category).ToListAsync();
            viewmodel.Games = games;


            var cat = _context.categories.Where(g => g.Id == Cid).FirstOrDefault();
            ViewBag.type = cat.Type;

            return View(viewmodel);
        }


        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            dynamic viewmodel = new ExpandoObject();

            if (id == null || _context.games == null)
            {
                return NotFound();
            }

            var game = await _context.games
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            viewmodel.game = game;

            if (game == null)
            {
                return NotFound();
            }

            var categories = await _context.GetCategoriesAsync();
            viewmodel.Categories = categories;

            var Relatedgames = await _context.games.Where(g => g.Category_id == game.Category_id && g.id != game.id).ToListAsync();
            viewmodel.Relatedgames = Relatedgames;

            return View(viewmodel);
        }


           

        private bool GamesExists(int id)
        {
            return (_context.games?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
