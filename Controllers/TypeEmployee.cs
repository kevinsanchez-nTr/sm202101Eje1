using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sm202101Eje1.Data;
using sm202101Eje1.Models;

namespace sm202101Eje1.Controllers
{
    public class TypeEmployeeController : Controller
    {
        private readonly ILogger<TypeEmployeeController> _logger;
        private readonly AplicationDBContext _context;

        public TypeEmployeeController(
           ILogger<TypeEmployeeController> logger,
           AplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var typeemployees = _context.TypeEmployees.ToList();
            ViewBag.TypeEmployees = typeemployees;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(Models.TypeEmployee typeemployee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    typeemployee.FechaCreacion = DateTime.Now;
                    _context.Add(typeemployee);
                    _context.SaveChanges();

                    // Redirigir a Index con el parámetro success=true
                    return RedirectToAction("Index", new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No se pudo guardar el tipo de empleado.");
                    ModelState.AddModelError("", "Comunicate con el administrador.");
                }
            }

            // Si algo falla, volvemos a la vista Create con el modelo actual
            return View("Create", typeemployee);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var typeemployee = _context.TypeEmployees.Where(x => x.Id == id).FirstOrDefault();

            if (typeemployee == null)
            {
                return NotFound();
            }

            return View(typeemployee);
        }

        [HttpPost]
        public IActionResult Update(Models.TypeEmployee typeemployee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingTypeEmployee = _context.TypeEmployees.FirstOrDefault(e => e.Id == typeemployee.Id);

                    if (existingTypeEmployee != null)
                    {
                        existingTypeEmployee.Nombre = typeemployee.Nombre;
                        existingTypeEmployee.FechaModificacion = DateTime.Now;
                        existingTypeEmployee.EsActivo = typeemployee.EsActivo;

                        _context.TypeEmployees.Update(existingTypeEmployee);
                        _context.SaveChanges();

                        // Redirigir a Index con el parámetro editSuccess=true
                        return RedirectToAction("Index", new { editSuccess = true });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No se pudo actualizar el tipo de empleado.");
                    ModelState.AddModelError("", "Comunicate con el administrador.");
                }
            }

            return View("Edit", typeemployee);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var typeemployee = _context.TypeEmployees.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (typeemployee == null)
            {
                return NotFound();
            }
            return View(typeemployee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var typeemployee = _context.TypeEmployees.Find(id);
                if (typeemployee != null)
                {
                    _context.TypeEmployees.Remove(typeemployee);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo borrar el tipo de empleado.");
                ModelState.AddModelError("", "Comunicate con el administrador.");
                return View("Delete", id);
            }

            return RedirectToAction("Index");
        }
    }
}
