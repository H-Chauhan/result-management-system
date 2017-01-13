using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RMS.Models
{
	public class RMSContext : DbContext
	{
		public RMSContext() : base("RMSContext")
        {
		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Result> Results { get; set; }
		public DbSet<Subject> Subjects { get; set; }
	}
}