﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleBlog.DAL.Context;
using SimpleBlog.DAL.DataService;
using SimpleBlog.DAL.Object_Model;
using SimpleBlog.DAL.ViewModel;

namespace SimpleBlog.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private UnitOfWork _unitOfWork;

        public UserController()
        {
            _unitOfWork = new UnitOfWork(ApplicationContext.Create());
        }

        public ActionResult List()
        {
            var users = _unitOfWork.DataContext.Users.ToList();
            if (users.Any())
            {
                return View(Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserViewModel>>(users));
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = _unitOfWork.DataContext.Users.Find(id);
            if (user != null)
            {
                return View(user);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DataContext.Entry(model).State = EntityState.Modified;
                _unitOfWork.Save();
                return RedirectToAction("LogOff", "Account", new {@area = ""});
            }
            return RedirectToAction("List");
        }
    }
}