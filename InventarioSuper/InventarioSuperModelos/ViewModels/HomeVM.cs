using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioSuperModelos.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }

        public IEnumerable<Producto> Productos { get; set; }

        public int Pagesize { get; set; }

        public int Totalpage { get; set; }
    }
}
