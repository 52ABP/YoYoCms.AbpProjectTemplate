namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BinaryObjects : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BinaryObjects", newName: "AppBinaryObjects");
            MoveTable(name: "dbo.AppBinaryObjects", newSchema: "ABP");
        }
        
        public override void Down()
        {
            MoveTable(name: "ABP.AppBinaryObjects", newSchema: "dbo");
            RenameTable(name: "dbo.AppBinaryObjects", newName: "BinaryObjects");
        }
    }
}
