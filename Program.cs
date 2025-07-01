using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TropiNailsPro.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// 🔌 1. Conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("ConexionMysql");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 🎨 2. Controladores + vistas + mensajes personalizados
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        _ => "Este campo es obligatorio.");
});

// 🗂 3. Sesión habilitada (30 minutos)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🌍 4. Cultura regional (es-DO por defecto)
var supportedCultures = new[] { new CultureInfo("es-DO"), new CultureInfo("en-US") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-DO");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// 📡 5. Acceso a HttpContext
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 🛠 6. Aplicar migraciones automáticas al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// 🔐 7. Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Soporte para wwwroot (CSS, JS, imágenes)
app.UseRouting();

app.UseRequestLocalization(); // Cultura regional
app.UseSession();             // Sesiones
app.UseAuthentication();      // Si usas autenticación
app.UseAuthorization();       // Permisos y validaciones

// 🧭 8. Ruta por defecto al login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
