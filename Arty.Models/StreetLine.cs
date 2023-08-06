using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class StreetLine
    {
        public int id { get; set; }
        public int personalTerritoryId { get; set; }
        public string? Street { get; set; }
        public ICollection<Building>? Buildings { get; set; }
        public StreetLine()
        {
            Buildings = new List<Building>();
        }
    }
}
