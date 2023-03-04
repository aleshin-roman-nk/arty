using Arty.Models;
using Arty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arty.Pages.personalTerritory
{
	public class attachTerritoryModel : PageModel
	{
		PTerritoryRepo pterrRepo;
		TerritotyRepo territotyRepo;

		public attachTerritoryModel()
		{
			pterrRepo = new PTerritoryRepo(new AppDataDb(@"..\db\arty.db"));
			territotyRepo = new TerritotyRepo(new AppDataDb(@"..\db\arty.db"));
		}

		public void OnGet()
		{
			Territories = new SelectList(territotyRepo.Get(), nameof(Models.Territory.Id), nameof(Models.Territory.Name));
		}

		public IActionResult OnPost()
		{
			//Console.WriteLine($"from {PersTerrFrom} to {PersTerrTo} is being connected to {TerritoryId}");
			//Territories = new SelectList(territotyRepo.Get(), nameof(Models.Territory.Id), nameof(Models.Territory.Name));

			if(string.IsNullOrEmpty(PersTerrFrom) || string.IsNullOrEmpty(PersTerrTo))
			{
				err = "you have not typed required data";
				Territories = new SelectList(territotyRepo.Get(), nameof(Models.Territory.Id), nameof(Models.Territory.Name));
				return Page();
			}

			pterrRepo.AttachRange(int.Parse(PersTerrFrom), int.Parse(PersTerrTo), new Models.Territory { Id = TerritoryId.Value });

			return RedirectToPage("/srvTools");
		}

		[BindProperty]
		public string? PersTerrFrom { get; set; }

		[BindProperty]
		public string? PersTerrTo { get; set; }
		[BindProperty]
		public int? TerritoryId { get; set; }
		public SelectList Territories { get; set; }
		public string err { get; set; }
	}
}
