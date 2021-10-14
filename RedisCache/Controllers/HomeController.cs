using Microsoft.AspNetCore.Mvc;
using RedisCache.Data;
using RedisCache.Models;
using System.Diagnostics;

namespace RedisCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = new();
            var cachedCategory = "";
            if (!string.IsNullOrEmpty(cachedCategory))
            {
                //cache
            }
            else
            {
                categoryList = _db.Category.ToList();
            }
            return View(categoryList);
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