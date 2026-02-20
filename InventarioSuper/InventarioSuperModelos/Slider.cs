using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioSuperModelos
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        [Display(Name = "Nombre de Slider")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Estado requerido")]
        public bool Estado { get; set; }

        
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string? Imagen { get; set; }
    }
}
