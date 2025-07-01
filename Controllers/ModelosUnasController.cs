using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TropiNailsPro.Data;
using TropiNailsPro.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace TropiNailsPro.Controllers
{
    public class ModelosUnasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ModelosUnasController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 🔓 Todos pueden ver los modelos
        public async Task<IActionResult> Index()
        {
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            return View(await _context.ModelosUnas.ToListAsync());
        }

        // ✅ SOLO Arturito puede crear modelos
        public IActionResult Create()
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModeloUnas modelo)
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
                return View(modelo);

            if (modelo.ImagenArchivo == null && string.IsNullOrWhiteSpace(modelo.ImagenUrl))
            {
                ModelState.AddModelError("", "Debes subir una imagen o pegar una URL válida.");
                return View(modelo);
            }

            if (modelo.ImagenArchivo != null && modelo.ImagenArchivo.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "modelos");
                Directory.CreateDirectory(uploadsFolder);

                string extension = Path.GetExtension(modelo.ImagenArchivo.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await modelo.ImagenArchivo.CopyToAsync(fileStream);
                }

                modelo.ImagenUrl = "/images/modelos/" + fileName;
            }

            _context.Add(modelo);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "✅ Modelo guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // ✅ SOLO Arturito puede editar modelos
        public async Task<IActionResult> Edit(int? id)
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");
            if (id == null) return NotFound();

            var modelo = await _context.ModelosUnas.FindAsync(id);
            return modelo == null ? NotFound() : View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ModeloUnas modelo)
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");
            if (id != modelo.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(modelo);

            try
            {
                var modeloOriginal = await _context.ModelosUnas.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

                if (modelo.ImagenArchivo != null && modelo.ImagenArchivo.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "modelos");
                    Directory.CreateDirectory(uploadsFolder);

                    string extension = Path.GetExtension(modelo.ImagenArchivo.FileName);
                    string fileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await modelo.ImagenArchivo.CopyToAsync(fileStream);
                    }

                    modelo.ImagenUrl = "/images/modelos/" + fileName;

                    // Borrar imagen anterior local
                    if (!string.IsNullOrEmpty(modeloOriginal.ImagenUrl) && modeloOriginal.ImagenUrl.StartsWith("/images/modelos/"))
                    {
                        string rutaAntigua = Path.Combine(_env.WebRootPath, modeloOriginal.ImagenUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(rutaAntigua))
                        {
                            System.IO.File.Delete(rutaAntigua);
                        }
                    }
                }
                else
                {
                    modelo.ImagenUrl = modeloOriginal.ImagenUrl;
                }

                _context.Update(modelo);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "✅ Modelo editado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloUnasExists(modelo.Id)) return NotFound();
                else throw;
            }
        }

        // ✅ SOLO Arturito puede eliminar modelos
        public async Task<IActionResult> Delete(int? id)
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");
            if (id == null) return NotFound();

            var modelo = await _context.ModelosUnas.FindAsync(id);
            return modelo == null ? NotFound() : View(modelo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!EsArturito()) return RedirectToAction("Login", "Auth");

            var modelo = await _context.ModelosUnas.FindAsync(id);
            if (modelo != null)
            {
                if (!string.IsNullOrEmpty(modelo.ImagenUrl) && modelo.ImagenUrl.StartsWith("/images/modelos/"))
                {
                    string rutaImagen = Path.Combine(_env.WebRootPath, modelo.ImagenUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                }

                _context.ModelosUnas.Remove(modelo);
                await _context.SaveChangesAsync();
            }

            TempData["Mensaje"] = "🗑️ Modelo eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // 🔓 Todas las clientas pueden elegir un modelo
        public async Task<IActionResult> Elegir(int id)
        {
            var modelo = await _context.ModelosUnas.FindAsync(id);
            return modelo == null ? NotFound() : View(modelo);
        }

        private bool ModeloUnasExists(int id)
        {
            return _context.ModelosUnas.Any(e => e.Id == id);
        }

        // 🔐 Verifica si el usuario es Arturito
        private bool EsArturito()
        {
            var usuario = HttpContext.Session.GetString("UsuarioNombre");
            return string.Equals(usuario, "Arturito", StringComparison.OrdinalIgnoreCase);
        }
    }
}
