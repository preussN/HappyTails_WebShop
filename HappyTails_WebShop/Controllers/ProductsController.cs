using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace HappyTails_WebShop.Controllers
{
    public class ProductsController : Controller
    {
        private HappyTailsEntities db = new HappyTailsEntities();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.prodName = product.Name;

            return View(product);
        }

        public ActionResult ShowDogProducts()
        {
            var dogProds = from p in db.Product
                           where p.AnimalCategory == "dog"
                           select p;

            List<Product> dogProdsList = dogProds.ToList();

            return View(dogProdsList);
        }

        public ActionResult ShowCatProducts()
        {
            var catProds = from p in db.Product
                           where p.AnimalCategory == "cat"
                           select p;

            List<Product> catProdList = catProds.ToList();

            return View(catProdList);
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
