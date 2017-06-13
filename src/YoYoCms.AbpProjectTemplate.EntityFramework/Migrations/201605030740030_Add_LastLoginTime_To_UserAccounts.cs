namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_LastLoginTime_To_UserAccounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUserAccounts", "LastLoginTime", c => c.DateTime());

            //Update LastLoginTimes
            Sql(@"UPDATE UA 
	                SET LastLoginTime = (SELECT U.LastLoginTime FROM AbpUsers U WHERE U.Id = UA.UserId AND U.TenantId = UA.TenantId) 
                FROM AbpUserAccounts UA");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUserAccounts", "LastLoginTime");
        }
    }
}
