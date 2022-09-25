using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.area
{
    public class areaInfoModel : PageModel
    {
        AppDataDb _appDataDb;
        AreaRepo areaRepo;

        public areaInfoModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            areaRepo = new AreaRepo(_appDataDb);
        }

        public void OnGet(int id)
        {
            Area = areaRepo.Get(id);
        }

        public IActionResult OnPost()
        {
            areaRepo.UpdateFields(Area, "info");
            return RedirectToPage("/area/detail", new { id = Area.Id });
        }

        [BindProperty]
        public PersonalTerritory? Area { get; set; }
    }
}
