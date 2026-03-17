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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
