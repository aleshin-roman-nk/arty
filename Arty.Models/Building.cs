using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class Building
    {
        public int id { get; set; }
        public int streetLineId { get; set; }
        public string name { get; set; }
        public int x { get;set; }
        public int y { get;set; }
        public int r { get;set; }
    }
}
