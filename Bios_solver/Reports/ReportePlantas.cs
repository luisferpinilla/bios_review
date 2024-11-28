using Accord.MachineLearning;
using Bios_solver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Reports
{
    public class ReportePlantas
    {
        #region "fields"
        Problema _problema;
        #endregion

        public ReportePlantas(Problema problema)
        {
            this._problema = problema;
        }

        public void ExportToCsv(string filePath)
        {
            // Generar el CSV
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine(",variable,planta,ingrediente,periodo,inventario,capacidad,consumo,backorder,objetivo,safety_stock,tiempo_turno"); // Encabezado
            int count = 0;
            int safetyStock;
            long capacidad;
            long[] inventario, backorder, consumo;

            foreach (Planta p in _problema.Plantas)
                foreach (InventarioPlanta i in p.GetInventariosPlanta().Values)
                {
                    inventario = i.Inventario;
                    capacidad = i.Capacidad;
                    consumo = i.GetConsumo();
                    backorder = i.GetBackorder();
                    safetyStock = i.SafetyStock;

                    for (int j = 0; j < _problema.Fechas.Count; j++)
                    {
                        csvContent.AppendLine($"{count},inventario en planta,{p.Nombre},{i.Ingrediente},{_problema.Fechas[j].ToString("yyyy-MM-dd")},{inventario[j]},{capacidad},{consumo[j]},{backorder[j]},0,{safetyStock * consumo[j]}, {p.TiempoTurno}");

                        count++;
                    }
                }

            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
