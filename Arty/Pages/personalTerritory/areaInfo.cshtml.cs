using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.personalTerritory
{
    public class areaInfoModel : PageModel
    {
        AppDataDb _appDataDb;
        PTerritoryRepo areaRepo;

        public areaInfoModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            areaRepo = new PTerritoryRepo(_appDataDb);
        }

        public void OnGet(int id)
        {
            Area = areaRepo.Get(id);
        }

        public IActionResult OnPost()
        {
            areaRepo.UpdateFields(Area, "info");
            return RedirectToPage("/personalTerritory/detail", new { id = Area.Id });
        }

        [BindProperty]
        public PersonalTerritory? Area { get; set; }
    }
}
