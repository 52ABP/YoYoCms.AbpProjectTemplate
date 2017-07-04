using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.Users
{
    public class UserCfg: EntityTypeConfiguration<User>
    {
        public UserCfg()
        {
            ToTable("User", AbpProjectTemplateConsts.SchemaName.ABP);
            Ignore(a => a.Name);
            Ignore(a => a.Surname);

            Property(a => a.EmailAddress).IsOptional();

        }
    }
}