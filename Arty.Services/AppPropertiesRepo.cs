using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
    public class AppPropertiesRepo
    {
        private readonly IAppDbFactory appDbFactory;

        public AppPropertiesRepo(IAppDbFactory appDbFactory)
        {
            this.appDbFactory = appDbFactory;
        }

        //public string Get(string name)
        //{

        //}

        public int GetInt(string name)
        {
			using (var db = appDbFactory.Create())
            {
				var p = db.AppProperties.FirstOrDefault(x => x.Name.Equals(name));
                return int.Parse(p.Value);
			}
		}

        public void Set(string name, string value)
        {
            using (var db = appDbFactory.Create())
            {
                var p = db.AppProperties.FirstOrDefault(x => x.Name.Equals(name));

                if (p == null)
                {
                    db.AppProperties.Add(new Models.AppProperty { Name = name, Value = value});
                    db.SaveChanges();
                    return;
                }

                p.Value = value;
                db.SaveChanges();
            }
        }
	}
}
