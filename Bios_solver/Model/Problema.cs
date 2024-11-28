using DocumentFormat.OpenXml.Bibliography;
using SpreadsheetLight;
using System.Numerics;
using Accord.MachineLearning;
using Google.OrTools.LinearSolver;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Diagnostics;
using Bios_solver.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;

namespace Bios_solver.Model
{
    public class Problema
    {

        private string _filename;
        private long _capacidad_camion;
        private Solver solver;
        private Solver.ResultStatus restulStatus;

        private DateTime _fechaReferencia;
        private SLDocument _document;
        private List<Alarma> _alarmas;
        private Dictionary<string, Planta> _plantas;
        private Dictionary<string, Importacion> _importaciones;
        private Dictionary<string, Dictionary<string,double>> _intercompanies;
        private Dictionary<int, DateTime> _fechas;
        private Dictionary<DateTime, int> _periodos;
        private List<Despacho> _despachos;
        private List<InventarioPlanta> _inventarioPlanta;

        public int CantidadPeriodos => _fechas.Count;

        public Problema(string filename, long capacidad_camion = 34000)
        {
            _filename = filename;
            solver = Solver.CreateSolver("SCIP");
            solver.SetSolverSpecificParametersAsString("display/verblevel = 3"); // Nivel de verbosidad (0 a 5)
            int maxThreads = Convert.ToInt32(Environment.ProcessorCount *0.6);
            Console.WriteLine($"Número máximo de hilos disponibles: {maxThreads}");
            solver.SetSolverSpecificParametersAsString($"parallel/maxnthreads={maxThreads}");

            _alarmas = new List<Alarma>();

            if (!System.String.IsNullOrEmpty(_filename))
            {
                _document = new SLDocument(_filename);
                _plantas = new Dictionary<string, Planta>();
                _importaciones = new Dictionary<string, Importacion>();
                _intercompanies = new Dictionary<string, Dictionary<string, double>>();
                _fechas = new Dictionary<int, DateTime>();
                _periodos = new Dictionary<DateTime, int>();
                _despachos = new List<Despacho>();
                _inventarioPlanta = new List<InventarioPlanta>();
                _capacidad_camion = capacidad_camion;
                
                Load();                
            }
        }

        private void Load()
        {
            LoadPlantas();
            LoadConsumos();
            LoadTiemposProceso();
            LoadInventarios();
            LoadTransitosPlanta();
            LoadImportacionesPuerto();
            LoadImportacionesTransito();
            LoadCostoVencimientoImportaicones();
            LoadCostosOperacionPortuaria();
            LoadIntercompanies();
            CrearDespachos();
            LoadFletes();
            LoadSafetyStock();
           
            CalcularClusters();            
        }

        public void Solve()
        {
            Fase_01_Solve();
            Fase_02_Solve();
            Fase_03_Test_Solve();
            Fase_04_Solve();
            switch (restulStatus)
            {
                case Solver.ResultStatus.OPTIMAL:
                case Solver.ResultStatus.FEASIBLE:
                    RemplazarSolucion();
                    break;
                default:
                    Debug.WriteLine("Fase 4 not solved");
                    
                    break;
            }
            solver.SetSolverSpecificParametersAsString("logfile=scip_log.txt");
        }

        #region "Cargar Información"
        private void LoadPlantas()
        {
            Planta planta;
            _document.SelectWorksheet("plantas");
            int fila = 2;

            while (!string.IsNullOrEmpty(_document.GetCellValueAsString(fila, 1)))
            {
                string nombre = _document.GetCellValueAsString(fila, 1);
                string empresa = _document.GetCellValueAsString(fila, 2);
                int operacion_minutos = _document.GetCellValueAsInt32(fila, 3);
                int plataformas = _document.GetCellValueAsInt32(fila, 5);

                planta = new Planta(this, nombre, empresa, operacion_minutos * plataformas);

                if (!_plantas.ContainsKey(nombre))
                {
                    _plantas.Add(nombre, planta);
                }
                else
                {
                    string error = $"La planta {nombre} parece estar duplicada en la hoja de plantas";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Plantas", Description = error, NivelAlarma=NivelAlarma.Informativo });
                }

                fila++;

                Console.WriteLine($"Creando la planta {planta.ToString()}");
            }
        }

        private void LoadConsumos()
        {
            _document.SelectWorksheet("consumo_proyectado");
            int row = 2;
            int col = 3;
            int index = 0;
            DateTime fecha;
            string nombre_planta;
            string ingrediente;
            long cantidad;
            Planta planta;
            InventarioPlanta inventarioPlanta;

            // Leer los periodos
            do
            {
                fecha = DateTime.ParseExact(_document.GetCellValueAsString(1, col), "d/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                _fechas.Add(index, fecha);
                index++;
                col++;

            } while (!string.IsNullOrEmpty(_document.GetCellValueAsString(1, col)));

            // Invertir el diccionario de periodos y fechas
            _periodos = _fechas.ToDictionary(kv => kv.Value, kv => kv.Key);

            _fechaReferencia = _fechas[0];

            while (!string.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                nombre_planta = _document.GetCellValueAsString(row, 1);
                ingrediente = _document.GetCellValueAsString(row, 2);

                planta = _plantas[nombre_planta];
                inventarioPlanta = new InventarioPlanta(this, planta, ingrediente);

                if (planta.ContainsIngrediente(ingrediente))
                {
                    string error = $"El ingrediente {ingrediente} de la planta {nombre_planta} parece estar duplicada en la hoja de consumos proyectados";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Consumos", Description = error, NivelAlarma = NivelAlarma.Critico });
                }
                
                planta.AddInventario(inventarioPlanta);
                _inventarioPlanta.Add(inventarioPlanta);

                for (int i = 3; i < _fechas.Count + 3; i++)
                {
                    cantidad = (long)_document.GetCellValueAsInt64(row, i);
                    inventarioPlanta.Add_Consumo(i - 3, cantidad);
                }
                row++;

                Console.WriteLine($"Se ha cargado consumo en {nombre_planta} de {ingrediente}");
            }
        }

        private void LoadTiemposProceso()
        {
            _document.SelectWorksheet("plantas");

            int row = 2;
            int col = 6;
            Planta planta;
            string nombre_planta;
            string ingrediente;
            int tiempo;

            do
            {
                nombre_planta = _document.GetCellValueAsString(row, 1);

                col = 6;

                while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(1, col)))
                {
                    ingrediente = _document.GetCellValueAsString(1, col);
                    tiempo = _document.GetCellValueAsInt32(row, col);

                    if (_plantas.ContainsKey(nombre_planta))
                    {
                        planta = _plantas[nombre_planta];
                        if (planta.ContainsIngrediente(ingrediente))
                        {
                            planta.SetTiempoProceso(ingrediente, tiempo);
                        }
                        Console.WriteLine($"Se ha cargado tiempo de proceso para {nombre_planta} con {ingrediente}: {tiempo}");
                    }
                    col++;
                }

                row++;

            } while (!System.String.IsNullOrEmpty(nombre_planta));
        }

        private void LoadInventarios()
        {
            _document.SelectWorksheet("unidades_almacenamiento");

            Dictionary<int, string> index = new Dictionary<int, string>();
            int row = 2;
            int col = 5;
            string titulo;
            string nombre_planta, nombreUnidad, ingrediente;
            long inventario;
            long capacidad;
            Planta planta;
            UnidadAlmacenamiento ua;


            // Identificar los titulos
            do
            {
                titulo = _document.GetCellValueAsString(1, col);
                index.Add(col, titulo);
                col++;
            } while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(1, col)));

            // Recorrer los registros
            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                nombre_planta = _document.GetCellValueAsString(row, 1);
                planta = _plantas[nombre_planta];
                nombreUnidad = _document.GetCellValueAsString(row, 2);
                ingrediente = _document.GetCellValueAsString(row, 3);
                inventario = _document.GetCellValueAsInt64(row, 4);

                ua = new UnidadAlmacenamiento(planta, nombreUnidad, inventario);

                if (ingrediente != "0" && !System.String.IsNullOrEmpty(ingrediente))
                    ua.SetIngrediente(ingrediente);

                foreach(int i in index.Keys)
                {
                    capacidad = _document.GetCellValueAsInt64(row, i);
                    ua.SetCapacidad(index[i], capacidad);
                }
                planta.AddUnidadAlmacenamiento(ua);

                row++;
            }

            // Resolver las no asignadas
            foreach(Planta p in _plantas.Values)
            {
                p.AsignarUnidadesNoAsignadas();
            }
        }

        private void LoadTransitosPlanta()
        {
            _document.SelectWorksheet("tto_plantas");
            int row = 2;

            string nombre_planta;
            string ingrediente;
            long cantidad;
            Planta planta;
            DateTime fecha;
            int periodo;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                nombre_planta = _document.GetCellValueAsString(row, 1);

                ingrediente = _document.GetCellValueAsString(row, 2);
                cantidad = _document.GetCellValueAsInt64(row, 3);
                fecha = _document.GetCellValueAsDateTime(row, 4);

                if (_plantas.ContainsKey(nombre_planta))
                {
                    planta = _plantas[nombre_planta];
                    if (_periodos.ContainsKey(fecha))
                    {
                        periodo = _periodos[fecha];
                        planta = _plantas[nombre_planta];
                        planta.AddTransitoPlanta(ingrediente, periodo, cantidad);
                    }
                    else
                    {
                        string error = $"El transito hacia la planta {nombre_planta} de {ingrediente} parece estar fuera del horizonte de planeación";
                        Console.WriteLine(error);
                        _alarmas.Add(new Alarma() { Name = "Transitos Planta", Description = error, NivelAlarma = NivelAlarma.Critico });

                    }
                }
                else
                {
                    string error = $"La planta {nombre_planta} Parece no tener consumos de ningun material";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Transitos Planta", Description = error, NivelAlarma = NivelAlarma.Critico });
                }
                row++;
            }

        }

        private void LoadImportacionesPuerto()
        {
            Importacion impo;
            _document.SelectWorksheet("inventario_puerto");
            int row = 2;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string empresa = _document.GetCellValueAsString(row, 1);
                string operador = _document.GetCellValueAsString(row, 2);
                string puerto = _document.GetCellValueAsString(row, 3);
                string ingrediente = _document.GetCellValueAsString(row, 4);
                string importacion = _document.GetCellValueAsString(row, 5);
                DateTime fecha_llegada = _document.GetCellValueAsDateTime(row, 6);
                long cantidad = _document.GetCellValueAsInt64(row, 7);
                double cif = _document.GetCellValueAsDouble(row, 8);

                string codigo = $"{ingrediente}_{puerto}_{operador}_{empresa}_{importacion}";

                if (!_importaciones.ContainsKey(codigo))
                {
                    impo = new Importacion(this, ingrediente, puerto, operador, empresa, importacion, cantidad, cif, fecha_llegada);
                    _importaciones[codigo] = impo;
                }

                // Calcular la diferencia en días
                int diferenciaDias = (_fechaReferencia - fecha_llegada).Days;

                if (diferenciaDias > 60)
                {
                    string error = $"La importación {importacion} en puerto {puerto}-{operador}-{empresa} de {ingrediente} parece tener más de 60 días";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Inventario Puertos", Description = error, NivelAlarma = NivelAlarma.Informativo });
                }

                if (fecha_llegada > _fechaReferencia)
                {
                    string error = $"La importación {importacion} en puerto {puerto}-{operador}-{empresa} de {ingrediente} Estar en tránsito con respecto al {_fechaReferencia.ToString("ddd-MM-yyyy")}";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Inventario Puertos", Description = error, NivelAlarma = NivelAlarma.Informativo });
                }

                row++;
            }


        }

        private void LoadImportacionesTransito()
        {
            Importacion impo;
            _document.SelectWorksheet("tto_puerto");
            int row = 2;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string empresa = _document.GetCellValueAsString(row, 1);
                string operador = _document.GetCellValueAsString(row, 2);
                string puerto = _document.GetCellValueAsString(row, 3);
                string ingrediente = _document.GetCellValueAsString(row, 4);
                string importacion = _document.GetCellValueAsString(row, 5);
                long cantidad = _document.GetCellValueAsInt64(row, 6);
                DateTime fecha_llegada = _document.GetCellValueAsDateTime(row, 7);
                double cif = _document.GetCellValueAsDouble(row, 8);
                DateTime fecha_fin = _document.GetCellValueAsDateTime(row, 9);

                string codigo = $"{ingrediente}_{puerto}_{operador}_{empresa}_{importacion}";

                if (!_importaciones.ContainsKey(codigo))
                {
                    impo = new Importacion(this, ingrediente, puerto, operador, empresa, importacion, 0, cif, fecha_llegada);
                    _importaciones[codigo] = impo;
                }

                impo = _importaciones[codigo];

                TimeSpan ts = fecha_fin - fecha_llegada;

                // Establecer periodo de bodegaje
                if (_periodos.ContainsKey(fecha_fin))
                    impo.SetPeriodoBodegaje(_periodos[fecha_fin]);

                int dias = ts.Days + 1;
                int cant_dia = (int)(cantidad / dias);
                int periodo = _periodos[fecha_llegada];

                do
                {
                    impo.AddLlegada(periodo, cant_dia);
                    impo.SetTipoDespacho(periodo, TipoDespacho.Directo);

                    cantidad -= cant_dia;
                    periodo++;
                } while (_fechas.ContainsKey(periodo) && cantidad>0 && _fechas[periodo]<=fecha_fin);









                    if (fecha_llegada < _fechaReferencia && impo.InventarioInicial == 0)
                {
                    string error = $"La importación {importacion} en puerto {puerto}-{operador}-{empresa} de {ingrediente} Parece tener fecha anterior a {_fechaReferencia.ToString("ddd-MM-yyyy")} y no tiene inventario en puerto";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Inventario Puertos", Description = error, NivelAlarma = NivelAlarma.Informativo });
                }

                row++;
            }
        }        

        private void LoadCostoVencimientoImportaicones()
        {
            Importacion impo;
            _document.SelectWorksheet("costos_almacenamiento_cargas");
            int row = 2;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string empresa = _document.GetCellValueAsString(row, 1);
                string ingrediente = _document.GetCellValueAsString(row, 2);
                string operador = _document.GetCellValueAsString(row, 3);
                string puerto = _document.GetCellValueAsString(row, 4);
                string importacion = _document.GetCellValueAsString(row, 5);
                DateTime fecha_corte = _document.GetCellValueAsDateTime(row, 6);
                double valor = _document.GetCellValueAsDouble(row, 7);

                string codigo = $"{ingrediente}_{puerto}_{operador}_{empresa}_{importacion}";

                if (_periodos.ContainsKey(fecha_corte))
                {
                    int periodo = _periodos[fecha_corte];

                    if (_importaciones.ContainsKey(codigo))
                    {
                        impo = _importaciones[codigo];
                        impo.AddCostoVencimiento(periodo, valor);
                    }
                }
                row++;
            }
        }

        private void LoadCostosOperacionPortuaria()
        {
            _document.SelectWorksheet("costos_operacion_portuaria");
            int row = 2;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string tipo = _document.GetCellValueAsString(row, 1);
                string operador = _document.GetCellValueAsString(row, 2);
                string puerto = _document.GetCellValueAsString(row, 3);
                string ingrediente = _document.GetCellValueAsString(row, 4);
                double valor = _document.GetCellValueAsDouble(row, 5);

                List<Importacion> lista = _importaciones.Values.Where(p => p.Operador == operador && p.Puerto == puerto && p.Ingrediente == ingrediente).ToList();

                if (tipo == "directo") foreach (Importacion importacion in lista) importacion.SetCostoDirecto(valor);

                if (tipo == "bodega") foreach (Importacion importacion in lista) importacion.SetCostoBodegaje(valor);

                row++;
            }
        }

        private void LoadFletes()
        {
            _document.SelectWorksheet("fletes_cop_per_kg");
            int row = 2;
            int col;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string puerto = _document.GetCellValueAsString(row, 1);
                string operador = _document.GetCellValueAsString(row, 2);
                string ingrediente = _document.GetCellValueAsString(row, 3);
                double valor;
                string planta;
                col = 4;

                while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, col)))
                {
                    planta = _document.GetCellValueAsString(1, col);
                    valor = _document.GetCellValueAsDouble(row, col);
                    col++;

                    Despacho[] despachos = _despachos.FindAll(
                        p => p.Planta.Nombre == planta &&
                            p.Importacion.Ingrediente == ingrediente &&
                            p.Importacion.Puerto == puerto &&
                            p.Importacion.Operador == operador).ToArray();


                    foreach (Despacho despacho in despachos)
                        despacho.CostoFleteKg = valor;
                }

                row++;

            }
        }

        private void LoadSafetyStock()
        {
            Planta planta;
            InventarioPlanta ip;
            _document.SelectWorksheet("safety_stock");
            int row = 2;

            while (!System.String.IsNullOrEmpty(_document.GetCellValueAsString(row, 1)))
            {
                string nombrePlanta = _document.GetCellValueAsString(row, 1);
                string ingrediente = _document.GetCellValueAsString(row, 2);
                int valor = _document.GetCellValueAsInt32(row, 3);

                planta = _plantas[nombrePlanta];
                planta.SetSafetyStock(ingrediente, valor);

                if (valor >0)
                {
                    if (planta.IngredienteExist(ingrediente))
                    {
                        ip = planta.GetInventariosPlanta()[ingrediente];
                        if (valor * ip.ConsumoMedio > ip.Capacidad)
                        {
                            string error = $"El ingrediente {ingrediente} en la planta {nombrePlanta} tiene un safety stock ({(valor * ip.ConsumoMedio).ToString("N00")} aprox) que es mayor que la capacidad de almacenamiento de {ip.Capacidad.ToString("N00")} Kg.";
                            Console.WriteLine(error);
                            _alarmas.Add(new Alarma() { Name = "Inventario Puertos", Description = error, NivelAlarma = NivelAlarma.Informativo });
                        }
                    }
                }

                row++;

            }
        }

        private void LoadIntercompanies()
        {
            _document.SelectWorksheet("venta_entre_empresas");

            double contegral_finca = _document.GetCellValueAsDouble(2,3);
            double contegral_contegral = _document.GetCellValueAsDouble(2, 2);
            double finca_finca = _document.GetCellValueAsDouble(3, 3);
            double finca_contegral = _document.GetCellValueAsDouble(3, 2);

            _intercompanies["contegral"] = new Dictionary<string, double>();
            _intercompanies["finca"] = new Dictionary<string, double>();

            _intercompanies["contegral"]["contegral"] = contegral_contegral;
            _intercompanies["contegral"]["finca"] = contegral_finca;
            _intercompanies["finca"]["finca"] = finca_finca;
            _intercompanies["finca"]["contegral"] = finca_contegral;
        }
        
        private void CrearDespachos()
        {
            Despacho despacho;
            Importacion impo;
            Planta planta;

            foreach (string codigo in this._importaciones.Keys)
            {
                impo = this._importaciones[codigo];

                foreach (string nombre_planta in this._plantas.Keys)
                {
                    planta = this._plantas[nombre_planta];

                    if (planta.ConsumoTotal(impo.Ingrediente) > 0)
                    {
                        for (int i = 0; i < this.CantidadPeriodos - 2; i++)
                        {
                            if (impo.CamionesDespachables > 0)
                            {
                                despacho = new Despacho(this, i, i + 2, impo, planta, _intercompanies[impo.Empresa][planta.Empresa]);

                                impo.AddDespacho(despacho);
                                planta.AddDespacho(despacho);
                                _despachos.Add(despacho);
                            }
                        }
                    }
                }
            }
        }

        public void CalcularClusters()
        {
            // Obtener la lista de importaciones con material despachable
            List<Importacion> importacionesDisponibles = _importaciones.Values.Where(p => p.CamionesDespachables > 0).ToList();

            string[] ingredientes = importacionesDisponibles.Select(p => p.Ingrediente).Distinct().ToArray();

            string[] puertos;

            foreach (string ingrediente in ingredientes)
                foreach (Planta planta in _plantas.Values)
                {

                    List<Despacho> despachos = _despachos.Where(p => p.Importacion.Ingrediente == ingrediente && p.Planta == planta).ToList();

                    puertos = despachos.Select(x => x.Importacion.Puerto).Distinct().ToArray();

                    if (puertos.Length > 2)
                    {

                        // Extraer los costos netos de todos los despachos con este ingrediente
                        double[][] puntos = despachos.Select(p => new double[] { p.NetoDespacho }).ToArray();

                        //Crear algoritmo K-means para 3 clusters
                        int numClusters = 3;
                        KMeans kmeans = new KMeans(numClusters);

                        // Ejecutar algoritmo clusters
                        KMeansClusterCollection clusters = kmeans.Learn(puntos);
                        int[] labels = clusters.Decide(puntos);

                        // Obtener los índices de los clusters ordenados por el valor de sus centroides
                        var clusterOrdenadoPorCentroides = clusters.Centroids
                            .Select((centroide, indice) => new { Indice = indice, Valor = centroide[0] })
                            .OrderBy(x => x.Valor)
                            .ToArray();

                        // Asignar los clusters ordenados a los valores del Enum
                        var asignacionClusterGrupo = new Dictionary<int, Cluster>
                            {
                                { clusterOrdenadoPorCentroides[0].Indice, Cluster.Bajo },
                                { clusterOrdenadoPorCentroides[1].Indice, Cluster.Medio },
                                { clusterOrdenadoPorCentroides[2].Indice, Cluster.Alto }
                            };

                        // Asignar los productos a cada grupo de acuerdo a la asignación de clusters
                        for (int i = 0; i < despachos.Count; i++)
                        {
                            despachos[i].Grupo = asignacionClusterGrupo[labels[i]];
                        }
                    }
                    else
                    {
                        // Asignar los productos a cada grupo de acuerdo a la asignación de clusters
                        for (int i = 0; i < despachos.Count; i++)
                        {
                            despachos[i].Grupo = Cluster.Medio;
                        }
                    }
                }
        }

        private void RunAdittionalValidations()
        {
            // Importaciones sin fechas de vencimiento
            foreach(Importacion impo in _importaciones.Values)
            {
                if (impo.GetCostoVencimiento().Sum() <= 0)
                {
                    string error = $"La importacion {impo.ToString()} Parece no tener costos de vencimiento";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Inventario Puertos", Description = error, NivelAlarma = NivelAlarma.Informativo });
                }
            }


            // Inventarios en planta sin consumos
            foreach(InventarioPlanta ip in _inventarioPlanta)
            {
                if(ip.InventarioInicial>0 && ip.ConsumoMedio <= 0)
                {
                    string error = $"Existe inventario de {ip.InventarioInicial} en la planta {ip.Planta.Nombre} Que parece no tener consumo proyectado";
                    Console.WriteLine(error);
                    _alarmas.Add(new Alarma() { Name = "Inventario Planta", Description = error, NivelAlarma = NivelAlarma.Informativo });
                }
            }
        }

        #endregion

        #region "Properties"

        public Alarma[] Alarmas
        {
            get
            {
                return _alarmas.ToArray();
            }
        }

        public Dictionary<int, DateTime> Fechas
        { get
            { return _fechas; }
        }

        public int[] Periodos
        {
            get
            {
                return this._periodos.Values.OrderBy(x => x).ToArray();
            }
        } 

        public int GetPeriodo(DateTime fecha)
        {
            if(_periodos.ContainsKey(fecha))
                return _periodos[fecha];
            return -1;
        }

        public DateTime? GetFecha(int periodo)
        {
            if (_fechas.ContainsKey(periodo)) return _fechas[periodo];

            return null;
        }

        public Planta[] Plantas
        {
            get { return _plantas.Values.ToArray(); }
        }

        public Importacion[] Importaciones
        {
           get { return _importaciones.Values.ToArray(); }
        }


        public Despacho[] Despachos
        {
            get { return _despachos.ToArray(); }
        }

        public double CostoFlete
        {
            get
            {
                double costo = 0;
                foreach (Despacho despacho in _despachos)
                {
                    costo += CapacidadCamion * despacho.Cantidad * despacho.CostoFleteKg;
                }

                return costo;
            }
        }

        public double CostoDirecto
        {
            get
            {
                double costo = 0;
                foreach (Despacho despacho in _despachos)
                {
                    costo += CapacidadCamion * despacho.Cantidad * despacho.Importacion.GetCostoDespachoDirecto()[despacho.PeriodoDespacho];
                }

                return costo;
            }
        }

        public double CostoBodegaje
        {
            get
            {
                double costo = 0;
                foreach (Importacion impo in _importaciones.Values)
                    for (int i = 0; i < this.CantidadPeriodos; i++)
                    {
                        costo += impo.Inventario[i] * impo.GetCostoBodegaje()[i];
                    }

                return costo;
            }
        }

        public double CostoVencimiento
        {
            get
            {
                double costo = 0;
                foreach (Importacion impo in _importaciones.Values)
                    for (int i = 0; i < this.CantidadPeriodos; i++)
                    {
                        costo += impo.Inventario[i] * impo.GetCostoVencimiento()[i];
                    }

                return costo;
            }
        }

        public Solver.ResultStatus RestulStatus { get { return this.restulStatus; } }

        public Solver Solver { get { return solver; } }

        public long CapacidadCamion { get { return _capacidad_camion; } }
        #endregion

        private void Fase_01_Solve()
        {
            Debug.Print("Iniciando fase 01");

            // Codigo oculto

            Debug.Print("Finalizando Fase 02");
        }

        private void Fase_02_Solve()
        {
            Debug.Print("Iniciando fase 02");

            // Codigo oculto

            Debug.Print("Finalizando Fase 02");
        }

        private void Fase_03_Solve()
        {
            // Codigo oculto
        }


        private void Fase_03_Test_Solve()
        {

            Debug.Print("Iniciando fase 03");

            // Codigo oculto
            Debug.Print("Finalizando Fase 03");
        }

        private void Fase_04_Solve()
        {
            // Codigo oculto
        }

        public void RemplazarSolucion()
        {

        }
    }
}
