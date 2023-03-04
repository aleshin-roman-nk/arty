using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models.exportToStd
{
    public class PTerrWithWorkers
    {
        public int? No { get; set; }
        public IEnumerable<Worker>? workers { get; set; }
    }
}
