using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages.worker
{
    public class EditModel : PageModel
    {
        AppDataDb _appDataDb;
        WorkerRepo workerRepo;
        PTerritoryRepo areaRepo;

        public EditModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            workerRepo = new WorkerRepo(_appDataDb);
            areaRepo = new PTerritoryRepo(_appDataDb);
        }

        public void OnGet()
        {

        }

        public void OnGetEdit(int id)
        {
            msg = $"editing : {id}";
            editMode = EntityEditMode.edit;

            Worker = workerRepo.Get(id);
        }

        public void OnGetCreate(int id)
        {
            msg = $"creating under personalTerritory : {id}";

            editMode = EntityEditMode.create;

            if (areaRepo.HasUnfinishedWork(id))
            {
                error = "Участок находится в работе. Повторная выдача невозможна";
                //Console.WriteLine(error);
                Worker = new Worker { pterrID = id };
                return;
            }

            var a = areaRepo.Get(id);

            if(a.StateCode == AreaState.inRest)
            {
                error = "Участок находится на отдыхе";
                Worker = new Worker { pterrID = id };
                return ;
            }

            Worker = new Worker { pterrID = id, start = DateTime.Today };
        }

        public IActionResult OnPost()
        {
            workerRepo.Save(Worker);

            var area = new PersonalTerritory { Id = Worker.pterrID, workerId = Worker.id };
            areaRepo.UpdateFields(area, "workerId");

            return RedirectToPage("/personalTerritory/detail", new { id = Worker.pterrID });
        }

        public IActionResult OnPostDelete()
        {
            workerRepo.Delete(Worker.id);

            var w = workerRepo.GetLast(Worker.pterrID);

            var area = new PersonalTerritory { Id = Worker.pterrID, workerId = w == null ? null : w.id };

            areaRepo.UpdateFields(area, "workerId");
            return RedirectToPage("/personalTerritory/detail", new { id = Worker.pterrID });
        }

        public string error { get; set; }

        public string msg { get; set; }
        public EntityEditMode editMode { get; set; }

        [BindProperty]
        public Worker? Worker { get; set; }
    }
}
