using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TropiNailsPro.Controllers
{
    public class DashboardController : Controller
    {
        // Acción principal del Dashboard
        public IActionResult Dashboard()
        {
            // Verificar si el usuario está logueado
            var nombre = HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(nombre))
            {
                // Si no hay sesión activa, redirige al login con mensaje de error
                TempData["Error"] = "Debes iniciar sesión para acceder al sistema.";
                return RedirectToAction("Login", "Auth");
            }

            // Pasar el nombre del usuario a la vista usando ViewBag
            ViewBag.UsuarioNombre = nombre;
            ViewBag.MensajeBienvenida = $"👋 Bienvenida, {nombre}!";

            // Retorna la vista Index.cshtml ubicada en Views/Dashboard/
            return View("Index");
        }

        // Acción para cerrar sesión desde el Dashboard (opcional)
        public IActionResult Salir()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la página de login
            return RedirectToAction("Login", "Auth");
        }
    }
}
