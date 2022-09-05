using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class OneArea
    {
        public int Id { get; set; }
        public string? Name { get; set; }// area number
        public List<BuildingItem>? BuildingItems { get; set; }

        public string SourceText { get; set; }// исходный текст из файла, который брат сделал в ворде
    }
}
