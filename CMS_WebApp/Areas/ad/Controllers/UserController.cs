using CMS_WebApp.Areas.ad.Common;
using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class UserController : Controller
    {
        // GET: ad/User
        public ActionResult Index()
        {            
            if(Auth.isAdminRole())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }        

        [HttpGet]
        public ActionResult GetUserList()
        {
            CMS_Entities context = new CMS_Entities();
            IEnumerable<User> users = context.Users.Where(u => u.IsDeleted == 0).ToList();
            var userList = new List<UserDTO>();
            foreach (User u in users)
            {
                string lastLogin = "";
                if (u.LastLogin != null)
                {
                    lastLogin = DateTime.Parse(u.LastLogin.ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
                userList.Add(new UserDTO()
                {
                    Username = u.Username,
                    Fullname = u.Fullname,
                    Email = u.Email,
                    Role = u.Role.Code + "-" + u.Role.Name,
                    IsActived = u.IsActived,
                    LastLogin = lastLogin
                });
            }

            return Json(new { data = userList }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult AddUser()
        {
            // checking if the user has admin permission
            if (!Auth.isAdminRole())
            {
                return RedirectToAction("Index", "Home");
            }

            UserModel user = new UserModel
            {
                IsActived = true,                
                RoleList = createRoleList()
            };
            return View(user);
        }

        [HttpPost]        
        public ActionResult AddUser([Bind(Exclude = "IsDeleted,LastLogin,LastLogout")] UserModel user)
        {
            // checking validation
            if (ModelState.IsValid)
            {
                try { 
                    User u = new User();

                    // User is already exist
                    if (IsExistUserId(user.UserId))
                    {
                        ModelState.AddModelError("UserIdExist", "Tên đăng nhập này đã được sử dụng");
                        user.RoleList = createRoleList();
                        return View(user);
                    }

                    // Email is already exist
                    if (IsEmailExisted(user.Email, "", false))
                    {
                        ModelState.AddModelError("EmailExist", "Email này đã được sử dụng bởi một người dùng khác");
                        user.RoleList = createRoleList();
                        return View(user);
                    }

                    u.Username = user.UserId;
                    // Password hashing
                    u.Password = Crypto.Hash(user.Password);
                    u.Fullname = user.FullName;
                    u.Email = user.Email;
                    u.Phone = user.Phone;
                    u.IsDeleted = 0;
                    u.IsActived = user.IsActived;
                    u.LastLogin = null;
                    u.LastLogout = null;
                    u.ProfileImage = user.ProfileImage;
                    u.RoleId = user.RoleId;
                    u.ProfileImage = "/Areas/ad/Profiles/default.png";
                    u.WrongTimes = 0;

                    using (CMS_Entities context = new CMS_Entities())
                    {
                        context.Users.Add(u);
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                catch
                {
                    return Redirect("/error");
                }
            }

            user.RoleList = createRoleList();
            return View(user);

        }// End of POST: AddUser

        public bool IsExistUserId(string userid)
        {
            CMS_Entities context = new CMS_Entities();
            var result = context.Users.Where(u => u.Username == userid).FirstOrDefault();
            return result != null;

        }

        public bool IsEmailExisted(string email, string uid, bool isUpdate)
        {
            CMS_Entities context = new CMS_Entities();
            User user = null;
            if(!isUpdate)
                user = context.Users.Where(u => u.Email == email).FirstOrDefault();
            else
                user = context.Users.Where(u => u.Username != uid && u.Email == email).FirstOrDefault();

            return user != null;

        }

        private List<SelectListItem> createRoleList()
        {
            List<SelectListItem> selectLists = new List<SelectListItem>();
            using (CMS_Entities context = new CMS_Entities())
            {
                var roles = context.Roles.ToList();

                if (roles != null && roles.Count() > 0)
                {
                    foreach (Role role in roles)
                    {
                        selectLists.Add(new SelectListItem()
                        {
                            Value = role.Id.ToString(),
                            Text = role.Name
                        });
                    }
                }
            }
            return selectLists;
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            // checking if the user has admin permission
            if (!Auth.isAdminRole())
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                CMS_Entities context = new CMS_Entities();
                User user = context.Users.Where(u => u.Username == id).FirstOrDefault();

                if (user != null)
                {
                    UserModel userMetaData = new UserModel();
                    userMetaData.UserId = user.Username;
                    userMetaData.FullName = user.Fullname;
                    userMetaData.Email = user.Email;
                    userMetaData.Phone = user.Phone;
                    userMetaData.IsActived = user.IsActived ?? default(bool);

                    var roles = context.Roles.ToList();
                    List<SelectListItem> selectLists = new List<SelectListItem>();
                    if (roles != null && roles.Count() > 0)
                    {
                        foreach (Role role in roles)
                        {
                            selectLists.Add(new SelectListItem()
                            {
                                Value = role.Id.ToString(),
                                Text = role.Name
                            });
                        }
                        userMetaData.RoleId = user.RoleId.GetValueOrDefault();
                        userMetaData.RoleList = selectLists;
                    }

                    return View(userMetaData);
                }
                else
                    return RedirectToAction("Index");
            }
            catch
            {
                return Redirect("/error");
            }
        }

        [HttpPost]
        public ActionResult EditUser([Bind(Exclude = "Password,ConfirmPassword,LastLogin,LastLogout")] UserModel user)
        {
            ModelState["Password"].Errors.Clear();
            ModelState["ConfirmPassword"].Errors.Clear();
            // checking validation
            if (ModelState.IsValid)
            {
                try { 
                    // Email is already exist
                    if (IsEmailExisted(user.Email, user.UserId, true))
                    {
                        ModelState.AddModelError("EmailExist", "Email này đã được sử dụng bởi một người dùng khác");
                        user.RoleList = createRoleList();
                        return View(user);
                    }

                    CMS_Entities context = new CMS_Entities();
                    User updateUser = context.Users.Where(u => u.Username == user.UserId).FirstOrDefault();

                    // Update Fields
                    updateUser.Fullname = user.FullName;
                    updateUser.Email = user.Email;
                    updateUser.Phone = user.Phone;
                    updateUser.IsActived = user.IsActived;
                    if (updateUser.IsActived == true)
                        updateUser.WrongTimes = 0;
                    updateUser.RoleId = user.RoleId;

                    context.Entry(updateUser).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return Redirect("/error");
                }
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Delete(string userid)
        {
            CMS_Entities context = new CMS_Entities();
            User user = context.Users.Where(u => u.Username == userid).FirstOrDefault();
            user.IsDeleted = 1;
            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return Json(new { success = true, message = "Nguời dùng đã được xóa" }, JsonRequestBehavior.AllowGet);
        }
    }
}