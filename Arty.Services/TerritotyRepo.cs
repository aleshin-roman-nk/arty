using Arty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
    public class TerritotyRepo
    {
        private readonly IAppDbFactory appDbFactory;

        public TerritotyRepo(IAppDbFactory appDbFactory)
        {
            this.appDbFactory = appDbFactory;
        }

        public IEnumerable<Territory> Get()
        {
            using (var db = appDbFactory.Create())
            {
                return db.Territories.ToList();
            }
        }

        public void Create(Territory t)
        {
            using (var db = appDbFactory.Create())
            {
                db.Territories.Add(t);
                db.SaveChanges();
            }
        }
    }
}
