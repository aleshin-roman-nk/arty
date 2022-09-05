using Arty.Services.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Arty.Pages
{
    public class TestPageModel : PageModel
    {
        public void OnGet()
        {
            AreaParser db = new AreaParser();

            DataObj = db.LoadLinesObjects("areas.txt");

            List<string> alllines = new List<string>();

            foreach (var item in DataObj)
            {
                foreach(var line in item.Strings)
                {
                    alllines.Add(line);
                }
                alllines.Add("------------------------------------------");
            }

            db.SaveAllLines(alllines.ToArray(), "out.txt");
        }

        [BindProperty]
        public IEnumerable<LinesObject> DataObj { get; set; }
    }
}
