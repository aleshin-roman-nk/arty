using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    [Table("Lines")]
    public class AreaLine
    {
        public int id { get; set; }
        public int PersonalTerritoryId { get; set; }
        public string data { get; set; }
    }
}
