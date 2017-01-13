namespace RMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                        MarksObtained = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.SubjectID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RollNo = c.String(),
                        Branch = c.String(),
                        Semester = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Title = c.String(),
                        Credits = c.Int(nullable: false),
                        MaxMarks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Results", "StudentID", "dbo.Students");
            DropIndex("dbo.Results", new[] { "SubjectID" });
            DropIndex("dbo.Results", new[] { "StudentID" });
            DropTable("dbo.Subjects");
            DropTable("dbo.Students");
            DropTable("dbo.Results");
        }
    }
}
