using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.area
{
    public class detailModel : PageModel
    {
        AppDataDb _appDataDb;
        AreaRepo areaRepo;
        WorkerRepo workerRepo;

        public detailModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            areaRepo = new AreaRepo(_appDataDb);
            workerRepo = new WorkerRepo(_appDataDb);
        }

        public void OnGet(int id)
        {
            Area = areaRepo.Get(id);
            Workers = workerRepo.GetAll(id);
        }

        [BindProperty]
        public PersonalTerritory? Area { get; set; }
        public IEnumerable<AreaWorker> Workers { get; set; }
    }
}
