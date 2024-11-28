using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using Google.OrTools.LinearSolver;


namespace Bios_solver.Model
{
    public class Importacion
    {
        private Problema _problema;
        private string _ingrediente;
        private string _puerto;
        private string _operador;
        private string _empresa;
        private string _carga;
        private long _inventario_inicial;
        private double _valor_cif;
        private DateTime _fechaLlegada;
        private int _periodoBodegaje = -1;
        private long[] _inventario_final;
        private Variable[] _Xinventario_final;
        private long[] _llegada;
        private TipoDespacho[] _tipoDespacho;
        private double[] _costo_vencimiento;
        private double[] _costo_bodegaje;
        private double[] _costo_despacho_directo;
        private double[] _ahorro_bodegaje;
        private double[] _ahorro_vencimiento;
        private List<Despacho>[] _despachos_programados;

        #region "Properties"
        public string Ingrediente { get { return this._ingrediente; } }
        public string Puerto { get { return this._puerto; } }
        public string Operador { get { return this._operador; } }
        public string Empresa { get { return this._empresa; } }
        public string Carga { get { return this._carga; } }
        public double ValorCif { get { return this._valor_cif; } }
        public long InventarioInicial { get { return _inventario_inicial; } }
        public DateTime FechaLlegada { get { return this._fechaLlegada; } }
        #endregion

        public Importacion(Problema problema, string ingrediente, string puerto, string operador, string empresa, string carga, long inventario_inicial, double valor_cif, DateTime fecha_llegada)
        {
            this._problema = problema;
            this._ingrediente = ingrediente;
            this._puerto = puerto;
            this._operador = operador;
            this._empresa = empresa;
            this._carga = carga;
            this._inventario_inicial = inventario_inicial;
            this._valor_cif = valor_cif;
            this._fechaLlegada = fecha_llegada;
            this._llegada = new long[_problema.CantidadPeriodos];
            this._costo_bodegaje = new double[_problema.CantidadPeriodos];
            this._costo_vencimiento = new double[_problema.CantidadPeriodos];
            this._costo_despacho_directo = new double[_problema.CantidadPeriodos];
            this._ahorro_bodegaje = new double[_problema.CantidadPeriodos];
            this._ahorro_vencimiento = new double[_problema.CantidadPeriodos];
            this._inventario_final = new long[_problema.CantidadPeriodos];
            this._tipoDespacho = new TipoDespacho[_problema.CantidadPeriodos];
            this._Xinventario_final = new Variable[_problema.CantidadPeriodos];

            this._despachos_programados = new List<Despacho>[_problema.CantidadPeriodos];

            for (int i = 0; i < _problema.CantidadPeriodos; i++)
            {
                _llegada[i] = 0;
                _inventario_final[i] = 0;
                _costo_bodegaje[i] = 0;
                _costo_despacho_directo[i] = 0;
                _costo_vencimiento[i] = 0;
                _ahorro_bodegaje[i] = 0;
                _ahorro_vencimiento[i] = 0;
                _tipoDespacho[i] = TipoDespacho.Bodega;
                _despachos_programados[i] = new List<Despacho>();
                _Xinventario_final[i] = problema.Solver.MakeNumVar(0.0, double.PositiveInfinity, $"{_ingrediente}_{_puerto}_{_operador}_{empresa}_{i}");
            }

            Calcular_Inventario();
        }

        public void AddLlegada(int periodo, long cantidad)
        {
            _llegada[periodo] = cantidad;
            Calcular_Inventario();
        }

        public void AddCostoBodegaje(int periodo, double cantidad)
        {
            this._costo_bodegaje[periodo] = cantidad;
            Calcular_Inventario();
        }

        public void AddCostoVencimiento(int periodo, double cantidad)
        {
            this._costo_vencimiento[periodo] = cantidad;
            Calcular_Inventario();
        }

        public void AddCostoDespachoDirecto(int periodo, double cantidad)
        {
            this._costo_despacho_directo[periodo] = cantidad;
            Calcular_Inventario();
        }

        public void AddDespacho(Despacho despacho)
        {
            if (!_despachos_programados[despacho.PeriodoDespacho].Contains(despacho))
            {
                _despachos_programados[despacho.PeriodoDespacho].Add(despacho);
                Calcular_Inventario();
            }
        }

        public void SetPeriodoBodegaje(int periodo)
        {
            _periodoBodegaje = periodo;
        }

        public void SetCostoBodegaje(double valor)
        {
            if (_periodoBodegaje >= 0)
                _costo_bodegaje[_periodoBodegaje] = valor;
        }

        public void SetCostoDirecto(double valor)
        {
            for (int i = 0; i < _problema.CantidadPeriodos; i++)
                if (_tipoDespacho[i] == TipoDespacho.Directo)
                    _costo_despacho_directo[i] = valor;
        }


        public long[] GetLlegadas()
        {
            return _llegada;
        }

        public long[] Inventario
        {
            get
            {
                return _inventario_final;
            }
        }

        public double[] GetCostoVencimiento()
        {
            return _costo_vencimiento;
        }

        public double GetCostoVencimientoCamion(int periodo)
        {
            return _costo_vencimiento[periodo] * _problema.CapacidadCamion;
        }

        public double[] GetCostoDespachoDirecto()
        {
            return _costo_despacho_directo;
        }

        public double[] GetCostoBodegaje()
        {
            return _costo_bodegaje;
        }

        public double GetAhorroBodegajeCamion(int periodo)
        {
            return _costo_bodegaje[periodo]*_problema.CapacidadCamion;
        }

        public double GetAhorroVencimientoCamion(int periodo)
        {
            return _costo_vencimiento[periodo] * _problema.CapacidadCamion;
        }

        public double[] AhorroBodegaje
        {
            get
            {
                return _ahorro_bodegaje;
            }
        }

        public double[] AhorroVencimiento
        {
            get
            {
                return _ahorro_vencimiento;
            }
        }

        public Despacho[] GetDespachos(int periodo)
        {
            Despacho[] lista = _despachos_programados[periodo].ToArray();
            return lista;
        }

        public Variable[] GetXInventario()
        {
            return _Xinventario_final.ToArray();
        }

        public int CamionesDespachables
        {
           get { return Convert.ToInt32(_inventario_final[_inventario_final.Length - 1] / _problema.CapacidadCamion); }
        }

        public void Calcular_Inventario()
        {
            // Iterar en los periodos
            long inventario = _inventario_inicial;
            int despachos_programados;
            double ahorro_bodegaje = 0.0;
            double ahorro_vencimiento = 0.0;

            for (int i = 0; i < _problema.CantidadPeriodos; i++)
            {
                // totalizar las llegadas programadas
                despachos_programados = 0;

                foreach (Despacho despacho in _despachos_programados[i])
                {
                    despachos_programados += despacho.Cantidad;
                }

                inventario += _llegada[i] - _problema.CapacidadCamion*despachos_programados;


                _inventario_final[i] = inventario;
            }

            for (int i = _problema.CantidadPeriodos -1; i >= 0; i--)
            {
                ahorro_bodegaje += _costo_bodegaje[i];
                ahorro_vencimiento += _costo_vencimiento[i];
                _ahorro_bodegaje[i] = ahorro_bodegaje;
                _ahorro_vencimiento[i] = ahorro_vencimiento;
            }

        }

        public void InicializarLP()
        {
            for (int i = 0; i < _problema.CantidadPeriodos; i++)
            {
                _Xinventario_final[i] = _problema.Solver.MakeNumVar(0, _inventario_final[i], $"Inv_{this.ToString()}");
            }

            LinearExpr rest = _Xinventario_final[1] ;
        }


        #region "Overrides"
        public override bool Equals(object? obj)
        {
            try
            {
                Importacion? importacion = obj as Importacion;
                if (importacion == null) return false;

                return importacion._ingrediente == this._ingrediente
                    && importacion._puerto == _puerto
                    && importacion._operador == _operador
                    && importacion._empresa == _empresa
                    && importacion._carga == _carga;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{_ingrediente}_{_puerto}_{_operador}_{_empresa}_{_carga}";
        }

        public override int GetHashCode()
        {
            return $"{_ingrediente}_{_puerto}_{_operador}_{_empresa}_{_carga}".GetHashCode();
        }

        public void SetTipoDespacho(int periodo, TipoDespacho tipoDespacho)
        {
            _tipoDespacho[periodo]= tipoDespacho;
        }

        public TipoDespacho GetTipoDespacho(int periodo)
        {
            return _tipoDespacho[periodo];
        }
        #endregion
    }
}
