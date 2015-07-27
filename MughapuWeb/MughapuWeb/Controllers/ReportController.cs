using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MughapuWeb.Models;
namespace MughapuWeb.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult PrintRepert()
           {
               return View();

   
          }

    }
}
