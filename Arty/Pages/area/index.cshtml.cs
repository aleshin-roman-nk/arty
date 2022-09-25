using Arty.Models;
using Arty.Services;
using Arty.Services.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Xml.Linq;

namespace Arty.Pages.area
{
    public class indexModel : PageModel
    {
        AppDataDb _appDataDb;
        AreaRepo areaRepo;

        public indexModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            areaRepo = new AreaRepo(_appDataDb);
        }

        public void OnGet(int id)
        {

            DataObj = areaRepo.GetOf(id);

            //AreaParser db = new AreaParser();
            //DataObj = db.BuildAreaCollection(db.ReadLines("out-corrected-manually.txt"));
        }

        public void OnGetSaveAll()
        {
            //AreaParser db = new AreaParser();
            //var strs = db.ReadLines("out-corrected-manually.txt");
            //var areas = db.BuildAreaCollection(strs);

            //DataObj = areas;

            //try
            //{
            //    pTerritoryRepo.CreateRange(areas);
            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //    return;
            //}

            //message = "OK";
        }

        [BindProperty]
        public IEnumerable<PersonalTerritory> DataObj { get; set; }
        public string? message { get; set; } = null;
    }
}
