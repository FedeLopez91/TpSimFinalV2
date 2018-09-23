using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simlib
{
    public class RndValor<T>
    {
        public double Random { get;protected set; }
        public T Valor  { get;protected set; }
        public int PosicionTabla { get; set; }
        public RndValor(double rnd, T valor, int posicionTabla)
        {
            Valor = valor;
            Random = rnd;
            PosicionTabla = posicionTabla;
        }
    }
}
