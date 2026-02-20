using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using InventarioSuperModelos;
using InventarioSuperModelos.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace InventarioSuper.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]

    [Area("Admin")]
    public class ProductosController : Controller
    {
        private readonly IContenedorTrabajo _contenedortrabajo;
        private readonly IWebHostEnvironment _carpetas;

        public ProductosController(IContenedorTrabajo trabajo , IWebHostEnvironment carpetas)
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
            ProductoVM nuevo = new ProductoVM()
            {
                _Producto = new InventarioSuperModelos.Producto(),
                ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias()
            };
            return View(nuevo);
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(ProductoVM Producto, IFormFile? foto)
        {
            if (ModelState.IsValid)
            {
                string RutaPrincipal = _carpetas.WebRootPath;
                if (foto is not null && foto.Length > 0)
                {
                    var extencion = Path.GetExtension(foto.FileName);
                    string[] validos = { ".jpg", ".png", ".jpeg" };

                    if (!validos.Contains(extencion))
                    {
                        ModelState.AddModelError("url", "La imagen debe ser de tipo jpg, png o jpeg.");
                        Producto.ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias();
                        return View(Producto);
                    }

                    string Nombre = Guid.NewGuid().ToString();
                    string ruta = Path.Combine(RutaPrincipal,"Imagenes", "Productos");
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
                        Producto.ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias();
                        return View(Producto);
                    }



                    Producto._Producto.url = @"/Imagenes/Productos/" + Nombre + extencion;
                }
                else
                {
                    Producto._Producto.url = @"/Imagenes/Productos/PorDefecto/ProductoDefecto.jpg"; // Imagen por defecto si no se sube una nueva
                }

                await _contenedortrabajo.Producto.Add(Producto._Producto);
                await _contenedortrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            Producto.ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias();
            return View(Producto);
        }

        // GET: ProductosController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Editar(int? id)
        {
            ProductoVM nuevo = new ProductoVM()
            {
                _Producto = new Producto(),
                ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias()
            };



            if (id != null)
            {
                var producto = await _contenedortrabajo.Producto.Get(id);
                if (producto != null)
                {
                    nuevo._Producto = producto;
                }
                else
                {
                    return NotFound();
                }
            }

            return View(nuevo);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(ProductoVM Datos, IFormFile? foto)
        {
            var Producto = await _contenedortrabajo.Producto.Get(Datos._Producto.Id);
            if (Producto == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string RutaPrincipal = _carpetas.WebRootPath;

                if (foto is not null && foto.Length > 0)
                {
                    var extencion = Path.GetExtension(foto.FileName);
                    string[] validos = { ".jpg", ".png", ".jpeg" };

                    if (!validos.Contains(extencion))
                    {
                        ModelState.AddModelError("url", "La imagen debe ser de tipo jpg, png o jpeg.");
                        var nuevo1 = new ProductoVM()
                        {
                            _Producto = Producto,
                            ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias()
                        };
                        
                        return View(nuevo1);
                    }

                    string Nombre = Guid.NewGuid().ToString();
                    string ruta = Path.Combine(RutaPrincipal, "Imagenes", "Productos");

                    if (Producto.url != "/Imagenes/Productos/PorDefecto/ProductoDefecto.jpg" && Producto.url is not null)
                    {
                        var rutaAnterior = Path.Combine(RutaPrincipal, Producto.url.TrimStart('/'));

                        Console.WriteLine($"Esta es la ruta que deveria aliminarse {rutaAnterior}"); 

                        if (System.IO.File.Exists(rutaAnterior))
                        {
                            System.IO.File.Delete(rutaAnterior);
                            Console.WriteLine($"Se elimino la foto {rutaAnterior}");
                        }
                        else
                        {
                            Console.WriteLine($"No se elimino la foto {rutaAnterior}");
                        }
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
                        ModelState.AddModelError("url", "Error al subir la imagen: " + ex.Message);

                        var nuevo2 = new ProductoVM()
                        {
                            _Producto = Producto,
                            ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias()
                        };

                        return View(nuevo2);
                    }
                    Datos._Producto.url = @"/Imagenes/Productos/" + Nombre + extencion;
                }
                else
                {
                    Datos._Producto.url = Producto.url;
                } 
                _contenedortrabajo.Producto.update(Datos._Producto);
                await _contenedortrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            var nuevo = new ProductoVM()
            {
                _Producto = Producto,
                ListaCategoria = _contenedortrabajo.Categoria.GetListaCategorias()
            };

            return View(nuevo);
        }


        #region Llamadas a la Api
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _contenedortrabajo.Producto.GetAll(includeProperties: "Categoria");
            return Json(new { data = productos });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _contenedortrabajo.Producto.Get(id);
            if (producto == null)
            {
                return Json(new { success = false, message = "Error al eliminar el producto" });
            }

            if (producto.url != "/Imagenes/Productos/PorDefecto/ProductoDefecto.jpg" && producto.url is not null)
            {
                string RutaPrincipal = _carpetas.WebRootPath;
                var rutaAnterior = Path.Combine(RutaPrincipal, producto.url.TrimStart('/'));
                if (System.IO.File.Exists(rutaAnterior))
                {
                    System.IO.File.Delete(rutaAnterior);
                    Console.WriteLine($"Se elimino la foto {rutaAnterior}");
                }
                else
                {
                    Console.WriteLine($"No se elimino la foto {rutaAnterior}");
                }
            }
            await _contenedortrabajo.Producto.Remove(producto);
            await _contenedortrabajo.Save();

            return Json(new { success = true, message = "Categoría Borrada Correctamente" });
        }
        #endregion
    }
}
