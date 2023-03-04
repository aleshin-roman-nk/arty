using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class AppProperty
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
