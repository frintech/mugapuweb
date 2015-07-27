using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MughapuWeb.DAL;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using PagedList;
using WebMatrix.WebData;

namespace MughapuWeb.Controllers
{
    public class AddArtistController : Controller
    {
        private peakzartEntities db = new peakzartEntities();

        //
        // GET: /AddArtist/
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            var model = db.Artists.ToList();
            int pagesize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            var products = (from p in model select p).OrderByDescending(x => x.ArtistsId).ToList();
            return View(products.ToPagedList(page, pagesize));
            
        }

       
        //
        // GET: /AddArtist/Create
          [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AddArtist/Create
          [Authorize]
        [HttpPost]
        public ActionResult Create(Artist artists)
        {
            ModelState.Remove("IsActive");
            if (ModelState.IsValid)
            {
                Artist newRecord = new Artist();
                newRecord.ArtistsId = Convert.ToInt32(db.Artists.Max(x => x.ArtistsId)) + 1;
                newRecord.ArtistsName = artists.ArtistsName;                
                newRecord.IsActive = true;
                newRecord.ModifieBy = WebSecurity.CurrentUserId;
                newRecord.CreatedBy = WebSecurity.CurrentUserId;
                newRecord.CreatedDate = System.DateTime.Now;
                newRecord.ModifiedDate = System.DateTime.Now;
                db.Artists.Add(newRecord);
                db.SaveChanges();               
                return RedirectToAction("Index");
            }

            return View(artists);
        }

        //
        // GET: /AddArtist/Edit/5
          [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Artist artists = db.Artists.Find(id);
            if (artists == null)
            {
                return HttpNotFound();
            }
            return View(artists);
        }

        //
        // POST: /AddArtist/Edit/5
          [Authorize]
        [HttpPost]
        public ActionResult Edit(Artist artists)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = db.Artists.SingleOrDefault(u => u.ArtistsId == artists.ArtistsId);
                if (userToUpdate != null)
                {
                    userToUpdate.ArtistsName = artists.ArtistsName;
                    userToUpdate.IsActive = artists.IsActive;                   
                    userToUpdate.ModifieBy = WebSecurity.CurrentUserId;
                    userToUpdate.ModifiedDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(artists);
        }

        //
        // GET: /AddArtist/Delete/5
          [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Artist artists = db.Artists.Find(id);
            if (artists == null)
            {
                return HttpNotFound();
            }
            return View(artists);
        }

        //
        // POST: /AddArtist/Delete/5
          [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artists = db.Artists.Find(id);
            db.Artists.Remove(artists);
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