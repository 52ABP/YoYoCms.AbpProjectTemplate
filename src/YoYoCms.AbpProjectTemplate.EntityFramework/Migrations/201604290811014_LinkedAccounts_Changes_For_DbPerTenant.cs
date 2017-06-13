namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class LinkedAccounts_Changes_For_DbPerTenant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpUserAccounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(),
                        UserId = c.Long(nullable: false),
                        UserLinkId = c.Long(),
                        UserName = c.String(),
                        EmailAddress = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApplicationLanguageText_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserAccount_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            //Copy existing users to UserAccounts
            Sql(@"INSERT INTO AbpUserAccounts(TenantId, UserId, UserName, EmailAddress, UserLinkId, CreationTime, IsDeleted) 
                  SELECT TenantId, Id AS UserId, UserName, EmailAddress, UserLinkId, CreationTime, IsDeleted FROM AbpUsers");

            //Update UserLinkId fields with AbpUserAccount's Id fields
            Sql(@"UPDATE UA SET 
	                UserLinkId = (SELECT UASub.Id FROM AbpUserAccounts UASub WHERE UASub.UserId = UA.UserLinkId) 
                FROM AbpUserAccounts UA 
                WHERE UA.UserLinkId IS NOT NULL");
        }
        
        public override void Down()
        {
            DropTable("dbo.AbpUserAccounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ApplicationLanguageText_MayHaveTenant", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                    { "DynamicFilter_UserAccount_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
