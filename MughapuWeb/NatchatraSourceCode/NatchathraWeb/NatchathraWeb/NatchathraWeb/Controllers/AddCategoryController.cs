using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NatchathraWeb.DAL;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using NatchathraWeb.Models;
using System.Data;
using PagedList;
using WebMatrix.WebData;

namespace NatchathraWeb.DAL
{
    public class AddCategoryController : Controller
    {
        private peakzartEntities db = new peakzartEntities();

        //
        // GET: /AddCategory/
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            ViewBag.GroupsList = new SelectList(db.Groups, "GroupId", "GroupName", "--Select--");           
           // return View(db.Categories.ToList());

            IEnumerable<CategoryViewDetailsModel> model = null;
            model = (from I in db.Groups
                     join c in db.Categories on I.GroupId equals c.GroupID                    
                     select new CategoryViewDetailsModel { catid=c.CategoryId,CategoryName = c.CategoryName,GroupName=I.GroupName, Active = I.IsActive } );

            int pagesize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var products = (from p in model select p).OrderByDescending(x => x.catid).ToList();
            return View(products.ToPagedList(page, pagesize));

        }

       
        //
        // GET: /AddCategory/Details/5
          [Authorize]
        public ActionResult Details(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // GET: /AddCategory/Create
          [Authorize]
        public ActionResult Create()
        {
            ViewBag.GroupsList = new SelectList(db.Groups, "GroupId", "GroupName", "--Select--");  
            return View();
        }

        //
        // POST: /AddCategory/Create
        [Authorize]
        [HttpPost]      
          public ActionResult Create([Bind(Exclude = "IsActive")]Category category)
        {
            ModelState.Remove("IsActive");
            if (ModelState.IsValid)
            {
                //save new record in database
                Category newRecord = new Category();
                newRecord.CategoryId = Convert.ToInt32(db.Categories.Max(x => x.CategoryId)) + 1;
                newRecord.GroupID = category.GroupID;
                newRecord.CategoryName = category.CategoryName;               
                newRecord.IsActive = true;
                newRecord.ModifieBy = WebSecurity.CurrentUserId;
                newRecord.CreatedBy = WebSecurity.CurrentUserId;
                newRecord.CreatedDate = System.DateTime.Now;
                newRecord.ModifiedDate = System.DateTime.Now;
                db.Categories.Add(newRecord);
                db.SaveChanges();
                return RedirectToAction("Index", "AddCategory");
            }

            return View(category);
        }

        //
        // GET: /AddCategory/Edit/5
          [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Category category = db.Categories.Find(id);
            ViewBag.GroupsList = new SelectList(db.Groups, "GroupId", "GroupName", category.GroupID);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /AddCategory/Edit/5
          [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = db.Categories.SingleOrDefault(u => u.CategoryId == category.CategoryId);
                if (userToUpdate != null)
                {
                    userToUpdate.CategoryName = category.CategoryName;
                    userToUpdate.GroupID = category.GroupID;
                    userToUpdate.IsActive = category.IsActive;
                    userToUpdate.ModifieBy = WebSecurity.CurrentUserId;
                    userToUpdate.ModifiedDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                              
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /AddCategory/Delete/5
          [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /AddCategory/Delete/5
          [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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