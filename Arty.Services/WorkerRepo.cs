using Arty.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
	public class WorkerRepo
	{
		private readonly IAppDbFactory appDbFactory;

		public WorkerRepo(IAppDbFactory appDbFactory)
		{
			this.appDbFactory = appDbFactory;
		}

		public Worker Get(int id)
		{
			using (var db = appDbFactory.Create())
			{
				return db.Workers.FirstOrDefault(x => x.id == id);
			}
		}

		public IEnumerable<Worker> GetAll(int area_id)
		{
			using (var db = appDbFactory.Create())
			{
				return db.Workers.Where(x => x.pterrID == area_id).OrderByDescending(x => x.id).ToList();
			}
		}

		public void Save(Worker w)
		{
			using (var db = appDbFactory.Create())
			{
				db.Entry(w).State = w.id == 0 ? EntityState.Added : EntityState.Modified;
				db.SaveChanges();
			}
		}

		public void Delete(int id)
		{
			using (var db = appDbFactory.Create())
			{
				//Console.WriteLine($"{id}");

				db.Entry(new Worker { id = id }).State = EntityState.Deleted;
				db.SaveChanges();
			}
		}

		public Worker? GetLast(int pterrId)
		{
			using (var db = appDbFactory.Create())
			{
				return db.Workers.OrderBy(x => x.id).LastOrDefault(x => x.pterrID == pterrId);
			}
		}

		public IEnumerable<Worker> GetWorking()
		{
			using (var db = appDbFactory.Create())
			{
				// find all ones with finish != null

				return db.Workers.Include(x => x.PersonalTerritory)
					.Where(x => x.finish == null)
					.Select(x => new Worker
					{
						id = x.id,
						start = x.start,
						name = x.name,
						pterrID = x.pterrID,
						finish = x.finish,
						PersonalTerritory = new PersonalTerritory
						{
							Number = x.PersonalTerritory.Number,
							Id = x.PersonalTerritory.Id
						}
					})
					.ToArray();
			}

		}
	}
}
