using Microsoft.AspNetCore.Mvc;
using TropiNailsPro.Data;
using TropiNailsPro.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace TropiNailsPro.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Proteger todo el controlador para que solo "Arturito" pueda acceder
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuario = context.HttpContext.Session.GetString("UsuarioNombre");
            if (!string.Equals(usuario, "Arturito", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Acceso restringido solo para la propietaria.";
                context.Result = new RedirectToActionResult("Index", "Dashboard", null);
                return;
            }
            base.OnActionExecuting(context);
        }

        // ✅ Listado de productos
        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        // ✅ Mostrar formulario para nuevo producto
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Guardar nuevo producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                TempData["Exito"] = "✅ Producto registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // ✅ Mostrar formulario para editar
        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // ✅ Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Update(producto);
                _context.SaveChanges();
                TempData["Exito"] = "✅ Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        // ✅ Confirmar eliminación
        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // ✅ Eliminar producto
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            _context.Productos.Remove(producto);
            _context.SaveChanges();
            TempData["Exito"] = "🗑️ Producto eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}
