using CMS_WebApp.Areas.ad.DTO;
using CMS_WebApp.Areas.ad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CMS_WebApp.Areas.ad.Controllers
{
    
    public class CalendarController : Controller
    {
        [Filters.AuthorizeAccount]
        // GET: ad/Calendar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEvents()
        {
            try
            {
                using (CMS_Entities _context = new CMS_Entities())
                {
                    var events = _context.Events.ToList();
                    List<EventDTO> eventDTOs = new List<EventDTO>();
                    foreach(Event e in events)
                    {
                        string desc = "";
                        if (e.Description != null)
                            desc = e.Description;

                        eventDTOs.Add(new EventDTO()
                        {
                            Id = e.Id,
                            Title = e.Title,
                            Description = desc,
                            Start = e.Start,
                            End = e.End,
                            ThemeColor = e.ThemeColor,
                            IsFullDay = e.IsFullDay == true,
                            RoomId = e.RoomId,
                            Room = e.Room.Name,
                            LocationId = e.Room.Location.Id,
                            Location = e.Room.Location.Name
                        }) ;
                    }
                    return Json(new { Data = eventDTOs }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Redirect("/error");
            }                     
        }

        [HttpPost]
        [Filters.AuthorizeAccount]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (CMS_Entities _context = new CMS_Entities())
            {
                var v = _context.Events.Where(a => a.Id == eventID).FirstOrDefault();
                if (v != null)
                {
                    _context.Events.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        
        [HttpGet]
        [Filters.AuthorizeAccount]
        public JsonResult GetLocationRoom()
        {
            var status = false;
            List<LocationModel> locationModels = new List<LocationModel>();
            List<RoomModel> roomDTOs = new List<RoomModel>();

            using (CMS_Entities _context = new CMS_Entities())
            {
                var locations = _context.Locations.Where(l => l.IsDeleted == false).ToList();
                
                if (locations != null)
                {    
                    foreach(Location l in locations)
                    {
                        if (l.Rooms.Count > 0)
                        {
                            locationModels.Add(new LocationModel()
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Description = l.Description
                            });
                        }
                    }

                    var rooms = _context.Rooms.Where(r => r.IsDeleted == false).ToList();
                    
                    if(rooms != null && rooms.Count > 0)
                    {
                        foreach(Room r in rooms)
                        {
                            roomDTOs.Add(new RoomModel() {
                                Id = r.Id,
                                Name = r.Name,
                                Description = r.Description,
                                Status = r.Status == true,
                                LocationId = (int)r.LocationId,
                                LocationList = null
                            });
                        }                        
                    }                    

                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status, locations = locationModels, rooms = roomDTOs }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Filters.AuthorizeAccount]
        public JsonResult SaveEvent(EventModel evt)
        {
            var status = false;
            string msg = "";
            using (CMS_Entities _context = new CMS_Entities())
            {
                try
                {
                    User activeUser = (User)Session["activeUser"];
                    string username = "";
                    if (activeUser != null)
                    {
                        username = activeUser.Username;
                    }
                    
                    if (evt.Id > 0)
                    {
                        //Update the event
                        var v = _context.Events.Where(e => e.Id == evt.Id).FirstOrDefault();
                        if (v != null)
                        {
                            v.Title = evt.Title;
                            v.Start = evt.Start;
                            v.End = evt.End;
                            v.Description = evt.Description;
                            v.IsFullDay = evt.IsFullDay;
                            v.ThemeColor = evt.ThemeColor;
                            v.RoomId = evt.RoomId;
                            v.Username = username;
                        }

                        msg = "Cập nhật thành công";
                    }
                    else
                    {  
                        Event v = new Event()
                        {
                            Title = evt.Title,
                            Start = evt.Start,
                            End = evt.End,
                            Description = evt.Description,
                            IsFullDay = evt.IsFullDay,
                            ThemeColor = evt.ThemeColor,
                            RoomId = evt.RoomId,
                            Username = username
                        };

                        _context.Events.Add(v);
                        msg = "Thêm thành công";
                    }

                    _context.SaveChanges();
                    status = true;
                }
                catch
                {
                    status = false;
                    msg = "Có lỗi khi thực hiện";
                }

            }
            return new JsonResult { Data = new { status = status, message = msg } };
        }
    }
}