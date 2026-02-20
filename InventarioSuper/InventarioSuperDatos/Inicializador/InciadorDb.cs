using InventarioSuper.Data;
using InventarioSuperModelos;
using InventarioSuperUtilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioSuperDatos.Inicializador
{
    public class InciadorDb : InciadorDbI
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InciadorDb(ApplicationDbContext _context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = _context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void Inicializar()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_context.Roles.Any(ro => ro.Name == Roles.Administrador)) return;

            roleManager.CreateAsync(new IdentityRole(Roles.Administrador)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(Roles.Colaborador)).GetAwaiter().GetResult();

            userManager.CreateAsync(new Usuario()
            {
                UserName = "yourName",
                Email = "YourEmail",
                EmailConfirmed = false,
                Nombre = "YourName",
                Apellido = "YourLastName",
                Telefono = "YourCelfon",
                Edad = 21

            }, "yourPassword").GetAwaiter().GetResult();

            Usuario? usuario = _context.Usuarios.Where(u => u.Email == "RepeatYourEmail").FirstOrDefault();

            if (usuario == null) return;

            userManager.AddToRoleAsync(usuario, Roles.Administrador).GetAwaiter().GetResult();

        }
    }
}
