using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;
using System.Security.Claims;

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
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (empleado.Cargo == "Gerente")
            {
                TempData["Error"] = "No está permitido registrar perfiles de Gerente desde el sistema web.";
                return RedirectToAction("Index");
            }

            if (currentUserRole == "Sub Gerente" && empleado.Cargo == "Sub Gerente")
            {
                TempData["Error"] = "Un Sub Gerente no tiene permisos para registrar a otro Sub Gerente.";
                return RedirectToAction("Index");
            }

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

            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("IdEmpleado")?.Value ?? "0");

            if (id == currentUserId)
            {
                TempData["Error"] = "Por seguridad, no puedes modificar tu propio usuario.";
                return RedirectToAction("Index");
            }

            if (currentUserRole == "Sub Gerente" && (empleado.Cargo == "Gerente" || empleado.Cargo == "Sub Gerente"))
            {
                TempData["Error"] = "Un Sub Gerente no tiene permisos para modificar a un Gerente o a otro Sub Gerente.";
                return RedirectToAction("Index");
            }

            return View(empleado);
        }

        [HttpPost]
        public IActionResult Edit(Empleado empleado)
        {
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var currentUserId = int.Parse(User.FindFirst("IdEmpleado")?.Value ?? "0");

            if (empleado.IdEmpleado == currentUserId)
            {
                TempData["Error"] = "Por seguridad, no puedes modificar tu propio usuario.";
                return RedirectToAction("Index");
            }

            if (empleado.Cargo == "Gerente")
            {
                TempData["Error"] = "No está permitido asignar perfiles de Gerente desde el sistema web.";
                return RedirectToAction("Index");
            }

            if (currentUserRole == "Sub Gerente")
            {
                if (empleado.Cargo == "Sub Gerente")
                {
                    TempData["Error"] = "Un Sub Gerente no tiene permisos para promover a un empleado a Sub Gerente.";
                    return RedirectToAction("Index");
                }

                var oldEmpleado = _empleadoDAO.ObtenerEmpleadoPorId(empleado.IdEmpleado);
                if (oldEmpleado.Cargo == "Gerente" || oldEmpleado.Cargo == "Sub Gerente")
                {
                    TempData["Error"] = "Un Sub Gerente no tiene permisos para modificar a un Gerente o a otro Sub Gerente.";
                    return RedirectToAction("Index");
                }
            }

            if (ModelState.IsValid)
            {
                _empleadoDAO.ActualizarEmpleado(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Delete(int id)
        {
            var empleadoATratar = _empleadoDAO.ObtenerEmpleadoPorId(id);
            if (empleadoATratar.IdEmpleado == 0) return NotFound();

            var currentUserId = int.Parse(User.FindFirst("IdEmpleado")?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (id == currentUserId)
            {
                TempData["Error"] = "Por seguridad, no puedes inhabilitar tu propio usuario.";
                return RedirectToAction("Index");
            }

            if (currentUserRole == "Sub Gerente" && (empleadoATratar.Cargo == "Gerente" || empleadoATratar.Cargo == "Sub Gerente"))
            {
                TempData["Error"] = "Un Sub Gerente no tiene permisos para inhabilitar a un Gerente o a otro Sub Gerente.";
                return RedirectToAction("Index");
            }

            _empleadoDAO.DesactivarEmpleado(id);
            return RedirectToAction("Index");
        }
    }
}