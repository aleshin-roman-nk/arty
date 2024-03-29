using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.personalTerritory
{
	// �������� � �������������� ������� PersonalTerritory
	public class CreateExModel : PageModel
	{
		public int modeEditing => 1;
		public int modeCreating => 2;

		PTerritoryRepo personalTerrRepo;
		public CreateExModel() 
		{
			personalTerrRepo = new PTerritoryRepo(new AppDataDb(@"..\db\arty.db"));
		}

		public void OnGetCreate(int terrain)
		{
			mode = modeCreating;

			personalTerritory = new PersonalTerritory
			{
				territoryId = terrain,
				Number = personalTerrRepo.NextPTerritoryNumber()
			};
		}

		public void OnGetEdit(int personalterritory)
		{
			mode = modeEditing;
			personalTerritory = personalTerrRepo.Get(personalterritory);
			streetLines = personalTerrRepo.GetStreetLines(personalterritory);
		}

		public IActionResult OnPostCreate()
		{
			personalTerrRepo.Create(personalTerritory);

            return RedirectToPage("/personalTerritory/createex", new { handler="edit", personalterritory = personalTerritory.Id });
		}

		public IActionResult OnPostAddress(string streetName)
		{
			personalTerrRepo.AddOldLine(personalTerritory.Id, streetName);
			return RedirectToPage("/personalTerritory/createex", new { handler = "edit", personalterritory = personalTerritory.Id });
		}

		public IActionResult OnPostBuilding()
		{
			return RedirectToPage("/personalTerritory/createex", new { handler = "edit", personalterritory = personalTerritory.Id });
		}

		[BindProperty]
		public PersonalTerritory personalTerritory { get; set; }

		[BindProperty]
		public int mode { get; set;}

		[BindProperty]
		public IEnumerable<StreetLine> streetLines { get; set; }
	}
}
