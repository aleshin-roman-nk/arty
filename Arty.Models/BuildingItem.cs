using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class BuildingItem
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public List<string>? Buildings { get; set; }
    }
}
