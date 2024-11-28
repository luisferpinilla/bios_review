using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios_solver.Model
{

    public enum NivelAlarma
    {
        Critico=0,
        Informativo=1,
    }
    public class Alarma
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public NivelAlarma NivelAlarma { get; set; }
    }
}
