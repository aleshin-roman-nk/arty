using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.personalTerritory
{
    public class detailModel : PageModel
    {
        AppDataDb _appDataDb;
        PTerritoryRepo persTerritoryRepo;
        WorkerRepo workerRepo;

        public detailModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            persTerritoryRepo = new PTerritoryRepo(_appDataDb);
            workerRepo = new WorkerRepo(_appDataDb);
        }

        public void OnGet(int id)
        {
            personalTerritory = persTerritoryRepo.Get(id);
            Workers = workerRepo.GetAll(id);
        }

        [BindProperty]
        public PersonalTerritory? personalTerritory { get; set; }
        public IEnumerable<Worker> Workers { get; set; }
    }
}
