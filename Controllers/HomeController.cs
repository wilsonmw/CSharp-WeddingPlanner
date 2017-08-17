using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;
using WeddingPlanner.Controllers;
using System.Linq;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private WeddingPlannerContext _context;
        public HomeController(WeddingPlannerContext context)
        {
            _context = context;
        }
        
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.existsError = HttpContext.Session.GetString("existsError");
            ViewBag.loginError = HttpContext.Session.GetString("loginError");
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model){
            if (ModelState.IsValid){
                bool exists = _context.Users.Any(u => u.Email == model.Email);
                if (exists == true){
                    HttpContext.Session.SetString("existsError", "That email address is already in use, please try again.");
                    return RedirectToAction ("Index");
                }
                else{
                    User newUser = new User{
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password
                    };
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    User currentUser = _context.Users.Single(u => u.Email == newUser.Email);
                    HttpContext.Session.SetInt32("userID", currentUser.UserID);
                    return RedirectToAction ("Dashboard", "Dashboard");
                }
            }
            else{
                return View("Index");
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password){
            if (Email.Length < 1 || Password.Length < 1){
                HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                return RedirectToAction("Index");
            }
            else{
                bool exists = _context.Users.Any(u => u.Email == Email);
                if(exists == true){
                    User currentUser = _context.Users.Single(u => u.Email == Email);
                    if (currentUser.Password == Password){
                        HttpContext.Session.SetInt32("userID", currentUser.UserID);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else{
                        HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                        return RedirectToAction("Index");
                    }
                }
                else{
                    HttpContext.Session.SetString("loginError", "Login failed, please try again.");
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
