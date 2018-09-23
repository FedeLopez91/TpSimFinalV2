using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simlib
{
    class ComisionTotal
    {
         int total { get; set; }
         int cantidadAutosVendidos { get; }
         int comision { get; } 

        public ComisionTotal() {
            total = 0;
            cantidadAutosVendidos = 0;
            comision = 0;

        }

        public int calcularComisiontotal(int cant, int comision) {
            total = cant * comision;
            return total;

        }
    }
}
