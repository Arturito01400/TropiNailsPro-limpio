using TropiNailsPro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TropiNailsPro.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TropiNailsPro.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioNombre") != null)
                return RedirectToAction("Dashboard", "Home"); // Ajusta si tu controlador Dashboard se llama distinto

            if (TempData["Mensaje"] != null)
                ViewBag.Mensaje = TempData["Mensaje"];

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Identificador)
        {
            if (string.IsNullOrWhiteSpace(Identificador))
            {
                TempData["Error"] = "Por favor ingresa tu número de teléfono o correo electrónico.";
                return RedirectToAction("Login");
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u =>
                    (!string.IsNullOrEmpty(u.Email) && u.Email == Identificador) ||
                    (!string.IsNullOrEmpty(u.Telefono) && u.Telefono == Identificador)
                );

            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado con ese teléfono o correo.";
                return RedirectToAction("Login");
            }

            // Guarda en sesión ambos valores por seguridad y para control en vistas
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
            HttpContext.Session.SetString("UsuarioEmail", usuario.Email ?? "");
            
            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Por favor corrige los errores del formulario.");
                return View(usuario);
            }

            if (!string.IsNullOrEmpty(usuario.Email) &&
                _context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                return View(usuario);
            }

            if (!string.IsNullOrEmpty(usuario.Telefono) &&
                _context.Usuarios.Any(u => u.Telefono == usuario.Telefono))
            {
                ModelState.AddModelError("Telefono", "El número de teléfono ya está registrado.");
                return View(usuario);
            }

            usuario.FechaRegistro = System.DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "✅ Registro exitoso. Ya puedes iniciar sesión con tu correo o teléfono.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
