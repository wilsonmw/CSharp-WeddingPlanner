using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;
using WeddingPlanner.Controllers;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class DashboardController : Controller
    {
        private WeddingPlannerContext _context;
        public DashboardController(WeddingPlannerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard(){
            List<Wedding> allWeddings = _context.Weddings.Include(g => g.Guests).Include(u => u.User).ToList();
            ViewBag.allWeddings = allWeddings;
            ViewBag.userID = HttpContext.Session.GetInt32("userID");
            List<Guest> allGuests = _context.Guests.ToList();
            ViewBag.allGuests = allGuests;
            return View("Dashboard");
        }

        [HttpGet]
        [Route("newWedding")]
        public IActionResult NewWedding(){
            return View("newWedding");
        }

        [HttpPost]
        [Route("createNew")]
        public IActionResult CreateNew(WeddingViewModel model){
            if(ModelState.IsValid){
                Wedding newWedding = new Wedding{
                    Name1 = model.Name1,
                    Name2 = model.Name2,
                    Date = model.Date,
                    Address = model.Address,
                    GuestCount = model.GuestCount,
                    UserID = (int)HttpContext.Session.GetInt32("userID")
                };
                _context.Weddings.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                return View("newWedding");
            }
        }

        [HttpGet]
        [Route("delete/{wedId}")]
        public IActionResult Delete(int wedId){
            ViewBag.weddingID = wedId;
            ViewBag.wedding = _context.Weddings.Single(i => i.WeddingID == wedId);
            return View("deletePage");
        }

        [HttpGet]
        [Route("yesDelete/{wedId}")]
        public IActionResult YesDelete(int wedId){
            User currentUser = _context.Users.Single(u => u.UserID == HttpContext.Session.GetInt32("userID"));
            Wedding deleteWedding = _context.Weddings.Single(i => i.WeddingID == wedId);
            if (currentUser.UserID == deleteWedding.UserID){
                _context.Weddings.Remove(deleteWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                ViewBag.deleteError = "You do not have permission to delete this wedding.";
                return View("deletePage");
            }
        }
        [HttpGet]
        [Route("rsvp/{wedId}")]
        public IActionResult RSVP(int wedId){
            Wedding currentWedding = _context.Weddings.Single(i => i.WeddingID == wedId);
            User currentUser = _context.Users.Single(u => u.UserID == HttpContext.Session.GetInt32("userID"));
            currentWedding.GuestCount++;
            Guest newGuest = new Guest{
                WeddingID = currentWedding.WeddingID,
                UserID = currentUser.UserID
            };
            _context.Guests.Add(newGuest);
            _context.SaveChanges();
            return RedirectToAction ("Dashboard");
        }

        [HttpGet]
        [Route("un-rsvp/{wedId}")]
        public IActionResult UnRSVP(int wedId){
            Wedding currentWedding = _context.Weddings.Single(i => i.WeddingID == wedId);
            User currentUser = _context.Users.Single(u => u.UserID == HttpContext.Session.GetInt32("userID"));
            currentWedding.GuestCount--;
            Guest removeGuest = _context.Guests.Single(i => i.UserID == currentUser.UserID && i.WeddingID == currentWedding.WeddingID);
            currentWedding.Guests.Remove(removeGuest);
            _context.SaveChanges();
            return RedirectToAction ("Dashboard");
        }

        [HttpGet]
        [Route("show/{wedId}")]
        public IActionResult Show(int wedId){
            Wedding currentWedding = _context.Weddings.Include(i => i.Guests).ThenInclude(u => u.User).SingleOrDefault(w => w.WeddingID == wedId);
            System.Console.WriteLine(currentWedding.Guests);
            ViewBag.currentWedding = currentWedding;
            ViewBag.guests = currentWedding.Guests;
            return View("show");
        }
    }
}