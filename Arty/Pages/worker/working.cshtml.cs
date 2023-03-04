using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Arty.Pages.worker
{
    public class workingModel : PageModel
    {
        private readonly WorkerRepo workerRepo;

        public workingModel()
        {
            workerRepo = new WorkerRepo(new AppDataDb(@"..\db\arty.db"));
        }

		public void OnGet()
        {
            Workers = workerRepo.GetWorking().OrderBy(x => x.PersonalTerritory.Number).ToArray();

			//JsonConvert.DefaultSettings = () => new JsonSerializerSettings
			//{
			//	ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			//};
			//WorkersJson = Newtonsoft.Json.JsonConvert.SerializeObject(Workers, Newtonsoft.Json.Formatting.Indented);
		}

        public IEnumerable<Worker> Workers { get; set; }

        public string WorkersJson { get; set; }
    }
}
