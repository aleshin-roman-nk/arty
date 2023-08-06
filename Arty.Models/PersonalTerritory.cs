using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Models
{
    public class PersonalTerritory
    {
        public int Id { get; set; }
        public string? Title { get; set; }// area number

        public int? Number { get; set; }// номер участка
        public string? AreaType { get; set; } // многоквартирки, частные, деловая

        public int? territoryId { get; set; }
        public string? territoryName { get; set; }

        public bool isWorking { get; set; }

        //public string SourceText { get; set; }// исходный текст из файла, который брат сделал в ворде

        // четность
        public string? formula { get; set; }

        //public List<string> data { get; set; } = new List<string>(); 

        public List<AreaLine> areaLines { get; set; } = new List<AreaLine>();


        public string? info { get; set; }
        public string? total { get; set; }


        public int? workerId { get; set; }
        public Worker? Worker { get; set; }

        public AreaState StateCode
        {
            get
            {
                // is at rest if less than six months have passed since the date of return
                if (Worker == null) return AreaState.neverBeenWorked;

                // returned to rest
                if (Worker.finish != null)
                {
                    var passed = DateTime.Today - Worker.finish;
                    if (passed?.TotalDays >= 30 * 6)
                    {
                        return AreaState.readyToStartWorked;
                    }
                    else return AreaState.inRest;
                }

				// anyway if Worker != null the pterritory is under work
				return AreaState.working;
            }

        }
    }

    public enum AreaState { working, inRest, neverBeenWorked, readyToStartWorked }
}
