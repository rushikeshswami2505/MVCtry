using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCtry.Models;

namespace MVCtry.Controllers
{
    public class HomeController : Controller
    {
        ProductContext db = new ProductContext();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }

        public ActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Buy(Product product, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid==true) {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Buy",product);
        }
        public ActionResult Sell()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sell(Product product, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                var existingProduct = db.Products.FirstOrDefault(p => p.itemtype == product.itemtype && p.itemsize == product.itemsize);
                if (existingProduct != null)
                {
                    existingProduct.itempiece -= product.itempiece;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Sell", product);
        }
        public ActionResult Edit(int id)
        {
            var row = db.Products.Where(p => p.id == id).FirstOrDefault();
            return View(row);
        }

        [HttpPost]
        public ActionResult Edit(Product product, string goHome) {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            return View("Sell", product);
        }

        public ActionResult Delete(int id)
        {
            var productToDelete = db.Products.Find(id);
            if (productToDelete != null)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

    }
}