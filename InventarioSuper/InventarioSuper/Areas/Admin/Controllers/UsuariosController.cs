using InventarioSuperDatos.Data.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventarioSuper.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]


    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public UsuariosController(IContenedorTrabajo contenedor)
        {
            _contenedorTrabajo = contenedor;
        }
        // GET: UsuariosController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            if (this.User.Identity == null || !this.User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            var claim = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claim.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioActual == null)
            {
                return NotFound();
            }
            var usuarios = await _contenedorTrabajo.Usuario.GetAll( n => n.Id != usuarioActual.Value);
            return View(usuarios);
        }

        [HttpGet]
        public async Task<ActionResult> Bloquear(string id)
        {
            if (id is null)
            {
                return NotFound();
            }

            await _contenedorTrabajo.Usuario.Bloquear(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Desbloquear(string id)
        {
            if (id is null)
            {
                return NotFound();
            }
            await _contenedorTrabajo.Usuario.Desbloquear(id);
            return RedirectToAction(nameof(Index));
        }

    }
}