using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrackPay.datos;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

// Configurar la autenticaci�n basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Entrada/Entrada"; // Ruta al Login
        options.LogoutPath = "/Home/Logout"; // Ruta para Logout
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Duraci�n de la sesi�n
    });

// Construir la aplicaci�n despu�s de configurar los servicios
var app = builder.Build();

// Configurar el HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Agregar autenticaci�n al pipeline
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// IMPORTANTE: Esto inicia la aplicaci�n
app.Run();

