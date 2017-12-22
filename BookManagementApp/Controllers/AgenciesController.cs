using System;
using System.Web.Mvc;
using System.Net;

using DAL.Entities;
using Services;


namespace BookManagementApp.Controllers
{
    public class AgenciesController : Controller
    {
        AgencyService agencyServ = new AgencyService();

        // GET: Agencies
        public ActionResult Index()
        {
            return View(agencyServ.GetAll());
        }

        // GET: Agencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agencies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,Phone,AccountNumber")] Agency agency)
        {
            if (ModelState.IsValid)
            {
                agencyServ.Add(agency);

                return RedirectToAction("Index");
            }

            return View(agency);
        }

        // GET: Agencies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int agencyID = Convert.ToInt32(id);
            Agency agency = agencyServ.GetByID(agencyID);
            if (agency == null)
            {
                return HttpNotFound();
            }

            return View(agency);
        }

        // POST: Agencies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Phone,AccountNumber")] Agency agency)
        {
            if (ModelState.IsValid)
            {
                agencyServ.Update(agency);
                return RedirectToAction("Index");
            }

            return View(agency);
        }
    }
}
