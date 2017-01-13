namespace RMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "SPI", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "SPI");
        }
    }
}
