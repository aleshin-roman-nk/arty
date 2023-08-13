using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    [Table("Lines")]
    public class PTerritoryLine
    {
        public int id { get; set; }
        public int PersonalTerritoryId { get; set; }
        public string address { get; set; }
        public string? objects { get; set; } // восход, ангар, сбербанк, все торговые места
        //public string? onjectType { get; set; } // магазин; тц (например ангар будет тц)
        public ICollection<BusinessPoint> BusinessPoints { get; set; }
        public PTerritoryLine() { BusinessPoints = new List<BusinessPoint>(); }
    }
}
