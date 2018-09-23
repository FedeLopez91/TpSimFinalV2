using SimLib;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Simlib
{
    public class ManejadorSimulacion
    {
        public List<VectorSimulacion> Simulacion { get; set; }
        public DataTable Info { get; protected set; }
        public Distribuciones<double>[] ProyectoA { get; protected set; }
        public Distribuciones<double>[] ProyectoB { get; protected set; }
        public Distribuciones<double>[] ProyectoC { get; protected set; }
        public Distribuciones<double> Inversion { get; protected set; }
        public List<Inversion> ListInversiones { get; set; }
        public double InversionProyectoA { get; set; }
        public double VPNProyectoA { get; set; }

        public double InversionProyectoB { get; set; }
        public double VPNProyectoB { get; set; }

        public double InversionProyectoC { get; set; }
        public double VPNProyectoC { get; set; }

        public ManejadorSimulacion(Distribuciones<double>[] proyectoA, Distribuciones<double>[] proyectoB,
            Distribuciones<double>[] proyectoC, Distribuciones<double> inversion)
        {
            ProyectoA = proyectoA;
            ProyectoB = proyectoB;
            ProyectoC = proyectoC;
            Inversion = inversion;
        }

        public void Simular(int cantIteraciones, int filasMostrar, int mostrarDesde, int presupuesto)
        {
            Simulacion = new List<VectorSimulacion>();
            var mostrarHasta = mostrarDesde + filasMostrar;
            var vAnterior = new VectorSimulacion();
            ListInversiones = new List<Inversion>();

            for (int nroIteracion = 1; nroIteracion <= cantIteraciones; nroIteracion++)
            {
                var vActual = new VectorSimulacion(); ;
                vActual.NroFila = nroIteracion;

                var RNDA = Inversion.Generar();
                vActual.RndProyectoA = RNDA.Random;
                vActual.InversionProyectoA = RNDA.Valor;
                var indicePA = RNDA.PosicionTabla;

                //var RNDVPNA = ProyectoA[indicePA].Generar();
                //vActual.RndVPNProyectoA = RNDVPNA.Random;
                //vActual.VPNProyectoA = RNDVPNA.Valor;

                var RNDB = Inversion.Generar();
                vActual.RndProyectoB = RNDB.Random;
                vActual.InversionProyectoB = RNDB.Valor;
                var indicePB = RNDB.PosicionTabla;

                //var RNDVPNB = ProyectoB[indicePB].Generar();
                //vActual.RndVPNProyectoB = RNDVPNB.Random;
                //vActual.VPNProyectoB = RNDVPNB.Valor;

                var RNDC = Inversion.Generar();
                vActual.RndProyectoC = RNDC.Random;
                vActual.InversionProyectoC = RNDC.Valor;
                var indicePC = RNDC.PosicionTabla;

                //var RNDVPNC = ProyectoC[indicePC].Generar();
                //vActual.RndVPNProyectoC = RNDVPNC.Random;
                //vActual.VPNProyectoC = RNDVPNC.Valor;

                vActual.SumPresupuesto = vActual.InversionProyectoA + vActual.InversionProyectoB + vActual.InversionProyectoC;

                vActual.PresupuestoValido = (vActual.SumPresupuesto <= presupuesto ? "SI" : "NO");

                if (vActual.PresupuestoValido == "SI")
                {
                    var RNDVPNA = ProyectoA[indicePA].Generar();
                    vActual.RndVPNProyectoA = RNDVPNA.Random;
                    vActual.VPNProyectoA = RNDVPNA.Valor;

                    var RNDVPNB = ProyectoB[indicePB].Generar();
                    vActual.RndVPNProyectoB = RNDVPNB.Random;
                    vActual.VPNProyectoB = RNDVPNB.Valor;

                    var RNDVPNC = ProyectoC[indicePC].Generar();
                    vActual.RndVPNProyectoC = RNDVPNC.Random;
                    vActual.VPNProyectoC = RNDVPNC.Valor;

                    if (!SetCounterInversionExistente(ListInversiones, vActual, cantIteraciones))
                    {
                        ListInversiones.Add(new Inversion
                        {
                            InversionProyectoA = vActual.InversionProyectoA,
                            InversionProyectoB = vActual.InversionProyectoB,
                            InversionProyectoC = vActual.InversionProyectoC,
                            VPNProyectoA = vActual.VPNProyectoA,
                            VPNProyectoB = vActual.VPNProyectoB,
                            VPNProyectoC = vActual.VPNProyectoC,
                            Contador = 1,
                            VPNAcum = VPNProyectoA + VPNProyectoB + VPNProyectoC
                        });
                    }



                    //vActual.AcumVPN = vActual.VPNProyectoA + vActual.VPNProyectoB + vActual.VPNProyectoC;

                }

                //if (vActual.AcumVPN > vAnterior.AcumMejorVPN)
                //{
                //    vActual.AcumMejorVPN = vActual.AcumVPN;

                //    vActual.InversionMejorProyectoA = vActual.InversionProyectoA;
                //    vActual.VPNMejorProyectoA = vActual.VPNProyectoA;

                //    vActual.InversionMejorProyectoB = vActual.InversionProyectoB;
                //    vActual.VPNMejorProyectoB = vActual.VPNProyectoB;

                //    vActual.InversionMejorProyectoC = vActual.InversionProyectoC;
                //    vActual.VPNMejorProyectoC = vActual.VPNProyectoC;
                //}
                //else
                //{
                //    vActual.AcumMejorVPN = vAnterior.AcumMejorVPN;

                //    vActual.InversionMejorProyectoA = vAnterior.InversionMejorProyectoA;
                //    vActual.VPNMejorProyectoA = vAnterior.VPNMejorProyectoA;

                //    vActual.InversionMejorProyectoB = vAnterior.InversionMejorProyectoB;
                //    vActual.VPNMejorProyectoB = vAnterior.VPNMejorProyectoB;

                //    vActual.InversionMejorProyectoC = vAnterior.InversionMejorProyectoC;
                //    vActual.VPNMejorProyectoC = vAnterior.VPNMejorProyectoC;
                //}

                vAnterior = vActual;
                //Agregar a la tabla a mostrar;
                if (nroIteracion >= mostrarDesde && nroIteracion < mostrarHasta)
                {
                    Simulacion.Add(vActual);
                }

            }
            //InversionProyectoA = vAnterior.InversionMejorProyectoA;
            //VPNProyectoA = vAnterior.VPNMejorProyectoA;

            //InversionProyectoB = vAnterior.InversionMejorProyectoB;
            //VPNProyectoB = vAnterior.VPNMejorProyectoB;

            //InversionProyectoC = vAnterior.InversionMejorProyectoC;
            //VPNProyectoC = vAnterior.VPNMejorProyectoC;

            Simulacion.Add(vAnterior);

        }

        public bool SetCounterInversionExistente(List<Inversion> inversionesValidas, VectorSimulacion vector, int cantIteraciones)
        {
            var response = false;
            var inversion = inversionesValidas.Where(x => x.InversionProyectoA == vector.InversionProyectoA
                                            && x.InversionProyectoB == vector.InversionProyectoB
                                            && x.InversionProyectoC == vector.InversionProyectoC
                                            && x.VPNProyectoA == vector.VPNProyectoA
                                            && x.VPNProyectoB == vector.VPNProyectoB
                                            && x.VPNProyectoC == vector.VPNProyectoC)
                                            .FirstOrDefault();

            if (inversion != null)
            {
                inversion.Contador++;
                inversion.VPNAcum = inversion.VPNProyectoA + inversion.VPNProyectoB + inversion.VPNProyectoC;
                //inversion.Probabilidad = ((double)inversion.Contador / (double)cantIteraciones); 
                response = true;
            }
            return response;

        }
    }
}
