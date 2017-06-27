namespace YoYoCms.AbpProjectTemplate
{
    /// <summary>
    /// Some general constants for the application.
    /// </summary>
    public static class AbpProjectTemplateConsts
    {
        public const string LocalizationSourceName = "AbpProjectTemplate";

        /// <summary>
        /// 表架构信息
        /// </summary>
        public static class SchemaName
        {
            public const string ABP = "ABP";
        }
        /// <summary>
        /// 身份请求类型
        /// </summary>
        public static class ClaimTypes
        {
            public const string PhoneNumber = "PhoneNumber";

             public const string FullName = "FullName";
            public const string SurName = "SurName";
            public const string UserName = "Name";
            
            public const string CompanyName = "CompanyName";
            public const string LinkMan = "LinkMan";
            public const string QQ = "QQ";
        }
    }
}