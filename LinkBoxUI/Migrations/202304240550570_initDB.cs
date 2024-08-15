namespace LinkBoxUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDB : DbMigration
    {
        public override void Down()
        {
            //RenameTable(name: "dbo.CrystalExtracts", newName: "CrystalExtractSetup");
            //RenameTable(name: "dbo.CrystalExtractSetup", newName: "CrystalExtractSetup");
        }
        
        public override void Up()
        {
            //RenameTable(name: "dbo.CrystalExtractSetup", newName: "CrystalExtracts");
            //RenameTable(name: "dbo.CrystalSetups", newName: "CrystalExtracts");
        }
    }
}
