using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IngematApp.DAO;
using IngematApp.Models;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class SubFormatoController : Controller
    {
        private readonly SubFormatoDAO _subFormatoDAO;
        private readonly FormatoDAO _formatoDAO;
        private readonly CategoriaDAO _categoriaDAO;

        public SubFormatoController(SubFormatoDAO subFormatoDAO, FormatoDAO formatoDAO, CategoriaDAO categoriaDAO)
        {
            _subFormatoDAO = subFormatoDAO;
            _formatoDAO = formatoDAO;
            _categoriaDAO = categoriaDAO;
        }

        public IActionResult Index()
        {
            return View(_subFormatoDAO.ListarSubFormatos());
        }

        public IActionResult ListarSubFormatosPorNombre(string pNombre)
        {
            return View("Index", _subFormatoDAO.ListarSubFormatosPorNombre(pNombre));
        }

        public IActionResult Create()
        {
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();
            return View();
        }

        [HttpPost]
        public IActionResult Create(SubFormato subFormato)
        {
            if (ModelState.IsValid)
            {
                _subFormatoDAO.InsertarSubFormato(subFormato);
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();
            return View(subFormato);
        }

        public IActionResult Edit(int id)
        {
            var sub = _subFormatoDAO.ObtenerSubFormatoPorId(id);
            if (sub.IdSubFormato == 0) return NotFound();

            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();

            // Buscamos a qué categoría pertenece el formato actual para seleccionarlo en el desplegable
            var formatoActual = _formatoDAO.ObtenerFormatoPorId(sub.IdFormato);
            ViewBag.IdCategoriaSeleccionada = formatoActual.IdCategoria;

            return View(sub);
        }

        [HttpPost]
        public IActionResult Edit(SubFormato subFormato)
        {
            if (ModelState.IsValid)
            {
                _subFormatoDAO.ActualizarSubFormato(subFormato);
                return RedirectToAction("Index");
            }
            ViewBag.Categorias = _categoriaDAO.ListarCategorias();
            ViewBag.Formatos = _formatoDAO.ListarFormatos();
            return View(subFormato);
        }

        public IActionResult Delete(int id)
        {
            _subFormatoDAO.EliminarSubFormato(id);
            return RedirectToAction("Index");
        }
    }
}