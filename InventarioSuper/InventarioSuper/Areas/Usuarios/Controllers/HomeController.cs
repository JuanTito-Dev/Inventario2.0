using System.Diagnostics;
using InventarioSuper.Models;
using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using InventarioSuperModelos;
using InventarioSuperModelos.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventarioSuper.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    public class HomeController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public HomeController(IContenedorTrabajo contenedorTrabjo)
        {
            _contenedorTrabajo  = contenedorTrabjo;
        }

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var datos = new HomeVM()
        //    {
        //        Sliders = await  _contenedorTrabajo.Slider.GetAll(),
        //        Productos = await _contenedorTrabajo.Producto.GetAll()

        //    };
        //    ViewBag.IsHome = true;
        //    return View(datos);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pagezise = 8)
        {
            var articulos = _contenedorTrabajo.Producto.Buscar();
            var paginas = articulos.OrderBy(m => m.Id).Skip((page - 1) * pagezise).Take(pagezise).ToList();

            var datos = new HomeVM()
            {
                Sliders = await _contenedorTrabajo.Slider.GetAll(),
                Productos = paginas.ToList(),
                Pagesize = page,
                Totalpage = (int)Math.Ceiling(articulos.Count() / (double)pagezise)
            };

            ViewBag.IsHome = true;

            return View(datos);
        }

        [HttpGet]
        public IActionResult Buscar(string palabra, int pagina = 1, int tam = 8)
        {
            var articulos = _contenedorTrabajo.Producto.Buscar();

            if (!string.IsNullOrEmpty(palabra))
            {
                articulos = articulos.Where(p => p.Nombre.Contains(palabra));
            }

            var Paginas = articulos.OrderBy(m => m.Id).Skip((pagina - 1) * tam).Take(tam).ToList();
            var model = new ListaBuscada<Producto>(Paginas, articulos.Count(), pagina, tam, palabra );

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
