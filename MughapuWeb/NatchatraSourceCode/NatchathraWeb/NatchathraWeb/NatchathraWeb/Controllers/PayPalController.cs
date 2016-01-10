using PaypalMVC.Models;
using MughapuWeb.DAL;
using MughapuWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MughapuWeb.Controllers
{
    public class PayPalController : Controller
    {
        //[Authorize(Roles="Customers")]
        public ActionResult ValidateCommand(string product, string totalPrice)
        {
            //bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            //var paypal = new PayPalModel(useSandbox);

            //paypal.item_name = product;
            //paypal.amount = totalPrice;
            //return View(paypal);
           
            NVPAPICaller payPalCaller = new NVPAPICaller();
            string retMsg = "";
            string token = "";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            string amt = "250";// Session["payment_amt"].ToString();

            bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
            if (ret)
            {
                Session["token"] = token;
                RedirectToAction("RedirectFromPaypal");
            }
            else
            {
                RedirectToAction("RedirectFromPaypal");
            }

            return View();
        }
        

        public ActionResult RedirectFromPaypal()
        {
              NVPAPICaller payPalCaller = new NVPAPICaller();

                string retMsg = "";
                string token = "";
                string PayerID = "";
                NVPCodec decoder = new NVPCodec();
                token = Session["token"].ToString();

                bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    Session["payerId"] = PayerID;

                    var myOrder = new Order();
                    myOrder.OrderDate = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());
                    myOrder.Username = User.Identity.Name;
                    //myOrder.FirstName = decoder["FIRSTNAME"].ToString();
                    //myOrder.LastName = decoder["LASTNAME"].ToString();
                    //myOrder.Address = decoder["SHIPTOSTREET"].ToString();
                    //myOrder.City = decoder["SHIPTOCITY"].ToString();
                    //myOrder.State = decoder["SHIPTOSTATE"].ToString();
                    //myOrder.PostalCode = decoder["SHIPTOZIP"].ToString();
                    //myOrder.Country = decoder["SHIPTOCOUNTRYCODE"].ToString();
                    //myOrder.Email = decoder["EMAIL"].ToString();
                    myOrder.Total = Convert.ToDecimal(decoder["AMT"].ToString());

                    // Verify total payment amount as set on CheckoutStart.aspx.
                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
                        decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());
                        if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
                        {
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                        }
                    }
                    catch (Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                    }

                    // Get DB context.
                    peakzartEntities _db = new peakzartEntities();

                    // Add order to DB.
                    _db.Orders.Add(myOrder);
                    _db.SaveChanges();

                    // Get the shopping cart items and process them.
                    ShoppingCart usersShoppingCart = new ShoppingCart();
                   
                        List<Cart> myOrderList = usersShoppingCart.GetCartItems();

                        // Add OrderDetail information to the DB for each product purchased.
                        for (int i = 0; i < myOrderList.Count; i++)
                        {
                            // Create a new OrderDetail object.
                            var myOrderDetail = new OrderDetail();
                            myOrderDetail.OrderId = myOrder.OrderId;
                            //myOrderDetail.Username = User.Identity.Name;
                            myOrderDetail.ImageId = myOrderList[i].ImageId;
                            myOrderDetail.Quantity = myOrderList[i].Count;
                            myOrderDetail.UnitPrice =Convert.ToDecimal(myOrderList[i].ImageDetail.Price);

                            // Add OrderDetail to DB.
                            _db.OrderDetails.Add(myOrderDetail);
                            _db.SaveChanges();
                        }

                        // Set OrderId.
                        Session["currentOrderId"] = myOrder.OrderId;

                        // Display Order information.
                        List<Order> orderList = new List<Order>();
                        orderList.Add(myOrder);
                     
                    }
               
              
            
            return View();
        }

        public ActionResult CancelFromPaypal()
        {
            return View();
        }

        public ActionResult NotifyFromPaypal()
        {
            return View();
        }

      
    }
}
