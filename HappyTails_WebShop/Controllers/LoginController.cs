using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HappyTails_WebShop.Controllers
{
    public class LoginController : Controller
    {
        private HappyTailsEntities db = new HappyTailsEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Customer loginCust)
        {
            if (loginCust.Username == null || loginCust.Password == null)
            {
                ModelState.AddModelError("", "Enter both a username and a password.");
                return View();
            }

            bool validUser = false;

            //validUser triggers a method called CheckUser, which is used to check if the user really exists in the database
            validUser = CheckUser(loginCust.Username, loginCust.Password);

            if (validUser == true)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(loginCust.Username, false);
                //If the user exists in the database, the user to another page
                return RedirectToAction("Welcome", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            //If the user enters a invalid username or password he/she is send back to the login page where message says that something is wrong
            return View();
        }

        //Checks if the user exists in the database
        private bool CheckUser(string username, string password)
        {
            //Query to check if the user exists in db
            var user = from rows in db.Customer
                       where rows.Username.ToUpper() == username.ToUpper()
                       && rows.Password == password
                       select rows;

            if (user.Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            //Will clear the session at the end of request
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}