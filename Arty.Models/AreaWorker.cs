using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class AreaWorker
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int areaId { get; set; }
        public bool closed { get; set; }
        [DisplayFormat(DataFormatString = "{уууу.MM.dd}")]
        public DateTime? start { get; set; }
        [DisplayFormat(DataFormatString = "{уууу.MM.dd}")]
        public DateTime? finish { get; set; }
    }
}
