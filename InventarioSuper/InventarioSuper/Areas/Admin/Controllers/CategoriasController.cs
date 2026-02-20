using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using InventarioSuperDatos.Data;
using InventarioSuperModelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InventarioSuper.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedor;

        public CategoriasController(IContenedorTrabajo contenedor)
        {
            _contenedor = contenedor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _contenedor.Categoria.Add(categoria);
                await _contenedor.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var categoria = await _contenedor.Categoria.Get(id);

            if (categoria is null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _contenedor.Categoria.update(categoria);
                await _contenedor.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        #region Apis
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new {Data = await _contenedor.Categoria.GetAll() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var categoria = await _contenedor.Categoria.Get(id);

            if (categoria == null)
            {
                return Json(new { success = false, message = "Error al eliminar la categoría" });
            }

            //consuk si la categoría existe
            var productos = await _contenedor.Producto.GetAll(p => p.CategoriaId == id);
            var cantidad = productos.Count();

            if (cantidad > 0)
            {
                return Json(new { success = false, message = $"No se puede eliminar la categoría porque tiene {cantidad} productos asociados" });

            }

            await _contenedor.Categoria.Remove(categoria);
            await _contenedor.Save();

            return Json(new { success = true, message = "Categoría Borrada Correctamente" });
        }
        #endregion
    }
}
