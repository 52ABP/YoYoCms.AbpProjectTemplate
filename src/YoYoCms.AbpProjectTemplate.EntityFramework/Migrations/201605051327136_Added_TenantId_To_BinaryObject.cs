namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Added_TenantId_To_BinaryObject : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.AppBinaryObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Int(),
                        Bytes = c.Binary(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_BinaryObject_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            AddColumn("dbo.AppBinaryObjects", "TenantId", c => c.Int());

            Sql(@"UPDATE AppBinaryObjects
SET TenantId = AbpUsers.TenantId
FROM AbpUsers
WHERE AppBinaryObjects.Id = AbpUsers.ProfilePictureId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppBinaryObjects", "TenantId");
            AlterTableAnnotations(
                "dbo.AppBinaryObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Int(),
                        Bytes = c.Binary(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_BinaryObject_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
        }
    }
}
