using NatchathraWeb.DAL;
using NatchathraWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NatchathraWeb.Controllers
{
    public class ImagedataController : Controller
    {
        //
        // GET: /Imagedata/
        private peakzartEntities db = new peakzartEntities();
        public ActionResult Index()
        {
           // IEnumerable<CatDetailsViewModel> catmodel = null;

            var catmodel = from c in db.Categories
                          join I in db.ImageDetails on c.CategoryId equals I.CategoryId
                          where I.IsActive == true
                          select new CatDetailsViewModel { ImageName = I.ImageTitle, ImagePath = @"~\Images\RESZIMGS\"+I.ImagePath, Imgid = I.ImageId ,
                                                           grpid = c.GroupID,
                                                           categoryid = I.CategoryId,
                                                           Price = I.Price,
                                                           categoryName=c.CategoryName
                          };
            return View(catmodel.ToList());
          
        }

    }
}
