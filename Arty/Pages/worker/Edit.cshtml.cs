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
        PTerritoryRepo personalTerrRepo;

        public EditModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            workerRepo = new WorkerRepo(_appDataDb);
            personalTerrRepo = new PTerritoryRepo(_appDataDb);
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

            if (personalTerrRepo.HasUnfinishedWork(id))
            {
                error = "Участок находится в работе. Повторная выдача невозможна";
                //Console.WriteLine(error);
                Worker = new Worker { pterrID = id };
                return;
            }

            var a = personalTerrRepo.Get(id);

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
            personalTerrRepo.UpdateFields(area, "workerId");

            return RedirectToPage("/personalTerritory/detail", new { id = Worker.pterrID });
        }

        public IActionResult OnPostDelete()
        {
            if (string.IsNullOrEmpty(DeleteConfirmingText)) return RedirectToPage("/worker/edit", new { handler = "edit", id = Worker.id });
            if (!DeleteConfirmingText.ToLower().Equals("отменить запись")) return RedirectToPage("/worker/edit", new { handler = "edit", id = Worker.id });

            workerRepo.Delete(Worker.id);

            var w = workerRepo.GetLast(Worker.pterrID);

            var persTerr = new PersonalTerritory { Id = Worker.pterrID, workerId = w == null ? null : w.id };

            personalTerrRepo.UpdateFields(persTerr, "workerId");
            
            return RedirectToPage("/personalTerritory/detail", new { id = Worker.pterrID });
        }

        
        //public /*JsonResult*/ IActionResult OnPostDeletingIsConfident([FromBody] Worker worker)
        //{
        //    //Console.WriteLine($"On server side: {o}");
        //    //return new JsonResult(o);

        //    Console.WriteLine($"EditModel:OnDeleteDeletingIsConfident; worker.id = {worker.id}; worker.pterrID = {worker.pterrID}");

        //    workerRepo.Delete(worker.id);

        //    var w = workerRepo.GetLast(worker.pterrID);

        //    var persTerr = new PersonalTerritory { Id = worker.pterrID, workerId = w == null ? null : w.id };

        //    personalTerrRepo.UpdateFields(persTerr, "workerId");
        //    Console.WriteLine($"EditModel:OnDeleteDeletingIsConfident RedirectToPage with worker.pterrID = {worker.pterrID}");

        //    var res = RedirectToPage("/personalTerritory/detail", new { id = worker.pterrID });

        //    Console.WriteLine($"{res.PageName}/{res.PageHandler}");
            
        //    foreach (var item in res.RouteValues)
        //    {
        //        Console.WriteLine($"{item.Key}={item.Value}");
        //    }

        //    return res;
        //}

        public string error { get; set; }

        public bool askForDelete { get; set; }

        public string msg { get; set; }
        public EntityEditMode editMode { get; set; }

        [BindProperty]
        public Worker? Worker { get; set; }

        // 02-08-2023
        [BindProperty]
        public string? DeleteConfirmingText { get; set; }
    }
}
