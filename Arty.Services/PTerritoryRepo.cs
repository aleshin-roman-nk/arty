using Arty.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Arty.Services
{
	public class PTerritoryRepo
	{
		private readonly IAppDbFactory appDbFactory;

		//private readonly AppDataDb db;

		private string? _streetFilter = null;

		public PTerritoryRepo(IAppDbFactory appDbFactory)
		{
			this.appDbFactory = appDbFactory;
		}

		public void SetStreetFilter(string street)
		{
			_streetFilter = street;
		}

		public void ClearStreetFilter()
		{
			_streetFilter = null;
		}

		public int? NextPTerritoryNumber()
		{
			using (var db = appDbFactory.Create())
			{
				var number = db.PersonalTerritories.Max(t => t.Number);
				number += 1;
				return number;
			}
		}

		public PersonalTerritory Create(PersonalTerritory o)
		{
			using (var db = appDbFactory.Create())
			{
				db.PersonalTerritories.Add(o);
				db.SaveChanges();
				return o;
			}
		}

		public void CreateRange(IEnumerable<PersonalTerritory> areas)
		{
			using (var db = appDbFactory.Create())
			{
				var terrNumbers = areas.Select(x => x.Number).ToList();

				// ищем, какие Number уже есть в базе данных
				var pTerrInDb = db.PersonalTerritories.Where(x => terrNumbers.Contains(x.Number)).Select(x => x.Number).ToList();

				// Операция Б
				// теперь из списка на добавление нужно выбрать те объекты, номера которых не найдены в бд
				var pTerrToSave = areas.Where(x => !pTerrInDb.Contains(x.Number)).ToList();

				// Проверить, есть ли территрия с таким названием.
				// Если есть, взять ее id и присвоить новому участку.
				// Если нет, создать и этот id дать участку.
				// Это должно проходить ПОСЛЕ операции Б.

				// Теперь получить из базы даннх все области
				db.Territories.Load();
				bool _needSave = false;
				// Пролистать все участка, если в списке allTerr нет области с таким именем, добавить ее
				foreach (var item in pTerrToSave)
				{
					if (!db.Territories.Local.Any(x => x.Name.Equals(item.territoryName)))
					{
						_needSave = true;
						db.Territories.Add(new Territory { Name = item.territoryName });
					}
				}

				IEnumerable<Territory> allTerr;

				if (_needSave) db.SaveChanges();

				allTerr = db.Territories.ToList();

				foreach (var t in pTerrToSave)
				{
					var existingTerr = allTerr.FirstOrDefault(x => x.Name.Equals(t.territoryName));
					t.territoryId = existingTerr.Id;
				}

				db.PersonalTerritories.AddRange(pTerrToSave);

				db.SaveChanges();
			}
		}

		public void AttachPTerritoriesToArea(int fr, int to, int areaId)
		{

		}

		public IEnumerable<PersonalTerritory> GetAllNeverWorked(int areaId)
		{
			bool strtF = string.IsNullOrEmpty(_streetFilter);

			using (var db = appDbFactory.Create())
			{
				return db.PersonalTerritories
					.Include(x => x.pterrLines)
					.Include(x => x.Worker)
					.Where(pt => pt.territoryId == areaId && !db.Workers.Any(wrk => wrk.pterrID == pt.Id)).ToList();
			}
		}

		public IEnumerable<PersonalTerritory> GetAllEverWorked(int areaId)
		{
			using (var db = appDbFactory.Create())
			{
				return db.PersonalTerritories
					.Include(x => x.pterrLines)
					.Include(x => x.Worker)
					.Where(pt => pt.territoryId == areaId && db.Workers.Any(wrk => wrk.pterrID == pt.Id)).ToList();
			}
		}

		public IEnumerable<PersonalTerritory> GetOf(int terrId)
		{
			using (var db = appDbFactory.Create())
			{
				return db.PersonalTerritories
					.Include(x => x.pterrLines)
					.Include(x => x.Worker)
					.Where(x => x.territoryId == terrId)
					.ToList();
			}
		}

		public enum PTerrState { all, never, ever }
		public IEnumerable<PersonalTerritory> _getPTerritories(int terrId, PTerrState state, string? strtFltr)
		{
			using (var db = appDbFactory.Create())
			{
				IQueryable<PersonalTerritory> res;

				if (terrId != 0)
				{
					res = db.PersonalTerritories
						.Include(x => x.pterrLines)
						.Include(x => x.Worker)
						.Where(pt => pt.territoryId == terrId);
				}
				else
				{
					res = db.PersonalTerritories
						.Include(x => x.pterrLines)
						.Include(x => x.Worker)
						.Where(pt => pt.territoryId == null || pt.territoryId == 0);
				};

				if (state == PTerrState.never)
				{
					res = res.Where(x => !db.Workers.Any(wrk => wrk.pterrID == x.Id));
				}
				else if (state == PTerrState.ever)
				{
					res = res.Where(x => db.Workers.Any(wrk => wrk.pterrID == x.Id));
				}

				if (!string.IsNullOrEmpty(strtFltr))
				{
					res = res.Where(x => db.Lines.Any(l => l.PersonalTerritoryId == x.Id
					&& EF.Functions.Like(l.address, $"%{strtFltr.ToUpper()}%")));
				}

				return res.ToList();
			}
		}

		public string GetTerritoryName(int terrId)
		{
			if (terrId == 0) return "-без территории-";

			using (var db = appDbFactory.Create())
				return db.Territories.FirstOrDefault(x => x.Id == terrId).Name;
		}

		public bool HasUnfinishedWork(int id)
		{
			using (var db = appDbFactory.Create())
			{
				var w = db.Workers.Where(x => x.pterrID == id).OrderByDescending(x => x.id).FirstOrDefault();

				if (w == null) return false;
				if (w.finish == null) return true;

				return false;
			}
		}

		public void UpdateFields(PersonalTerritory a, params string[] flds)
		{
			using (var db = appDbFactory.Create())
			{
				//User u = new User { id = userId };

				db.PersonalTerritories.Attach(a);

				foreach (var item in flds)
				{
					db.Entry(a).Property(item).IsModified = true;
					//Console.WriteLine($"{item} = {}");// Ну да... А где значения?
				}

				db.SaveChanges();
			}
		}

		public void AddBusinessPoint(int pTerrLineId, string bpName)
		{
			using (var db = appDbFactory.Create())
			{
				db.BusinessPoints.Add(new BusinessPoint { name = bpName, PTerritoryLineId = pTerrLineId });
				db.SaveChanges();
			}
		}

		public void UpdateBusinessPoint(int id, string newname)
		{
			using (var db = appDbFactory.Create())
			{
				var o = db.BusinessPoints.FirstOrDefault(x => x.id == id);

				if (o == null) return;

				o.name = newname;

				db.Entry(o).Property(x => x.name).IsModified = true;
				db.SaveChanges();
			}
		}

		public void DeleteBusinessPoint(int id)
		{
			var o = new BusinessPoint { id = id };

			using (var db = appDbFactory.Create())
			{
				db.BusinessPoints.Remove(o);
				db.SaveChanges();
			}

		}

		public PersonalTerritory? Get(int id)
		{
            using (var db = appDbFactory.Create())
			{
				return db.PersonalTerritories
					.Include(x => x.pterrLines)
					.ThenInclude(x => x.BusinessPoints)
					.Include(x => x.Worker)
					.FirstOrDefault(x => x.Id == id);
			}
		}

		public IEnumerable<StreetLine> GetStreetLines(int pterrId)
		{
			using (var db = appDbFactory.Create())
			{
				return db.StreetLines.Include(x => x.Buildings).ToArray();
			}
		}

		public void AddOldLine(int pterrId, string str)
		{
			using (var db = appDbFactory.Create())
			{
				db.Lines.Add(new PTerritoryLine { PersonalTerritoryId = pterrId, address = str });
				db.SaveChanges();
			}
		}

		public void UpdateOldLine(int pterrLineId, string newaddress, string newobjects)
		{
			using (var db = appDbFactory.Create())
			{
				var o = db.Lines.FirstOrDefault(x => x.id == pterrLineId);

				if (o == null) return;

				bool save = false;

				if(!string.IsNullOrEmpty(newaddress))
				{
					o.address = newaddress.ToUpper();
					db.Entry(o).Property(x => x.address).IsModified = true;
					save = true;
				}

				if(!string.IsNullOrEmpty(newobjects))
				{
					o.objects = newobjects.ToUpper();
					db.Entry(o).Property(x => x.objects).IsModified = true;
					save = true;
				}

				if (save)
					db.SaveChanges();
			}
		}

		public PTerritoryLine? GetOldLine(int lineId)
		{
			using (var db = appDbFactory.Create())
			{
				return db.Lines
					.Include(x => x.BusinessPoints)
					.FirstOrDefault(x => x.id == lineId);
			}
		}

		public void AddStreetLine(int pterrId/*, string str*/)
		{

		}

		public void AttachRange(int frNumber, int toNumber, Territory t)
		{
			using (var db = appDbFactory.Create())
			{
				var pterrs = db.PersonalTerritories
					.Where(x => x.Number >= frNumber && x.Number <= toNumber)
					.Select(x => new PersonalTerritory { Id = x.Id }).ToArray();

				foreach (var item in pterrs)
				{
					item.territoryId = t.Id;
					db.Entry(item).Property(x => x.territoryId).IsModified = true;
				}

				db.SaveChanges();
			}
		}
	}
}
