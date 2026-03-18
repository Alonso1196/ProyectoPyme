using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoPyme.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Stripe
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Auth (Cookies)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuentas/Login";
        options.AccessDeniedPath = "/Cuentas/AccessDenied";
    });

builder.Services.AddAuthorization();


// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Manejo de Errores (equivalente a Global.asax / Application_Error)
app.UseExceptionHandler("/Error");
// Captura codigos de estado HTTP sin cuerpo (404, 403, etc.)
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

// Servir imágenes desde Uploads (fuera de wwwroot) para evitar que Hot Reload reinicie la app
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, "Uploads");
Directory.CreateDirectory(uploadsPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Asegurar carpeta centralizada para imágenes en wwwroot/images/Productos
var imagesFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "images", "Productos");
Directory.CreateDirectory(imagesFolder);

// Copiar imágenes existentes de Uploads a la carpeta centralizada si aún no existen
if (Directory.Exists(uploadsPath))
{
    foreach (var file in Directory.GetFiles(uploadsPath))
    {
        try
        {
            var dest = Path.Combine(imagesFolder, Path.GetFileName(file));
            if (!System.IO.File.Exists(dest))
            {
                System.IO.File.Copy(file, dest);
            }
        }
        catch
        {
            // noop: no detener la aplicación por un archivo que no se pudo copiar
        }
    }
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
