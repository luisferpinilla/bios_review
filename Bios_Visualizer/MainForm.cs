using Bios_solver;
using Bios_solver.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Bios_solver.Reports;
using System.Windows.Forms.DataVisualization.Charting;
using Accord.IO;
using Google.OrTools.LinearSolver;

namespace Bios_Visualizer
{

    public partial class MainForm : Form
    {

        Problema _problema;
        public MainForm()
        {
            InitializeComponent();
        }


        private async Task<bool> VerificarFechaLimite()
        {
            DateTime fechaLimite = new DateTime(2024, 11, 15);

            try
            {
                DateTime fechaActual = await ObtenerFechaHoraServidor();
                return fechaActual <= fechaLimite;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar la fecha de expiración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task<DateTime> ObtenerFechaHoraServidor()
        {
            using (HttpClient client = new HttpClient())
            {
                // Agregar encabezado User-Agent para simular un navegador
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                // Nueva URL alternativa para obtener la hora UTC
                string url = "https://www.timeapi.io/api/Time/current/zone?timeZone=UTC";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonResponse);

                // Cambiar el nombre del campo según la estructura de la API
                string datetimeStr = json["dateTime"].ToString();
                return DateTime.Parse(datetimeStr);
            }
        }
        public void PrepararInterface()
        {
            ColumnHeader column1, column2;

            this.importacionesListView.Items.Clear();
            this.importacionesListView.Columns.Clear();
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Ingrediente", Width = 80 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Puerto", Width = 80 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Operador", Width = 80 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Empresa", Width = 80 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Importacion", Width = 120 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "ValorCif", Width = 80 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Fecha Llegada", Width = 90 });
            this.importacionesListView.Columns.Add(new ColumnHeader() { Text = "Camiones Despachables", Width = 80 });

            this.detalleImportacionListView.Items.Clear();
            this.detalleImportacionListView.Columns.Clear();
            this.detalleImportacionListView.Columns.Add(new ColumnHeader() { Text = "Variable", Width = 160 });
            this.detalleImportacionListView.Columns.Add(new ColumnHeader() { Text = "Ayer", Width = 90 });


            this.PlantaListView.Items.Clear();
            this.PlantaListView.Columns.Clear();
            this.PlantaListView.Columns.Add(new ColumnHeader() { Text = "Planta", Width = 120 });
            this.PlantaListView.Columns.Add(new ColumnHeader() { Text = "Empresa", Width = 120 });
            this.PlantaListView.Columns.Add(new ColumnHeader() { Text = "Tiempo Disponible", Width = 120 });

            this.PlantaIngredientesListView.Items.Clear();
            this.PlantaIngredientesListView.Columns.Clear();
            this.PlantaIngredientesListView.Columns.Add(new ColumnHeader() { Text = "Variable", Width = 160 });
            this.PlantaIngredientesListView.Columns.Add(new ColumnHeader() { Text = "Ayer", Width = 90 });

            //Agregar columnas

            foreach (DateTime fecha in _problema.Fechas.Values)
            {
                column1 = new ColumnHeader();
                column1.Text = fecha.ToString("MMM-dd");
                column1.Width = 75;
                column1.TextAlign = HorizontalAlignment.Right;

                column2 = new ColumnHeader();
                column2.Text = fecha.ToString("MMM-dd");
                column2.Width = 75;
                column2.TextAlign = HorizontalAlignment.Right;
                column2.Tag = fecha;

                this.PlantaIngredientesListView.Columns.Add(column1);
                this.detalleImportacionListView.Columns.Add(column2);
            }

            this.alarmasListView.Items.Clear();

            foreach (Alarma alarma in _problema.Alarmas)
            {
                ListViewItem item = new ListViewItem(alarma.NivelAlarma.ToString());

                switch (alarma.NivelAlarma)
                {
                    case NivelAlarma.Critico:
                        item.ImageIndex = 0;
                        break;
                    case NivelAlarma.Informativo:
                        item.ImageIndex = 1;
                        break;
                    default:
                        break;
                }

                item.SubItems.Add(alarma.Name);
                item.SubItems.Add(alarma.Description);
                this.alarmasListView.Items.Add(item);
            }

        }

        public void CargarProblema()
        {


            ListViewItem planta_item;
            ListViewItem impo_item;


            //this.PlantaListView.Items.Clear();
            //this.PlantaListView.Groups.Clear();
            //this.PlantaListView.Columns.Clear();

            //this.PlantaIngredientesListView.Items.Clear();
            //this.PlantaIngredientesListView.Groups.Clear();



            foreach (Planta planta in _problema.Plantas)
            {
                planta_item = new ListViewItem(planta.Nombre);
                planta_item.Tag = planta;
                planta_item.SubItems.Add(planta.Empresa);
                planta_item.SubItems.Add(planta.TiempoTurno.ToString());
                this.PlantaListView.Items.Add(planta_item);
            }

            foreach (Importacion impo in _problema.Importaciones)
            {
                impo_item = new ListViewItem(impo.Ingrediente);
                impo_item.Tag = impo;
                impo_item.SubItems.Add(impo.Puerto);
                impo_item.SubItems.Add(impo.Operador);
                impo_item.SubItems.Add(impo.Empresa);
                impo_item.SubItems.Add(impo.Carga);
                impo_item.SubItems.Add(impo.ValorCif.ToString("C02"));
                impo_item.SubItems.Add(impo.FechaLlegada.ToString("yyyy-MM-dd"));
                impo_item.SubItems.Add(impo.CamionesDespachables.ToString("N00"));

                this.importacionesListView.Items.Add(impo_item);
            }

            this.bodegajeTextBox.Text = _problema.CostoBodegaje.ToString("C00");
            this.vencimientosTextBox.Text = _problema.CostoVencimiento.ToString("C00");
            this.fletesTextBox.Text = _problema.CostoFlete.ToString("C00");
            this.directoTextBox.Text = _problema.CostoDirecto.ToString("C00");
            this.TotalTextBox.Text = (_problema.CostoBodegaje + _problema.CostoVencimiento + _problema.CostoFlete + _problema.CostoDirecto).ToString("C00");




        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PlantaListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewItem item;
            ListViewItem consumo_item, inventario_item, llegada_planeada_item, backorder_item, capacidad_item, dias_item, safety_item;
            long[] consumo, inventario, llegada_planeada, backorder;
            int[] dias;
            int safety;

            this.PlantaIngredientesListView.Groups.Clear();

            if (e.IsSelected)
            {
                if (e.Item is not null)
                {

                    item = e.Item;

                    if (item.Tag is not null)
                    {
                        Planta planta = (Planta)item.Tag;
                        ListViewGroup ingrediente_group;

                        foreach (InventarioPlanta ip in planta.GetInventariosPlanta().Values)
                        {
                            ingrediente_group = new ListViewGroup(ip.Ingrediente, HorizontalAlignment.Left);
                            this.PlantaIngredientesListView.Groups.Add(ingrediente_group);

                            consumo_item = new ListViewItem("Consumo");
                            consumo_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(consumo_item);
                            consumo_item.SubItems.Add("--");

                            inventario_item = new ListViewItem("inventario");
                            inventario_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(inventario_item);
                            inventario_item.SubItems.Add(ip.InventarioInicial.ToString("N00"));

                            llegada_planeada_item = new ListViewItem("Transito");
                            llegada_planeada_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(llegada_planeada_item);
                            llegada_planeada_item.SubItems.Add("--");

                            backorder_item = new ListViewItem("Backorder");
                            backorder_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(backorder_item);
                            backorder_item.SubItems.Add("--");

                            capacidad_item = new ListViewItem("Capacidad");
                            capacidad_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(capacidad_item);
                            capacidad_item.SubItems.Add("--");

                            dias_item = new ListViewItem("Dias");
                            dias_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(dias_item);
                            dias_item.SubItems.Add("--");

                            safety_item = new ListViewItem("Safety Stock Dias");
                            safety_item.Group = ingrediente_group;
                            this.PlantaIngredientesListView.Items.Add(safety_item);
                            safety_item.SubItems.Add("--");

                            consumo = ip.GetConsumo();
                            inventario = ip.Inventario;
                            llegada_planeada = ip.GetLlegadaPlaneada();
                            backorder = ip.GetBackorder();
                            dias = ip.Dias;
                            safety = ip.SafetyStock;

                            for (int i = 0; i < consumo.Length; i++)
                            {
                                consumo_item.SubItems.Add(consumo[i].ToString("N00"));

                                inventario_item.SubItems.Add(inventario[i].ToString("N00"));
                                llegada_planeada_item.SubItems.Add(llegada_planeada[i].ToString("N00"));
                                backorder_item.SubItems.Add(backorder[i].ToString("N00"));
                                capacidad_item.SubItems.Add(ip.Capacidad.ToString("N00"));
                                dias_item.SubItems.Add(dias[i].ToString("N00"));
                                safety_item.SubItems.Add(safety.ToString("N00"));

                            }
                        }
                        CargarChart(planta);
                    }
                }
            }
        }

        private void importacionesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListViewItem item, llegadas_item, inventario_item, vecimiento_item, bodegaje_item, directo_item;
            ListViewGroup general = new ListViewGroup("General", HorizontalAlignment.Left);
            long[] llegadas, inventario;
            double[] vencimiento, bodegaje, directo;

            this.detalleImportacionListView.Groups.Clear();
            this.detalleImportacionListView.Items.Clear();

            this.detalleImportacionListView.Groups.Add(general);

            if (e.IsSelected)
                if (e.Item.Tag != null)
                {
                    item = (ListViewItem)e.Item;

                    if (item.Tag is not null)
                    {
                        Importacion importacion = (Importacion)item.Tag;

                        llegadas_item = new ListViewItem("Llegadas");
                        llegadas_item.Group = general;
                        this.detalleImportacionListView.Items.Add(llegadas_item);
                        llegadas_item.SubItems.Add("--");

                        inventario_item = new ListViewItem("Inventario");
                        inventario_item.Group = general;
                        this.detalleImportacionListView.Items.Add(inventario_item);
                        inventario_item.SubItems.Add(importacion.InventarioInicial.ToString("N00"));

                        bodegaje_item = new ListViewItem("Costo Bodegaje/kg");
                        bodegaje_item.Group = general;
                        this.detalleImportacionListView.Items.Add(bodegaje_item);
                        bodegaje_item.SubItems.Add("--");

                        vecimiento_item = new ListViewItem("Costo Vencimiento/kg");
                        vecimiento_item.Group = general;
                        this.detalleImportacionListView.Items.Add(vecimiento_item);
                        vecimiento_item.SubItems.Add("--");

                        directo_item = new ListViewItem("Costo Despacho Directo/kg");
                        directo_item.Group = general;
                        this.detalleImportacionListView.Items.Add(directo_item);
                        directo_item.SubItems.Add("--");

                        llegadas = importacion.GetLlegadas();
                        inventario = importacion.Inventario;
                        vencimiento = importacion.GetCostoVencimiento();
                        bodegaje = importacion.GetCostoBodegaje();
                        directo = importacion.GetCostoDespachoDirecto();


                        for (int i = 0; i < llegadas.Length; i++)
                        {
                            llegadas_item.SubItems.Add(llegadas[i].ToString("N00"));
                            inventario_item.SubItems.Add(inventario[i].ToString("N00"));
                            vecimiento_item.SubItems.Add(vencimiento[i] > 0 ? vencimiento[i].ToString("C02") : "--");
                            bodegaje_item.SubItems.Add(bodegaje[i] > 0 ? bodegaje[i].ToString("C02") : "--");
                            directo_item.SubItems.Add(directo[i] > 0 ? directo[i].ToString("C02") : "--");
                        }

                        // Informacion de despachos
                        ListViewItem flete_camion_item, directo_camion_item, inter_camion_item, ahorro_bodegaje_item, neto_camion_item, camiones_item, cluster_item, ahorro_vencimiento_item;

                        Dictionary<string, ListViewGroup> grupos = new Dictionary<string, ListViewGroup>();
                        Dictionary<string, ListViewItem> flete_camion_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> directo_camion_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> intercom_camion_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> ahorro_bodegaje_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> neto_camion_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> camiones_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> cluster_items = new Dictionary<string, ListViewItem>();
                        Dictionary<string, ListViewItem> ahorro_vencimiento_items = new Dictionary<string, ListViewItem>();

                        for (int i = 0; i < llegadas.Length; i++)
                        {
                            Despacho[] despachos = importacion.GetDespachos(i);

                            foreach (Despacho despacho in despachos)
                            {
                                string nombrePlanta = despacho.Planta.Nombre;
                                if (!grupos.ContainsKey(nombrePlanta))
                                {
                                    grupos.Add(nombrePlanta, new ListViewGroup($"Despacho a {nombrePlanta}", HorizontalAlignment.Left));
                                    this.detalleImportacionListView.Groups.Add(grupos[nombrePlanta]);

                                    flete_camion_item = new ListViewItem("Flete (+)");
                                    flete_camion_item.Group = grupos[nombrePlanta];
                                    flete_camion_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(flete_camion_item);
                                    flete_camion_items.Add(nombrePlanta, flete_camion_item);

                                    directo_camion_item = new ListViewItem("Directo(+)");
                                    directo_camion_item.Group = grupos[nombrePlanta];
                                    directo_camion_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(directo_camion_item);
                                    directo_camion_items.Add(nombrePlanta, directo_camion_item);

                                    inter_camion_item = new ListViewItem("Intercompany(+)");
                                    inter_camion_item.Group = grupos[nombrePlanta];
                                    inter_camion_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(inter_camion_item);
                                    intercom_camion_items.Add(nombrePlanta, inter_camion_item);

                                    ahorro_bodegaje_item = new ListViewItem("Ahorro Bodegaje(-)");
                                    ahorro_bodegaje_item.Group = grupos[nombrePlanta];
                                    ahorro_bodegaje_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(ahorro_bodegaje_item);
                                    ahorro_bodegaje_items.Add(nombrePlanta, ahorro_bodegaje_item);

                                    ahorro_vencimiento_item = new ListViewItem("Ahorro Vencimiento(-)");
                                    ahorro_vencimiento_item.Group = grupos[nombrePlanta];
                                    ahorro_vencimiento_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(ahorro_vencimiento_item);
                                    ahorro_vencimiento_items.Add(nombrePlanta, ahorro_vencimiento_item);

                                    neto_camion_item = new ListViewItem("Neto Camion(=)");
                                    neto_camion_item.Group = grupos[nombrePlanta];
                                    neto_camion_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(neto_camion_item);
                                    neto_camion_items.Add(nombrePlanta, neto_camion_item);

                                    cluster_item = new ListViewItem("Grupos Costo");
                                    cluster_item.Group = grupos[nombrePlanta];
                                    cluster_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(cluster_item);
                                    cluster_items.Add(nombrePlanta, cluster_item);

                                    camiones_item = new ListViewItem("Camiones Despachados");
                                    camiones_item.Tag = despacho;
                                    camiones_item.Group = grupos[nombrePlanta];
                                    camiones_item.SubItems.Add("--");
                                    this.detalleImportacionListView.Items.Add(camiones_item);
                                    camiones_items.Add(nombrePlanta, camiones_item);
                                }

                                double flete_camion = inventario[i] + llegadas[i] > 0 ? despacho.CostoFleteKg * _problema.CapacidadCamion : 0;
                                double directo_camion = inventario[i] + llegadas[i] > 0 ? importacion.GetCostoDespachoDirecto()[i] * _problema.CapacidadCamion : 0;
                                double intercom_camion = inventario[i] + llegadas[i] > 0 ? despacho.CostoIntercompanyCamion : 0;
                                double ahorro_bodegaje_camion = inventario[i] + llegadas[i] > 0 ? importacion.AhorroBodegaje[i] * _problema.CapacidadCamion : 0;
                                double ahorro_vencimiento_camion = inventario[i] + llegadas[i] > 0 ? importacion.AhorroVencimiento[i] * _problema.CapacidadCamion : 0;
                                int cantidad = inventario[i] + llegadas[i] > 0 ? despacho.Cantidad : 0;
                                Cluster cluster = inventario[i] + llegadas[i] > 0 ? despacho.Grupo : 0;
                                double netoCamión = inventario[i] + llegadas[i] > 0 ? despacho.NetoDespacho : 0;

                                flete_camion_items[nombrePlanta].SubItems.Add(flete_camion > 0 ? flete_camion.ToString("C00") : "--");
                                directo_camion_items[nombrePlanta].SubItems.Add(directo_camion > 0 ? directo_camion.ToString("C00") : "--");
                                intercom_camion_items[nombrePlanta].SubItems.Add(intercom_camion > 0 ? intercom_camion.ToString("C00") : "--");
                                ahorro_bodegaje_items[nombrePlanta].SubItems.Add(ahorro_bodegaje_camion > 0 ? ahorro_bodegaje_camion.ToString("C00") : "--");
                                ahorro_vencimiento_items[nombrePlanta].SubItems.Add(ahorro_vencimiento_camion > 0 ? ahorro_vencimiento_camion.ToString("C00") : "--");
                                neto_camion_items[nombrePlanta].SubItems.Add(netoCamión > 0 ? netoCamión.ToString("C00") : "--");
                                camiones_items[nombrePlanta].SubItems.Add(cantidad > 0 ? cantidad.ToString("N00") : "--");
                                cluster_items[nombrePlanta].SubItems.Add(cluster != Cluster.Indefinido ? cluster.ToString() : "--");

                            }
                        }
                    }
                }
        }

        private void detalleImportacionListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Obtener el ítem en la posición del clic
            ListViewItem item = detalleImportacionListView.GetItemAt(e.X, e.Y);
            Despacho despacho;
            DateTime fechaDespacho;
            int periodoDespacho;
            if (item != null)
                if (item.Tag != null)
                {
                    // Recorrer las columnas para determinar en cuál se hizo clic
                    int colIndex = -1;
                    int totalWidth = 0;

                    for (int i = 0; i < detalleImportacionListView.Columns.Count; i++)
                    {
                        totalWidth += detalleImportacionListView.Columns[i].Width;

                        if (e.X <= totalWidth)
                        {
                            colIndex = i;



                            // Mostrar el ítem y la columna en la que se hizo clic
                            if (colIndex != -1)
                            {
                                string clickedText = item.SubItems[colIndex].Text;
                                despacho = (Despacho)item.Tag;

                                if (detalleImportacionListView.Columns[i].Tag != null)
                                {

                                    fechaDespacho = (DateTime)detalleImportacionListView.Columns[i].Tag;

                                    periodoDespacho = _problema.GetPeriodo(fechaDespacho);

                                    ModificarDespachoForm form = new ModificarDespachoForm(despacho, periodoDespacho);
                                    form.ShowDialog();

                                    MessageBox.Show($"Ítem: {item.Text}, Columna: {detalleImportacionListView.Columns[colIndex].Text}, Valor: {clickedText}");
                                }
                            }
                            break;
                        }
                    }


                }
        }

        private void CargarChart(Planta planta)
        {
            chart1.Series.Clear();

            foreach (InventarioPlanta inv in planta.GetInventariosPlanta().Values)
            {
                Series inventarioSerie = new Series();
                inventarioSerie.Name = $"Inventario {inv.Ingrediente}";
                inventarioSerie.ChartType = SeriesChartType.Line;


                int[] dios = inv.Dias;
                DateTime[] fechas = _problema.Fechas.Values.ToArray();

                for (int i = 0; i < dios.Length; i++)
                    if (dios[i] < 60)
                    {
                        inventarioSerie.Points.AddXY(fechas[i], dios[i]);
                    }

                chart1.Series.Add(inventarioSerie);
            }
        }

        private void exportarReportesButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.AutoUpgradeEnabled = true;
            folderBrowserDialog.OkRequiresInteraction = true;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ReportePuertos reportePuertos = new ReportePuertos(_problema);
                reportePuertos.ExportToCsv(folderBrowserDialog.SelectedPath + "\\reporte_puerto.csv");

                ReportePlantas reportePlantas = new ReportePlantas(_problema);
                reportePlantas.ExportToCsv(folderBrowserDialog.SelectedPath + "\\reporte_plantas.csv");

                ReporteDespachos reporteDespachos = new ReporteDespachos(_problema);
                reporteDespachos.ExportToCsv(folderBrowserDialog.SelectedPath + "\\reporte_despachos.csv");

                MessageBox.Show(this, "La información se ha exportado", "Bios Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cargarProblemaButton_Click(object sender, EventArgs e)
        {

            cargarProblemaButton.Enabled = false;
            OpenFile_Button.Enabled = false;
            exportarReportesButton.Enabled = false;

            backgroundWorker.RunWorkerAsync();

        }

        private void OpenFile_Button_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bios Solver Template File|*.xlsm";
            openFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(openFileDialog.FileName))
            {
                exportarReportesButton.Enabled = false;
                fileTextBox.Text = openFileDialog.FileName;
                cargarProblemaButton.Enabled = false;
                _problema = new Problema(fileTextBox.Text);
                PrepararInterface();
                CargarProblema();
                cargarProblemaButton.Enabled = true;
            }
            else
                fileTextBox.Text = "";
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _problema.Solve();
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            PrepararInterface();
            CargarProblema();
            exportarReportesButton.Enabled = true;
            OpenFile_Button.Enabled = true;
            cargarProblemaButton.Enabled = false;
            if (_problema.RestulStatus == Solver.ResultStatus.OPTIMAL)
                MessageBox.Show("Se ha encontrado una solución óptima", "Bios Solver", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("El solver puede necesitar más tiempo para encontrar la solución óptima", "Bios Solver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);     
        }

        private void alarmasListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}