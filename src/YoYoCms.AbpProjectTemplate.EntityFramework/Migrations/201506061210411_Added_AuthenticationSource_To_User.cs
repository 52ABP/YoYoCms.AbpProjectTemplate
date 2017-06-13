namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AuthenticationSource_To_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "AuthenticationSource", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "AuthenticationSource");
        }
    }
}
