using Arty.Models;
using Arty.Models.exportToStd;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
    public class PTerrWithWorkersRepo
    {
        private readonly IAppDbFactory appDbFactory;

        public PTerrWithWorkersRepo(IAppDbFactory appDbFactory) 
        {
            this.appDbFactory = appDbFactory;
        }

        public IEnumerable<PTerrWithWorkers> GetOf(int terrid)
        {
            using (var db = appDbFactory.Create())
            {
                return db.PersonalTerritories
                    .Where(x => x.territoryId == terrid)
                    .Select(x => new PTerrWithWorkers 
                    { 
                        No = x.Number,
                        workers = db.Workers.Where(w => w.pterrID == x.Id).OrderBy(w => w.start).ToArray()
                    }).ToArray();
            }
        }

        public IEnumerable<Territory> GetTerritories()
        {
            using (var db = appDbFactory.Create())
            {
                return db.Territories.ToArray();
            }
        }
    }
}
