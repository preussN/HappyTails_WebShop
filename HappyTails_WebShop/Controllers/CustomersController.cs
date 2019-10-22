using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HappyTails_WebShop;

namespace HappyTails_WebShop.Controllers
{
    public class CustomersController : Controller
    {
        private HappyTailsEntities db = new HappyTailsEntities();

        // GET: Customers
        //Shows the customers profile
        [Authorize]
        public ActionResult Index(int? id)
        {
            var custID = db.Customer.FirstOrDefault(x => x.Username == System.Web.HttpContext.Current.User.Identity.Name).CustomerID;

            var custInfo = from cust in db.Customer
                where cust.CustomerID == custID
                select cust;

            return View(custInfo);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer newCustomer)
        {
            bool validEmail = true;
            bool validUsername = true;

            //validEmail triggers a method called CheckEmail, which is used to check if the entered email already exists in the database
            validEmail = CheckEmail(newCustomer.Email);

            //validEmail triggers a method called CheckUsername, which is used to check if the entered username already exists in the database
            validUsername = CheckUsername(newCustomer.Username);

            if (validEmail == true)
            {
                if (validUsername == true)
                {
                    try
                    {
                        //Creates a new valid user to db, and store his/hers data
                        db.Customer.Add(newCustomer);
                        db.SaveChanges();

                        return RedirectToAction("SuccessfulNewUser", "Customers");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error =
                            "There was an error trying to create an account for you.\nWe are so sorry, please try again soon! " +
                            ex.Message;
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Entered username is already in use.\n");
                }
            }
            else
            {
                ModelState.AddModelError("", "Entered email is already in use.\n");
            }
            //If the user enters a invalid username or password he/she is sent back to the login page where message says that something is wrong
            return View();
        }


        //Method for checking if entered email already exists in the database
        private bool CheckEmail(string email)
        {
            var enteredEmail = from rows in db.Customer
                               where rows.Email == email
                               select rows;

            if (enteredEmail.Count() > 0)
            {
                return false;
            }
            return true;
        }


        //Method for checking if entered username already exists in the database
        private bool CheckUsername(string username)
        {
            var enteredUsername = from rows in db.Customer
                                  where rows.Username == username
                                  select rows;

            if (enteredUsername.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ActionResult SuccessfulNewUser()
        {
            return View();
        }

        // GET: Customers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Firstname,Surname,Email,Street,ZipCode,Password,Username")] Customer customer)
        {
            //The user should not be able to alter his or hers username
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.editSuccess = "Your profile has been updated!";
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.savingError = "We are so sorry, but there was an error trying to save your new information.\nPlease try again soon!\n" +
                                          ex.Message;
                    return View();
                }
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);

            try
            {
                db.Customer.Remove(customer);
                db.SaveChanges();
                return RedirectToAction("AccountDeleted");
            }
            catch (Exception ex)
            {
                ViewBag.deletionError = "We are so sorry, but there was an error trying to delete your account.\nPlease try again soon.\n" +
                                      ex.Message;
                return View();
            }

        }
    
        public ActionResult AccountDeleted()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
