using Arty.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arty.Services
{
    public class AreaRepo
    {
        private readonly IAppDbFactory appDbFactory;

        public AreaRepo(IAppDbFactory appDbFactory)
        {
            this.appDbFactory = appDbFactory;
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
                    if (!db.Territories.Local.Any(x => x.Name.Equals(item.territoryName)) )
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

        public void AttachPTerritories()
        {

        }

        public IEnumerable<PersonalTerritory> GetOf(int terrId)
        {
            using (var db = appDbFactory.Create())
            {
                return db.PersonalTerritories.Include(x => x.areaLines).Include(x => x.Worker).Where(x => x.territoryId == terrId).ToList();
            }
        }

        public bool HasUnfinishedWork(int id)
        {
            using (var db = appDbFactory.Create())
            {
                var w = db.Workers.Where(x => x.areaId == id).OrderByDescending(x => x.id).FirstOrDefault();

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

        public PersonalTerritory? Get(int id)
        {
            using (var db = appDbFactory.Create())
            {
                return db.PersonalTerritories.Include(x => x.areaLines).Include(x => x.Worker).FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
