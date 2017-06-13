namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Make_FriendShip_And_Chat_TenantId_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppChatMessages", "TenantId", c => c.Int());
            AlterColumn("dbo.AppChatMessages", "TargetTenantId", c => c.Int());
            AlterColumn("dbo.AppFriendships", "TenantId", c => c.Int());
            AlterColumn("dbo.AppFriendships", "FriendUserId", c => c.Long(nullable: false));
            AlterColumn("dbo.AppFriendships", "FriendTenantId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppFriendships", "FriendTenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.AppFriendships", "FriendUserId", c => c.Int(nullable: false));
            AlterColumn("dbo.AppFriendships", "TenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.AppChatMessages", "TargetTenantId", c => c.Int(nullable: false));
            AlterColumn("dbo.AppChatMessages", "TenantId", c => c.Int(nullable: false));
        }
    }
}
