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


namespace NatchathraWeb.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/
        peakzartEntities db = new peakzartEntities();
       [Authorize]
        public ActionResult Index()
        {
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", "--Select--");
            ViewBag.Artist = new SelectList(db.Artists, "ArtistsId", "ArtistsName", "--Select--");
            return View();
        }
         [Authorize]
       public ActionResult ImageView()
         {
            ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", "--Select--");
            ViewBag.Artist = new SelectList(db.Artists, "ArtistsId", "ArtistsName", "--Select--");
            return View();
        }
        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
[HttpPost]
 public ActionResult FileUpload(CatList clist,HttpPostedFileBase file)
{
    if (ModelState.IsValid)
    {
        if (file != null)
        {

            string ImageName = System.IO.Path.GetFileName(file.FileName);
            string physicalPath = Server.MapPath(ConfigurationManager.AppSettings["ORIGINALPATH"] + ImageName);

            // save image in folder
            file.SaveAs(physicalPath);

            string targetPath = Server.MapPath(ConfigurationManager.AppSettings["RESZEFILEPATH"] + ImageName);
            Stream strm = file.InputStream;
            var targetFile = targetPath;
            //Based on scalefactor image size will vary
            GenerateThumbnails(0.5, strm, targetFile);


            //save new record in database
            ImageDetail newRecord = new ImageDetail();
            newRecord.ImageId = Convert.ToInt32(db.ImageDetails.Max(x => x.ImageId)) + 1;
            newRecord.ImageTitle = Request.Form["Imgtitle"];
            newRecord.ImageDescription = Request.Form["Desc"];
            newRecord.CategoryId = Convert.ToInt32(clist.CategoryId);
            newRecord.ArtistId = Convert.ToInt32(clist.ArtistId);
            newRecord.ImagePath = ImageName;
            newRecord.IsActive = true;
            newRecord.ModifieBy = WebSecurity.CurrentUserId;
            newRecord.CreatedBy = WebSecurity.CurrentUserId; 
            newRecord.CreatedDate = System.DateTime.Now;
            newRecord.ModifiedDate = System.DateTime.Now;
            newRecord.Price = clist.Price;
            db.ImageDetails.Add(newRecord);
            db.SaveChanges();

        }
        //Display records
        return RedirectToAction("Display", "FileUpload");
    }
    //Display records
    return RedirectToAction("Index", "FileUpload");
   
    
}

         [HttpPost]
         [Authorize]
public ActionResult ImageView([Bind(Exclude = "ArtistId")] ImageInfo model)
         {

             //Check server side validation using data annotation

             if (ModelState.IsValid)
             {

                 try
                 {



                     string ImageName = System.IO.Path.GetFileName(model.ImagePhoto.FileName);
                     string physicalPath = Server.MapPath(ConfigurationManager.AppSettings["ORIGINALPATH"] + ImageName);

                     // save image in folder
                     model.ImagePhoto.SaveAs(physicalPath);

                     string targetPath = Server.MapPath(ConfigurationManager.AppSettings["RESZEFILEPATH"] + ImageName);
                     Stream strm = model.ImagePhoto.InputStream;
                     var targetFile = targetPath;
                     //Based on scalefactor image size will vary
                     GenerateThumbnails(0.5, strm, targetFile);


                     //save new record in database
                     ImageDetail newRecord = new ImageDetail();
                     newRecord.ImageId = Convert.ToInt32(db.ImageDetails.Max(x => x.ImageId)) + 1;
                     newRecord.ImageTitle = model.ImageTitile;
                     newRecord.ImageDescription = model.ImageDesc;
                     newRecord.CategoryId = model.CategoryId;
                     newRecord.ArtistId = model.ArtistId;
                     newRecord.ImagePath = ImageName;
                     newRecord.IsActive = true;
                     newRecord.ModifieBy = WebSecurity.CurrentUserId;
                     newRecord.CreatedBy = WebSecurity.CurrentUserId;
                     newRecord.CreatedDate = System.DateTime.Now;
                     newRecord.ModifiedDate = System.DateTime.Now;
                     newRecord.Price = model.Price;
                     db.ImageDetails.Add(newRecord);
                     db.SaveChanges();

                     ModelState.Clear();

                 }

                 catch { }

                 return RedirectToAction("Display", "FileUpload");

             }

             else
             {
                 ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", model.CategoryId);
                 ViewBag.Artist = new SelectList(db.Artists, "ArtistsId", "ArtistsName", model.ArtistId);
                 return View(model);
             }



         }

 [Authorize]
  public ActionResult Display(int page = 1)
{
    
    IEnumerable<ImageDetailsModel> model = null;
    model = (from I in db.ImageDetails
                  join c in db.Categories on I.CategoryId equals c.CategoryId
                  join A in db.Artists on I.ArtistId equals A.ArtistsId into AT
                  from subAT in AT.DefaultIfEmpty()
                  select new ImageDetailsModel {ImageId= I.ImageId,ImageTitle= I.ImageTitle,ImageDescription= I.ImageDescription,ImagePath= I.ImagePath, ArtistName = (subAT == null ? String.Empty : subAT.ArtistsName),CategoryName= c.CategoryName,Active=I.IsActive,Price=I.Price }
    );

    int pagesize =Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
    var products = (from p in model select p).OrderByDescending(x => x.ImageId).ToList();
    return View(products.ToPagedList(page, pagesize));
}
private IEnumerable<CatList> Getcategory()
{
    IEnumerable<CatList> catlist = null;

    catlist = from c in db.Categories             
              select new CatList { CategoryId =c.CategoryId, CategoryName = c.CategoryName };
    return catlist;
}

 [Authorize]
public ActionResult Edit(int id = 0)
{
    
    ImageDetail imgdtls = db.ImageDetails.Find(id);
    if (imgdtls == null)
    {
        return HttpNotFound();
    }
    ViewBag.Categories = new SelectList(db.Categories, "CategoryId", "CategoryName", imgdtls.CategoryId);

    List<SelectListItem> items = new SelectList(db.Artists, "ArtistsId", "ArtistsName", imgdtls.ArtistId).ToList();
    items.Insert(0, (new SelectListItem { Text = "- Select Artist -", Value = "0" }));
    ViewBag.Artist = items;
   // ViewBag.Artist = new SelectList(db.Artists, "ArtistsId", "ArtistsName", imgdtls.ArtistId);
    return View(imgdtls);
}

// POST: /AddCategory/Edit/5

[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit(ImageDetail imgdtls)
{
    if (ModelState.IsValid)
    {
        //db.Entry(imgdtls).State = EntityState.Modified;
        var userToUpdate = db.ImageDetails.SingleOrDefault(u => u.ImageId == imgdtls.ImageId);
        if (userToUpdate != null)
        {
            userToUpdate.ImageTitle = imgdtls.ImageTitle;
            userToUpdate.ImageDescription = imgdtls.ImageDescription;
            userToUpdate.IsActive = imgdtls.IsActive;
            userToUpdate.CategoryId = imgdtls.CategoryId;
            userToUpdate.ArtistId = imgdtls.ArtistId;
            userToUpdate.ModifieBy = WebSecurity.CurrentUserId;
            userToUpdate.ModifiedDate = System.DateTime.Now;
            userToUpdate.Price = imgdtls.Price;
            db.SaveChanges();
        }
       // db.SaveChanges();
        return RedirectToAction("Display");
    }
    return View(imgdtls);
}

//
// GET: /AddCategory/Delete/5
 [Authorize]
public ActionResult Delete(int id = 0)
{
    ImageDetail imgedtls = db.ImageDetails.Find(id);
    if (imgedtls == null)
    {
        return HttpNotFound();
    }
    return View(imgedtls);
}

//
// POST: /AddCategory/Delete/5

[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    //ImageDetail imgedtls = db.ImageDetails.Find(id);
    // var ResizePath = Server.MapPath(ConfigurationManager.AppSettings["RESZEFILEPATH"]) + imgedtls.ImagePath;
    // var originalimgPath = Server.MapPath(ConfigurationManager.AppSettings["ORIGINALPATH"])+ imgedtls.ImagePath;
    //if (System.IO.File.Exists(ResizePath))
    //{
    //    System.IO.File.Delete(ResizePath);
        
    //}
    // if (System.IO.File.Exists(originalimgPath))
    //{
    //    System.IO.File.Delete(originalimgPath);
        
    //}
    //db.ImageDetails.Remove(imgedtls);
    //db.SaveChanges();

    var userToUpdate = db.ImageDetails.SingleOrDefault(u => u.ImageId == id);
    if (userToUpdate != null)
    {
       
        userToUpdate.IsActive = false;  
        userToUpdate.ModifieBy = WebSecurity.CurrentUserId;
        userToUpdate.ModifiedDate = System.DateTime.Now;
     
        db.SaveChanges();
    }

    return RedirectToAction("Display");
}

protected override void Dispose(bool disposing)
{
    db.Dispose();
    base.Dispose(disposing);
}


    }
}
