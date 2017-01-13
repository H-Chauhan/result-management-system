using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace RMS.Models
{
	public class Student
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string RollNo { get; set; }
		public string Branch { get; set; }
		public string Semester { get; set; }
		[DefaultValue(0.0)]
		public float? SPI { get; set; }

		public virtual ICollection<Result> Results { get; set; }
	}
}