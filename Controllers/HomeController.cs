using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TropiNailsPro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TropiNailsPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Página principal genérica (Index)
        public IActionResult Index()
        {
            return View();
        }

        // Página de privacidad o estática
        public IActionResult Privacy()
        {
            return View();
        }

        // Página de error estándar
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Dashboard que verifica sesión y muestra nombre de usuario
        public IActionResult Dashboard()
        {
            var usuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(usuarioNombre))
            {
                // Si no hay usuario en sesión, redirige a login
                return RedirectToAction("Login", "Auth");
            }

            // Pasa el nombre del usuario a la vista
            ViewBag.UsuarioNombre = usuarioNombre;

            return View();
        }
    }
}
