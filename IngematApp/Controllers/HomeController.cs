using Microsoft.AspNetCore.Authorization; // 1. Agrega esto
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    [Authorize] // 2. ESTO OBLIGA A PASAR POR EL LOGIN
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}