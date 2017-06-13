namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_IsActive_From_ApplicationLanguage : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AbpLanguages", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AbpLanguages", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
