namespace YoYoCms.AbpProjectTemplate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Create_Indexes_For_Edition_Tables : DbMigration
    {
        public override void Up()
        {
            CreateIndex("AbpEditions", new[] { "Name" });

            CreateIndex("AbpFeatures", new[] { "Discriminator", "TenantId", "Name" });
            CreateIndex("AbpFeatures", new[] { "Discriminator", "EditionId", "Name" });
            CreateIndex("AbpFeatures", new[] { "TenantId", "Name" });
        }

        public override void Down()
        {
            DropIndex("AbpFeatures", new[] { "TenantId", "Name" });
            DropIndex("AbpFeatures", new[] { "Discriminator", "EditionId", "Name" });
            DropIndex("AbpFeatures", new[] { "Discriminator", "TenantId", "Name" });

            DropIndex("AbpEditions", new[] { "Name" });
        }
    }
}
