namespace RMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "SPI", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "SPI", c => c.Single(nullable: false));
        }
    }
}
