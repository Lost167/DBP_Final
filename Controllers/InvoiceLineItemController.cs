using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HansenProject3.Controllers
{
    public class InvoiceLineItemController : Controller
    {
        /// <summary>
        /// Get all data listed inside of the InvoiceLineItems table
        /// </summary>
        /// <param name="id">Used to check ID and search through table</param>
        /// <param name="sortBy">Used to check which column is being sorted</param>
        /// <returns></returns>
        public ActionResult AllInvoiceLineItems(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<InvoiceLineItem> invoiceLineItems;

            switch (sortBy)
            {
                case 1:
                    {
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(x => x.ProductCode).ToList();
                        break;
                    }
                case 2:
                    {
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(x => x.UnitPrice).ToList();
                        break;
                    }
                case 3:
                    {
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(x => x.Quantity).ToList();
                        break;
                    }
                case 4:
                    {
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(x => x.ItemTotal).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        invoiceLineItems = context.InvoiceLineItems.OrderBy(x => x.InvoiceID).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                invoiceLineItems = invoiceLineItems.Where(x => x.ProductCode.ToLower().Contains(id)).ToList();
            }

            return View(invoiceLineItems);
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
            InvoiceLineItem invoiceLineItems = context.InvoiceLineItems.Where(x => x.InvoiceID == id).FirstOrDefault();

            return View(invoiceLineItems);
        }

        /// <summary>
        /// Add new data into the table
        /// </summary>
        /// <param name="newInvoiceLineItems">Sent in params to add new item to table</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(InvoiceLineItem newInvoiceLineItems)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.InvoiceLineItems.Where(x => x.InvoiceID == newInvoiceLineItems.InvoiceID).Count() > 0)
                {
                    var invoiceLineItemToSave = context.InvoiceLineItems.Where(x => x.InvoiceID == newInvoiceLineItems.InvoiceID).FirstOrDefault();

                    invoiceLineItemToSave.InvoiceID = newInvoiceLineItems.InvoiceID;
                    invoiceLineItemToSave.ProductCode = newInvoiceLineItems.ProductCode;
                    invoiceLineItemToSave.UnitPrice = newInvoiceLineItems.UnitPrice;
                    invoiceLineItemToSave.Quantity = newInvoiceLineItems.Quantity;
                    invoiceLineItemToSave.ItemTotal = newInvoiceLineItems.ItemTotal;
                }
                else
                {
                    context.InvoiceLineItems.Add(newInvoiceLineItems);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllInvoiceLineItems");
        }
    }
}