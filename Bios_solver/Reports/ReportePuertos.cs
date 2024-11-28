using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bios_solver.Model;

namespace Bios_solver.Reports
{
    public class ReportePuertos
    {
        #region "fields"
        Problema _problema;
        #endregion

        public ReportePuertos(Problema problema)
        {
            this._problema = problema;
        }

        public void ExportToCsv(string filePath)
        {
            // Generar el CSV
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine(",Empresa,Puerto,Operador,ingrediente,Importacion,Fecha,Inventario,llegadas,Costo_Almacenamiento,Costo_Total_Almacenamiento"); // Encabezado
            int count = 0;
            long[] inventario, llegadas;
            double[] costoAlmacenamiento, costoBodegaje;
            double costoCorte, costoTotal;
            foreach (Importacion i in _problema.Importaciones)
            {
                inventario = i.Inventario;
                llegadas = i.GetLlegadas();
                costoAlmacenamiento = i.GetCostoVencimiento();
                costoBodegaje = i.GetCostoBodegaje();
                for (int j = 0; j < _problema.Fechas.Count; j++)
                {
                    costoCorte = costoAlmacenamiento[j] + costoBodegaje[j];
                    costoTotal = costoCorte*inventario[j];

                    csvContent.AppendLine($"{count}, {i.Empresa},{i.Puerto},{i.Operador},{i.Ingrediente},{i.Carga},{_problema.Fechas[j].ToString("yyyy-MM-dd")},{inventario[j]},{llegadas[j]},{costoCorte},{costoTotal}");

                    count++;
                }
            }

            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
