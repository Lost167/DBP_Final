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
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }
    }
}