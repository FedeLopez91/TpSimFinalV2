using Simlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TpSimFinal
{
    public partial class Form1 : Form
    {
        //Proyecto A
        public Distribuciones<double> distProyA1;
        public Distribuciones<double> distProyA2;
        public Distribuciones<double> distProyA3;
        public Distribuciones<double> distProyA4;
        Distribuciones<double>[] ProyectoA = new Distribuciones<double>[4];
        

        //Proyecto B
        public Distribuciones<double> distProyB1;
        public Distribuciones<double> distProyB2;
        public Distribuciones<double> distProyB3;
        public Distribuciones<double> distProyB4;
        Distribuciones<double>[] ProyectoB = new Distribuciones<double>[4];
        //Proyecto C
        public Distribuciones<double> distProyC1;
        public Distribuciones<double> distProyC2;
        public Distribuciones<double> distProyC3;
        public Distribuciones<double> distProyC4;
        Distribuciones<double>[] ProyectoC = new Distribuciones<double>[4];

        //Inversion
        Distribuciones<double> Inversion;


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            txtNroIteraciones.Text = "15000";
            txtMostrarDesde.Text = "1";
            txtCantMostrar.Text = "1000";
            txtPresupuesto.Text = "2000000";

            CreateParamProyectos();
        }

        private void ValidarMostrarDesde_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarSoloNumeros(e);
        }

        private void ValidarNroIteraciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarSoloNumeros(e);
        }

        private void ValidarMostrarHasta_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarSoloNumeros(e);
        }

        private void ValidarPresupuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarSoloNumeros(e);
        }

        private void ValidarSoloNumeros(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void calcularDivisionPresupuesto(object sender, EventArgs e)
        {
            CreateDistribucionPresupuesto();
        }

        private void CreateDistribucionPresupuesto()
        {
            var distribucionInversion = new List<Probabilidades<double>>();
            var labels = panelPresupuesto.Controls.OfType<Label>().Where(x => x.Name.Contains("lblCantPres"));
            var presupuesto = Convert.ToDecimal(this.txtPresupuesto.Text, new CultureInfo("en-US")) / labels.Count();
            var i = 4;
            var prob = (double)((decimal)1 / 4);
            foreach (Control fi in labels)
            {
                var name = string.Format("lblCantPres{0}", i);
                if (fi.Name == name)
                {
                    fi.Text = string.Format("{0}", presupuesto * i);
                    distribucionInversion.Add(new Probabilidades<double>(Convert.ToDouble(presupuesto * i), prob));
                }
                i--;
            }
            this.Inversion = new Distribuciones<double>(distribucionInversion);
        }

        private void CreateParamProyectos()
        {
            var dgvParam = gbParametros.Controls.OfType<Panel>().Where(x => x.Name.Contains("pProy"));

            foreach (var item in dgvParam)
            {
                var a = item.Controls.OfType<DataGridView>();
                foreach (var dgv in a)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        dgv.Rows.Add(i, 0.25);
                    }

                }
            }

        }

        private void btnSimular_Click(object sender, EventArgs e)
        {
            var validacion = validar();
            if (validacion != null)
            {
                var a = "";
                foreach (var item in validacion)
                {
                    a += item + "\n\t";
                }
                MessageBox.Show("La Suma de las probabilidades porcentuales debe ser igual a 100\n Errores en: \n \t" + a);
                return;
            }
            CreateDistribucionPresupuesto();
            GetdgwProyectos();

            Simular();
           
        }

        private void GetdgwProyectos()
        {
            
            var dgvParam = gbParametros.Controls.OfType<Panel>().Where(x => x.Name.Contains("pProy"));

            foreach (var item in dgvParam)
            {
                var a = item.Controls.OfType<DataGridView>();
                foreach (var dgv in a)
                {
                    List<Probabilidades<double>> ListProbabilidad = new List<Probabilidades<double>>();
                    var name = dgv.Name.Substring(3, 6);
                    foreach (DataGridViewRow r in dgv.Rows)
                    {
                        var valor = r.Cells[0].Value;
                        var probabilidad = r.Cells[1].Value;
                        ListProbabilidad.Add(new Probabilidades<double>(Convert.ToDouble(valor), Convert.ToDouble(probabilidad)));
                    }
                    
                    Type type = typeof(Distribuciones<double>);
                    ConstructorInfo ctor = type.GetConstructor(new[] { typeof(List<Probabilidades<double>>) });
                    object instance = ctor.Invoke(new object[] { ListProbabilidad });
                    var h = this.GetType().GetFields().FirstOrDefault(x => x.Name.Contains(name));
                    h?.SetValue(this, instance);
                }
            }
        }

        private List<string> validar()
        {
            var dgvParam = gbParametros.Controls.OfType<Panel>().Where(x => x.Name.Contains("pProy"));
            var listError = new List<string>();
            foreach (var item in dgvParam)
            {
                var a = item.Controls.OfType<DataGridView>();
                foreach (var dgv in a)
                {
                    var acum = 0.0m;
                    var name = dgv.Name.Substring(3, 6);
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        var ab = Convert.ToDecimal(row.Cells[1].Value, new CultureInfo("en-US"));
                        acum += ab;
                    }
                    if (acum != 1)  listError.Add(name);

                    
                }
            }
            return listError;
        }

        private void Simular()
        {
            ProyectoA[0] = distProyA1;
            ProyectoA[1] = distProyA2;
            ProyectoA[2] = distProyA3;
            ProyectoA[3] = distProyA4;

            ProyectoB[0] = distProyB1;
            ProyectoB[1] = distProyB2;
            ProyectoB[2] = distProyB3;
            ProyectoB[3] = distProyB4;

            ProyectoC[0] = distProyC1;
            ProyectoC[1] = distProyC2;
            ProyectoC[2] = distProyC3;
            ProyectoC[3] = distProyC4;

            ManejadorSimulacion manejador = new ManejadorSimulacion(ProyectoA, ProyectoB, ProyectoC, Inversion);

            manejador.Simular(int.Parse(txtNroIteraciones.Text), int.Parse(txtCantMostrar.Text), int.Parse(txtMostrarDesde.Text), int.Parse(txtPresupuesto.Text));

            //Seteo la Lista de Vectores del Gestor como Fuente de la Tabla.
            dgvSimulacion.DataSource = manejador.Simulacion;
            dgvResultado.DataSource = manejador.ListInversiones;

            var resultInversiones = manejador.ListInversiones.OrderByDescending(x => x.Contador).First();
            //dgvSimulacion.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //var resultado = manejador.getLastItemSimulacion();
            lblResultadoA.Text = $"Inversion ($): {resultInversiones.InversionProyectoA}" + " -- " +
                        $"VPN: {resultInversiones.VPNProyectoA}";
            lblResultadoB.Text = $"Inversion ($): {resultInversiones.InversionProyectoB}" + " -- " +
                        $"VPN: {resultInversiones.VPNProyectoB}";
            lblResultadoC.Text = $"Inversion ($): {resultInversiones.InversionProyectoC}" + " -- " +
                        $"VPN: {resultInversiones.VPNProyectoC}";
            //lblProbabilidad.Text = (resultInversiones.Contador / int.Parse(txtNroIteraciones.Text)).ToString();

        }

    }
}
