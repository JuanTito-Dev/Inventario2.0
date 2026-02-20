using InventarioSuper.Data.Migrations;
using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using InventarioSuperModelos;
using InventarioSuperModelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace InventarioSuper.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]

    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedortrabajo;
        private readonly IWebHostEnvironment _carpetas;

        public SlidersController(IContenedorTrabajo trabajo , IWebHostEnvironment carpetas)
        {
            _contenedortrabajo = trabajo;
            _carpetas = carpetas;
        }

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        // GET: ProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Slider slider, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto is not null)
                {
                    string RutaPrincipal = _carpetas.WebRootPath;
                    var extencion = Path.GetExtension(foto.FileName);
                    string[] validos = { ".jpg", ".png", ".jpeg" };
                    if (!validos.Contains(extencion))
                    {
                        ModelState.AddModelError("url", "La imagen debe ser de tipo jpg, png o jpeg.");
                        return View(slider);
                    }
                    string Nombre = Guid.NewGuid().ToString();
                    string ruta = Path.Combine(RutaPrincipal, "Imagenes", "Sliders");

                    try
                    {
                        using (var file = new FileStream(Path.Combine(ruta, Nombre + extencion), FileMode.Create))
                        {
                            await foto.CopyToAsync(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("url", "Error al subir la imagen: " + ex.Message);
                        return View(slider);
                    }



                    slider.Imagen = @"/Imagenes/Sliders/" + Nombre + extencion;

                    await _contenedortrabajo.Slider.Add(slider);
                    await _contenedortrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("url", "La imagen es obligatoria y debe ser de tipo jpg, png o jpeg.");
                    return View(slider);
                }
            }
            return View(slider);
        }

        // GET: ProductosController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Editar(int? id)
        {
            if (id != null)
            {
                var slider = await _contenedortrabajo.Slider.Get(id.Value);

                if (slider == null)
                {
                    return NotFound();
                }
                return View(slider);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Slider slider, IFormFile? foto)
        {
            var sliderDb = await _contenedortrabajo.Slider.Get(slider.Id);

            if (sliderDb is null || sliderDb.Imagen is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var rutaPrincipal = _carpetas.WebRootPath;

                if (foto is not null) { 
                    
                    var extencion = Path.GetExtension(foto.FileName);
                    string[] validos = { ".jpg", ".png", ".jpeg" };

                    if (!validos.Contains(extencion))
                    {
                        ModelState.AddModelError("Imagen", "La imagen debe ser de tipo jpg, png o jpeg.");
                        return View(slider);
                    }

                    string Nombre = Guid.NewGuid().ToString();
                    string ruta = Path.Combine(rutaPrincipal, "Imagenes", "Sliders");
                    var RutaAnterior = Path.Combine(rutaPrincipal, sliderDb.Imagen.TrimStart('/'));

                    if (System.IO.File.Exists(RutaAnterior))
                    {
                        System.IO.File.Delete(RutaAnterior);
                        Console.WriteLine($"Se elimino la foto {RutaAnterior}");
                    }
                    else
                    {
                        Console.WriteLine($"No se elimino la foto {RutaAnterior}");
                    }

                    try
                    {
                        using (var file = new FileStream(Path.Combine(ruta, Nombre + extencion), FileMode.Create))
                        {
                            await foto.CopyToAsync(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Imagen", "Error al subir la imagen: " + ex.Message);
                        return View(slider);
                    }
                    slider.Imagen = @"/Imagenes/Sliders/" + Nombre + extencion; // Actualizar la imagen con la nueva

                }
                else
                {
                    slider.Imagen = sliderDb.Imagen; // Mantener la imagen existente si no se sube una nueva
                }
                _contenedortrabajo.Slider.update(slider);
                await _contenedortrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View (slider);

        }


        #region Llamadas a la Api
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Sliders = await _contenedortrabajo.Slider.GetAll();
            return Json(new { data = Sliders });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _contenedortrabajo.Slider.Get(id);
            if (slider == null || slider.Imagen is null)
            {
                return Json(new { success = false, message = "Error al eliminar el slider" });
            }

            string RutaPrincipal = _carpetas.WebRootPath;
            var rutaAnterior = Path.Combine(RutaPrincipal, slider.Imagen.TrimStart('/'));
            if (System.IO.File.Exists(rutaAnterior))
            {
                System.IO.File.Delete(rutaAnterior);
                Console.WriteLine($"Se elimino la foto {rutaAnterior}");
            }
            else
            {
                Console.WriteLine($"No se elimino la foto {rutaAnterior}");
            }
            await _contenedortrabajo.Slider.Remove(slider);
            await _contenedortrabajo.Save();

            return Json(new { success = true, message = "Categoría Borrada Correctamente" });
        }
        #endregion
    }
}
