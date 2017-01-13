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
    public class StudentsController : Controller
    {
        private RMSContext db = new RMSContext();

		[Authorize]
		// GET: Students
		public ActionResult Index()
        {
            return View(db.Students.OrderBy(x => x.RollNo).ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
				return View("Views/Home/Index");
            }
            Student student = db.Students.Where(x => x.RollNo == id).FirstOrDefault();
            if (student == null)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            return View(student);
        }

		[Authorize]
		// GET: Students/Create
		public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,RollNo,Branch,Semester")] Student student)
        {
            if (ModelState.IsValid)
            {
				if (db.Students.Any(x => x.RollNo == student.RollNo))
				{
					ViewBag.Error = "Roll no. already exists.";
					return View(student);
				}
				for(int i = 0; i < student.Name.Length; i++)
				{
					if(student.Name[i] != ' ' && student.Name[i] != '.' && (!Char.IsLetter(student.Name[i])))
					{
						ViewBag.Error = "Name is invalid";
						return View(student);
					}
				}
				for (int i = 0; i < student.Branch.Length; i++)
				{
					if (student.Branch[i] != ' ' && (!Char.IsLetter(student.Branch[i])))
					{
						ViewBag.Error = "Branch is invalid";
						return View(student);
					}
				}
				for (int i = 0; i < student.Semester.Length; i++)
				{
					if (!Char.IsLetter(student.Semester[i]))
					{
						ViewBag.Error = "Semester is invalid";
						return View(student);
					}
				}
				if (student.RollNo.Length != 11)
				{
					ViewBag.Error = "Roll No is invalid";
					return View(student);
				}

				for (int i = 0; i < student.RollNo.Length; i++)
				{
					if (student.RollNo[i] != '-' && !Char.IsLetterOrDigit(student.RollNo[i]))
					{
						ViewBag.Error = "Roll No is invalid";
						return View(student);
					}
				}

				if(!(Char.IsDigit(student.RollNo[0]) &&
					(student.RollNo[1] == 'k') && 
					Char.IsDigit(student.RollNo[2]) &&
					Char.IsDigit(student.RollNo[3]) &&
					(student.RollNo[4] == '-') &&
					Char.IsLetter(student.RollNo[5]) && Char.IsUpper(student.RollNo[5]) &&
					Char.IsLetter(student.RollNo[6]) && Char.IsUpper(student.RollNo[6]) &&
					(student.RollNo[7] == '-') &&
					Char.IsDigit(student.RollNo[8]) &&
					Char.IsDigit(student.RollNo[9]) &&
					Char.IsDigit(student.RollNo[10])))
				{
					ViewBag.Error = "Roll No is invalid";
					return View(student);
				}

				student.SPI = 0.0f;
				db.Students.Add(student);
				db.SaveChanges();
				return RedirectToAction("Index");
				
            }

            return View(student);
        }

		[Authorize]
		// GET: Students/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

		[Authorize]
		// POST: Students/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,RollNo,Branch,Semester,SPI")] Student student)
        {
            if (ModelState.IsValid)
            {
				for (int i = 0; i < student.Name.Length; i++)
				{
					if (student.Name[i] != ' ' && student.Name[i] != '.' && (!Char.IsLetter(student.Name[i])))
					{
						ViewBag.Error = "Name is invalid";
						return View(student);
					}
				}
				for (int i = 0; i < student.Branch.Length; i++)
				{
					if (student.Branch[i] != ' ' && (!Char.IsLetter(student.Branch[i])))
					{
						ViewBag.Error = "Branch is invalid";
						return View(student);
					}
				}
				for (int i = 0; i < student.Semester.Length; i++)
				{
					if (!Char.IsLetter(student.Semester[i]))
					{
						ViewBag.Error = "Semester is invalid";
						return View(student);
					}
				}


				db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

		[Authorize]
		// GET: Students/Delete/5
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

		[Authorize]
		// POST: Students/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
