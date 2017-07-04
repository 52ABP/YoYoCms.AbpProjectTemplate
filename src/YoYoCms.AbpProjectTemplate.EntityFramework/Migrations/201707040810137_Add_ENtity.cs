namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ENtity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AbpUsers", newName: "User");
            MoveTable(name: "dbo.User", newSchema: "ABP");
        }
        
        public override void Down()
        {
            MoveTable(name: "ABP.User", newSchema: "dbo");
            RenameTable(name: "dbo.User", newName: "AbpUsers");
        }
    }
}
