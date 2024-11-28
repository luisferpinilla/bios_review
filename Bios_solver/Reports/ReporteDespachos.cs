using Bios_solver.Model;
using System.Text;

namespace Bios_solver.Reports
{
    public class ReporteDespachos
    {
        #region "fields"
        Problema _problema;
        #endregion

        public ReporteDespachos(Problema problema)
        {
            this._problema = problema;
        }

        public void ExportToCsv(string filePath)
        {
            // Generar el CSV
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine(",Empresa,Puerto,Operador,ingrediente,Importacion,Fecha,Planta,Camiones_despachados,Costo_Transporte_camion,Costo_Transprote,Minimo Despacho,Despacho para Safety Stock,Depsacho hasta Target,intercompany_camion,costo_despacho_camion,cluster_despacho,costos_despacho_directo,ahorro_almacenamiento_camion,ahorro_bodegaje_camion,tipo_despacho,tiempo_recepcion"); // Encabezado
            int count = 0;

            foreach (Despacho i in _problema.Despachos)
            {
                csvContent.AppendLine($"{count},{i.Importacion.Empresa},{i.Importacion.Puerto},{i.Importacion.Operador},{i.Importacion.Ingrediente},{i.Importacion.Carga},{_problema.Fechas[i.PeriodoDespacho].ToString("yyyy-MM-dd")},{i.Planta.Nombre},{i.Cantidad},{i.CostoFleteKg * _problema.CapacidadCamion},{i.CostoFleteKg * _problema.CapacidadCamion * i.Cantidad},{i.Cantidad},0,0,{i.CostoItercompany},{i.NetoDespacho},{i.Grupo.ToString()},{0},{0},{(i.Importacion.AhorroVencimiento[i.PeriodoDespacho] + i.Importacion.AhorroBodegaje[i.PeriodoDespacho]).ToString()},{i.Importacion.GetTipoDespacho(i.PeriodoDespacho)}, {i.Planta.GetInventarioPlanta(i.Importacion.Ingrediente).TiempoProceso}");
                count++;

            }

            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
