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
        AreaRepo areaRepo;

        public EditModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            workerRepo = new WorkerRepo(_appDataDb);
            areaRepo = new AreaRepo(_appDataDb);
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
            msg = $"creating under area : {id}";

            editMode = EntityEditMode.create;

            if (areaRepo.HasUnfinishedWork(id))
            {
                error = "Участок находится в работе. Повторная выдача невозможна";
                //Console.WriteLine(error);
                Worker = new AreaWorker { areaId = id };
                return;
            }

            var a = areaRepo.Get(id);

            if(a.StateCode == AreaState.inRest)
            {
                error = "Участок находится на отдыхе";
                Worker = new AreaWorker { areaId = id };
                return ;
            }

            Worker = new AreaWorker { areaId = id, start = DateTime.Today };
        }

        public IActionResult OnPost()
        {
            workerRepo.Save(Worker);

            var area = new PersonalTerritory { Id = Worker.areaId, workerId = Worker.id };
            areaRepo.UpdateFields(area, "workerId");

            return RedirectToPage("/area/detail", new { id = Worker.areaId });
        }

        public IActionResult OnPostDelete()
        {
            workerRepo.Delete(Worker.id);
            var area = new PersonalTerritory { Id = Worker.areaId, workerId = null };
            areaRepo.UpdateFields(area, "workerId");
            return RedirectToPage("/area/detail", new { id = Worker.areaId });
        }

        public string error { get; set; }

        public string msg { get; set; }
        public EntityEditMode editMode { get; set; }

        [BindProperty]
        public AreaWorker? Worker { get; set; }
    }
}
