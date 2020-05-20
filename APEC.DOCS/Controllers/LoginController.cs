using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using APEC.DOCS.Models;
using APEC.DOCS;
using APEC.DOCS.Common;

namespace APEC.DOCS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PostSeessionUser(LoginModel loginModel)
        {
            Session.Add(CommonConstants.UserSession, loginModel);
            return Json(new{Status = 1}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.UserSession] = null;
            return Redirect("Index");
        }
    }
}