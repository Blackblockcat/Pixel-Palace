using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pixel_Palace.Models;

namespace Pixel_Palace.Controllers
{
    public class AdminGamesController : Controller
    {
        private readonly PixelEntity _context;

        public AdminGamesController(PixelEntity context)
        {
            _context = context;
        }

        // GET: AdminGames
        public async Task<IActionResult> Index()
        {
            var pixelEntity = _context.games.Include(g => g.Category);

            ViewData["Category_id"] = new SelectList(_context.categories, "Id", "Type");

            return View(await pixelEntity.ToListAsync());
        }
        // GET: AdminGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.games == null)
            {
                return NotFound();
            }

            var games = await _context.games
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (games == null)
            {
                return NotFound();
            }

            return View(games);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin == "false")
            {
                return NotFound();
            }
            ViewData["Category_id"] = new SelectList(_context.categories, "Id", "Type");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,price,Category_id,Os_mode,Total_rating,Average_rating,ClientFile")] Games games)
        {
            
            if (games.ClientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                games.ClientFile.CopyTo(stream);
                games.Game_Images = stream.ToArray();
            }

            _context.Add(games);
            await _context.SaveChangesAsync();


            ViewData["Category_id"] = new SelectList(_context.categories, "Id", "Type", games.Category_id);
            return RedirectToAction("index", "Home");
        }


        // GET: AdminGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin == "false")
            {
                return NotFound();
            }
            if (id == null || _context.games == null)
            {
                return NotFound();
            }

            var games = await _context.games.FindAsync(id);
            if (games == null)
            {
                return NotFound();
            }
            ViewData["Category_id"] = new SelectList(_context.categories, "Id", "Type", games.Category_id);
            return View(games);
        }

        // POST: AdminGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,price,Category_id,Os_mode,Total_rating,Average_rating,ClientFile")] Games games)
        {
            if (id != games.id)
            {
                return NotFound();
            }

            if (games.ClientFile != null)
            {
                MemoryStream stream = new MemoryStream();
                games.ClientFile.CopyTo(stream);
                games.Game_Images = stream.ToArray();
            }

            try
            {
                _context.Update(games);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GamesExists(games.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["Category_id"] = new SelectList(_context.categories, "Id", "Type", games.Category_id);
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (isAdmin == "false")
            {
                return NotFound();
            }
            if (id == null || _context.games == null)
            {
                return NotFound();
            }

            var games = await _context.games
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.id == id);
            if (games == null)
            {
                return NotFound();
            }

            return View(games);
        }

        // POST: AdminGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.games == null)
            {
                return Problem("Entity set 'PixelEntity.Games'  is null.");
            }
            var games = await _context.games.FindAsync(id);
            if (games != null)
            {
                _context.games.Remove(games);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamesExists(int id)
        {
            return (_context.games?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}

