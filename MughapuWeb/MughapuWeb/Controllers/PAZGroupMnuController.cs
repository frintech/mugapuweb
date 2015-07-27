using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MughapuWeb.DAL;
using MughapuWeb.Models;

namespace MughapuWeb.Controllers
{
    public class PAZGroupMnuController : Controller
    {
        private peakzartEntities db = new peakzartEntities();

        //
        // GET: /PAZGroupMnu/

        public ActionResult Index(int GrpID)
        {
            IEnumerable<PAZMnuViewModel> model = null;
            // model = (from G in db.Groups
            //                  join c in db.Categories on G.GroupId equals c.GroupID
            //                  join I in db.ImageDetails on c.CategoryId equals I.CategoryId
            //                  where G.GroupId == GrpID  && I.ImageId==1
            //          select new PAZMnuViewModel{CategoryName= c.CategoryName,ImagePath= I.ImagePath });
            //return View(model.ToList());


           var catlist = from G in db.Groups
                     join c in db.Categories on G.GroupId equals c.GroupID                     
                     where G.GroupId == GrpID 
                     select c;
          var img = (
                   from I in db.ImageDetails where I.IsActive==true
                   group I by I.CategoryId into g
                   select new { catid=g.Key , Img = g.Max(x => x.ImagePath)}
                 );
          model = (from cl in catlist
                   join IM in img  on cl.CategoryId equals IM.catid
                   select new PAZMnuViewModel { CategoryName = cl.CategoryName, ImagePath = @"~\Images\RESZIMGS\" + IM.Img, categoryid=cl.CategoryId,grpid=cl.GroupID }
                );
            return View(model.ToList());
        }

        public ActionResult GetCatDetails(int catID)
        {
            IEnumerable<CatDetailsViewModel> catmodel = null;           

            var catlist = from c in db.Categories
                          join I in db.ImageDetails on c.CategoryId equals I.CategoryId
                          where c.CategoryId == catID &&  I.IsActive==true
                          select new CatDetailsViewModel { ImageName = I.ImageTitle, ImagePath = I.ImagePath, Imgid = I.ImageId };          
            return View(catmodel.ToList());
        }

        //
        // GET: /PAZGroupMnu/Details/

        public ActionResult Details(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // GET: /PAZGroupMnu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PAZGroupMnu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }

        //
        // GET: /PAZGroupMnu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /PAZGroupMnu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }

        //
        // GET: /PAZGroupMnu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        //
        // POST: /PAZGroupMnu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}