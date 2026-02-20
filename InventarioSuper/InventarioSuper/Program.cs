using InventarioSuper.Data;
using InventarioSuperDatos.Data.Repositorio;
using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using InventarioSuperModelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventarioSuperDatos.Inicializador;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<Usuario, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

//Agregar contenedor de trabajo
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabjo>();

builder.Services.AddScoped<InciadorDbI, InciadorDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}



app.UseStaticFiles();

siembradedatos();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Usuarios}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();

void siembradedatos()
{
    using (var scope = app.Services.CreateScope())
    {
        var inicializador = scope.ServiceProvider.GetRequiredService<InciadorDbI>();
        inicializador.Inicializar();
    }
    
}