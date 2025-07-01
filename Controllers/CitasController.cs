using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TropiNailsPro.Models;
using System.Threading.Tasks;
using System.Linq;
using TropiNailsPro.Data;

namespace TropiNailsPro.Controllers
{
    public class CitasController : Controller
    {
        private readonly AppDbContext _context;

        public CitasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Citas
        public async Task<IActionResult> Index()
        {
            var citas = await _context.Citas.ToListAsync();
            return View(citas);
        }

        // GET: Citas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Citas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cita cita)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cita);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

        // GET: Citas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // POST: Citas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cita cita)
        {
            if (id != cita.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Citas.AnyAsync(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cita);
        }

        // ✅ POST: Citas/Delete (para formulario desde la vista Index)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
