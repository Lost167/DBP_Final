using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HansenProject3.Controllers
{
    public class InvoiceController : Controller
    {
        /// <summary>
        /// Get all data listed inside of the Invoices table
        /// </summary>
        /// <param name="id">Used to check ID and search through table</param>
        /// <param name="sortBy">Used to check which column is being sorted</param>
        /// <returns></returns>
        public ActionResult AllInvoices(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Invoice> invoices;

            switch (sortBy)
            {
                case 1:
                    {
                        invoices = context.Invoices.OrderBy(x => x.CustomerID).ToList();
                        break;
                    }
                case 2:
                    {
                        invoices = context.Invoices.OrderBy(x => x.InvoiceDate).ToList();
                        break;
                    }
                case 3:
                    {
                        invoices = context.Invoices.OrderBy(x => x.ProductTotal).ToList();
                        break;
                    }
                case 4:
                    {
                        invoices = context.Invoices.OrderBy(x => x.SalesTax).ToList();
                        break;
                    }
                case 5:
                    {
                        invoices = context.Invoices.OrderBy(x => x.Shipping).ToList();
                        break;
                    }
                case 6:
                    {
                        invoices = context.Invoices.OrderBy(x => x.InvoiceTotal).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        invoices = context.Invoices.OrderBy(x => x.InvoiceID).ToList();
                        break;
                    }
            }

            return View(invoices);
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
            Invoice invoices = context.Invoices.Where(x => x.InvoiceID == id).FirstOrDefault();

            //if (Customer.IsDeleted)
            //{
            //    return RedirectToAction("AllCustomers");
            //}

            return View(invoices);
        }

        /// <summary>
        /// Add new data into the table
        /// </summary>
        /// <param name="newInvoiceLineItems">Sent in params to add new item to table</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(Invoice newInvoice)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Invoices.Where(x => x.InvoiceID == newInvoice.InvoiceID).Count() > 0)
                {
                    var invoiceToSave = context.Invoices.Where(x => x.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();

                    invoiceToSave.InvoiceID = newInvoice.InvoiceID;
                    invoiceToSave.CustomerID = newInvoice.CustomerID;
                    invoiceToSave.InvoiceDate = newInvoice.InvoiceDate;
                    invoiceToSave.ProductTotal = newInvoice.ProductTotal;
                    invoiceToSave.SalesTax = newInvoice.SalesTax;
                    invoiceToSave.Shipping = newInvoice.Shipping;
                    invoiceToSave.InvoiceTotal = newInvoice.InvoiceTotal;
                }
                else
                {
                    context.Invoices.Add(newInvoice);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllInvoices");
        }
    }
}