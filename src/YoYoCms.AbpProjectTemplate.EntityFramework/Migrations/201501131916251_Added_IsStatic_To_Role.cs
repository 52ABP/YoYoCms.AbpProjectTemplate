using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Added_IsStatic_To_Role : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpRoles", "IsStatic", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.AbpRoles", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.AbpRoles", "DisplayName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.AbpUsers", "Name", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.AbpUsers", "Surname", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.AbpUsers", "Password", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpUsers", "EmailAddress", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AbpUsers", "Password", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AbpUsers", "Surname", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AbpUsers", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.AbpRoles", "DisplayName", c => c.String());
            AlterColumn("dbo.AbpRoles", "Name", c => c.String());
            DropColumn("dbo.AbpRoles", "IsStatic");
        }
    }
}
