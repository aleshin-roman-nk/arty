using Arty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Arty.Services.Tools
{
    public class AreaParser
    {

        public IEnumerable<PersonalTerritory> Parse(string strs)
        {
            string[] work = strs.Split('\n');

            List<PersonalTerritory> res = new List<PersonalTerritory>();
            PersonalTerritory? pTerritory = null;

            foreach (var item in work)
            {
                string line = item.Trim();

                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("Участок", StringComparison.InvariantCultureIgnoreCase))
                {
                    pTerritory = new PersonalTerritory();
                    pTerritory.Title = line;
                    pTerritory.Number = int.Parse(line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
                    res.Add(pTerritory);
                    continue;
                }
                else if (line.StartsWith("Всего", StringComparison.InvariantCultureIgnoreCase))
                {
                    var t = line.Split(':', StringSplitOptions.RemoveEmptyEntries);

                    pTerritory.total = t[1];

                    continue;
                }
                else if(line.StartsWith("[терр]"))
                {
                    pTerritory.territoryName = line.Substring("[терр]".Length, line.Length - "[терр]".Length).Trim();
                }
                else if (line.StartsWith("[тип]"))
                {
                    pTerritory.AreaType = line.Substring("[тип]".Length, line.Length - "[тип]".Length).Trim(); ;
                }
                else if (line.StartsWith("[четность]"))
                {
                    pTerritory.formula = line.Substring("[четность]".Length, line.Length - "[четность]".Length).Trim();
                }
                else // just addresses
                {
                    pTerritory.pterrLines.Add(new PTerritoryLine { address = line.ToUpper() });
                }
            }

            return res;
        }

        public string GetJson(IEnumerable<PersonalTerritory> items)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
        }




        //public IEnumerable<string> PreParse(string strs)
        //{
        //    string[] work = strs.Split('\n');

        //    List<string> res = new List<string>();

        //    res.Add($"[prepared]");

        //    foreach (var item in work)
        //    {
        //        string line = item.Trim();

        //        if (string.IsNullOrWhiteSpace(line)) continue;

        //        if (line.StartsWith("Участок", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            res.Add($"");
        //            res.Add($"[n] {line}");
        //            continue;
        //        }
        //        else if (line.StartsWith("Всего", StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            res.Add($"[total] {line}");
        //            continue;
        //        }
        //        else
        //        {
        //            string[] adresses = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);

        //            foreach (var adr in adresses)
        //            {
        //                res.Add($"[d] {line}");
        //            }
        //        }
        //    }

        //    return res;
        //}



        public IEnumerable<LinesObject> LoadLinesObjects(string fname)
        {
            var strs = File.ReadAllLines(fname, Encoding.UTF8);

            List<LinesObject> res = new List<LinesObject>();

            LinesObject? lobj = null;

            foreach (var item in strs)
            {
                string line = item.Trim();

                if(string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("Участок"))
                {
                    if(lobj != null) res.Add(lobj);
                    lobj = new LinesObject();

                    line = $"[n] {line}";

                    lobj.Strings.Add(line);
                    continue;
                }
                else if (line.StartsWith("Всего"))
                {
                    line = $"[i] {line}";
                    if (lobj != null) lobj.Strings.Add(line);
                    continue;
                }
                else
                {
                    string[] adresses = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var adr in adresses)
                    {
                        if (lobj != null)
                        {
                            string d = adr.Trim();
                            lobj.Strings.Add($"[d] {d}");
                        }
                    }
                }

                //string[] lines = line.Split('\t');
                //if (lobj != null)
                //{
                //    foreach (var item1 in lines)
                //    {
                //        if(!string.IsNullOrWhiteSpace(item1))
                //            lobj.Strings.Add(item1);
                //    }
                //}


                //if (lobj != null)
                //{
                //    if (!string.IsNullOrWhiteSpace(line))
                //        lobj.Strings.Add(line);
                //}
            }

            return res;
        }

        public string[] ReadLines(string fname)
        {
           return File.ReadAllLines(fname, Encoding.UTF8);
        }

        public IEnumerable<PersonalTerritory> BuildAreaCollection(string[] lines)
        {
            List<PersonalTerritory> res = new List<PersonalTerritory>();
            PersonalTerritory? current = null;

            int addrlineNo = 1;

            foreach (var item in lines)
            {
                if (item.StartsWith("[n]"))
                {
                    addrlineNo = 1;

                    current = new PersonalTerritory();
                    res.Add(current);
                    current.Title = item.Substring("[n]".Length, item.Length - "[n]".Length).Trim();

                    var detail = current.Title.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                    current.Number = int.Parse(detail[1]);

                    if (detail[2].Equals("Частный")) current.AreaType = "Частный сектор";
                    else current.AreaType = "Многоэтажки";

                    //string[] parts = current.Title.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    //foreach (var part in parts)
                    //    Console.Write($"({part})");
                    //Console.WriteLine();

                }
                else if (item.StartsWith("[i]"))
                {
                    current.formula = item.Substring("[i]".Length, item.Length - "[i]".Length).Trim();

                }
                else if (item.StartsWith("[d]"))
                {
                    //current.data.Add(item.Substring("[d]".Length, item.Length - "[d]".Length).Trim());
                    current.pterrLines.Add(new PTerritoryLine { address = item.Substring("[d]".Length, item.Length - "[d]".Length).Trim()});
                }
            }

            return res;
        }

        public void SaveJSON(IEnumerable<PersonalTerritory> d, string fname)
        {
            var j = Newtonsoft.Json.JsonConvert.SerializeObject(d, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fname, j);
        }

        public List<PersonalTerritory> Areas { get; } = new List<PersonalTerritory>();

        public void SaveAllLines(string[] lns, string fname)
        {
            File.WriteAllLines(fname, lns);
        }
    }
}
