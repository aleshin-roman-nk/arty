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

        public AreaWorker Get(int id)
        {
            using (var db = appDbFactory.Create())
            {
                return db.Workers.FirstOrDefault(x => x.id == id);
            }
        }

        public IEnumerable<AreaWorker> GetAll(int area_id)
        {
            using (var db = appDbFactory.Create())
            {
                return db.Workers.Where(x => x.areaId == area_id).OrderByDescending(x => x.id).ToList();
            }
        }

        public void Save(AreaWorker w)
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

                db.Entry(new AreaWorker { id = id }).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
