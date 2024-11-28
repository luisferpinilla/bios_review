using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Model
{
    public class Despacho
    {
        #region "internal variables"
        private Problema _problema;
        private Importacion _importacion;
        private Planta _planta;
        private int _periodoDespacho;
        private int _periodoLlegada;
        private int _cantidad=0;
        private double _costoFleteKg = 0;
        private double _costoIntercompany = 0;
        private Cluster _cluster = Cluster.Indefinido;
        #endregion

        public Importacion Importacion { get { return _importacion; } }
        public Planta Planta { get { return _planta; } }
        public int PeriodoDespacho { get{return _periodoDespacho;} }

        public int PeriodoLlegada { get {return _periodoLlegada;} }
        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value; 
                // cambio_cantidad.Invoke(this, new EventArgs());
            }
        }

        public Cluster Grupo{get { return _cluster; } set { _cluster = value; } }

        public double CostoFleteKg { get { return _costoFleteKg; } set { this._costoFleteKg = value; } }

        public double CostoFleteCamion {  get { return _costoFleteKg*_problema.CapacidadCamion; } }

        public double CostoDirectoCamion { get { return _importacion.GetCostoDespachoDirecto()[_periodoDespacho] * _problema.CapacidadCamion; } }

        public double CostoIntercompanyCamion { get { return this.CostoItercompany * _importacion.ValorCif * _problema.CapacidadCamion; } }

        public double CostoItercompany { get { return _costoIntercompany; }}

        public Variable XCantidad{ get; set; }

        public Despacho(Problema problema, int periodoDespacho, int periodoLlegada, Importacion importacion, Planta planta, double intercompany)
        {
            _problema = problema;
            _importacion = importacion;
            _planta = planta;
            _periodoDespacho = periodoDespacho;
            _periodoLlegada = periodoLlegada;
            _costoIntercompany = intercompany;
            _cantidad = 0;
        }

        public double NetoDespacho
        {
            get
            {
                double ahorro_camion = (_importacion.AhorroBodegaje[_periodoDespacho] + _importacion.AhorroVencimiento[_periodoDespacho]) * _problema.CapacidadCamion;
                double neto_camion = CostoFleteCamion + CostoDirectoCamion + CostoIntercompanyCamion - ahorro_camion;

                return neto_camion;
            }
        }

        public override bool Equals(object? obj)
        {
            try
            {
                Despacho? objeto = obj as Despacho;
                
                if (objeto == null) return false;

                return objeto._importacion == _importacion
                    && objeto._planta == _planta
                    && objeto._periodoDespacho == _periodoDespacho;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{_importacion}_{_planta}_{_periodoDespacho}";
        }

        public override int GetHashCode()
        {
            return $"{_importacion}_{_planta}_{_periodoDespacho}".GetHashCode();
        }
    }
}
