using CMS_WebApp.Areas.ad.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;
using System.Net;
using CaptchaMvc.HtmlHelpers;

namespace CMS_WebApp.Areas.ad.Controllers{
    

    public class AuthController : Controller
    {
        // GET: Administrator/Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {           
            string msg = "";
            bool success = false;
            CMS_Entities context = new CMS_Entities();
            var user = context.Users.Where(u => u.Username == loginModel.Username).SingleOrDefault();
            if (user != null)
            {
                if (user.IsDeleted == 1 || user.IsActived == false)
                {
                    msg = "Tài khoản không đúng hoặc đang khóa, vui lòng kiểm tra lại";
                    success = false;
                }
                // login successfully
                else if (user.Password == Crypto.Hash(loginModel.Password))
                {
                    success = true;
                    
                    Session["activeUser"] = new User()
                    {
                        Username = user.Username,
                        Fullname = user.Fullname,
                        ProfileImage = user.ProfileImage,
                        RoleId = user.RoleId,
                        Role = user.Role
                    };
                    
                    // update last login datetime
                    user.LastLogin = DateTime.Now;
                    // reset wrongtimes password
                    user.WrongTimes = 0;

                    context.SaveChanges();
                }
                else
                {
                    if(IsOverWrongTimes(context, user))
                    {
                        msg = "Tài khoản bị khóa vì bạn đã nhập sai mật khẩu nhiều lần. Vui lòng liên hệ quản trị viên để hỗ trợ";
                    }
                    else
                    {
                        msg = "Mật khẩu không đúng";
                    }
                    
                    success = false;
                }
            }
            else
            {
                msg = "Tài khoản không tồn tại";
                success = false;
            }
            return Json(new { Success = success, Msg = msg }, JsonRequestBehavior.AllowGet);
        }

        private bool IsOverWrongTimes(CMS_Entities _context, User user)
        {
            bool isOver = false;
            user.WrongTimes = (user.WrongTimes ?? 0) + 1;
            if(user.WrongTimes > 3)
            {
                isOver = true;
                user.IsActived = false;                
            }

            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return isOver;
        }

        public ActionResult Logout()
        {
            if (Session["activeUser"] != null)
            {
                var userid = ((User)Session["activeUser"]).Username;
                CMS_Entities context = new CMS_Entities();
                var user = context.Users.Where(u => u.Username == userid).FirstOrDefault();
                if (user != null)
                {
                    user.LastLogout = DateTime.Now;
                    context.SaveChanges();
                }
            }
            Session["activeUser"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult UserProfile()
        {
            CMS_Entities context = new CMS_Entities();
            User userSession = Session["activeUser"] as User;
            if (userSession != null)
            {
                User user = context.Users.Where(u => u.Username == userSession.Username).FirstOrDefault();
                if (user != null)
                {
                    // convert uset to UserModel
                    UserProfileModel userModel = new UserProfileModel()
                    {
                        Username = user.Username,
                        Password = string.Empty,
                        ConfirmPassword = string.Empty,
                        FullName = user.Fullname,
                        Email = user.Email,
                        Phone = user.Phone,
                        ProfileImage = user.ProfileImage
                    };

                    return View(userModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfileModel userModel)
        {
            // checking validation
            if (ModelState.IsValid)
            {
                // Profile Image Path
                if (userModel.UploadFile != null && userModel.UploadFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(userModel.UploadFile.FileName);
                    string fileExtension = Path.GetExtension(userModel.UploadFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                    userModel.ProfileImage = "/Areas/ad/Profiles/" + fileName;
                    userModel.UploadFile.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Profiles/"), fileName));
                }

                // Save to database
                CMS_Entities context = new CMS_Entities();
                User updateUser = context.Users.Where(u => u.Username == userModel.Username).SingleOrDefault();

                // Update Fields
                updateUser.Fullname = userModel.FullName;
                updateUser.Email = userModel.Email;
                updateUser.Phone = userModel.Phone;
                updateUser.ProfileImage = userModel.ProfileImage;

                // password
                if (!string.IsNullOrEmpty(userModel.Password))
                {
                    updateUser.Password = Crypto.Hash(userModel.Password);
                }
                context.Entry(updateUser).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    context.SaveChanges();

                    // update image profile and name
                    ((User)Session["activeUser"]).Fullname = updateUser.Fullname;
                    ((User)Session["activeUser"]).ProfileImage = updateUser.ProfileImage;

                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return Redirect("/error");
                }
            }
            return View(userModel);
        }

        public ActionResult ForgotPassword()
        {
            ForgotPasswordModel model = new ForgotPasswordModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel fpmodel)
        {
            if (ModelState.IsValid)
            {
                if (this.IsCaptchaValid(""))
                {
                    CMS_Entities context = new CMS_Entities();
                    User user = context.Users.Where(u => u.Username == fpmodel.Username && u.Email == fpmodel.Email).FirstOrDefault();
                    if (user != null)
                    {
                        string resetcode = Guid.NewGuid().ToString();
                        // send email
                        SendEmail(fpmodel.Email, resetcode, true);
                        user.ResetCode = resetcode;
                        context.SaveChanges();
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Tên đăng nhập hoặc email hoặc cả hai không đúng, vui lòng kiểm tra và thử lại";
                        return View(fpmodel);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Mã captcha đã nhập không đúng";
                    return View(fpmodel);
                }
            }

            return View(fpmodel);
        }

        // reset password
        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Login", "Auth");
            CMS_Entities context = new CMS_Entities();
            var user = context.Users.Where(u => u.ResetCode == id).FirstOrDefault();
            if (user != null && id != null & id.Length > 0)
            {
                ResetPasswordModel rpModel = new ResetPasswordModel()
                {
                    ResetCode = id
                };

                return View(rpModel);
            }
            else
            {
                return RedirectToAction("Index", "Error", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel rpModel)
        {
            bool success = false;
            if (ModelState.IsValid)
            {
                CMS_Entities context = new CMS_Entities();
                var user = context.Users.Where(u => u.ResetCode == rpModel.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Crypto.Hash(rpModel.Password);
                    user.ResetCode = string.Empty;
                    context.SaveChanges();
                    success = true;
                }
            }

            ViewBag.Success = success;
            return View(rpModel);
        }

        // send email;
        public void SendEmail(string email, string code, bool isforgotpwd = false)
        {
            string verifyURL = string.Empty;
            if (isforgotpwd)
                verifyURL = "/ad/reset-password/" + code;
            else
            {

            }
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);
            var fromEmail = new MailAddress("kiendang.cskh@gmail.com", "Reset Password");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "kiendang";
            string subject = string.Empty, body = string.Empty;
            if (isforgotpwd)
            {
                subject = "Hệ thống Quản lý Nội dung - CMS | Đặt lại mật khẩu";
                body = "Xin chào,<br/><br/>Chúng tôi nhận được yêu cầu đặt lại mật khẩu của bạn. Vui lòng click vào đường liên kết bên dưới để reset mật khẩu.<br/><a href='" + link + "'>Reset Password Link</a><br/><br/>Trân trọng,<br/>Quản trị viên hệ thống";
            }
            else
            {

            }

            var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

                smtp.Send(message);
        }
    }
}