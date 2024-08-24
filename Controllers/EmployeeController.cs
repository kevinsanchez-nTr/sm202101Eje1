using sm202101Eje1.Data;
using sm202101Eje1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace sm202101Eje1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly AplicationDBContext _context;

        public EmployeeController(
           ILogger<EmployeeController> logger,
           AplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees.Include(e => e.TypeEmployee).AsNoTracking().ToList();
            ViewBag.Employees = employees;
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var typeEmployees = _context.TypeEmployees.AsNoTracking().ToList();
            ViewBag.TypeEmployees = typeEmployees;
            return View();
        }

        [HttpPost]
        public IActionResult Save(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    employee.FechaCreacion = DateTime.Now;
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    // Redirigir a Index con parámetro success para mostrar alerta de éxito
                    return RedirectToAction("Index", new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No se pudo guardar el empleado.");
                    ModelState.AddModelError("", "Comunicate con el administrador.");
                }
            }

            ViewBag.TypeEmployees = _context.TypeEmployees.AsNoTracking().ToList();
            return View("Create", employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Include(e => e.TypeEmployee).AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var typeEmployees = _context.TypeEmployees.AsNoTracking().ToList();
            ViewBag.TypeEmployees = typeEmployees;
            return View(employee);
        }

        [HttpPost]
        public IActionResult Update(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);

                    if (existingEmployee != null)
                    {
                        existingEmployee.Nombre = employee.Nombre;
                        existingEmployee.Apellido = employee.Apellido;
                        existingEmployee.Dui = employee.Dui;
                        existingEmployee.NumeroTelefonico = employee.NumeroTelefonico;
                        existingEmployee.TypeEmployeeId = employee.TypeEmployeeId;
                        existingEmployee.FechaModificacion = DateTime.Now;
                        existingEmployee.EsActivo = employee.EsActivo;

                        _context.Employees.Update(existingEmployee);
                        _context.SaveChanges();
                        // Redirigir a Index con parámetro editSuccess para mostrar alerta de edición exitosa
                        return RedirectToAction("Index", new { editSuccess = true });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No se pudo actualizar el empleado.");
                    ModelState.AddModelError("", "Comunicate con el administrador.");
                }
            }

            ViewBag.TypeEmployees = _context.TypeEmployees.AsNoTracking().ToList();
            return View("Edit", employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var employee = _context.Employees.Find(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo borrar el empleado.");
                ModelState.AddModelError("", "Comunicate con el administrador.");
                return View("Delete", id);
            }

            return RedirectToAction("Index");
        }
    }
}
