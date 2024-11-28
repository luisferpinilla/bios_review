using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Model
{
    public class Planta
    {
        private Problema _problema;
        private Queue<UnidadAlmacenamiento> _unidadesNoAsignadas;
        private string _nombre;
        private string _empresa;
        private int _operacion_minutos;
        private Dictionary<string, InventarioPlanta> _inventarios;

        public Planta(Problema problema, string nombre, string empresa, int operacion_minutos)
        {
            _problema = problema;
            _nombre = nombre;
            _empresa = empresa;
            _operacion_minutos = operacion_minutos;
            _inventarios = new Dictionary<string, InventarioPlanta>();
            _unidadesNoAsignadas = new Queue<UnidadAlmacenamiento>();
        }

        public void AddInventario(InventarioPlanta inventario)
        {
            if (_inventarios.ContainsKey(inventario.Ingrediente))
                throw new Exception("El inventario que intenta agregar ya existe");
            _inventarios.Add(inventario.Ingrediente, inventario);
        }

        public void AddTransitoPlanta(string ingrediente, int periodo, long cantidad)
        {
            if (!_inventarios.ContainsKey(ingrediente))
                throw new Exception("El transito a planta que esta agregando no tiene consumo");
            _inventarios[ingrediente].Add_llegada_planeada(periodo, cantidad);
        }

        public bool IngredienteExist(string ingrediente)
        {
            return _inventarios.ContainsKey(ingrediente);
        }

        public void AddUnidadAlmacenamiento(UnidadAlmacenamiento unidadAlmacenamiento)
        {
            if (unidadAlmacenamiento.Ingrediente != "")
            {
                if (!_inventarios.ContainsKey(unidadAlmacenamiento.Ingrediente))
                    _inventarios[unidadAlmacenamiento.Ingrediente] = new InventarioPlanta(_problema, this, unidadAlmacenamiento.Ingrediente);

                _inventarios[unidadAlmacenamiento.Ingrediente].AddUnidadAlmacenamiento(unidadAlmacenamiento);
            }
            else
            {
                _unidadesNoAsignadas.Enqueue(unidadAlmacenamiento);
            }
        }

        public void AsignarUnidadesNoAsignadas()
        {
            UnidadAlmacenamiento ua;
            string[] uaIngredientes;
            InventarioPlanta ip;
            InventarioPlanta[] candidatos;
            // Mientras que existan Unidades de almacenamiento sin asignar
            while (_unidadesNoAsignadas.Count > 0)
            {
                // Tome la primera Unidad
                ua = _unidadesNoAsignadas.Dequeue();
                // Obtenga el ingrediente que tienen la minima cantidad de dias donde la unidad tiene capacidad
                uaIngredientes = ua.GetCapacidades()
                    .Where(par => par.Value > 0) // donde las capacidades (values) sean > 0
                    .Select(par => par.Key) // Obtenga solo las claves (ingredientes)
                    .ToArray();

                candidatos = _inventarios.Values.Where(x => uaIngredientes.Contains(x.Ingrediente) && x.ConsumoMedio > 0).ToArray();

                if (candidatos.Length > 0)
                {
                    ip = _inventarios.Values.OrderBy(x => x.CapacidadDias)
                        .Where(x => uaIngredientes.Contains(x.Ingrediente) && x.ConsumoMedio > 0).First();

                    if (ip != null)
                    {
                        // Asigne el ingrediente a la unidad
                        ua.SetIngrediente(ip.Ingrediente);
                        ip.AddUnidadAlmacenamiento(ua);
                    }

                }
            }
        }

        public bool ContainsIngrediente(string ingrediente)
        {
            return _inventarios.ContainsKey(ingrediente);
        }

        public void SetTiempoProceso(string ingrediente, int tiempo)
        {
            if (!_inventarios.ContainsKey(ingrediente))
                throw new Exception($"el ingrediente {ingrediente} no existe en la planta {_nombre}");

            _inventarios[ingrediente].TiempoProceso = tiempo;
        }

        public void AddDespacho(Despacho despacho)
        {
            InventarioPlanta inventarioPlanta = _inventarios[despacho.Importacion.Ingrediente];

            inventarioPlanta.Add_Llegada(despacho, despacho.PeriodoLlegada);
        }
             
        public string Nombre
        {
            get { return this._nombre; }
        }

        public string Empresa
        {
            get { return this._empresa; }
        }

        public int TiempoTurno
        {
            get { return this._operacion_minutos; }
        }

        public long ConsumoTotal(string ingrediente)
        {
            if (_inventarios.ContainsKey(ingrediente))
                return _inventarios[ingrediente].GetTotalConsumo();

            return 0;
        }


        /// <summary>
        /// Calcula la capacidad restante para recibir camiones de un ingrediente específico en un periodo determinado.
        /// </summary>
        /// <param name="periodo">El periodo de tiempo para el cual se evalúa la capacidad de recepción.</param>
        /// <param name="ingrediente">El nombre del ingrediente para el cual se necesita calcular la capacidad de recepción.</param>
        /// <returns>
        /// Un entero que representa la cantidad máxima de camiones del ingrediente específico que se pueden recibir,
        /// considerando el tiempo de operación disponible y el tiempo ya utilizado en la recepción de camiones.
        /// </returns>
        /// <remarks>
        /// Este método suma el tiempo ya utilizado en la recepción de camiones para todos los inventarios en la planta.
        /// Luego, calcula el tiempo restante para la recepción de camiones y divide este valor por el tiempo de proceso
        /// específico del ingrediente indicado para obtener la capacidad máxima de recepción en unidades de camiones.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">
        /// Lanza una excepción si el ingrediente especificado no se encuentra en el diccionario de inventarios.
        /// </exception>
        public int CapacidadRecepcionCamiones(int periodo, string ingrediente)
        {
            int tiempoUsado = 0;
            int cantidadCamiones = 0;

            foreach(InventarioPlanta ip in _inventarios.Values)
            {
                tiempoUsado += ip.GetTiempoUsadoProceso(periodo);
            }
           
            cantidadCamiones = Convert.ToInt32((TiempoTurno - tiempoUsado) / _inventarios[ingrediente].TiempoProceso);
            return cantidadCamiones;
        }

        public Dictionary<string, InventarioPlanta> GetInventariosPlanta()
        {
            return this._inventarios; 
        }

        public InventarioPlanta? GetInventarioPlanta(string ingrediente)
        {
            if (IngredienteExist(ingrediente))
                return _inventarios[ingrediente];
            return null;
        }

        public void SetSafetyStock(string ingrediente, int safetyStock)
        {
            if (_inventarios.ContainsKey(ingrediente))
                _inventarios[ingrediente].SafetyStock = safetyStock;
        }

        public override string ToString()
        {
            return $"{_empresa}_{_nombre}";
        }

        public override bool Equals(object? obj)
        {
            try
            {
                Planta? o = obj as Planta;

                if (o == null) return false;

                return o._nombre == _nombre;
            }
            catch(Exception)
            {
                return false ;
            }
            
        }

        public override int GetHashCode()
        {
            return _nombre.GetHashCode();
        }
    }
}
