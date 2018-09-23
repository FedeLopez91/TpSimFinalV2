using System;
using System.Linq;
using System.Collections.Generic;


namespace Simlib
{
    class Promedio
    {

        private double total { get; set; }
        private IList<double> acumuladosPorVendedor { get; }

        public Promedio(List<double> acumulados)
        {
            acumuladosPorVendedor = acumulados;
            total = 0;

        }
        
        public double CalcularSiguiente(double AcumuladoPorVendedor)
        {
            total = acumuladosPorVendedor.Average();
            return total;

        }

    }
}
