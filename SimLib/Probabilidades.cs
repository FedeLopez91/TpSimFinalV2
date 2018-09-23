using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simlib
{
    public class Probabilidades <T>
    {
        public T ValorAsociado { get; set; }
        public double ProbabilidadAsociada { get; set; }

        public Probabilidades(T Valor, double probabilidad)
        { 
            ValorAsociado = Valor;
            ProbabilidadAsociada = probabilidad;
        }

    }
}
