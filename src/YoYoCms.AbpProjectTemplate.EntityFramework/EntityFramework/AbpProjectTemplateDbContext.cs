using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using YoYoCms.AbpProjectTemplate.Authorization.Roles;
using YoYoCms.AbpProjectTemplate.EntityMapper.BinaryObjects;
using YoYoCms.AbpProjectTemplate.EntityMapper.SmsMessagelogs;
using YoYoCms.AbpProjectTemplate.EntityMapper.Users;
using YoYoCms.AbpProjectTemplate.MultiTenancy;
 using YoYoCms.AbpProjectTemplate.SmsMessagelogs;
using YoYoCms.AbpProjectTemplate.Storage;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.EntityFramework
{
    /* Constructors of this DbContext is important and each one has it's own use case.
     * - Default constructor is used by EF tooling on design time.
     * - constructor(nameOrConnectionString) is used by ABP on runtime.
     * - constructor(existingConnection) is used by unit tests.
     * - constructor(existingConnection,contextOwnsConnection) can be used by ABP if DbContextEfTransactionStrategy is used.
     * See http://www.aspnetboilerplate.com/Pages/Documents/EntityFramework-Integration for more.
     */

    public class AbpProjectTemplateDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
		/* Define an IDbSet for each entity of the application */
		//定义一个IDbSet为每个应用程序的实体
		//在此处为每个实体，定义为IDbSet
		public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }
      

        public IDbSet<SmsMessagelog> SmsMessagelogs { get; set; }
      


        public AbpProjectTemplateDbContext()
            : base("Default")
        {
            
        }

        public AbpProjectTemplateDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public AbpProjectTemplateDbContext(DbConnection existingConnection)
           : base(existingConnection, false)
        {

        }

        public AbpProjectTemplateDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region 修改ABP默认的架构设置功能
        //    InitialCreate
       modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("", "ABP");
           modelBuilder.Configurations.Add(new BinaryObjectCfg());

         
            //Change_Table_Name
            #endregion

            modelBuilder.Configurations.Add(new UserCfg());
            modelBuilder.Configurations.Add(new SmsMessagelogCfg());

            base.OnModelCreating(modelBuilder);
        }
    }
}
