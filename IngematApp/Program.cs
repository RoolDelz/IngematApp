using Microsoft.AspNetCore.Authentication.Cookies;
using IngematApp.DAO;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURAR LÍMITE DE SUBIDA DE ARCHIVOS EN KESTREL
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB global
});

builder.Services.AddControllersWithViews();

// Configurar límite de subida de formularios multipart
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
});

// 1. CONFIGURACIÓN DE COOKIES Y LOGIN AUTOMÁTICO
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Redirige aquí si no hay sesión
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Redirige aquí si el rol no coincide
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // La sesión dura toda la jornada laboral
    });

// 2. REGISTRO DE TUS DAOs
builder.Services.AddScoped<CategoriaDAO>();
builder.Services.AddScoped<FormatoDAO>();
builder.Services.AddScoped<SubFormatoDAO>();
builder.Services.AddScoped<ClienteDAO>();
builder.Services.AddScoped<ProformaDAO>();
builder.Services.AddScoped<OrdenServicioDAO>();
builder.Services.AddScoped<EmpleadoDAO>();
builder.Services.AddScoped<OrdenTrabajoDAO>();
builder.Services.AddScoped<FacturaDAO>();
builder.Services.AddScoped<ProyectoDAO>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Atrapa excepciones y muestra detalle en el navegador
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. ESTAS DOS LÍNEAS DEBEN IR EN ESTE ORDEN EXACTO
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();