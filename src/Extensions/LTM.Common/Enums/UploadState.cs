using System.ComponentModel;

namespace LTM.Common.Enums
{

    /// <summary>
    /// 上传状态
    /// </summary>
    public enum UploadState
    {

        /// <summary>
        /// 上传成功SUCCESS
        /// </summary>
        [Description("上传成功SUCCESS")]
        Success = 0,
        /// <summary>
        /// 文件大小超出服务器限制
        /// </summary>
        [Description("文件大小超出服务器限制")]
        SizeLimitExceed = -1,
        /// <summary>
        /// 不允许的文件格式
        /// </summary>
        [Description("不允许的文件格式")]
        TypeNotAllow = -2,
        /// <summary>
        /// 文件访问出错，请检查写入权限
        /// </summary>
        [Description("文件访问出错，请检查写入权限")]
        FileAccessError = -3,
        /// <summary>
        /// 网络错误
        /// </summary>
        [Description("网络错误")]
        NetworkError = -4,
        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        Unknown = 1,
    }
}