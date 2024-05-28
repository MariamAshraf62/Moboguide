using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moboguide.Models;

namespace Moboguide.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MobilesController : Controller
    {
        Pcontext context = new Pcontext();
        private readonly IWebHostEnvironment env;
        public MobilesController(IWebHostEnvironment environment)
        {
            env = environment;
        }
        //AddMobile
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Mobile mobile, IFormFile img_file)
        {
            string path = Path.Combine(env.WebRootPath, "images"); // wwwroot/imgages/
            if (img_file != null)
            {
                if (img_file.FileName.EndsWith(".jpg") || img_file.FileName.EndsWith(".png"))
                {
                    path = Path.Combine(path, img_file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        img_file.CopyTo(stream);
                        mobile.ImagePath = img_file.FileName;
                    }
                }
                else
                {
                    ViewBag.imgNote = "This file not an image!";
                    return View("Create", mobile);
                }
            }
            else
            {
                mobile.ImagePath = "ph.jpg";
            }

            if (context.mobiles.Where(a => a.MobileName == mobile.MobileName).FirstOrDefault() != null)
            {
                ViewBag.Note = "This mobile already exist!";
                return View("Create", mobile);
            }
            if (mobile.MobileName == null)
            {
                return View("Create", mobile);
            }
            context.Add(mobile);
            context.SaveChanges();
            return RedirectToAction("allMobs", "Search");

        }
        //EditMobile
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = context.mobiles.Find(id);
            if (mobile == null)
            {
                return NotFound();
            }
            return View(mobile);
        }
        [HttpPost]
        public IActionResult Edit(string id, Mobile mobile, IFormFile img_file)
        {
            if (id != mobile.MobileName)
            {
                return NotFound();
            }
            string imgpath = context.mobiles.Where(o => o.MobileName == id).Select(o => o.ImagePath).FirstOrDefault();
            if(imgpath != null)
                mobile.ImagePath = imgpath;
            try
            {

                string path = Path.Combine(env.WebRootPath, "images"); // wwwroot/images/
                if (img_file != null)
                {
                    if (img_file.FileName.EndsWith(".jpg") || img_file.FileName.EndsWith(".png"))
                    {
                        //delete old img
                        string oldpath = Path.Combine(path, imgpath);
                        path = Path.Combine(path, img_file.FileName);
                        if (oldpath != path)
                        {
                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                            //save new img
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                img_file.CopyTo(stream);
                                mobile.ImagePath = img_file.FileName;
                            }
                        }

                    }
                    else
                    {
                        ViewBag.imgNote = "This file not an image!";
                        return View("Edit", mobile);
                    }
                }
                context.Update(mobile);
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MobileExists(mobile.MobileName))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("allMobs", "Search");
        }
        //DeleteMobile
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = context.mobiles
                .FirstOrDefault(m => m.MobileName == id);
            if (mobile == null)
            {
                return NotFound();
            }

            return View(mobile);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var mobile = context.mobiles.Find(id);
            if (mobile != null)
            {
                context.mobiles.Remove(mobile);
            }

            context.SaveChanges();
            return RedirectToAction("allMobs", "Search");
        }
        private bool MobileExists(string id)
        {
            return context.mobiles.Any(e => e.MobileName == id);
        }
    }
}
