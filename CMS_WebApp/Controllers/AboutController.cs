using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult Team()
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                var employees = _context.Employees.OrderBy(e => e.Order).ToList();
                List<EmployeeDTO> employeeDTOs = new List<EmployeeDTO>();
                foreach (Employee emp in employees)
                {
                    employeeDTOs.Add(new EmployeeDTO() {
                        Id = emp.Id,
                        FullName = emp.FullName,
                        Degree = emp.Degree,
                        Position = emp.Position,
                        ImagePath = emp.ImagePath,
                        MetaTitle = emp.MetaTitle
                    });
                }
                return View(employeeDTOs);
            }
        }

        public ActionResult EmpDetail(int id)
        {
            using (CMS_Entities _context = new CMS_Entities())
            {
                var employee = _context.Employees.Where(e => e.Id == id).FirstOrDefault();
                EmployeeDTO empDTO = new EmployeeDTO();
                if(employee != null)
                {
                    string dob = "";
                    if(employee.DOB != null)
                    {
                        dob = DateTime.Parse(employee.DOB.ToString()).ToString("dd/MM/yyyy");
                    }
                    empDTO.Id = employee.Id;
                    empDTO.FullName = employee.FullName;
                    empDTO.Position = employee.Position;
                    empDTO.Degree = employee.Degree;
                    empDTO.POB = employee.POB;
                    empDTO.DOB = dob;
                    empDTO.ImagePath = employee.ImagePath;
                    empDTO.Email = employee.Email;
                    empDTO.Biography = employee.Biography;
                }
                
                return View(empDTO);
            }
        }
    }
}