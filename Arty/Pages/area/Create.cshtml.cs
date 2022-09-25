using Arty.Services;
using Arty.Services.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace Arty.Pages.area
{


    public class CreateModel : PageModel
    {
        AppDataDb _appDataDb;
        AreaRepo pTerritoryRepo;

        public CreateModel()
        {
            _appDataDb = new AppDataDb(@"..\db\arty.db");
            pTerritoryRepo = new AreaRepo(_appDataDb);
        }

        public void OnGet()
        {
        }

        //public IActionResult OnPost()
        public void OnPost()
        {
            //Console.WriteLine(SourceText);

            //if (string.IsNullOrEmpty(SourceText)) 
            //{
            //    err = "ERROR";
            //    ResultText = null;
            //    return RedirectToPage("/area/create", "ParseResult", new { ferr = true, msg="ERROR. Text have not to be empty" });
            //} 
            //else
            //{
            //    msg = "OK";
            //    ResultText = $"[parsed] {SourceText}";

            //    return RedirectToPage("/area/create", "ParseResult", new { ferr = false, msg = "OK. Area is added" });
            //}

            if (string.IsNullOrEmpty(SourceText))
            {
                err = "ERROR";
                ResultText = null;
            }
            else
            {
                msg = "OK";
                //ResultText = $"[parsed] {SourceText}";

                AreaParser parser = new AreaParser();
                var r = parser.Parse(SourceText);

                pTerritoryRepo.CreateRange(r);

                ResultText = parser.GetJson(r);
            }
        }

        public void OnGetParseResult(bool ferr, string msg)
        {
            if (ferr) err = msg;
            else this.msg = msg;
            PageMode = 1;
        }

        [BindProperty]
        public string? SourceText { get; set; }
        [BindProperty]
        public string? ResultText { get; set; }
        [BindProperty]
        public string? msg { get; set; }
        [BindProperty]
        public string? err { get; set; }

        public int PageMode { get; set; } = 0;
    }
}
