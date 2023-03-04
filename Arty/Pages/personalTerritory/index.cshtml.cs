using Arty.Models;
using Arty.Services;
using Arty.Services.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Xml.Linq;

namespace Arty.Pages.personalTerritory
{
	public class indexModel : PageModel
	{
		AppDataDb _appDataDb;
		PTerritoryRepo areaRepo;
		AppPropertiesRepo appPropertiesRepo;

		public indexModel()
		{
			_appDataDb = new AppDataDb(@"..\db\arty.db");
			appPropertiesRepo = new AppPropertiesRepo(_appDataDb);
			areaRepo = new PTerritoryRepo(_appDataDb);
		}

		public void OnGet(int id)
		{
			areaId = id;

			number = appPropertiesRepo.GetInt("filter_status");

			//DataObj = areaRepo._getPTerritories(id, PTerritoryRepo.PTerrState.never, null);
			DataObj = areaRepo._getPTerritories(id, (PTerritoryRepo.PTerrState)(number - 1), null);

			areaName = areaRepo.GetTerritoryName(id);
		}

		public void OnPostSetFilter()
		{
			appPropertiesRepo.Set("filter_status", number.ToString());

			if (number == 1)// all
			{
				DataObj = areaRepo._getPTerritories(areaId, PTerritoryRepo.PTerrState.all, streetFilter);
			}
			else if (number == 2)// only never worked
			{
				DataObj = areaRepo._getPTerritories(areaId, PTerritoryRepo.PTerrState.never, streetFilter);
			}
			else if (number == 3)// only those been worked
			{
				DataObj = areaRepo._getPTerritories(areaId, PTerritoryRepo.PTerrState.ever, streetFilter);
			}
		}

		[BindProperty]
		public IEnumerable<PersonalTerritory> DataObj { get; set; }
		[BindProperty]
		public int number { get; set; }
		[BindProperty]
		public int areaId { get; set; }
		[BindProperty]
		public string areaName { get; set; }

		[BindProperty]
		public string streetFilter { get; set; }
		public string? message { get; set; } = null;
	}
}
