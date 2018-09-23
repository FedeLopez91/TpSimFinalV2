namespace SimLib
{
    class ComisionAcumulada
    {
        public ComisionAcumulada()
        {
            Total = 0;
        }
        public ComisionAcumulada(double valor)
        {
            Total = valor;
        }

        public double CalcularSiguiente(double ComisionTotalParcial)
        {
            Total += ComisionTotalParcial;

            return Total;
        }
        public double Total { get; protected set; }
    }
}