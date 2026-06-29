using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data;
using IngematApp.DAO;

namespace IngematApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly EmpleadoDAO _empleadoDAO;

        public AuthController(EmpleadoDAO empleadoDAO)
        {
            _empleadoDAO = empleadoDAO;
        }

        // GET: Muestra la pantalla de Login
        public IActionResult Login()
        {
            // Si ya está logueado, lo mandamos al menú principal
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Procesa las credenciales
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string dni)
        {
            DataTable dt = _empleadoDAO.ValidarLogin(correo, dni);

            if (dt.Rows.Count > 0)
            {
                string id = dt.Rows[0]["IdEmpleado"].ToString() ?? "";
                string nombre = dt.Rows[0]["NombreEmpleado"].ToString() ?? "";
                string cargo = dt.Rows[0]["Cargo"].ToString() ?? "";

                // Guardamos los datos en la memoria del navegador
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, id),
                    new Claim(ClaimTypes.Name, nombre),
                    new Claim(ClaimTypes.Role, cargo),
                    new Claim(ClaimTypes.Email, correo),
                    new Claim("Dni", dni),
                    new Claim("IdEmpleado", id)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas o usuario inactivo.";
            return View();
        }

        // GET: Cierra la sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Pantalla de bloqueo
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}