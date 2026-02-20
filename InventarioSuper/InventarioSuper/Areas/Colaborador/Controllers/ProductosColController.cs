using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventarioSuper.Areas.Colaborador.Controllers
{
    [Authorize(Roles = "Colaborador")]
    [Area("Colaborador")]
    public class ProductosColController : Controller
    {
        private readonly IContenedorTrabajo _contenedortrabajo;
        private readonly IWebHostEnvironment _carpetas;
        // GET: Productos
        public ProductosColController(IContenedorTrabajo context, IWebHostEnvironment _carpetas)
        {
            _contenedortrabajo = context;
            this._carpetas = _carpetas;
        }

        public IActionResult Detalles()
        {
            return View();
        }

        #region Llmadas a la Api
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _contenedortrabajo.Producto.GetAll(includeProperties: "Categoria");
            return Json(new { data = productos });
        }
        #endregion
    }
}
