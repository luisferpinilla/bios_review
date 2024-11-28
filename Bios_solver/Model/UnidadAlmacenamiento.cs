using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Model
{
    public class UnidadAlmacenamiento
    {
        private Planta _planta;
        private string _nombre;
        private string _ingrediente;
        private long _inventario;
        private Dictionary<string, long> _capacidades;

        public UnidadAlmacenamiento(Planta planta, string nombre, long inventario)
        {
            _planta = planta;
            _nombre = nombre;
            _inventario = inventario;
            _ingrediente = "";
            _capacidades = new Dictionary<string, long>();
        }

        public void SetIngrediente(string ingrediente)
        {
            _ingrediente = ingrediente;
        }

        public void SetCapacidad(string ingrediente, long capacidad)
        {
            _capacidades[ingrediente] = capacidad;
        }

        public Dictionary<string, long> GetCapacidades()
        {
            return _capacidades;
        }

        public string Ingrediente { get { return _ingrediente;} }

        public long Inventario {  get { return _inventario; } }

        public long Capacidad { get { return String.IsNullOrEmpty(_ingrediente)? 0 : _capacidades[_ingrediente]; } }
    }
}
