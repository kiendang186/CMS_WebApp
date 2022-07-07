using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    [Filters.AuthorizeAccount]
    public class LocationController : Controller
    {           
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLocationList()
        {
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    var locations = _context.Locations.Where(l => l.IsDeleted == false).ToList();
                    List<LocationModel> retLocations = new List<LocationModel>();
                    foreach (Location l in locations)
                    {                       
                        retLocations.Add(new LocationModel()
                        {
                            Id = l.Id,
                            Name = l.Name,
                            Description = l.Description
                        });
                    }

                    return Json(new { data = retLocations }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Redirect("/error");
            }            
        }

        [HttpPost]        
        public ActionResult AddLocation(LocationModel loc)
        {
            bool success = false;
            string message = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                try
                {
                    Location location = new Location()
                    {
                        Name = loc.Name,
                        Description = loc.Description,
                        IsDeleted = false
                    };
                    _context.Locations.Add(location);
                    _context.SaveChanges();
                    success = true;
                    message = "Thêm thành công";
                }
                catch
                {
                    success = false;
                    message = "Thêm không thành công";
                }
            }
            return Json(new { success, message}, JsonRequestBehavior.AllowGet);
        } 

        [HttpGet]
        public ActionResult EditLocation(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Location loc = _context.Locations.Where(l => l.Id == id && l.IsDeleted == false).FirstOrDefault();
                    if (loc != null)
                    {
                        LocationModel locationModel = new LocationModel()
                        {
                            Id = loc.Id,
                            Name = loc.Name,
                            Description = loc.Description
                        };

                        return View(locationModel);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch(Exception)
            {
                return Redirect("/error");
            }
        }


        [HttpPost]
        public ActionResult EditLocation(LocationModel location)
        {
            if (ModelState.IsValid)
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    try
                    {
                        var loc = _context.Locations.Where(l => l.Id == location.Id && l.IsDeleted == false).FirstOrDefault();   
                        loc.Name = location.Name;
                        loc.Description = location.Description;
                        _context.Entry(loc).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return Redirect("/error");
                    }
                }
            }

            return View(location);
        }
        
        [HttpPost]
        public ActionResult DeleteLocation(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            
            try
            {
                CMS_Entities _context = new CMS_Entities();
                Location loc = _context.Locations.Where(l => l.Id == id && l.IsDeleted == false).FirstOrDefault();                
                if (loc != null)
                {
                    loc.IsDeleted = true;
                    _context.Entry(loc).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();                    

                    // delete related rooms
                    DeleteRelatedRooms(loc.Rooms, _context);                                        

                    ret = 1;
                    message = "Địa điểm \'" + loc.Name + "\' đã được xóa";
                }
                else
                {
                    ret = 0;
                    message = "Không tìm thấy địa điểm cần xóa";
                }
            }
            catch(Exception)
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện yêu cầu xóa";
            }
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        } 
        
        private void DeleteRelatedRooms(IEnumerable<Room> rooms, CMS_Entities context)
        {
            if (rooms.Count() > 0)
            {
                rooms.ToList().ForEach(r => r.IsDeleted = true);
                context.SaveChanges();
            }
        }
    }
}