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
    public class RoomController : Controller
    {           
        public ActionResult Index()
        {
            try
            {
                List<SelectListItem> locList = CreateLocationList(-1);

                if(locList.Count > 0)
                {
                    locList[0].Selected = true;
                }

                ViewData["LocationList"] = locList;

                return View();
            }
            catch (Exception)
            {
                return Redirect("/error");
            }            
        }

        private List<SelectListItem> CreateLocationList(int? selectedId)
        {
            List<SelectListItem> locList = new List<SelectListItem>();

            using (CMS_Entities _context = new CMS_Entities())
            {
                var locations = _context.Locations.Where(l => l.IsDeleted == false).ToList();                
                foreach (Location l in locations)
                {
                    locList.Add(new SelectListItem()
                    {
                        Value = l.Id.ToString(),
                        Text = l.Name,
                        Selected = (l.Id == selectedId)
                    });
                }
            }

            return locList;
        }

        [HttpGet]
        public ActionResult GetRoomList()
        {
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    var rooms = _context.Rooms.Where(r => r.IsDeleted == false).ToList();
                    List<RoomDTO> roomModels = new List<RoomDTO>();
                    foreach (Room r in rooms)
                    {                        
                        string desc = r.Description;
                        if(r.Description.Length > 100)
                        {
                            desc = r.Description.Substring(0, 100) + "...";
                        }
                        roomModels.Add(new RoomDTO()
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Status = r.Status,
                            Description = r.Description,
                            LocationName = r.Location.Name                            
                        });
                    }

                    return Json(new { data = roomModels }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Redirect("/error");
            }
        }

        [HttpPost]        
        public ActionResult AddRoom(RoomModel roomModel)
        {
            bool success = false;
            string message = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                try
                {
                    Room room = new Room()
                    {
                        Name = roomModel.Name,
                        Description = roomModel.Description,
                        Status = roomModel.Status,
                        LocationId = roomModel.LocationId,
                        IsDeleted = false
                    };
                    _context.Rooms.Add(room);
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
        public ActionResult EditRoom(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    Room room = _context.Rooms.Where(r => r.Id == id && r.IsDeleted == false).FirstOrDefault();
                    if (room != null)
                    {                        
                        RoomModel roomModel = new RoomModel()
                        {
                            Id = room.Id,
                            Name = room.Name,
                            Description = room.Description,
                            Status = (room.Status == true),
                            LocationId = (int)room.LocationId,
                            LocationList = CreateLocationList(room.LocationId)
                        };

                        return View(roomModel);
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
        public ActionResult EditRoom(RoomModel roomModel)
        {
            if (ModelState.IsValid)
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    try
                    {
                        var room = _context.Rooms.Where(r => r.Id == roomModel.Id && r.IsDeleted == false).FirstOrDefault();
                        room.Name = roomModel.Name;
                        room.Description = roomModel.Description;
                        room.Status = roomModel.Status;
                        room.LocationId = roomModel.LocationId;
                        _context.Entry(room).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return Redirect("/error");
                    }
                }
            }

            roomModel.LocationList = CreateLocationList(roomModel.LocationId);
            return View(roomModel);
        }
        
        [HttpPost]
        public ActionResult DeleteRoom(int id)
        {
            int ret = -1; // -1: failed; 0: not found; 1: success
            string message = "";
            
            try
            {
                CMS_Entities _context = new CMS_Entities();
                Room room = _context.Rooms.Where(r => r.Id == id && r.IsDeleted == false).FirstOrDefault();
                if (room != null)
                {
                    room.IsDeleted = true;
                    _context.Entry(room).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    ret = 1;
                    message = "Phòng \'" + room.Name + "\' đã được xóa";
                }
                else
                {
                    ret = 0;
                    message = "Không tìm thấy phòng cần xóa";
                }
            }
            catch
            {
                ret = -1;
                message = "Gặp lỗi khi thực hiện yêu cầu xóa";
            }
            return Json(new { result = ret, responseText = message }, JsonRequestBehavior.AllowGet);
        }
    }
}