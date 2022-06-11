using HansenProject3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HansenProject3.Controllers
{
    public class StateController : Controller
    {
        /// <summary>
        /// Get all data listed inside of the States table
        /// </summary>
        /// <param name="id">Used to check ID and search through table</param>
        /// <param name="sortBy">Used to check which column is being sorted</param>
        /// <returns></returns>
        public ActionResult AllStates(string id, int sortBy = 0)
        {
            BooksEntities context = new BooksEntities();
            List<State> states;

            switch (sortBy)
            {
                case 1:
                    {
                        states = context.States.OrderBy(x => x.StateName).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        states = context.States.OrderBy(x => x.StateCode).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                states = states.Where(x =>
                                            x.StateCode.ToLower().Contains(id) ||
                                            x.StateName.ToLower().Contains(id)
                                            ).ToList();
            }

            return View(states);
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
            State states = context.States.Where(x => x.StateCode == id).FirstOrDefault();

            return View(states);
        }

        /// <summary>
        /// Add new data into the table
        /// </summary>
        /// <param name="newInvoiceLineItems">Sent in params to add new item to table</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upsert(State newState)
        {
            BooksEntities context = new BooksEntities();

            try
            {
                if (context.States.Where(x => x.StateCode == newState.StateCode).Count() > 0)
                {
                    var stateToSave = context.States.Where(x => x.StateCode == newState.StateCode).FirstOrDefault();

                    stateToSave.StateCode = newState.StateCode;
                    stateToSave.StateName = newState.StateName;
                }
                else
                {
                    context.States.Add(newState);
                }
                context.SaveChanges();
            }
            catch (Exception ex) { }

            return RedirectToAction("AllStates");
        }
    }
}