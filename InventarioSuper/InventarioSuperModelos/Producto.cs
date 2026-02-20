using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventarioSuperModelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string? url { get; set; }

        [Display(Name = "Nombre del producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="Descripción obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Precio compra obligatorio")]
        [Display(Name = "Precio compra")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor que cero.")]
        [Precision(6, 2)]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "Precio obligatorio")]
        [Display(Name = "Precio venta")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        [Precision(6,2)]
        public decimal Precio { get; set; }

        [Display(Name = "Cantidad en inventario")]
        [Required(ErrorMessage = "La cantidad en inventario es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad en inventario no puede ser negativa.")]
        public int Cantidad { get; set; }

        [Display(Name = "Fecha de creación")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }
}
