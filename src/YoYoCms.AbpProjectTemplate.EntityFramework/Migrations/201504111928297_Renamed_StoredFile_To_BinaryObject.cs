using System.Data.Entity.Migrations;

namespace YoYoCms.AbpProjectTemplate.Migrations
{
    public partial class Renamed_StoredFile_To_BinaryObject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbpBinaryObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Bytes = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.StoredFiles");
        }
        
        public override void Down()
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
            
            DropTable("dbo.AbpBinaryObjects");
        }
    }
}
