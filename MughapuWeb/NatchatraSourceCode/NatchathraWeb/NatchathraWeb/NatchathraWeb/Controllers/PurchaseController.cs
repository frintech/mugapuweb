using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MughapuWeb.Models;
using PayPalMvc;
using MughapuWeb.Services;
using PayPalMvc.Enums;
using MughapuWeb.DAL;
using System.IO;
using System.Configuration;
using System.Net.Mail;

namespace MughapuWeb.Controllers
{
    public class PurchaseController : Controller
    {
        private static TransactionService transactionService = new TransactionService();

        #region Set Express Checkout and Get Checkout Details

        public ActionResult PayPalExpressCheckout()
        {
            WebUILogging.LogMessage("Express Checkout Initiated");

            GetSession();
            // SetExpressCheckout
            ApplicationCart cart = (ApplicationCart)Session["Cart"];
            string serverURL = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/");
            SetExpressCheckoutResponse transactionResponse = transactionService.SendPayPalSetExpressCheckoutRequest(cart, serverURL);
            // If Success redirect to PayPal for user to make payment
            if (transactionResponse == null || transactionResponse.ResponseStatus != PayPalMvc.Enums.ResponseType.Success)
            {
                SetUserNotification("Sorry there was a problem with initiating a PayPal transaction. Please try again and contact an Administrator if this still doesn't work.");
                string errorMessage = (transactionResponse == null) ? "Null Transaction Response" : transactionResponse.ErrorToString;
                WebUILogging.LogMessage("Error initiating PayPal SetExpressCheckout transaction. Error: " + errorMessage);
                return RedirectToAction("Error", "Purchase");
            }
            TempData["cartresponse"] = transactionResponse;
            return Redirect(string.Format(PayPalMvc.Configuration.Current.PayPalRedirectUrl, transactionResponse.TOKEN));
        }
        private void GetSession()
        {
            var cartes = ShoppingCart.GetCart(this.HttpContext);            
            var critems = cartes.GetCartItems();
            List<ApplicationCartItem> ItemsView = new List<ApplicationCartItem>();
            foreach (DAL.Cart drAC in critems)
            {
                ApplicationCartItem objACT = new ApplicationCartItem();
                objACT.Description = drAC.ImageDetail.ImageDescription;
                objACT.Name = drAC.ImageDetail.ImageTitle;
                objACT.Price = Convert.ToDecimal(drAC.ImageDetail.Price);
                objACT.Quantity = drAC.Count;
                ItemsView.Add(objACT);
            }
           
            string strFormat = "PO-" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + GetRandomNumber().ToString();           
            ApplicationCart cart = new ApplicationCart
            {
                Id = strFormat, //Guid.NewGuid(), // Unique cart Id
                Currency = ConfigurationManager.AppSettings["currency_code"],//"USD",
                PurchaseDescription = "Peakzart-Sale Deed", // This is what appears in the user's PayPal order history, not the individual items
                Items = ItemsView
            };

            // Storing this in session, you might want to store in it a database
            Session["Cart"] = cart;

        }

        public ActionResult PayPalExpressCheckoutAuthorisedSuccess(string token, string PayerID) // Note "PayerID" is returned with capitalisation as written
        {
            // PayPal redirects back to here
            WebUILogging.LogMessage("Express Checkout Authorised");
            // GetExpressCheckoutDetails
            TempData["token"] = token;
            TempData["payerId"] = PayerID;
            GetExpressCheckoutDetailsResponse transactionResponse = transactionService.SendPayPalGetExpressCheckoutDetailsRequest(token);
            if (transactionResponse == null || transactionResponse.ResponseStatus != PayPalMvc.Enums.ResponseType.Success)
            {
                SetUserNotification("Sorry there was a problem with initiating a PayPal transaction. Please try again and contact an Administrator if this still doesn't work.");
                string errorMessage = (transactionResponse == null) ? "Null Transaction Response" : transactionResponse.ErrorToString;
                WebUILogging.LogMessage("Error initiating PayPal GetExpressCheckoutDetails transaction. Error: " + errorMessage);
                return RedirectToAction("Error", "Purchase");
            }
            TempData["Address"] = transactionResponse.PAYMENTREQUEST_0_SHIPTONAME + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOSTREET + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOCITY + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOSTATE + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOZIP + Environment.NewLine;
            TempData["tranresponse"] = transactionResponse;
            return RedirectToAction("ConfirmPayPalPayment");
        }

        #endregion

        #region Confirm Payment

        public ActionResult ConfirmPayPalPayment()
        {
            WebUILogging.LogMessage("Express Checkout Confirmation");
            ApplicationCart cart = (ApplicationCart)Session["Cart"];
            ViewBag.Address = TempData["Address"].ToString();
            return View(cart);
        }

        [HttpPost]
        public ActionResult ConfirmPayPalPayment(bool confirmed = true)
        {
            WebUILogging.LogMessage("Express Checkout Confirmed");
            ApplicationCart cart = (ApplicationCart)Session["Cart"];
            // DoExpressCheckoutPayment
            string token = TempData["token"].ToString();
            string payerId = TempData["payerId"].ToString();
            DoExpressCheckoutPaymentResponse transactionResponse = transactionService.SendPayPalDoExpressCheckoutPaymentRequest(cart, token, payerId);

            if (transactionResponse == null || transactionResponse.ResponseStatus != PayPalMvc.Enums.ResponseType.Success)
            {
                if (transactionResponse != null && transactionResponse.L_ERRORCODE0 == "10486")
                {
                    // Redirect user back to PayPal in case of Error 10486 (bad funding method)
                    // https://www.x.com/developers/paypal/documentation-tools/how-to-guides/how-to-recover-funding-failure-error-code-10486-doexpresscheckout
                    WebUILogging.LogMessage("Redirecting User back to PayPal due to 10486 error (bad funding method - typically an invalid or maxed out credit card)");
                    return Redirect(string.Format(PayPalMvc.Configuration.Current.PayPalRedirectUrl, token));
                }
                SetUserNotification("Sorry there was a problem with taking the PayPal payment, so no money has been transferred. Please try again and contact an Administrator if this still doesn't work.");
                string errorMessage = (transactionResponse == null) ? "Null Transaction Response" : transactionResponse.ErrorToString;
                WebUILogging.LogMessage("Error initiating PayPal DoExpressCheckoutPayment transaction. Error: " + errorMessage);
                return RedirectToAction("Error", "Purchase");
            }

            if (transactionResponse.PaymentStatus == PaymentStatus.Completed) //|| transactionResponse.PaymentStatus == PaymentStatus.Pending
            {
                
                return RedirectToAction("PostPaymentSuccess");
            }
            else if (transactionResponse.PaymentStatus == PaymentStatus.Pending)
            {
                WebUILogging.LogMessage("Error taking PayPal payment. Error: " + transactionResponse.PAYMENTINFO_0_PENDINGREASON + " - Payment Error: " + transactionResponse.PAYMENTINFO_0_PENDINGREASON);
                //TempData["TransactionResult"] = "You payment is pending due to this reason " + transactionResponse.PAYMENTINFO_0_PENDINGREASON + " Please contact Peakzart Support.";
                TempData["TransactionResult"] =  transactionResponse.PAYMENTINFO_0_PENDINGREASON;
                return RedirectToAction("PostPaymentPending");
            }
            else
            {
                // Something went wrong or the payment isn't complete
                WebUILogging.LogMessage("Error taking PayPal payment. Error: " + transactionResponse.ErrorToString + " - Payment Error: " + transactionResponse.PaymentErrorToString);
                TempData["TransactionResult"] = transactionResponse.PAYMENTREQUEST_0_LONGMESSAGE;
                return RedirectToAction("PostPaymentFailure");
            }
        }

        #endregion

        #region Post Payment and Cancellation

        public ActionResult PostPaymentSuccess()
        {
            WebUILogging.LogMessage("Post Payment Result: Success");
            ApplicationCart cart = (ApplicationCart)Session["Cart"];
            ViewBag.TrackingReference = cart.Id;
            ViewBag.Description = cart.PurchaseDescription;
            ViewBag.TotalCost = cart.TotalPrice;
            ViewBag.Currency = cart.Currency;
            var cartes = ShoppingCart.GetCart(this.HttpContext);
            SaveOrder();
            cartes.EmptyCart();
            SendEmail();
            return View();
        }
        public ActionResult PostPaymentPending()
        {
            WebUILogging.LogMessage("Post Payment Result: Pending");
            ApplicationCart cart = (ApplicationCart)Session["Cart"];
            ViewBag.TrackingReference = cart.Id;
            ViewBag.Description = cart.PurchaseDescription;
            ViewBag.TotalCost = cart.TotalPrice;
            ViewBag.Currency = cart.Currency;
            ViewBag.PendingReason = TempData["TransactionResult"];          
            var cartes = ShoppingCart.GetCart(this.HttpContext);
            SaveOrder();
            cartes.EmptyCart();
            SendEmail();
            return View();
        }
        private void SaveOrder()
        {
            ApplicationCart cartTets = (ApplicationCart)Session["Cart"];
            var cartes = ShoppingCart.GetCart(this.HttpContext);
            var critems = cartes.GetCartItems();
            List<DAL.OrderDetail> ItemsView = new List<DAL.OrderDetail>();
            foreach (DAL.Cart drAC in critems)
            {
                OrderDetail objACT = new OrderDetail();
                objACT.ImageId = drAC.ImageId;
                objACT.Quantity = drAC.Count;
                objACT.UnitPrice = Convert.ToDecimal(drAC.ImageDetail.PDT_Price);              
                ItemsView.Add(objACT);
            }

            peakzartEntities db = new DAL.peakzartEntities();
            Order newRecord = new Order();           
            newRecord.Username = cartes.GetSessionUserName();
            newRecord.Tracking_REF_ID = cartTets.Id.ToString();
            newRecord.OrderDate = System.DateTime.Now;
            newRecord.Total = cartTets.TotalPrice;
            newRecord.Currency = cartTets.Currency;
            newRecord.Description = cartTets.PurchaseDescription;
            newRecord.OrderDetails = ItemsView;
            db.Orders.Add(newRecord);
            db.SaveChanges();               
           
          

        }
        public void SendEmail()
        {
            string body;
          
                //using (var sr = new StreamReader(Server.MapPath("~/App_Data/Emailtemplate/") + "EmailTemplate.txt"))
            using (var sr = new StreamReader(Server.MapPath(ConfigurationManager.AppSettings["ConfirmationEmail"])))
                {
                    body = sr.ReadToEnd();
                }
          
                GetExpressCheckoutDetailsResponse transactionResponse = (GetExpressCheckoutDetailsResponse)TempData["tranresponse"];           
                ApplicationCart cartTets = (ApplicationCart)Session["Cart"];
                string strItem = "";
                foreach (MughapuWeb.Models.ApplicationCartItem item in cartTets.Items)
                {
                    strItem += item.Quantity + " x " + item.Name + " @ " + item.Price + " each" + Environment.NewLine;
                }
                string address = transactionResponse.PAYMENTREQUEST_0_SHIPTONAME + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOSTREET + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOCITY + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOSTATE + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOCOUNTRYCODE + Environment.NewLine + transactionResponse.PAYMENTREQUEST_0_SHIPTOZIP + Environment.NewLine;
                string sender = ConfigurationManager.AppSettings["EmailFromAddress"];
                string emailSubject = ConfigurationManager.AppSettings["ConfirmationEmailSubject"];
                string messageBody = string.Format(body, transactionResponse.TrackingReference, cartTets.PurchaseDescription, cartTets.Currency, cartTets.TotalPrice, strItem, address);
                string ToEmail = ConfigurationManager.AppSettings["EmailToEmail"];
                MailMessage mail = new MailMessage();
                mail.To.Add(transactionResponse.EMAIL);
               // mail.CC.Add(response.EMAIL);
                mail.From = new MailAddress(sender);
                mail.Subject = emailSubject;
                mail.Body = messageBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["EmailHost"];
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["EmailPort"]);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                (ConfigurationManager.AppSettings["EmailUid"], ConfigurationManager.AppSettings["EmailPwd"]);// Enter seders User name and password
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSSL"]); ;
                smtp.Send(mail);

        }

        //Function to get random number
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public int GetRandomNumber()
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(1, 99999);
            }
        }
        public ActionResult PostPaymentFailure()
        {
            WebUILogging.LogMessage("Post Payment Result: Failure");
            ViewBag.ErrorMessage = TempData["TransactionResult"];
            return View();
        }

        public ActionResult CancelPayPalTransaction()
        {
            return View();
        }

        #endregion

        #region Transaction Error

        private void SetUserNotification(string notification)
        {
            TempData["ErrorMessage"] = notification;
        }

        public ActionResult Error()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }

        #endregion

    }
}
