using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services.Tools
{
    public class LinesObject
    {
        public LinesObject()
        {
            Strings = new List<string>();
        }
        public List<string> Strings { get; set; }
    }
}
