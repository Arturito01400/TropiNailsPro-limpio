using Microsoft.AspNetCore.Mvc;
using TropiNailsPro.Data;
using TropiNailsPro.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;

namespace TropiNailsPro.Controllers
{
    public class PagosTransferenciaController : Controller
    {
        private readonly AppDbContext _context;

        public PagosTransferenciaController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Restringir acceso solo a la propietaria (Arturito)
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuario = context.HttpContext.Session.GetString("Usuario");
            if (usuario != "Arturito")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }

        // ✅ Listar pagos por transferencia
        public IActionResult Index()
        {
            var pagos = _context.PagosTransferencia
                .OrderByDescending(p => p.FechaPago)
                .ToList();

            return View(pagos);
        }

        // ✅ Mostrar formulario para registrar transferencia
        public IActionResult Transferencia()
        {
            return View();
        }

        // ✅ Registrar transferencia - POST (con imagen)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transferencia(PagoTransferencia modelo, IFormFile Imagen)
        {
            if (ModelState.IsValid)
            {
                // Subir imagen si existe
                if (Imagen != null && Imagen.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    modelo.ImagenComprobante = "/uploads/" + nombreArchivo;
                }

                modelo.FechaPago = DateTime.Now;
                _context.PagosTransferencia.Add(modelo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(modelo);
        }
    }
}
