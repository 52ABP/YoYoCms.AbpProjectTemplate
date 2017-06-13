using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Added_StoredFile_Entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoredFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(nullable: false),
                        FileType = c.String(),
                        Bytes = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AbpUsers", "ProfilePictureId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "ProfilePictureId");
            DropTable("dbo.StoredFiles");
        }
    }
}
