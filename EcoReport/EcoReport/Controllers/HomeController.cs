using EcoReport.Data;
using EcoReport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics;

namespace EcoReport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly EcoReportContext _context;

        public HomeController(ILogger<HomeController> logger, EcoReportContext context)
        {
            this.logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tiposDeArea = await _context.TipoDeArea
                .OrderBy(t => t.Classificacao)
                .ToListAsync();
            //ViewData["TiposDeArea"] = tiposDeArea;
            return View(tiposDeArea);
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
