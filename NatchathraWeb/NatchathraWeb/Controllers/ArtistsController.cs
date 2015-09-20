using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NatchathraWeb.DAL;
using NatchathraWeb.Models;


namespace NatchathraWeb.Controllers
{
    public class ArtistsController : Controller
    {
        //
        // GET: /Artists/
        private peakzartEntities db = new peakzartEntities();
      
        public ActionResult Index(int ArtistsId = 0)
        {
            IEnumerable<artistDetailsViewModel> artistsList = null;
            if (ArtistsId == 0)
            {

                var artist = from A in db.Artists                                                         
                              select A;
                var img = (
                         from I in db.ImageDetails where I.IsActive==true
                         group I by I.ArtistId into g
                         select new { ArtistsId = g.Key, Img = g.Max(x => x.ImagePath) }
                       );
                artistsList = (from A in artist
                               join IM in img on A.ArtistsId equals IM.ArtistsId
                               select new artistDetailsViewModel
                               {
                                   ArtistsId = A.ArtistsId
                                   ,
                                   ArtistsName = A.ArtistsName
                                   ,
                                   ImagePath = @"~\Images\RESZIMGS\" + IM.Img
                                   
                               }
                               
                      );
            }
            else
            {
                artistsList = from A in db.Artists
                              join I in db.ImageDetails on A.ArtistsId equals I.ArtistId
                              where A.ArtistsId == ArtistsId && I.IsActive == true
                              select new artistDetailsViewModel
                              {
                                  ImageName = I.ImageTitle
                                 ,
                                  ImagePath = @"~\Images\RESZIMGS\" + I.ImagePath
                                 ,
                                  ImgId = I.ImageId
                                 ,
                                  ArtistsName = A.ArtistsName,
                                  Price=I.Price
                              };
            }
            ViewBag.ArtistsId = ArtistsId;
            return View(artistsList.ToList());
        }

    }
}
