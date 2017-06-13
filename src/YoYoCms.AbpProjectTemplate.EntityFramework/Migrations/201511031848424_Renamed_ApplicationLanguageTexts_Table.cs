namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renamed_ApplicationLanguageTexts_Table : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationLanguageTexts", newName: "AbpLanguageTexts");
            AlterColumn("dbo.AbpLanguageTexts", "Key", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpLanguageTexts", "Key", c => c.String(nullable: false, maxLength: 128));
            RenameTable(name: "dbo.AbpLanguageTexts", newName: "ApplicationLanguageTexts");
        }
    }
}
