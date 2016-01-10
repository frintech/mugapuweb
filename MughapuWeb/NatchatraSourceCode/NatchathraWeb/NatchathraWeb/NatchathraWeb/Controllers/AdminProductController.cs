using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MughapuWeb.DAL;
using System.Data;
using PagedList;
using WebMatrix.WebData;
using MughapuWeb.Models;
using System.Configuration;


namespace MughapuWeb.Controllers
{
    public class AdminProductController : Controller
    {
        //
        // GET: /AddProduct/
        private MughapuEntities db = new MughapuEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Addproduct()
        {

            var query = from c in db.Menus
                        where !(from o in db.menuTrees
                                where o.Parent_Menuid != 0
                                select o.Menu_id)
                               .Contains(c.Mnu_id)
                        select new { c.Mnu_id,c.Mnu_Name};
            ViewBag.Menus = new SelectList(query, "mnu_Id", "Mnu_Name", "--Select Category--");
            ViewBag.Brands = new SelectList(db.Brands, "BRND_id", "BRND_Name", "--Select Brand--");

            var offer = from of in db.offers
                       join ofv in db.OFFER_VALUE_TYPE on of.OFR_Value_type equals ofv.OFR_VAl_Type_Id
                        select new { of.offer_id,OFF_Name=of.offer_Name + ofv.Ofr_Value_type };
            ViewBag.offers = new SelectList(offer, "offer_id", "OFF_Name", "--Select Offer--");
          
            return View();
        }
        public JsonResult LoadSubCategory(string menuid)
        {
            int mid = Convert.ToInt32(menuid);
            var query = (from c in db.Menus
                        where (from o in db.menuTrees
                               where o.Parent_Menuid != 0 && o.Parent_Menuid == mid
                                select o.Menu_id)
                               .Contains(c.Mnu_id)
                        select new { c.Mnu_id, c.Mnu_Name }).ToList();          
            return Json(query);
        }

       [HttpPost]
        public ActionResult Addproduct(ProductInsert model)
        {
            //ViewBag.PossibleParadigms = _context.Paradigms; 
            //viewModel.Entries = _context.Entries.ToList();
            //FormCollection form
            //Check server side validation using data annotation

            if (ModelState.IsValid)
            {

                try
                {
                    //ProductInsert model = new ProductInsert();


                    //// Insert Product
                    //PRODUCT objnewproduct = new PRODUCT();
                    //objnewproduct.PDT_CODE = model["Product_Code"];
                    //objnewproduct.PDT_Name = model["Product_Name"];
                    //objnewproduct.PDT_Description = model["Product_Description"];
                    //objnewproduct.PDT_Price =Convert.ToDecimal(model["Product_Price"]);
                    //objnewproduct.Category_id =int.Parse(model["Category_id"]);
                    //objnewproduct.Sub_Category_id = int.Parse(model["Sub_Category_id"]);
                    //objnewproduct.offer_id = int.Parse(model["offer_id"]);
                    //objnewproduct.Brand_id = int.Parse(model["brand_id"]);
                    //objnewproduct.Add_user_Id = WebSecurity.CurrentUserId;
                    //objnewproduct.Mod_user_id = WebSecurity.CurrentUserId;
                    //db.PRODUCTs.Add(objnewproduct);
                    //db.SaveChanges();

                   //int productid = db.PRODUCTs.Where(x => x.PDT_CODE == model["Product_Code"]).FirstOrDefault().Pid;
                    ////insert PDT_IMAGE_PATH 
                    // // save image in folder
                    //string ImageName = System.IO.Path.GetFileName(model["Imagepath"].FileName);
                    //string physicalPath = Server.MapPath(ConfigurationManager.AppSettings["ORIGINALPATH"] + ImageName);
                    //model["Imagepath"].SaveAs(physicalPath);
                    //string targetPath = Server.MapPath(ConfigurationManager.AppSettings["RESZEFILEPATH"] + ImageName);
                    //model["Imagepath"].SaveAs(targetPath);
                    //PDT_IMAGE_PATH objImages = new PDT_IMAGE_PATH();
                    //objImages.PDT_ID = productid;
                    //objImages.Image_Path = ImageName;
                    //objImages.Banner_Flag =Convert.ToBoolean(model["Bannerflag"]);
                    //objImages.Home_Flag =Convert.ToBoolean(model["Homeflag"]);
                    //objImages.First_Flag = Convert.ToBoolean(model["Firstflag"]); 
                    //objImages.User_id = WebSecurity.CurrentUserId;
                    //db.PRODUCTs.Add(objnewproduct);

                    int productid = 10;

                    //Specifications
                    foreach (var p in model.GetType().GetProperties())
                    {
                       // Console.WriteLine("Property {0} type {1} value {2}", p.Name, p.GetValue(obj, null).GetType().Name, p.GetValue(model, null));
                        Value_Specification objvaluespec = new Value_Specification();
                        objvaluespec.Product_id = productid;
                        objvaluespec.Key_spec_id = db.KEY_Specifications.Where(x => x.spec_col_Name == p.Name).FirstOrDefault().spec_col_id;
                        if (objvaluespec.Key_spec_id != 0)
                        {
                            objvaluespec.key_Col_Value = p.GetValue(model, null).ToString();
                        }
                        objvaluespec.AddUser_id = WebSecurity.CurrentUserId;
                        objvaluespec.mod_user_Id = WebSecurity.CurrentUserId;
                        


                    }


                    db.SaveChanges();

                   
                    //Stream strm = model.ImagePhoto.InputStream;
                    //var targetFile = targetPath;
                    ////Based on scalefactor image size will vary
                    //GenerateThumbnails(0.5, strm, targetFile);


                    ////save new record in database
                    //ImageDetail newRecord = new ImageDetail();
                    //newRecord.ImageId = Convert.ToInt32(db.ImageDetails.Max(x => x.ImageId)) + 1;
                    //newRecord.ImageTitle = model.ImageTitile;
                    //newRecord.ImageDescription = model.ImageDesc;
                    //newRecord.CategoryId = model.CategoryId;
                    //newRecord.ArtistId = model.ArtistId;
                    //newRecord.ImagePath = ImageName;
                    //newRecord.IsActive = true;
                    //newRecord.ModifieBy = WebSecurity.CurrentUserId;
                    //newRecord.CreatedBy = WebSecurity.CurrentUserId;
                    //newRecord.CreatedDate = System.DateTime.Now;
                    //newRecord.ModifiedDate = System.DateTime.Now;
                    //newRecord.Price = model.Price;
                    //db.ImageDetails.Add(newRecord);
                    //db.SaveChanges();

                    ModelState.Clear();

                }

                catch { }

               // return RedirectToAction("Display", "FileUpload");

            }

            else
            {
             
                return View();
            }

            return View();

        }

    }
}
