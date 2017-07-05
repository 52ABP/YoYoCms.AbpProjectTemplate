namespace LTM.Common.Web
{
    /// <summary>
    ///     UserAgent辅助操作类
    /// </summary>
    public static class UserAgentHelper
    {
        /// <summary>
        ///     根据 UserAgent 获取操作系统名称
        /// </summary>
        public static string GetOperatingSystemName(string userAgent)
        {
            if (userAgent.Contains("NT 10.0"))
            {
                return "Windows 10";
            }
            if (userAgent.Contains("NT 8.0"))
            {
                return "Windows 8";
            }
            if (userAgent.Contains("NT 6.1"))
            {
                return "Windows 7";
            }
            if (userAgent.Contains("NT 6.0"))
            {
                return "Windows Vista/Server 2008";
            }
            if (userAgent.Contains("NT 5.2"))
            {
                return "Windows Server 2003";
            }
            if (userAgent.Contains("NT 5.1"))
            {
                return "Windows XP";
            }
            if (userAgent.Contains("NT 5"))
            {
                return "Windows 2000";
            }
            if (userAgent.Contains("Mac"))
            {
                return "Mac";
            }
            if (userAgent.Contains("Unix"))
            {
                return "UNIX";
            }
            if (userAgent.Contains("Linux"))
            {
                return userAgent.Contains("Android") ? "Android" : "Linux";
            }
            if (userAgent.Contains("SunOS"))
            {
                return "SunOS";
            }
            return "Other OperatingSystem";
        }

        /// <summary>
        ///     根据 UserAgent 获取浏览器名称
        /// </summary>
        public static string GetBrowserName(string userAgent)
        {
            if (userAgent.Contains("Maxthon"))
            {
                return "遨游浏览器";
            }
            if (userAgent.Contains("MetaSr"))
            {
                return "搜狗高速浏览器";
            }
            if (userAgent.Contains("BIDUBrowser"))
            {
                return "百度浏览器";
            }
            if (userAgent.Contains("QQBrowser"))
            {
                return "QQ浏览器";
            }
            if (userAgent.Contains("GreenBrowser"))
            {
                return "Green浏览器";
            }
            if (userAgent.Contains("360se"))
            {
                return "360安全浏览器";
            }
            if (userAgent.Contains("MSIE 6.0"))
            {
                return "Internet Explorer 6.0";
            }
            if (userAgent.Contains("MSIE 7.0"))
            {
                return "Internet Explorer 7.0";
            }
            if (userAgent.Contains("MSIE 8.0"))
            {
                return "Internet Explorer 8.0";
            }
            if (userAgent.Contains("MSIE 9.0"))
            {
                return "Internet Explorer 9.0";
            }
            if (userAgent.Contains("MSIE 10.0"))
            {
                return "Internet Explorer 10.0";
            }
            if (userAgent.Contains("Trident") && userAgent.Contains("11.0"))
            {
                return "Internet Explorer 11.0";
            }
            if (userAgent.Contains("Edge"))
            {
                return "Internet Explorer Edge";
            }
            if (userAgent.Contains("Firefox"))
            {
                return "Firefox";
            }
            if (userAgent.Contains("Opera"))
            {
                return "Opera";
            }
            if (userAgent.Contains("Chrome"))
            {
                return "Chrome";
            }
            if (userAgent.Contains("Safari"))
            {
                return "Safari";
            }
            return "Other Browser";
        }
    }
}