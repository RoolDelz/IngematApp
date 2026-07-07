using IngematApp.DAO;
using IngematApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Sub Gerente, Ayudante Tecnico")]
    public class ProyectoController : Controller
    {
        private readonly ProyectoDAO _dao;

        public ProyectoController(ProyectoDAO dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            var model = new ProyectoIndexViewModel
            {
                Activos = _dao.ListarActivos(),
                Finalizados = _dao.ListarFinalizados()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ObtenerOTs(int idProyecto)
        {
            var ots = _dao.ListarOTsPorProyecto(idProyecto);
            return PartialView("_OTsPartial", ots);
        }
    }
}
