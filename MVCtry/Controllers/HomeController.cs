using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCtry.Models;
using MVCtry.Data;
using System.Web.WebPages;

namespace MVCtry.Controllers
{
    public class HomeController : Controller
    {
        ProductContext db = new ProductContext();
        // GET: Home

        string useremail = "rushi@gmail.com";
        string userpassword = "123123";

        [Route("Home/Index")]
        [Route("Home")]
        // [Route("")]
        public ActionResult Index()
        {
            var data = db.Products.ToList();
            return View(data);
        }
        [Route("Home/Buy")]


        [Route("")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public ActionResult Login(string email,string password)
        {
            if(email.IsEmpty() || password.IsEmpty())
            {
                if (email.IsEmpty()) ModelState.AddModelError("email", "Please enter email");
                if (password.IsEmpty()) ModelState.AddModelError("password", "Please enter password");
            }
            if (email != useremail)
            {
                ModelState.AddModelError("email", "User not found");
            }
            else if (password != userpassword)
            {
                ModelState.AddModelError("password", "Password did not match");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        [Route("Home/Signup")]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [Route("Home/Signup")]
        public ActionResult Signup(string email, string firstname, string lastname, string gender, string phone, string password, string cpassword)
        {
            Response.Write("<h2>Signup Confirmation</h2>");
            Response.Write("<p>Email: " + email + "</p>");
            Response.Write("<p>First Name: " + firstname + "</p>");
            Response.Write("<p>Last Name: " + lastname + "</p>");
            Response.Write("<p>Gender: " + gender + "</p>");
            Response.Write("<p>Phone: " + phone + "</p>");

            return RedirectToAction("Login");
        }

        [Route("Home/Buy")]
        public ActionResult Buy()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Buy")]
        public ActionResult Buy(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Buy", productModel);
        }

        [Route("Home/Sell")]
        public ActionResult Sell()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Sell")]
        public ActionResult Sell(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                var existingProduct = db.Products.FirstOrDefault(p => p.itemtype == productModel.itemtype && p.itemsize == productModel.itemsize);
                if (existingProduct != null)
                {
                    existingProduct.itempiece -= product.itempiece;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Sell", productModel);
        }

        [Route("Home/Edit")]
        public ActionResult Edit(int id)
        {
            var row = db.Products.Where(p => p.id == id).FirstOrDefault();
            ProductModel product = DataBaseToModel(row);
            return View(product);
        }

        [HttpPost]
        [Route("Home/Edit")]
        public ActionResult Edit(ProductModel productModel, string goHome)
        {
            if (goHome != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid == true)
            {
                Product product = ModelToDataBase(productModel);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", productModel);
        }

        // [HttpDelete]
        [Route("Home/Delete")]
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
        private Product ModelToDataBase(ProductModel productModel)
        {
            Product product = new Product();
            product.id = productModel.id;
            product.itemtype = productModel.itemtype;
            product.itemsize = productModel.itemsize;
            product.itempiece = productModel.itempiece;
            product.itemprice = productModel.itemprice;
            return product;
        }

        private ProductModel DataBaseToModel(Product product)
        {
            ProductModel productModel = new ProductModel();
            productModel.id = product.id;
            productModel.itemtype = product.itemtype;
            productModel.itemsize = product.itemsize;
            productModel.itempiece = product.itempiece;
            productModel.itemprice = product.itemprice;
            return productModel;
        }

    }
}