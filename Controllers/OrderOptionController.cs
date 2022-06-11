using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HansenProject3.Controllers
{
    public class OrderOptionController : Controller
    {
        /// <summary>
        /// Get all data listed inside of the OrderOptions table
        /// </summary>
        /// <param name="id">Used to check ID and search through table</param>
        /// <param name="sortBy">Used to check which column is being sorted</param>
        /// <returns></returns>
        public ActionResult AllOrderOptions(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<OrderOption> orderOptions;

            switch (sortBy)
            {
                case 1:
                    {
                        orderOptions = context.OrderOptions.OrderBy(x => x.AdditionalBookShipCharge).ToList();
                        break;
                    }
                case 2:
                    {
                        orderOptions = context.OrderOptions.OrderBy(x => x.FirstBookShipCharge).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        orderOptions = context.OrderOptions.OrderBy(x => x.SalesTaxRate).ToList();
                        break;
                    }
            }

            return View(orderOptions);
        }

        /// <summary>
        /// Update existing data into the table
        /// </summary>
        /// <param name="id">Checks if ID matches an existing ID to update</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            OrderOption orderOptions = context.OrderOptions.Where(x => x.SalesTaxRate == id).FirstOrDefault();

            //if (Customer.IsDeleted)
            //{
            //    return RedirectToAction("AllCustomers");
            //}

            return View(orderOptions);
        }

        /// <summary>
        /// Add new data into the table
        /// </summary>
        /// <param name="newInvoiceLineItems">Sent in params to add new item to table</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(OrderOption newOrderOptions)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.OrderOptions.Where(x => x.SalesTaxRate == newOrderOptions.SalesTaxRate).Count() > 0)
                {
                    var orderOptionToSave = context.OrderOptions.Where(x => x.SalesTaxRate == newOrderOptions.SalesTaxRate).FirstOrDefault();

                    orderOptionToSave.SalesTaxRate = newOrderOptions.SalesTaxRate;
                    orderOptionToSave.FirstBookShipCharge = newOrderOptions.FirstBookShipCharge;
                    orderOptionToSave.AdditionalBookShipCharge = newOrderOptions.AdditionalBookShipCharge;
                }
                else
                {
                    context.OrderOptions.Add(newOrderOptions);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllOrderOptions");
        }
    }
}