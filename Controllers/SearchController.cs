using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moboguide.Models;
namespace Moboguide.Controllers
{
	public class SearchController : Controller
	{
		Pcontext  context = new Pcontext();
        ViewModel v = new ViewModel();
        [AllowAnonymous]
        public IActionResult srch(Mobile mob)
        {
            if (mob.MobileName == null)
            {
                ViewBag.searchNote = "Please Enter The Phone You Want To Search.";
                return View("../Home/Index");
            }
            List<Mobile> mobs = context.mobiles.ToList();
            List<Mobile> mobsreq = new List<Mobile>();
            foreach (var item in mobs)
            {
                if (item.MobileName.ToLower().Contains(mob.MobileName.ToLower()))
                {
                    mobsreq.Add(item);
                }
            }
            if (mobsreq.IsNullOrEmpty())
            {
                ViewBag.searchNote = "No Results Found!";
                return View("../Home/Index");
            }
            return View("allMobs", mobsreq);
        }
        [AllowAnonymous]
        public IActionResult allMobs()
        {
            List<Mobile> mobs = context.mobiles.ToList();
            return View(mobs);
        }
        [AllowAnonymous]
        public IActionResult details(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            var mob = context.mobiles.FirstOrDefault(x => x.MobileName == name);
            var cmnt = context.comments.Where(a => a.MobileName == name);
            List<KeyValuePair<string, Comment>> names = new();
            foreach (var c in cmnt)
            {  
                string? userName = context.users.Where(a => a.UserId == c.UserId).Select(a => a.UserName).FirstOrDefault();
                names.Add(new KeyValuePair<string, Comment>(userName, c));
            }
            ViewBag.cmmnts = names;
            v.mobile = mob;
            string cookieval = Request.Cookies["Username"];
            if (cookieval == null)
            {
                return View("srch", v);
            }
            int userid = context.users.FirstOrDefault(e => e.UserName == cookieval).UserId;
            var fav0 = context.mobilesUsers.FirstOrDefault(e => e.UserId == userid && e.MobileName == name);
            v.mobile_user = fav0;
            return View("srch", v);
        }
        [AllowAnonymous]
        public IActionResult compare(string name1, string? name2 = null)
        {
            if (name2 != null)
            {
                Mobile mob1 = context.mobiles.FirstOrDefault(x => x.MobileName == name1);
                Mobile mob2 = context.mobiles.FirstOrDefault(x => x.MobileName == name2);
                ViewBag.mob1 = mob1;
                ViewBag.mob2 = mob2;
                return View();
            }
            else
            {
                List<Mobile> mobs = context.mobiles.ToList();
                ViewBag.name1 = name1;
                return View("allMobs", mobs);
            }
        }
        public IActionResult favo(ViewModel fav)
        {
            string cookieval = Request.Cookies["UserName"];
            var mob = context.mobiles.FirstOrDefault(e => e.MobileName == fav.mobile.MobileName);
			v.mobile = mob;
			if (cookieval == null)
            {
                ViewBag.err = "Please sign in first";
                return View("srch", v);
            }
            var cmnt = context.comments.Where(a => a.MobileName == fav.mobile.MobileName);
            List<KeyValuePair<string, Comment>> names = new List<KeyValuePair<string, Comment>>();
            foreach (var c in cmnt)
            {
                string userName = context.users.Where(a => a.UserId == c.UserId).Select(a => a.UserName).FirstOrDefault();
                names.Add(new KeyValuePair<string, Comment>(userName, c));
            }
            ViewBag.cmmnts = names;
            int userid = context.users.FirstOrDefault(e => e.UserName == cookieval).UserId;
            MobileUser fav0 = context.mobilesUsers.FirstOrDefault(e => (e.UserId == userid && e.MobileName == fav.mobile.MobileName));
            if (fav0 == null)
            {
                fav0 = new MobileUser();
                fav0.UserId = userid;
                fav0.Rate = fav.mobile_user.Rate;
                fav0.MobileName = fav.mobile.MobileName;
                fav0.IsFavorite = fav.mobile_user.IsFavorite;
                context.mobilesUsers.Add(fav0);
                context.SaveChanges();
                v.mobile_user = fav0;
            }
            else
            {
                fav0.IsFavorite = fav.mobile_user.IsFavorite;
                fav0.Rate = fav.mobile_user.Rate;
                context.Update(fav0);
                context.SaveChanges();
            }
			var rates = context.mobilesUsers.Where(a => a.MobileName == fav.mobile.MobileName && a.Rate > 0).ToList();
			int count = 0; float totalrates = 0;
			foreach (var rate in rates)
			{
				count++;
				totalrates += rate.Rate;
			}
            if(count > 0)
			    mob.AvgRate = totalrates / count;
			context.SaveChanges();
            v.mobile = mob;
			v.mobile_user = fav0;
            return View("srch", v);
        }
        [Authorize(Policy = "AuthenticatedOnly")]
        public IActionResult favlist()
        {
            string? cookieval = Request.Cookies["UserName"];
            List<Mobile> favs = new List<Mobile>();

            if (cookieval == null)
            {
                ViewBag.errmsg = "Please sign in first ";
                return View("allmobs", favs);
            }
            int userid = context.users.Where(e => e.UserName == cookieval).Select(a=>a.UserId).SingleOrDefault();
            List<MobileUser> favlis = context.mobilesUsers.ToList();
            foreach (var fav in favlis)
            {
                if (fav.UserId == userid && fav.IsFavorite)
                {
                    var mob = context.mobiles.Where(e => e.MobileName == fav.MobileName).SingleOrDefault();
                    favs.Add(mob);
                }
            }
            return View("allmobs", favs);
        }
    }
}
