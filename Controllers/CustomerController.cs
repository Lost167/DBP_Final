﻿using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace HansenProject3.Controllers
{

    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult AllCustomers(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<Customer> customers;

            switch (sortBy)
            {
                case 1:
                    {
                        customers = context.Customers.OrderBy(x => x.Name).ToList();
                        break;
                    }
                case 2:
                    {
                        customers = context.Customers.OrderBy(x => x.Address).ToList();
                        break;
                    }
                case 3:
                    {
                        customers = context.Customers.OrderBy(x => x.City).ToList();
                        break;
                    }
                case 4:
                    {
                        customers = context.Customers.OrderBy(x => x.State).ToList();
                        break;
                    }
                case 5:
                    {
                        customers = context.Customers.OrderBy(x => x.ZipCode).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        customers = context.Customers.OrderBy(x => x.CustomerID).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                customers = customers.Where(x =>
                                            x.Name.ToLower().Contains(id) ||
                                            x.Address.ToLower().Contains(id) ||
                                            x.City.ToLower().Contains(id) ||
                                            x.State.ToLower().Contains(id)
                                            ).ToList();
            }

            return View(customers);
        }

        [HttpGet]
        public ActionResult Upsert(int id)
        {
            BooksEntities context = new BooksEntities();
            Customer customers = context.Customers.Where(x => x.CustomerID == id).FirstOrDefault();

            //if (Customer.IsDeleted)
            //{
            //    return RedirectToAction("AllCustomers");
            //}

            return View(customers);
        }

        [HttpPost]
        public ActionResult Upsert(Customer newCustomer)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.Customers.Where(x => x.CustomerID == newCustomer.CustomerID).Count() > 0)
                {
                    var customerToSave = context.Customers.Where(x => x.CustomerID == newCustomer.CustomerID).FirstOrDefault();

                    customerToSave.CustomerID = newCustomer.CustomerID;
                    customerToSave.Name = newCustomer.Name;
                    customerToSave.Address = newCustomer.Address;
                    customerToSave.City = newCustomer.City;
                    customerToSave.State = newCustomer.State;
                    customerToSave.ZipCode = newCustomer.ZipCode;
                }
                else
                {
                    context.Customers.Add(newCustomer);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllCustomers");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities context = new BooksEntities();

            int customerId = 0;
            if (int.TryParse(id, out customerId))
            {
                try
                {
                    Customer customer = context.Customers.Where(x => x.CustomerID == customerId).FirstOrDefault();
                    context.Customers.Remove(customer);

                    context.SaveChanges();
                }
                catch (Exception) { }
            }
            else { }

            return RedirectToAction("AllCustomers");
        }
    }
}