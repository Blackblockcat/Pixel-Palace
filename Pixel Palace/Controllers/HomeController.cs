using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixel_Palace.Models;
using System;
using System.Diagnostics;
using System.Dynamic;

namespace Pixel_Palace.Controllers
{
    public class HomeController : Controller
    {
        private readonly PixelEntity _context;
        public HomeController(PixelEntity context)
        {
            _context = context;
        }



        //public IActionResult Index()
        //{

        //    var randomGames = _context.games.OrderBy(c => Guid.NewGuid()).Take(8).Include(p=>p.Category).ToList();
        //    return View(randomGames);
        //}
        public async Task<IActionResult> index()
        {
            dynamic viewmodel = new ExpandoObject();
          
            var categories = await _context.GetCategoriesAsync();
            viewmodel.Categories = categories;
         
            var randomGames = _context.games.OrderBy(c => Guid.NewGuid()).Take(8).Include(p=>p.Category).ToList();
            viewmodel.Games = randomGames;

            return View(viewmodel);
        }
        public async Task<IActionResult> Profile()
        {
            dynamic viewmodel = new ExpandoObject();

            var categories = await _context.GetCategoriesAsync();
            viewmodel.Categories = categories;
            return View(viewmodel);
        }
        [HttpGet]
        public IActionResult search(string query)
        {
           var data = _context.games.Where(b => b.name.Contains(query)).Include(b=>b.Category).ToList();
            return PartialView("_Partialviewsearch", data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
