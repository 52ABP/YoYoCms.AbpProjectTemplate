using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.Users
{
    public class UserCfg: EntityTypeConfiguration<User>
    {
        public UserCfg()
        {
            ToTable("Users", AbpProjectTemplateConsts.SchemaName.ABP);
           
            Property(a => a.Name).IsOptional();
            Property(a => a.Surname).IsOptional();
            Property(a => a.EmailAddress).IsOptional();

        }
    }
}