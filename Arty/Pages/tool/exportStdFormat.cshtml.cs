using Arty.Models.exportToStd;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arty.Pages.tool
{
    public class exportStdFormatModel : PageModel
    {
        PTerrWithWorkersRepo pTerr;

        public void OnGet()
        {
            mode = 0;
            pTerr = new PTerrWithWorkersRepo(new AppDataDb(@"..\db\arty.db"));
            Terrs = pTerr.GetTerritories()
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name}).ToList();
        }

        public void OnGetTerrs(int terrid)
        {
            mode = 1;
            pTerr = new PTerrWithWorkersRepo(new AppDataDb(@"..\db\arty.db"));
            Coll = pTerr.GetOf(terrid);
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/tool/exportStdFormat", "terrs", new { terrid = TerrID });
        }

        public IEnumerable<PTerrWithWorkers>? Coll { get; set; }
        
        public List<SelectListItem>? Terrs { get; set; }
        [BindProperty]
        public int TerrID { get; set; }

        public int mode { get; set; }
    }
}
