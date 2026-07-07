using IngematApp.DAO;
using IngematApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Sub Gerente, Area de ventas")]
    public class OrdenServicioController : Controller
    {
        private readonly OrdenServicioDAO _osDAO;
        public OrdenServicioController(OrdenServicioDAO osDAO) { _osDAO = osDAO; }

        // Pantalla 1: Listado general de OS
        public IActionResult Index()
        {
            return View(_osDAO.ListarOrdenesServicio());
        }

        // Pantalla 2: Listado de Cotizaciones sin OS asignada
        public IActionResult SeleccionarProforma()
        {
            return View(_osDAO.ListarProformasSinOS());
        }

        // Pantalla 3: Formulario (GET) pasándole la cotización elegida
        public IActionResult Create(int idProforma)
        {
            var datosProforma = _osDAO.ObtenerDatosProformaParaOS(idProforma);
            if (datosProforma.IdProforma == 0) return RedirectToAction("SeleccionarProforma");

            // Llenamos el ViewModel con la información de solo lectura
            var modelo = new OrdenServicioViewModel
            {
                IdProforma = datosProforma.IdProforma,
                NomCliente = datosProforma.NomCliente,
                Motivo = datosProforma.Motivo,
                PrecioFinal = datosProforma.PrecioFinal
            };

            return View(modelo);
        }

        // Pantalla 3: Guardar registro (POST)
        [HttpPost]
        public IActionResult GuardarOS(OrdenServicioViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                _osDAO.InsertarOrdenServicio(modelo);
                return RedirectToAction("Index");
            }
            return View("Create", modelo);
        }

        [Authorize(Roles = "Gerente, Sub Gerente")]
        // Pantalla 4: Bandeja de Aprobación para Gerente
        public IActionResult BandejaAprobacion()
        {
            return View(_osDAO.ListarOSPendientes());
        }


        [Authorize(Roles = "Gerente, Sub Gerente")]
        // Pantalla 4: Procesar Aprobación (POST)
        [HttpPost]
        public IActionResult ProcessAprobacion(int idOS)
        {
            _osDAO.AprobarOrdenServicio(idOS);
            return RedirectToAction("BandejaAprobacion");
        }
    }
}