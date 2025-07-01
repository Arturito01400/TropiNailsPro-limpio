using Microsoft.AspNetCore.Mvc;
using TropiNailsPro.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TropiNailsPro.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TropiNailsPro.Controllers
{
    public class PagosController : Controller
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        // Vista principal de métodos de pago
        public IActionResult InicioPago()
        {
            return View();
        }

        // Vista de confirmación de cualquier pago
        public IActionResult Confirmacion()
        {
            if (ViewBag.Mensaje == null)
                ViewBag.Mensaje = "✅ Tu pago fue procesado correctamente. ¡Gracias por elegir TropiNails Pro!";
            return View();
        }

        // Mostrar formulario para transferencias
        [HttpGet]
        public IActionResult Transferencia()
        {
            return View();
        }

        // Procesar imagen del comprobante y guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transferencia(PagoTransferencia model, IFormFile Imagen)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Por favor completa todos los campos obligatorios.");
                return View(model);
            }

            if (Imagen == null || Imagen.Length == 0)
            {
                ModelState.AddModelError("", "Debes subir una imagen válida del comprobante.");
                return View(model);
            }

            try
            {
                var rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "comprobantes");
                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                model.ImagenComprobante = "/comprobantes/" + nombreArchivo;
                model.FechaPago = DateTime.Now;

                _context.PagosTransferencia.Add(model);
                await _context.SaveChangesAsync();

                ViewBag.Mensaje = "✅ Comprobante recibido correctamente. Te confirmaremos por WhatsApp.";
                return View("Confirmacion");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el comprobante: {ex.Message}");
                return View(model);
            }
        }

        // Simulación de pago PayPal (se guarda como transferencia con origen PayPal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcesarPago(string producto, decimal monto, string cliente, string modelo, string paypalId)
        {
            if (string.IsNullOrEmpty(producto) || monto <= 0 || string.IsNullOrEmpty(paypalId))
            {
                ViewBag.Mensaje = "❌ El pago no se pudo procesar. Datos inválidos.";
                return View("Confirmacion");
            }

            var nuevoPago = new PagoTransferencia
            {
                NombreCliente = cliente,
                BancoOrigen = "PayPal",
                NumeroReferencia = paypalId,
                Monto = monto,
                ImagenComprobante = null,
                FechaPago = DateTime.Now
            };

            _context.PagosTransferencia.Add(nuevoPago);
            await _context.SaveChangesAsync();

            ViewBag.Mensaje = $"✅ Tu pago de RD${monto:0.00} por '{modelo}' fue procesado exitosamente con PayPal.";
            return View("Confirmacion");
        }

        // Ver listado de pagos - solo para el admin "Arturito"
        public async Task<IActionResult> Lista()
        {
            var usuario = HttpContext.Session.GetString("UsuarioNombre");
            if (!string.Equals(usuario, "Arturito", StringComparison.OrdinalIgnoreCase))
            {
                // Si no es Arturito, redirige al login o dashboard según convenga
                return RedirectToAction("Login", "Auth");
            }

            var pagos = await _context.PagosTransferencia
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();

            return View(pagos);
        }
    }
}
