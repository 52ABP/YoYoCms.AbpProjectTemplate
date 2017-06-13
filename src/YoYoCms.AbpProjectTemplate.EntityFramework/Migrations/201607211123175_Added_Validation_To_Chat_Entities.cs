namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Validation_To_Chat_Entities : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppChatMessages", "Message", c => c.String(nullable: false));
            AlterColumn("dbo.AppFriendships", "FriendUserName", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppFriendships", "FriendUserName", c => c.String());
            AlterColumn("dbo.AppChatMessages", "Message", c => c.String());
        }
    }
}
