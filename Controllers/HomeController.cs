using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using belt1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace belt1.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(user => user.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!!");

                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpPost("checkLogin")]
        public IActionResult CheckLogin(LoginUser login)
        {
            if(ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(user => user.Email == login.LoginEmail);

                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid login");

                    return View("Index");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(login, userInDb.Password, login.LoginPassword);

                if(result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid login");

                    return View("Index");
                }

                Console.WriteLine("logged in");
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                HttpContext.Session.SetString("userName", userInDb.Name);
                return RedirectToAction("Dashboard");
            }

            return View("Index");
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");
            
            ViewBag.Stuffs = _context.Stuffs
            .Include(stuff => stuff.Creator)
            .Include(stuff => stuff.Rsvp)
            .OrderBy(stuff => stuff.Date)
            .ToList();

            ViewBag.UserId = loggedUserId;
            ViewBag.Name = HttpContext.Session.GetString("userName");

            return View();
        }
        [HttpGet("stuffsForm")]
        public IActionResult StuffsForm()
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");
            if(ModelState.IsValid)
            ViewBag.UserId = HttpContext.Session.GetInt32("userId");
            ViewBag.Name = HttpContext.Session.GetString("userName");

            return View();
        }
        [HttpPost("CreateStuff")]
        public IActionResult CreateStuff(Stuff newStuff)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");
            if(ModelState.IsValid)
            {
                Console.WriteLine("We have an Activity!");
                _context.Stuffs.Add(newStuff);
                _context.SaveChanges();

                Stuff stuff = _context.Stuffs
                .Where(stuff => stuff.CreatorId == loggedUserId)
                .OrderByDescending(stuff => stuff.CreatedAt)
                .FirstOrDefault();

                return RedirectToAction("SingleStuff", new {id = stuff.StuffId});
            }
            Console.WriteLine("Something went wrong!");
            ViewBag.UserId = HttpContext.Session.GetInt32("userId");
            ViewBag.Name = HttpContext.Session.GetString("userName");
            return View("StuffsForm");
        }
        [HttpGet("stuffs/delete/{id}")]
        public IActionResult DeleteStuff(int id)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");
            Stuff stuff = _context.Stuffs.FirstOrDefault(stuff => stuff.StuffId == id);
            _context.Stuffs
            .Remove(stuff);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("rsvp/add/{stuffId}")]
        public IActionResult AddRsvp(int stuffId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");

            Rsvp rsvp = new Rsvp();
            rsvp.UserId = (int)loggedUserId;
            rsvp.StuffId = (int)stuffId;

            _context.Rsvps
            .Add(rsvp);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
        [HttpGet("rsvp/add2/{stuffId}")]
        public IActionResult AddRsvp2(int stuffId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");

            Rsvp rsvp = new Rsvp();
            rsvp.UserId = (int)loggedUserId;
            rsvp.StuffId = (int)stuffId;

            _context.Rsvps
            .Add(rsvp);
            _context.SaveChanges();

            return RedirectToAction("SingleStuff", new {id = stuffId});
        }

        [HttpGet("rsvp/delete/{stuffId}")]
        public IActionResult DeleteRsvp(int stuffId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");

            Rsvp rsvp = _context.Rsvps.FirstOrDefault(rsvp => rsvp.UserId == loggedUserId && rsvp.StuffId == stuffId);

            _context.Rsvps
            .Remove(rsvp);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("rsvp/delete2/{stuffId}")]
        public IActionResult DeleteRsvp2(int stuffId)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");

            Rsvp rsvp = _context.Rsvps.FirstOrDefault(rsvp => rsvp.UserId == loggedUserId && rsvp.StuffId == stuffId);

            _context.Rsvps
            .Remove(rsvp);
            _context.SaveChanges();

            return RedirectToAction("SingleStuff", new {id = stuffId});
        }

        [HttpGet("stuffs/{id}")]
        public IActionResult SingleStuff(int id)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("userId");
            if(loggedUserId == null) return RedirectToAction("Index");

            ViewBag.Stuff = _context.Stuffs
            .Include(stuff => stuff.Creator)
            .FirstOrDefault(stuff => stuff.StuffId == id);

            ViewBag.Guests = _context.Users
            .Include (user => user.Rsvp)
            .Where(user => user.Rsvp.Any(rsvp => rsvp.StuffId == id))
            .ToList();

            ViewBag.Button = _context.Stuffs
            .Include(stuff => stuff.Rsvp)
            .Where(stuff => stuff.Rsvp.Any(rsvp => rsvp.UserId == loggedUserId && rsvp.StuffId == id))
            .FirstOrDefault();

            ViewBag.UserId = HttpContext.Session.GetInt32("userId");
            ViewBag.Name = HttpContext.Session.GetString("userName");

            return View();
        }



        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
