using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioSuperModelos
{
    public class ListaBuscada<T> : List<T>
    {

        public int PaginaInicio { get; private set; }

        public int PaginasTotales { get; private set; }

        public string Palabra { get; private set; }

        public ListaBuscada(List<T> items, int count, int PaginaInicio, int tampagina, string Palabra)
        {
            this.PaginaInicio = PaginaInicio;
            PaginasTotales = (int)Math.Ceiling(count / (double)tampagina);
            this.Palabra = Palabra;

            AddRange(items);
        }

        public bool PaginaAnterior => (PaginaInicio > 1);
        public bool PaginaSiguiente => (PaginaInicio < PaginasTotales);
    }
}

