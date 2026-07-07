using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Sub Gerente")]
    public class FormatoController : Controller
    {
        private readonly FormatoDAO _formatoDAO;
        private readonly CategoriaDAO _categoriaDAO;

        // Inyectamos AMBOS DAOs
        public FormatoController(FormatoDAO formatoDAO, CategoriaDAO categoriaDAO)
        {
            _formatoDAO = formatoDAO;
            _categoriaDAO = categoriaDAO;
        }

        public IActionResult Index()
        {
            var formatos = _formatoDAO.ListarFormatos();
            return View(formatos);
        }

        // GET: Create
        public IActionResult Create()
        {
            // Enviamos la lista de categorías a la vista para llenar el <select>
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            return View();
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(Formato formato)
        {
            if (ModelState.IsValid)
            {
                _formatoDAO.InsertarFormato(formato);
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = _categoriaDAO.ListarCategorias(); // Recargar si hay error
            return View(formato);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var formato = _formatoDAO.ObtenerFormatoPorId(id);
            if (formato.IdFormato == 0) return NotFound();

            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            return View(formato);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(Formato formato)
        {
            if (ModelState.IsValid)
            {
                _formatoDAO.ActualizarFormato(formato);
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            return View(formato);
        }

        // ==========================================
        // READ: Buscador por Nombre (Lupita)
        // ==========================================
        public IActionResult ListarFormatosPorNombre(string pNombre)
        {
            var formatos = _formatoDAO.ListarFormatosPorNombre(pNombre);
            return View("Index", formatos); // Reutiliza la vista de la tabla
        }

        // ==========================================
        // DELETE: Borrado directo
        // ==========================================
        public IActionResult Delete(int id)
        {
            try
            {
                _formatoDAO.EliminarFormato(id);
            }
            catch (Exception)
            {
                // Manejo de error si el formato está en uso
            }
            return RedirectToAction("Index");
        }



    }
}