using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HansenProject3.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// Get all data listed inside of the Products table
        /// </summary>
        /// <param name="id">Used to check ID and search through table</param>
        /// <param name="sortBy">Used to check which column is being sorted</param>
        /// <returns></returns>
        public ActionResult AllProducts(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Product> products;

            switch (sortBy)
            {
                case 1:
                    {
                        products = context.Products.OrderBy(x => x.Description).ToList();
                        break;
                    }
                case 2:
                    {
                        products = context.Products.OrderBy(x => x.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        products = context.Products.OrderBy(x => x.OnHandQuantity).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        products = context.Products.OrderBy(x => x.ProductCode).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                products = products.Where(x =>
                                            x.ProductCode.ToLower().Contains(id) ||
                                            x.Description.ToLower().Contains(id)
                                            ).ToList();
            }

            return View(products);
        }

        /// <summary>
        /// Update existing data into the table
        /// </summary>
        /// <param name="id">Checks if ID matches an existing ID to update</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities context = new BooksEntities();
            Product products = context.Products.Where(x => x.ProductCode == id).FirstOrDefault();

            //if (Customer.IsDeleted)
            //{
            //    return RedirectToAction("AllCustomers");
            //}

            return View(products);
        }

        /// <summary>
        /// Add new data into the table
        /// </summary>
        /// <param name="newInvoiceLineItems">Sent in params to add new item to table</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(Product newProduct)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Products.Where(x => x.ProductCode == newProduct.ProductCode).Count() > 0)
                {
                    var productToSave = context.Products.Where(x => x.ProductCode == newProduct.ProductCode).FirstOrDefault();

                    productToSave.ProductCode = newProduct.ProductCode;
                    productToSave.Description = newProduct.Description;
                    productToSave.UnitPrice = newProduct.UnitPrice;
                    productToSave.OnHandQuantity = newProduct.OnHandQuantity;
                }
                else
                {
                    context.Products.Add(newProduct);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllProducts");
        }
    }
}