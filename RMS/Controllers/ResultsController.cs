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
	public class ResultsController : Controller
    {
        private RMSContext db = new RMSContext();

        // GET: Results
        public ActionResult Index(string searchString)
        {
			var results = db.Results.Include(r => r.Student).Include(r => r.Subject);
			if (!String.IsNullOrEmpty(searchString))
			{
				results = results.Where(r => r.Student.Name.Contains(searchString)
											|| r.Student.RollNo.Contains(searchString)
											|| r.Student.Branch.Contains(searchString)
											|| r.Subject.Code.Contains(searchString)
											|| r.Subject.Title.Contains(searchString));
			}
            return View(results.OrderBy(x => x.Student.RollNo).ThenBy(x => x.Subject.Code).ToList());
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }



		// GET: Results/Create
		public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo");
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,SubjectID,MarksObtained")] Result result)
        {
            if (ModelState.IsValid)
            {
				var sub = db.Subjects.Find(result.SubjectID);

				if(db.Results.Any(x => x.StudentID == result.StudentID && x.SubjectID == result.SubjectID))
				{
					ViewBag.Error = "Record for this student and this subject already exists.";
					ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
					ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
					return View(result);
				}

				if(result.MarksObtained < 0 || result.MarksObtained > sub.MaxMarks)
				{
					ViewBag.Error = "Marks should be between 0 and max marks of the subject.";
					ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
					ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
					return View(result);
				}

				if(((result.MarksObtained * 100.0) / sub.MaxMarks) < 40.0)
				{
					result.Credits = 0;
				}
				else
				{
					result.Credits = sub.Credits;
				}
                db.Results.Add(result);
                db.SaveChanges();

				calculateSPI(result);

                return RedirectToAction("Index");
            }

            ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
            return View(result);
        }

        // GET: Results/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,SubjectID,MarksObtained")] Result result)
        {
            if (ModelState.IsValid)
            {
				var sub = db.Subjects.Find(result.SubjectID);

				if (result.MarksObtained < 0 || result.MarksObtained > sub.MaxMarks)
				{
					ViewBag.Error = "Marks should be between 0 and max marks of the subject.";
					ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
					ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
					return View(result);
				}

				if (((result.MarksObtained * 100.0) / sub.MaxMarks) < 40.0)
				{
					result.Credits = 0;
				}
				else
				{
					result.Credits = sub.Credits;
				}
				db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();

				calculateSPI(result);

				return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.Students, "ID", "RollNo", result.StudentID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Code", result.SubjectID);
            return View(result);
        }

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		private void calculateSPI(Result result)
		{
			int totalMarksObtained = 0;
			int totalMaxMarks = 0;
			Student student = db.Students.Find(result.StudentID);
			foreach (Result r in student.Results)
			{
				totalMarksObtained += r.MarksObtained * r.Credits;
				totalMaxMarks += r.Subject.MaxMarks * r.Subject.Credits;
			}
			float SPI = (float)((totalMarksObtained * 100.0) / totalMaxMarks);
			student.SPI = SPI;
			db.SaveChanges();
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
