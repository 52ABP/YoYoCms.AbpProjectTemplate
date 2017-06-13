using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Tenant_Changed_To_FullAuditedEntity_From_AuditedEntity : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.AbpTenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenancyName = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Abp_SoftDelete",
                        new AnnotationValues(oldValue: null, newValue: "True")
                    },
                });
            
            AddColumn("dbo.AbpTenants", "IsDeleted", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AbpTenants", "DeleterUserId", c => c.Long());
            AddColumn("dbo.AbpTenants", "DeletionTime", c => c.DateTime());
            CreateIndex("dbo.AbpTenants", "DeleterUserId");
            AddForeignKey("dbo.AbpTenants", "DeleterUserId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AbpTenants", "DeleterUserId", "dbo.AbpUsers");
            DropIndex("dbo.AbpTenants", new[] { "DeleterUserId" });
            DropColumn("dbo.AbpTenants", "DeletionTime");
            DropColumn("dbo.AbpTenants", "DeleterUserId");
            DropColumn("dbo.AbpTenants", "IsDeleted");
            AlterTableAnnotations(
                "dbo.AbpTenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenancyName = c.String(nullable: false, maxLength: 64),
                        Name = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Abp_SoftDelete",
                        new AnnotationValues(oldValue: "True", newValue: null)
                    },
                });
            
        }
    }
}
