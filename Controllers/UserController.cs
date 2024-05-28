using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moboguide.Models;
using NuGet.Common;
using System.Security.Claims;
namespace Moboguide.Controllers
{
	public class UserController : Controller
	{
		Pcontext context = new Pcontext();
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            if (Request.Cookies["UserName"] == null)
            {
                User user = new User();
                return View(user);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp(User user, string confirm, string remember)
		{
            if (context.users.Where(a => a.UserName == user.UserName).FirstOrDefault() != null)
            {
                ViewBag.userNote = "This UserName already exist!";
                return View("SignUp", user);
            }

            Regex userReg = new Regex(@"^(?!\d)[_A-Za-z\d]{3,}$");
            if (!userReg.IsMatch(user.UserName))
            {
                ViewBag.userNote = "Invalid UserName!";
                return View("SignUp", user);
            }
            if (context.users.Where(a => a.Email == user.Email).FirstOrDefault() != null)
            {
                ViewBag.emailNote = "This Email already exist!";
                return View("SignUp", user);
            }
            Regex emailReg = new Regex(@"^\w+([-_.]\w+)*@\w+\.\w+$");
            if (!emailReg.IsMatch(user.Email))
            {
                ViewBag.emailNote = "Invalid Email!";
                return View("SignUp", user);
            }
            Regex passReg = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$");
            if (!passReg.IsMatch(user.Password))
            {
                ViewBag.passNote = "Must contain at least one number, " +
                    "one uppercase letter, one lowercase letter, one speacial " +
                    "character and at least 8 or more characters.";
                return View("SignUp", user);
            }
            if (confirm != user.Password)
            {
                ViewBag.confirmNote = "This input must match the above password.";
                return View("SignUp", user);
            }
            ClaimsIdentity? identity = null;
            identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, "User")
            ],CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            context.users.Add(user);
            context.SaveChanges();
            if (remember == "1")
            {
                CookieOptions cookieOption = new CookieOptions();
                cookieOption.Expires = DateTimeOffset.Now.AddMonths(1);
                Response.Cookies.Append("UserName", user.UserName, cookieOption);
            }
            else
            {
                Response.Cookies.Append("UserName", user.UserName);
            }
            user.UserName = "";
            user.Email = "";
            user.Password = "";
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult LogIn(User user, string remember)
        {
            if (context.users.Where(a => a.Email == user.Email).SingleOrDefault() != null)
            {
                var account = context.users.SingleOrDefault(a => a.Email == user.Email);
                if (BCrypt.Net.BCrypt.Verify(user.Password, account.Password))
                {
                    ClaimsIdentity? identity = null;
                    bool IsAuthenticated = false;
                    if (!account.IsAdmin)
                    {
                        identity = new ClaimsIdentity([
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.Role, "User")
                        ], CookieAuthenticationDefaults.AuthenticationScheme);
                        IsAuthenticated = true;
                    }
                    else
                    {
                        identity = new ClaimsIdentity([
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.Role, "Admin")
                        ], CookieAuthenticationDefaults.AuthenticationScheme);
                        IsAuthenticated = true;
                    }
                    if (IsAuthenticated)
                    {
                        var principal = new ClaimsPrincipal(identity);
                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    }
                    if (remember == "1")
                    {
                        CookieOptions cookieOption = new CookieOptions();
                        cookieOption.Expires = DateTimeOffset.Now.AddMonths(1);
                        Response.Cookies.Append("UserName", account.UserName, cookieOption);
                    }
                    else
                    {
                        Response.Cookies.Append("UserName", account.UserName);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.passNote = "Incorrect Password!";
                    return View("LogIn", user);
                }
            }
            else
            {
                ViewBag.emailNote = "Incorrect Email!";
                return View("LogIn", user);
            }
            
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("UserName");
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult Edit(int id)
        {
            var user = context.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult Edit(User user, string OldPassword)
        {
            if (user == null)
            {
                return NotFound();
            }
            if (context.users.Where(a => a.UserId != user.UserId && a.UserName == user.UserName).FirstOrDefault() != null)
            {
                ViewBag.userNote = "This UserName already exist!";
                return View("Edit", user);
            }

            Regex userReg = new Regex(@"^(?!\d)[_A-Za-z\d]{3,}$");
            if (!userReg.IsMatch(user.UserName))
            {
                ViewBag.userNote = "Invalid UserName!";
                return View("Edit", user);
            }
            if (context.users.Where(a => a.UserId != user.UserId && a.Email == user.Email).FirstOrDefault() != null)
            {
                ViewBag.emailNote = "This Email already exist!";
                return View("Edit", user);
            }
            Regex emailReg = new Regex(@"^\w+([-_.]\w+)*@\w+\.\w+$");
            if (!emailReg.IsMatch(user.Email))
            {
                ViewBag.emailNote = "Invalid Email!";
                return View("Edit", user);
            }
            int id = user.UserId;
            var hashedPass = context.users.Where(a => a.UserId == id).Select(a => a.Password).FirstOrDefault();
            if (!BCrypt.Net.BCrypt.Verify(OldPassword ,hashedPass))
            {
                ViewBag.oldpassNote = "The Old Password Isn't Correct.";
                return View("Edit", user);
            }
            Regex passReg = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$");
            if (!passReg.IsMatch(user.Password))
            {
                ViewBag.newpassNote = "Must contain at least one number, " +
                    "one uppercase letter, one lowercase letter, one speacial " +
                    "character and at least 8 or more characters.";
                return View("Edit", user);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            context.users.Update(user);
            context.SaveChanges();
            return RedirectToAction("Profile",new { id = user.UserName });
        }
        [AllowAnonymous]
        public IActionResult Profile(string id)
        {
            var user = context.users.FirstOrDefault(user => user.UserName == id);
            if(user == null)
            {
                return NotFound();
            }
            var favs = context.mobilesUsers.Where(a=>a.UserId == user.UserId);
            List<Mobile> mobs = new();
            foreach (var fav in favs)
            {
                var mob = context.mobiles.Where(a => a.MobileName == fav.MobileName).FirstOrDefault();
                if(mob != null)
                    mobs.Add(mob);
            }
            ViewBag.favs = mobs;
            return View(user);
        }
        
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AllUsers()
        {
            List<User> users = context.users.ToList();
            return View(users);
        }
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult Delete(int id)
        {
            var user = context.users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = context.users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                context.users.Remove(user);
            }
            context.SaveChanges();
            return RedirectToAction("AllUsers", "User");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminList()
        {
            List<User> admins = context.users.Where(e => e.IsAdmin == true).ToList();
            return View(admins);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddAdmin(int id)
        {
            User user = context.users.FirstOrDefault(e => e.UserId == id);
            if (user.IsAdmin == true)
            {
                List<User> users = context.users.ToList();
                ViewBag.oldadmin = "User is already an admin";
                return View("AllUsers", users);
            }
            user.IsAdmin = true;
            context.SaveChanges();
            return RedirectToAction("AdminList");
        }
		[Authorize(Roles = "Admin")]
		public IActionResult RemoveAdmin(int id)
        {
            User admin = context.users.FirstOrDefault(e => e.UserId == id);
            admin.IsAdmin = false;
            context.SaveChanges();
            if (Request.Cookies["UserName"] == admin.UserName)
            {
                LogOut();
                return RedirectToAction("Index", "Home");
            }
            else 
            {
                List<User> admins = context.users.Where(e => e.IsAdmin == true).ToList();
                return View("AdminList", admins);
            }
        }
    }
}
