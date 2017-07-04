namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserChange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "ABP.Users", newName: "AbpUsers");
            MoveTable(name: "ABP.AbpUsers", newSchema: "dbo");
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(maxLength: 256));
            DropColumn("dbo.AbpUsers", "Name");
            DropColumn("dbo.AbpUsers", "Surname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpUsers", "Surname", c => c.String(nullable: false, maxLength: 32));
            AddColumn("dbo.AbpUsers", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(nullable: false, maxLength: 256));
            MoveTable(name: "dbo.AbpUsers", newSchema: "ABP");
            RenameTable(name: "ABP.AbpUsers", newName: "Users");
        }
    }
}
