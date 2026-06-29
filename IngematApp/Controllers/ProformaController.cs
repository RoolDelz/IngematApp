using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Area de ventas")]
    public class ProformaController : Controller
    {
        private readonly ProformaDAO _proformaDAO;
        private readonly CategoriaDAO _categoriaDAO;
        private readonly FormatoDAO _formatoDAO;

        public ProformaController(ProformaDAO proformaDAO, CategoriaDAO categoriaDAO, FormatoDAO formatoDAO)
        {
            _proformaDAO = proformaDAO;
            _categoriaDAO = categoriaDAO;
            _formatoDAO = formatoDAO;
        }

        // Mostrar formulario
        public IActionResult Create()
        {
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();
            return View();
        }



        // 1. AGREGA ESTE MÉTODO PARA MOSTRAR LA TABLA
        public IActionResult Index()
        {
            var proformas = _proformaDAO.ListarProformas();
            return View(proformas);
        }

        // ... (Tu método Create GET se queda igual) ...

        // 2. MODIFICA EL REDIRECT DE TU MÉTODO CREATE POST
        [HttpPost]
        public IActionResult Create(ProformaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var formatoSeleccionado = _formatoDAO.ObtenerFormatoPorId(modelo.IdFormato);
                _proformaDAO.RegistrarCotizacionCompleta(modelo, formatoSeleccionado.PrecioFormato);

                // CÁMBIALO A ESTO: Para que tras guardar te regrese a la tabla de proformas
                return RedirectToAction("Index");
            }

            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();
            return View(modelo);
        }



        // ==========================================
        // BUSCADOR
        // ==========================================
        public IActionResult ListarProformasPorCliente(string pNombre)
        {
            var proformas = _proformaDAO.ListarProformasPorCliente(pNombre);
            return View("Index", proformas);
        }

        // ==========================================
        // IMPRIMIR
        // ==========================================
        public IActionResult Imprimir(int id)
        {
            var proforma = _proformaDAO.ObtenerProformaParaImpresion(id);
            if (proforma.IdProforma == 0) return NotFound();

            return View(proforma);
        }


    }
}