namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renamed_BinaryObject_Table : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpBinaryObjects", newName: "AppBinaryObjects");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.AppBinaryObjects", newName: "AbpBinaryObjects");
        }
    }
}
