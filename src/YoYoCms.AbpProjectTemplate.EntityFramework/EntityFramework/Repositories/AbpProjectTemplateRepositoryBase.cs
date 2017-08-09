using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions;

namespace YoYoCms.AbpProjectTemplate.EntityFramework.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class AbpProjectTemplateRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpProjectTemplateDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AbpProjectTemplateRepositoryBase(IDbContextProvider<AbpProjectTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="AbpProjectTemplateRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class AbpProjectTemplateRepositoryBase<TEntity> : AbpProjectTemplateRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected AbpProjectTemplateRepositoryBase(IDbContextProvider<AbpProjectTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
        public new IAbpSessionExtensions AbpSession { get; set; }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
