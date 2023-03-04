using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.personalTerritory
{
    public class detailModel : PageModel
    {
        AppDataDb _appDataDb;
        PTerritoryRepo areaRepo;
        WorkerRepo workerRepo;

        public detailModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            areaRepo = new PTerritoryRepo(_appDataDb);
            workerRepo = new WorkerRepo(_appDataDb);
        }

        public void OnGet(int id)
        {
            Area = areaRepo.Get(id);
            Workers = workerRepo.GetAll(id);
        }

        [BindProperty]
        public PersonalTerritory? Area { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }
}
