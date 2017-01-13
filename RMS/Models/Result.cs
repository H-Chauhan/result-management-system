using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RMS.Models
{
	public class Result
	{
		public int ID { get; set; }
		public int StudentID { get; set; }
		public int SubjectID { get; set; }
		public int MarksObtained { get; set; }
		[DefaultValue(0)]
		public int Credits { get; set; }

		[ForeignKey("StudentID")]
		public virtual Student Student { get; set; }
		[ForeignKey("SubjectID")]
		public virtual Subject Subject { get; set; }
	}
}