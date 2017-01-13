using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMS.Models
{
	public class Subject
	{
		public int ID { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public int Credits { get; set; }
		public int MaxMarks { get; set; }

		public virtual ICollection<Result> Results { get; set; }
	}
}