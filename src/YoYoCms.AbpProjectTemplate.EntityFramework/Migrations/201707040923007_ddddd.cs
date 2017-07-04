namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ddddd : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "ABP.User", newName: "Users");
        }
        
        public override void Down()
        {
            RenameTable(name: "ABP.Users", newName: "User");
        }
    }
}
