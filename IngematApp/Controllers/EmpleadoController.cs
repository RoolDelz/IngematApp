using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    // Candado para que temporalmente solo un Gerente administre este módulo
    [Authorize(Roles = "Gerente, Sub Gerente")]
    public class EmpleadoController : Controller
    {
        private readonly EmpleadoDAO _empleadoDAO;
        public EmpleadoController(EmpleadoDAO empleadoDAO) { _empleadoDAO = empleadoDAO; }

        public IActionResult Index()
        {
            return View(_empleadoDAO.ListarEmpleados());
        }

        public IActionResult ListarEmpleadosPorNombre(string pNombre)
        {
            return View("Index", _empleadoDAO.ListarEmpleadosPorNombre(pNombre));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _empleadoDAO.InsertarEmpleado(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Edit(int id)
        {
            var empleado = _empleadoDAO.ObtenerEmpleadoPorId(id);
            if (empleado.IdEmpleado == 0) return NotFound();
            return View(empleado);
        }

        [HttpPost]
        public IActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _empleadoDAO.ActualizarEmpleado(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Delete(int id)
        {
            _empleadoDAO.DesactivarEmpleado(id);
            return RedirectToAction("Index");
        }
    }
}