using IngematApp.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IngematApp.Controllers
{
    [Authorize(Roles = "Gerente, Ayudante Tecnico")]
    public class OrdenTrabajoController : Controller
    {
        private readonly OrdenTrabajoDAO _dao;
        private readonly IWebHostEnvironment _env;

        public OrdenTrabajoController(OrdenTrabajoDAO dao, IWebHostEnvironment env)
        {
            _dao = dao;
            _env = env;
        }

        public IActionResult Index()
        {
            // Obtener el rol del usuario logueado por Claims
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol == "Gerente")
            {
                ViewBag.Tecnicos = _dao.ListarTecnicos();
                return View(_dao.ListarParaGerente());
            }
            else
            {
                // Si es técnico, obtener su ID de los claims
                var idEmpleadoClaim = User.FindFirst("IdEmpleado")?.Value;
                int idEmpleado = string.IsNullOrEmpty(idEmpleadoClaim) ? 0 : int.Parse(idEmpleadoClaim);

                return View(_dao.ListarParaTecnico(idEmpleado));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")]
        public IActionResult Asignar(int IdOT, int IdEmpleado)
        {
            _dao.AsignarTecnico(IdOT, IdEmpleado);
            TempData["Mensaje"] = "Técnico asignado correctamente.";
            return RedirectToAction("Index");
        }

        // NUEVOS MÉTODOS DEL WORKSPACE (GESTOR DOCUMENTAL)

        [HttpGet]
        public IActionResult Workspace(int id)
        {
            var model = _dao.ObtenerWorkspace(id);
            if (model.IdOT == 0) return RedirectToAction("Index"); // Si no existe, vuelve al listado

            model.Archivos = _dao.ListarReportes(id);

            // LECTURA DEL ARCHIVO .TXT (Si existe)
            string carpetaDestino = Path.Combine(_env.WebRootPath, "ArchivosOT", $"Proyecto_{model.IdProyecto}", $"OT_{model.IdOT}");
            string rutaTxt = Path.Combine(carpetaDestino, $"{model.N_OT}.txt");

            if (System.IO.File.Exists(rutaTxt))
            {
                model.ComentariosTexto = System.IO.File.ReadAllText(rutaTxt);
            }

            return View(model);
        }

        [HttpPost]
        [RequestSizeLimit(52_428_800)] // 50 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 52_428_800)]
        public async Task<IActionResult> GuardarWorkspace(int IdOT, string ComentariosTexto, List<IFormFile> ArchivosNuevos)
        {
            try
            {
                var ot = _dao.ObtenerWorkspace(IdOT);

                // 1. Validar que wwwroot exista
                if (string.IsNullOrEmpty(_env.WebRootPath))
                {
                    TempData["Error"] = "Error crítico: La carpeta 'wwwroot' no existe en el proyecto. Debes crearla en la raíz de tu proyecto en Visual Studio.";
                    return RedirectToAction("Workspace", new { id = IdOT });
                }

                // Crear la estructura de carpetas anidadas
                string carpetaDestino = Path.Combine(_env.WebRootPath, "ArchivosOT", $"Proyecto_{ot.IdProyecto}", $"OT_{ot.IdOT}");
                if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                // 2. GUARDAR O ACTUALIZAR EL ARCHIVO .TXT
                string rutaTxt = Path.Combine(carpetaDestino, $"{ot.N_OT}.txt");
                await System.IO.File.WriteAllTextAsync(rutaTxt, ComentariosTexto ?? "");

                // 3. PROCESAR MÚLTIPLES ARCHIVOS
                long limiteMegas = 5 * 1024 * 1024; // 5 MB
                string[] extensionesValidas = { ".pdf", ".jpg", ".jpeg", ".png", ".docx", ".doc", ".xls", ".xlsx" };

                if (ArchivosNuevos != null && ArchivosNuevos.Count > 0)
                {
                    foreach (var archivo in ArchivosNuevos)
                    {
                        if (archivo.Length > 0)
                        {
                            if (archivo.Length > limiteMegas)
                            {
                                TempData["Error"] = $"El archivo {archivo.FileName} supera los 5MB permitidos.";
                                continue;
                            }

                            string extension = Path.GetExtension(archivo.FileName).ToLower();
                            if (!extensionesValidas.Contains(extension))
                            {
                                TempData["Error"] = $"Extensión no válida en el archivo {archivo.FileName}.";
                                continue;
                            }

                            // Guardado físico y relacional
                            string nombreUnico = Guid.NewGuid().ToString().Substring(0, 8) + "_" + archivo.FileName;
                            string rutaFisica = Path.Combine(carpetaDestino, nombreUnico);
                            string rutaBD = $"/ArchivosOT/Proyecto_{ot.IdProyecto}/OT_{ot.IdOT}/{nombreUnico}";

                            using (var stream = new FileStream(rutaFisica, FileMode.Create))
                            {
                                await archivo.CopyToAsync(stream);
                            }

                            _dao.InsertarReporte(IdOT, archivo.FileName, extension, rutaBD);
                        }
                    }
                }

                TempData["Mensaje"] = "Progreso del Workspace guardado correctamente.";
                return RedirectToAction("Workspace", new { id = IdOT });
            }
            catch (Exception ex)
            {
                // En lugar de que la app se muera, atrapamos el error y lo mostramos en la vista
                TempData["Error"] = $"Ocurrió un error al guardar: {ex.Message}";
                return RedirectToAction("Workspace", new { id = IdOT });
            }
        }

        [HttpPost]
        public IActionResult EliminarArchivo(int idReporte, string rutaArchivo, int idOt)
        {
            // Eliminar de disco físico local
            string rutaFisica = Path.Combine(_env.WebRootPath, rutaArchivo.TrimStart('/'));
            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }
            // Eliminar registro de la Base de Datos
            _dao.EliminarReporte(idReporte);

            TempData["Mensaje"] = "Archivo eliminado correctamente.";
            return RedirectToAction("Workspace", new { id = idOt });
        }

        [HttpPost]
        public IActionResult FinalizarOT(int idOt)
        {
            _dao.FinalizarOrdenTrabajo(idOt);
            TempData["Mensaje"] = "Orden de Trabajo finalizada con éxito. Si no quedan tareas pendientes, el Proyecto se ha cerrado automáticamente.";
            return RedirectToAction("Index");
        }
    }
}