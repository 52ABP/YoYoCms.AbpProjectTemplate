using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class User_AndAuditLog_Data_Size_Changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AbpAuditLogs", "Exception", c => c.String(maxLength: 2000));
            AlterColumn("dbo.AbpUsers", "EmailConfirmationCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.AbpUsers", "PasswordResetCode", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AbpUsers", "PasswordResetCode", c => c.String(maxLength: 32));
            AlterColumn("dbo.AbpUsers", "EmailConfirmationCode", c => c.String(maxLength: 16));
            AlterColumn("dbo.AbpAuditLogs", "Exception", c => c.String(maxLength: 2048));
        }
    }
}
