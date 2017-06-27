using Abp.Runtime.Session;

namespace YoYoCms.AbpProjectTemplate.AppExtensions.AbpSessions
{
    public interface IAbpSessionExtensions : IAbpSession
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        string EmailAddress { get; }
      
    }
}