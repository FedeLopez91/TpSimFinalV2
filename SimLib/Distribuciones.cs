using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Simlib
{
    public class Distribuciones<T>
    {
        public List<Probabilidades<T>> Valores { get; set; }
        public List<double> intervaloHasta { get; set; }
        public int indice { get; set; }

        //Cada Distribucion tiene su cadena de randoms.
        private Random random;

        public Distribuciones(List<Probabilidades<T>> Valores)
        {
            this.random = new Random();
            this.Valores = Valores ?? new List<Probabilidades<T>>();

            GenerarTabla();
        }


        //genera rnd
        public double GenerarRnd()
        {
            var num = this.random.NextDouble();
            num = Math.Truncate(num * 100);
            return num;
        }

        public RndValor<T> Generar()
        {
            var rnd = this.GenerarRnd();
            return new RndValor<T>(rnd, ObtenerValorAsociado(rnd), indice);
        }

        //apartir del rnd, devuelve el valor asociado
        public T ObtenerValorAsociado(double rnd)
        {

            for (var i = 0; i < intervaloHasta.Count; i++)
            {
                if (rnd < intervaloHasta[i] * 100)
                {
                    indice = i;
                    return Valores[i].ValorAsociado;
                }
            }

            indice = intervaloHasta.Count-1;
            return Valores.Last().ValorAsociado;
        }


        //Genera la tabla con los intervalos
        private void GenerarTabla()
        {   //ordena las p(x)
            Valores = Valores.OrderBy(v => v.ValorAsociado).ToList();

            intervaloHasta = new List<double>();

            //setea los intervalos
            for (var i = 0; i < Valores.Count; i++)
            {
                if (intervaloHasta.Count == 0)
                {
                    intervaloHasta.Insert(i, 0 + Valores[i].ProbabilidadAsociada - 0.01);

                }
                else
                {
                    intervaloHasta.Insert(i, (intervaloHasta[i - 1] + Valores[i - 1].ProbabilidadAsociada));
                }
            }
        }


    }
}
