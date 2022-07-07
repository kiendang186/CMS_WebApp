using CMS_WebApp.Areas.ad.Common;
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
    public class EmployeeController : Controller
    {
        // GET: ad/Employee
        public ActionResult Index()
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                return View(_context.Employees.OrderBy(e => e.Order).ToList());
            } 
        }

        [HttpGet]
        public ActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoEmp(EmployeeModel empModel)
        {
            bool result = false; // true: success; false: failed
            int action = 0; // 0: add; 1: edit; -1: exception
            string msg = "";
            try
            {
                CMS_Entities _context = new CMS_Entities();
                // edit
                if(empModel.Id > 0)
                {
                    action = 1;
                    Employee emp = _context.Employees.Where(e => e.Id == empModel.Id).FirstOrDefault();
                    if(emp != null)
                    {
                        emp.FullName = empModel.FullName;
                        emp.Position = empModel.Position;
                        emp.Degree = empModel.Degree;
                        emp.DOB = empModel.DOB;
                        emp.POB = empModel.POB;
                        emp.Email = empModel.Email;
                        emp.Biography = empModel.Biography;
                        emp.MetaTitle = Utils.ConvertToUnSign(empModel.FullName);
                        // Profile Image Path
                        HttpPostedFileBase file = empModel.FileImage;
                        if (file != null && file.ContentLength > 0)
                        {
                            string oldPath = emp.ImagePath;

                            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string fileExtension = Path.GetExtension(file.FileName);
                            fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                            emp.ImagePath = "/Areas/ad/Upload/img-emp/" + fileName;
                            file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/img-emp/"), fileName));

                            // delete old file
                            string path = Server.MapPath("~" + oldPath);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }                        

                        _context.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();

                        result = true;
                        msg = "Cập nhật thành công";
                    }
                    else
                    {
                        result = false;
                        msg = "Không tìm thấy nhân viên cần cập nhật";
                    }
                }
                else
                {
                    action = 0;
                    var maxOrder = _context.Employees.Max(e => e.Order);
                    Employee emp = new Employee();
                    emp.FullName = empModel.FullName;
                    emp.Position = empModel.Position;
                    emp.Degree = empModel.Degree;
                    emp.DOB = empModel.DOB;
                    emp.POB = empModel.POB;
                    emp.Email = empModel.Email;
                    emp.Biography = empModel.Biography;
                    emp.Order = (maxOrder == null) ? 1 : (maxOrder + 1);
                    emp.MetaTitle = Utils.ConvertToUnSign(empModel.FullName);
                    // Profile Image Path
                    HttpPostedFileBase file = empModel.FileImage;
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                        emp.ImagePath = "/Areas/ad/Upload/img-emp/" + fileName;
                        file.SaveAs(Path.Combine(Server.MapPath("~/Areas/ad/Upload/img-emp/"), fileName));
                    }
                    else
                    {
                        emp.ImagePath = "/Areas/ad/Upload/img-emp/default-user.png";
                    }

                    _context.Employees.Add(emp);
                    _context.SaveChanges();

                    result = true;
                    msg = "Thêm nhân viên thành công";
                }  
            }
            catch(Exception e)
            {
                result = false;
                action = -1;
                msg = "Có lỗi khi thực hiện thêm nhân viên";
            }
            return Json(new {act = action, status = result, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditEmp(int id)
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                Employee emp = _context.Employees.Where(e => e.Id == id).FirstOrDefault();
                return View(emp);
            }
        }

        [HttpPost]
        public ActionResult DeleteEmp(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Employee emp = _context.Employees.Where(e => e.Id == id).FirstOrDefault();
                    if (emp != null)
                    {
                        string imagePath = Server.MapPath("~" + emp.ImagePath);                        

                        _context.Employees.Remove(emp);
                        _context.SaveChanges();

                        // delete old image                
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }                       

                        ret = 1;
                        message = string.Format("Nhân viên \"{0}\" đã được xóa", emp.FullName);                        
                    }
                    else
                    {
                        ret = 0;
                        message = "Không tìm thấy nhân viên cần xóa";
                    }
                }
            }
            catch
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện hành động xóa";
            }
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateOrder(List<OrderItem> orders)
        {
            bool ret = false;
            string msg = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                foreach (OrderItem item in orders)
                {
                    var emp = _context.Employees.Find(item.Id);
                    emp.Order = item.Order;
                }
                _context.SaveChanges();

                ret = true;
                msg = "Thứ tự được đã được cập nhật";
            }

            return Json(new { status = ret, message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}