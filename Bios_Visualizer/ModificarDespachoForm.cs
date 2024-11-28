using Bios_solver.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bios_Visualizer
{
    public partial class ModificarDespachoForm : Form
    {
        public ModificarDespachoForm(Despacho despacho, int periodoDespacho)
        {
            InitializeComponent();

            this.cantCamionesNumericUpDown.Value = despacho.Cantidad;
            this.inventarioPuertoTextBox.Text = despacho.Importacion.Inventario[30].ToString("N00");
            this.CapacidadRecepcionTextBox.Text = despacho.Planta.CapacidadRecepcionCamiones(periodoDespacho+2, despacho.Importacion.Ingrediente).ToString("N00");
            this.inventarioPlantaTextBox.Text = despacho.Planta.GetInventariosPlanta()[despacho.Importacion.Ingrediente].Inventario[periodoDespacho + 2].ToString("N00");


        }

    }
}
