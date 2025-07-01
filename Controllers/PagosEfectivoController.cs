using Microsoft.AspNetCore.Mvc;
using TropiNailsPro.Data;
using TropiNailsPro.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace TropiNailsPro.Controllers
{
    public class PagosEfectivoController : Controller
    {
        private readonly AppDbContext _context;

        public PagosEfectivoController(AppDbContext context)
        {
            _context = context;
        }

        // Restringir acceso solo a "Arturito"
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuario = context.HttpContext.Session.GetString("Usuario");
            if (usuario != "Arturito")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            var pagos = _context.PagosEfectivo.OrderByDescending(p => p.FechaPago).ToList();
            return View(pagos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PagoEfectivo pago)
        {
            if (ModelState.IsValid)
            {
                pago.FechaPago = DateTime.Now;
                _context.PagosEfectivo.Add(pago);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }
    }
}
