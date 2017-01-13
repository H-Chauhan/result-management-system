using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RMS.Models;

namespace RMS.Controllers
{
	[Authorize]
	public class SubjectsController : Controller
    {
        private RMSContext db = new RMSContext();

        // GET: Subjects
        public ActionResult Index()
        {
            return View(db.Subjects.OrderBy(x => x.Code).ToList());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Title,Credits,MaxMarks")] Subject subject)
        {
            if (ModelState.IsValid)
            {
				if (db.Subjects.Any(x => x.Code == subject.Code))
				{
					ViewBag.Error = "Code already exists.";
					return View(subject);
				}

				if(subject.Credits < 0)
				{
					ViewBag.Error = "Invalid Credits.";
					return View(subject);
				}

				if(subject.MaxMarks < 0)
				{
					ViewBag.Error = "Invalid MaxMarks.";
					return View(subject);
				}

				if(subject.Code.Length != 6)
				{
					ViewBag.Error = "Invalid Code.";
					return View(subject);
				}

				if(!(Char.IsLetter(subject.Code[0]) && Char.IsUpper(subject.Code[0]) &&
					Char.IsLetter(subject.Code[1]) && Char.IsUpper(subject.Code[1]) &&
					subject.Code[2] == '-' &&
					Char.IsDigit(subject.Code[3]) &&
					Char.IsDigit(subject.Code[4]) &&
					Char.IsDigit(subject.Code[5]))) {
					ViewBag.Error = "Invalid Code.";
					return View(subject);
				}

				db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Title,Credits,MaxMarks")] Subject subject)
        {
            if (ModelState.IsValid)
            {
				if (subject.Credits < 0)
				{
					ViewBag.Error = "Invalid Credits.";
					return View(subject);
				}

				if (subject.MaxMarks < 0)
				{
					ViewBag.Error = "Invalid MaxMarks.";
					return View(subject);
				}

				db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
