using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services.Tools
{
    public class AreaParser
    {
        public IEnumerable<LinesObject> LoadLinesObjects(string fname)
        {
            var strs = File.ReadAllLines(fname, Encoding.UTF8);

            List<LinesObject> res = new List<LinesObject>();

            LinesObject? lobj = null;

            foreach (var item in strs)
            {
                string line = item.Trim();

                if(string.IsNullOrWhiteSpace(line)) continue;

                string[] lines = line.Split('\t');

                if (line.StartsWith("Участок"))
                {
                    if(lobj != null) res.Add(lobj);
                    lobj = new LinesObject();
                }

                if(lobj != null)
                {
                    foreach (var item1 in lines)
                    {
                        if(!string.IsNullOrWhiteSpace(item1))
                            lobj.Strings.Add(item1);
                    }
                }
            }

            return res;
        }

        public void SaveAllLines(string[] lns, string fname)
        {
            File.WriteAllLines(fname, lns);
        }
    }
}
