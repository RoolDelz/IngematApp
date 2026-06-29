using IngematApp.DAO;
using IngematApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Area de ventas")]
    public class FacturaController : Controller
    {
        private readonly FacturaDAO _dao;

        public FacturaController(FacturaDAO dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var model = new FacturaIndexViewModel
            {
                Pendientes = _dao.ListarPendientes(),
                Realizadas = _dao.ListarRealizadas(),
                Pagadas = _dao.ListarPagadas()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult GuardarMetodoPago(int IdFactura, string MetodoPago)
        {
            _dao.GuardarMetodoPago(IdFactura, MetodoPago);
            TempData["Mensaje"] = "Método de pago guardado correctamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Realizar(int IdFactura)
        {
            _dao.RealizarFactura(IdFactura);
            TempData["Mensaje"] = "Factura realizada exitosamente. Ya no se puede modificar.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Pagar(int IdFactura)
        {
            _dao.PagarFactura(IdFactura);
            TempData["Mensaje"] = "Pago confirmado. La factura ha sido marcada como Pagada.";
            return RedirectToAction("Index");
        }

        public IActionResult Imprimir(int id)
        {
            var model = _dao.ObtenerParaImpresion(id);
            if (model.IdFactura == 0) return RedirectToAction("Index");
            return View(model);
        }
    }
}
