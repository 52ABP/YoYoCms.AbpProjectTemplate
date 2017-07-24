namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_User_Name : DbMigration
    {
        public override void Up()
        {
            AlterColumn("ABP.Users", "Name", c => c.String(maxLength: 32));
            AlterColumn("ABP.Users", "Surname", c => c.String(maxLength: 32));
            AlterColumn("ABP.Users", "EmailAddress", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("ABP.Users", "EmailAddress", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("ABP.Users", "Surname", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("ABP.Users", "Name", c => c.String(nullable: false, maxLength: 32));
        }
    }
}
