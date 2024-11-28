using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Model
{
    public class InventarioPlanta
    {
        private Problema _problema;
        private Planta _planta;
        private List<UnidadAlmacenamiento> _unidadesAlmacenamiento;
        private string _ingrediente;
        private long[] _inventario_final;
        private long[] _consumo;
        private long[] _backorder;
        private long[] _llegadas_planeadas;
        private int _tiempo_proceso;
        private int[] _tiempo_total_proceso;
        private int[] _dias;
        private int _safetySTock;
        private List<Despacho>[] _llegadas_programadas;

        public InventarioPlanta(Problema problema, Planta planta, string ingrediente)
        {
            _problema = problema;
            _planta = planta;
            _unidadesAlmacenamiento = new List<UnidadAlmacenamiento>();
            _ingrediente = ingrediente;
            _inventario_final = new long[_problema.CantidadPeriodos];
            _consumo = new long[_problema.CantidadPeriodos];
            _backorder = new long[_problema.CantidadPeriodos];
            _tiempo_total_proceso = new int[_problema.CantidadPeriodos];
            _tiempo_proceso = 1;
            _llegadas_planeadas = new long[_problema.CantidadPeriodos];
            _dias = new int[_problema.CantidadPeriodos];
            _llegadas_programadas = new List<Despacho>[_problema.CantidadPeriodos];
            _safetySTock = 0;

            for (int i = 0; i < _problema.CantidadPeriodos; i++)
            {
                _llegadas_programadas[i] = new List<Despacho>();
                _tiempo_total_proceso[i] = 0;
            }

            Calcular_Inventario();
        }

        public void Calcular_Inventario()
        {
            // Iterar en los periodos
            long inventario = InventarioInicial;
            long cobertura;
            int tiempo_proceso;
            int llegadas_programadas;
            int dias;
            int ref_dia;
            for (int i = 0; i < _problema.CantidadPeriodos; i++)
            {
                // totalizar las llegadas programadas
                llegadas_programadas = 0;
                tiempo_proceso = 0;

                foreach (Despacho despacho in _llegadas_programadas[i])
                {
                    llegadas_programadas += despacho.Cantidad;
                    tiempo_proceso += this._tiempo_proceso * despacho.Cantidad;
                }

                _tiempo_total_proceso[i] = tiempo_proceso;

                inventario += _llegadas_planeadas[i] + _problema.CapacidadCamion * llegadas_programadas - _consumo[i];

                if (inventario < 0)
                {
                    _backorder[i] = -1 * inventario;
                    _inventario_final[i] = 0;
                    inventario = 0;
                }
                else
                {
                    _backorder[i] = 0;
                    _inventario_final[i] = inventario;
                }

                if (GetTotalConsumo() > 0)
                {
                    cobertura = inventario;
                    ref_dia = i;
                    dias = 0;
                    while (cobertura > _consumo[ref_dia])
                    {
                        dias++;
                        cobertura -= _consumo[ref_dia];
                        if (ref_dia < _problema.CantidadPeriodos - 1) ref_dia++;
                    }

                    _dias[i] = dias;
                }
                else
                {
                    _dias[i] = 365 * 10;
                }

            }
        }

        public void Add_llegada_planeada(int periodo, long cantidad)
        {
            if (_llegadas_planeadas[periodo] > 0)
            {
                Console.WriteLine($"Es posible que los transitos a planta de {_ingrediente} estén duplicados");
            }
            _llegadas_planeadas[periodo] += cantidad;
            Calcular_Inventario();
        }

        public void Add_Consumo(int periodo, long cantidad)
        {
            if (_consumo[periodo] > 0)
            {
                Console.WriteLine($"Es posible que los consumos en planta de {_ingrediente} estén duplicados");
            }
            _consumo[periodo] += cantidad;
            Calcular_Inventario();
        }

        public void Add_Llegada(Despacho despacho, int periodo)
        {
            _llegadas_programadas[periodo].Add(despacho);
            Calcular_Inventario();
        }

        public void AddUnidadAlmacenamiento(UnidadAlmacenamiento ua)
        {
            if (ua.Ingrediente != this.Ingrediente)
                throw new Exception("El ingrediente de la unidad de almacenamiento no corresponde al inventario de planta");

            this._unidadesAlmacenamiento.Add(ua);

            Calcular_Inventario();

        }

        public long InventarioInicial
        {
            get { return _unidadesAlmacenamiento.Sum(x => x.Inventario); }

        }

        public long Capacidad
        {
            get { return _unidadesAlmacenamiento.Sum(x => x.Capacidad); }
    
        }

        public int SafetyStock
        {
            get { return _safetySTock; }
            set { _safetySTock = value; }
        }

        public int TiempoProceso
        {
            get { return _tiempo_proceso; }
            set { _tiempo_proceso = value; }
        }

        public string Ingrediente { get { return _ingrediente; } }

        public long[] GetConsumo()
        {
            return _consumo;
        }

        public long GetTotalConsumo()
        {
            return _consumo.Sum();
        }

        public double ConsumoMedio
        {
            get
            {
                return _consumo.Average();
            }
        }

        public double CapacidadDias
        {
            get
            {
                return ConsumoMedio > 0 && Capacidad>0 ? Capacidad / ConsumoMedio : 0;
            }
        }
        public long[] GetLlegadaPlaneada()
        {
            return _llegadas_planeadas;
        }

        public long[] Inventario
        {
            get
            {
                return _inventario_final;
            }
        }

        public int GetTiempoUsadoProceso(int periodo)
        {            
            int tiempoUsado =0;

            foreach (Despacho despacho in _llegadas_programadas[periodo])
                tiempoUsado += despacho.Cantidad * TiempoProceso;

            return tiempoUsado;
        }

        public long[] GetBackorder()
        {
            return _backorder;
        }

        public long[] LlegadasPlanta
        {
            get
            {
                return _llegadas_planeadas;
            }
        }

        public int[] Dias { get { return _dias; } }

        public int GetDistanciaSafetyStock(int periodo)
        {
            if (_dias[periodo] < _safetySTock)
                return _safetySTock - _dias[periodo];
            
            return 0;
        }


        private int CalcularPeriodoMinimoRecepcionTarget()
        {
            int i;
                for(i = Inventario.Length - 1; i>0; i--)
                {
                    if (_inventario_final[i] + _problema.CapacidadCamion > Capacidad)
                        return i;
                }
                return 0;
        }

        public int PeriodoMinimoRecepcionTarget
        {
            get
            {
                int i;
                for(i = Inventario.Length - 1; i>0; i--)
                {
                    if (_inventario_final[i] + _problema.CapacidadCamion > Capacidad)
                        return i;
                }
                return 0;
            }
        }

        public Planta Planta { get { return _planta; } }

        public override string ToString()
        {
            return $"{_planta.Nombre}: {_ingrediente.ToString()}";
        }
    }
}
