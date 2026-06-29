using IngematApp.DAO;
using IngematApp.Models;
using Microsoft.AspNetCore.Authorization; // 1. IMPORTANTE: Agregar esta librería
using Microsoft.AspNetCore.Mvc;

namespace IngematApp.Controllers
{
    // 2. AQUÍ VA EL AUTHORIZE. Bloquea todo el controlador excepto para estos roles.
    [Authorize(Roles = "Gerente")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaDAO _categoriaDAO;

        public CategoriaController(CategoriaDAO categoriaDAO)
        {
            _categoriaDAO = categoriaDAO;
        }

        public IActionResult Index()
        {
            var categorias = _categoriaDAO.ListarCategorias();
            return View(categorias);
        }

        public IActionResult ListarCategoriasPorNombre(string pNombre)
        {
            var categorias = _categoriaDAO.ListarCategoriasPorNombre(pNombre);
            // Reutilizamos la misma vista Index para mostrar los resultados filtrados
            return View("Index", categorias);
        }



        // ==========================================
        // 1. MÉTODOS PARA CREAR (GUARDAR)
        // ==========================================

        // Este método muestra la pantalla vacía que acabamos de crear
        public IActionResult Create()
        {
            return View();
        }

        // Este método recibe los datos cuando presionas el botón verde "Guardar Categoría"
        [HttpPost]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaDAO.InsertarCategoria(categoria);
                return RedirectToAction("Index"); // Te devuelve a la tabla
            }
            return View(categoria);
        }

        // ==========================================
        // 2. MÉTODO PARA ELIMINAR
        // ==========================================

        // Este método se activa cuando presionas el botón rojo "Eliminar" en la tabla
        public IActionResult Delete(int id)
        {
            try
            {
                _categoriaDAO.EliminarCategoria(id);
            }
            catch (Exception ex)
            {
                // Si hay error (ej. la categoría está en uso), aquí podrías manejarlo
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index"); // Recarga la tabla actualizada
        }

        // ---------------------------------------------
        // ACTUALIZAR (EDITAR)
        // ---------------------------------------------
        public IActionResult Edit(int id)
        {
            // Busca la categoría por el ID para llenar el formulario
            var categoria = _categoriaDAO.ObtenerCategoriaPorId(id);
            if (categoria.IdCategoria == 0) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaDAO.ActualizarCategoria(categoria);
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        
    }
}