using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class UserLogin_Property_Restrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AbpUserLogins", "LoginProvider", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AbpUserLogins", "ProviderKey", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpUserLogins", "ProviderKey", c => c.String());
            AlterColumn("dbo.AbpUserLogins", "LoginProvider", c => c.String());
        }
    }
}
