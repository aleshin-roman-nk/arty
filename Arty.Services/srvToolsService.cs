using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
	public class srvToolsService
	{
		private readonly IAppDbFactory appDbFactory;

		public srvToolsService(IAppDbFactory appDbFactory)
		{
			this.appDbFactory = appDbFactory;
		}

		public void MakeAdressesUpper()
		{
			using (var db = appDbFactory.Create())
			{
				var l = db.Lines.ToArray();
				foreach (var item in l)
				{
					item.data = item.data.ToUpper();
				}
				db.SaveChanges();
			}
		}
	}
}
