namespace YoYoCms.AbpProjectTemplate.WebAppApi.Api.Models
{

    /// <summary>
    /// 认证结果信息
    /// </summary>
    public class AuthenticateResultModel
    {
        /// <summary>
        /// token值
        /// </summary>
        public string AccessToken { get; set; }
         
        /// <summary>
        /// 剩余秒数
        /// </summary>
        public int ExpireInSeconds { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
    }
}
