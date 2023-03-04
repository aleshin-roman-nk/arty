using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages
{
    public class srvToolsModel : PageModel
    {
        srvToolsService srv;
		AppPropertiesRepo appPropertiesRepo;

		public srvToolsModel()
        {
            srv = new srvToolsService(new AppDataDb(@"..\db\arty.db"));
            appPropertiesRepo = new AppPropertiesRepo(new AppDataDb(@"..\db\arty.db"));
		}

		public void OnGetUpperAllAdresses()
        {
            srv.MakeAdressesUpper();
            msg = "All adresses are upper now";
		}

        public string msg { get; set; }
    }
}
