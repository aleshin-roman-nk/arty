using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.Territory
{
    public class IndexModel : PageModel
    {
        AppDataDb _appDataDb;
        TerritotyRepo territotyRepo;

        public IndexModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            territotyRepo = new TerritotyRepo(_appDataDb);
        }

        public void OnGet()
        {
            Territories = territotyRepo.Get();
        }

        public IEnumerable<Models.Territory> Territories { get; set; }
    }
}
