using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioSuperModelos.ViewModels
{
    public class ProductoVM
    {
        public Producto _Producto { get; set; }

        public IEnumerable<SelectListItem>? ListaCategoria { get; set; }
    }
}
