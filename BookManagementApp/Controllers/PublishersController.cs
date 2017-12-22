using System;
using System.Web.Mvc;
using System.Net;

using DAL.Entities;
using Services;

namespace BookManagementApp.Controllers
{
    public class PublishersController : Controller
    {
        PublisherService publisherServ = new PublisherService();

        // GET: Publishers
        public ActionResult Index()
        {
            return View(publisherServ.GetAll());
        }
       
        // GET: Publishers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,Phone,AccountNumber")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                publisherServ.Add(publisher);

                return RedirectToAction("Index");
            }

            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int publisherID = Convert.ToInt32(id);
            Publisher publisher = publisherServ.GetByID(publisherID);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Phone,AccountNumber")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                publisherServ.Update(publisher);

                return RedirectToAction("Index");
            }
            return View(publisher);
        }
    }
}
