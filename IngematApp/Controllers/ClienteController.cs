using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class ClienteController : Controller
    {
        private readonly ClienteDAO _clienteDAO;

        public ClienteController(ClienteDAO clienteDAO)
        {
            _clienteDAO = clienteDAO;
        }

        // Muestra la tabla con el listado general de clientes
        public IActionResult Index()
        {
            var clientes = _clienteDAO.ListarClientes();
            return View(clientes);
        }

        // Carga el formulario de edición con los datos del cliente seleccionado
        public IActionResult Edit(int id)
        {
            var cliente = _clienteDAO.ObtenerClientePorId(id);
            if (cliente.IdCliente == 0) return NotFound();
            return View(cliente);
        }

        // Recibe los datos corregidos y ejecuta la actualización
        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _clienteDAO.ActualizarCliente(cliente);
                return RedirectToAction("Index"); // Regresa al listado tras guardar
            }
            return View(cliente);
        }
    }
}