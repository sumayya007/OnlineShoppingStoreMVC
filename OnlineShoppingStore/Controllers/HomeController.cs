using OnlineShoppingStore.DAL;
using OnlineShoppingStore.Models;
using OnlineShoppingStore.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Net;

namespace OnlineShoppingStore.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities1 db = new dbMyOnlineShoppingEntities1();
       dbMyOnlineShoppingEntities ctx = new dbMyOnlineShoppingEntities();
        public ActionResult Index(string search,int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search,4,page));
        }
        public ActionResult AboutUS()
        {
            return View();

        }
        public ActionResult ContactUS()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult SaveData(SiteUser model)
        {
            model.IsValid = false;
            db.SiteUsers.Add(model);
            db.SaveChanges();
            BuildEmailTemplate(model.ID);
            return Json("Registration successfull!",JsonRequestBehavior.AllowGet);
        }
        public ActionResult Confirm(int regId)
        {
            ViewBag.regID = regId;
            return View();
        }
        public ActionResult RegisterConfirm(int regId)
        {
            SiteUser Data = db.SiteUsers.Where(x => x.ID == regId).FirstOrDefault();
            Data.IsValid = true;
            db.SaveChanges();
            var msg = "Your email is verified successfully";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckValidUser(SiteUser model)
        {
            string result = "Fail";
            var DataItem = db.SiteUsers.Where(x => x.Email == model.Email && x.Password==model.Password).SingleOrDefault();
            if (DataItem != null)
            {
                Session["UserId"] = DataItem.ID.ToString();
                Session["UserName"] = DataItem.Username;
                result = "Success";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AfterLogin()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
        
        public ActionResult ForgotPassword()
        {
            return View();
        }

        public void SendPassword(string pemail)
        {
            SiteUser data= db.SiteUsers.Where(x => x.Email == pemail).SingleOrDefault();
            string username = data.Username;
            string password = data.Password;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("sumayya.kareem6@gmail.com", "rahmaniraheem");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "Forgot Password ( The Hijab Store)";
            msg.Body = "Dear " + username + ", Your Password is  " + password + "\n\n\nThanks & Regards\nThe Hijab Store Team";
            string toaddress = pemail;
            msg.To.Add(toaddress);
            string fromaddress = "The Hijab Store <sumayya.kareem6@gmail.com>";
            msg.From = new MailAddress(fromaddress);
            try
            {
                smtp.Send(msg);
                //var result1 = "Your password has been sent to your mail";
                Response.Write("<script>alert('Your password has been sent to your mail');</script>");

            }
            catch
            {

                Response.Write("<script>alert('Your email is not registered with us');</script>");
               
            }
        }

        public void BuildEmailTemplate(int regID)
        {
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var regInfo = db.SiteUsers.Where(x => x.ID == regID).FirstOrDefault();
            var url= "http://localhost:50725/"+"Home/Confirm?regId="+regID;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your Account is successfuly created",body,regInfo.Email);
        }

        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "sumayya.kareem6@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendMail(subject,body,to);
        }
        static void SendMail(string sSubject, string sBody,string tAddress)
        {
            const string senderID = "sumayya.kareem6@gmail.com"; // use sender's email id here..
          
            const string senderPassword = "rahmaniraheem"; // sender password here...
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here...
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, tAddress, sSubject, sBody);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
               
                Console.ResetColor();
            }
        }
        //public static void SendEmail(MailMessage mail)
        //{

        //    SmtpClient client = new SmtpClient();
        //    client.Host = "smtp@gmail.com";
        //    client.Port = 25;
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.Credentials = new System.Net.NetworkCredential("sumayya.kareem6@gmail.com", "rahmaniraheem");
        //    try
        //    {
        //        client.Send(mail);
        //    }
        //    catch(Exception ex)
        //    {
        //        var m = ex.Message;
        //        Console.WriteLine(m);
        //        throw ex;
                
        //    }
        //}

        public ActionResult AddToCart(int productId,string url)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = ctx.Tbl_Product.Find(productId);
                cart.Add(new Item()
                {

                    Product = product,
                    Quantity = 1

                });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                    var count = cart.Count();
                var product = ctx.Tbl_Product.Find(productId);
                for (int i = 0; i < count; i++)
                {

                    if (cart[i].Product.ProductId == productId)
                    {
                        int prevQnty = cart[i].Quantity;
                        cart.Remove(cart[i]);
                        cart.Add(new Item()
                        {

                            Product = product,
                            Quantity = prevQnty + 1

                        });
                        break;
                    }
                    else

                    {
                        var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
                        if (prd == null)
                        {
                            cart.Add(new Item()
                            {

                                Product = product,
                                Quantity = 1

                            });
                        }





                    }


                


                }

                Session["cart"] = cart;
            }
           
            return Redirect(url);
        }

        public ActionResult RemoveFromCart(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
         
            foreach(var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
           
            Session["cart"] = cart;

            return Redirect("Index");
        }
        public ActionResult Checkout()
        {
            return View();
        }
        public ActionResult CheckoutDetails()
        {
            return View();
        }
        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart=(List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach(var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQnty = item.Quantity;
                        if (prevQnty > 0)
                        {
                            cart.Remove(item);
                            cart.Add(new Item
                            {
                                Product = product,
                                Quantity = prevQnty - 1
                            });
                        }
                        break;
                    }
                   
                }
                Session["cart"] = cart;
            }

            return Redirect("Checkout");
        }
    }
}