using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.personalTerritory.streetLine
{
    public class EditModel : PageModel
    {
        PTerritoryRepo personalTerrRepo;

        public EditModel()
        {
            personalTerrRepo = new PTerritoryRepo(new AppDataDb(@"..\db\arty.db"));
        }

        public void OnGet(int id)
        {
            this.id = id;

            pterrLine = personalTerrRepo.GetOldLine(id);
        }

        public IActionResult OnPost()
        {
            personalTerrRepo.UpdateOldLine(pterrLine.id, pterrLine.address, pterrLine.objects);

            return RedirectToPage("/personalTerritory/createex", new { handler = "edit", personalterritory = pterrLine.PersonalTerritoryId });
        }

        public IActionResult OnPostUpdateBusinessPoint(int bpId, string bpName)
        {

            if (!string.IsNullOrEmpty(bpName))
                personalTerrRepo.UpdateBusinessPoint(bpId, bpName);

			return RedirectToPage("/personalTerritory/streetLine/edit", new { id = id });
		}

		public IActionResult OnPostDeleteBusinessPoint(int bpId)
        {
            personalTerrRepo.DeleteBusinessPoint(bpId);

			return RedirectToPage("/personalTerritory/streetLine/edit", new { id = id });
		}


		public IActionResult OnPostAddBusinessPoint(string busnPointName)
        {
            personalTerrRepo.AddBusinessPoint(this.id, busnPointName);

			return RedirectToPage("/personalTerritory/streetLine/edit", new { id = id });
		}

		[BindProperty]
        public int id { get; set; }

        [BindProperty]
        public PTerritoryLine pterrLine { get; set; }
    }
}
