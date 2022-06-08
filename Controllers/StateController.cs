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
        // GET: State
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

        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities context = new BooksEntities();
            State states = context.States.Where(x => x.StateCode == id).FirstOrDefault();

            //if (Customer.IsDeleted)
            //{
            //    return RedirectToAction("AllCustomers");
            //}

            return View(states);
        }

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