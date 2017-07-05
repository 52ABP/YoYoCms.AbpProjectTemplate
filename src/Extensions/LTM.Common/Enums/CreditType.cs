using System.ComponentModel;

namespace LTM.Common.Enums
{
    /// <summary>
    /// 信用类型枚举
    /// </summary>
    public enum CreditType
    {
        /// <summary>
        /// 注册+10
        /// </summary>
        [Description("注册+10")]
        Register = 0,
        /// <summary>
        /// 完善资料成为供应商+10
        /// </summary>
        [Description("完善资料成为供应商+10")]
        PerfectInfo = 1,
        /// <summary>
        /// 询价响应+2
        /// </summary>
        [Description("询价响应+2")]
        Enquiry = 2,
        /// <summary>
        /// 询价响应不及时-3
        /// </summary>
        [Description("询价响应不及时-3")]
        EnquiryUnseasonal = 3,
        /// <summary>
        /// 询价价格偏离-3
        /// </summary>
        [Description("询价价格偏离-3")]
        EnquiryPriceDeviation = 4,
        /// <summary>
        /// 假冒伪劣产品-5
        /// </summary>
        [Description("假冒伪劣产品-5")]
        Fake = 5,
        /// <summary>
        /// 确认收货+0.5
        /// </summary>
        [Description("确认收货+0.5")]
        ConfirmReceipt = 6,
        /// <summary>
        /// 交易无法兑现-5
        /// </summary>
        [Description("交易无法兑现-5")]
        TransactionAnomaly = 7,
        /// <summary>
        /// 无法按照约定时间交货-3
        /// </summary>
        [Description("无法按照约定时间交货-3")]
        ReceivingTimeout = 8,
        /// <summary>
        /// 在约定时间内交货+0.5
        /// </summary>
        [Description("在约定时间内交货+0.5")]
        ReceivingOntime = 9,
        /// <summary>
        /// 投诉-2
        /// </summary>
        [Description("投诉-2")]
        Complaint = 10,
        /// <summary>
        /// 无投诉+0.5
        /// </summary>
        [Description("无投诉+0.5")]
        NoComplaint = 11,
        /// <summary>
        /// 过期资质被举报或查处-1
        /// </summary>
        [Description("过期资质被举报或查处-1")]
        OverdueQualification = 12,
        /// <summary>
        /// 拒绝整改-10
        /// </summary>
        [Description("拒绝整改-10")]
        RefuseRectification = 13,
        /// <summary>
        /// 扣分后依旧拒绝整改 直接按照分数调至差评
        /// </summary>
        [Description("扣分后依旧拒绝整改 直接按照分数调至差评")]
        StillRefuseRectification = 14,
        /// <summary>
        /// 证书+2
        /// </summary>
        [Description("证书+2")]
        Certificate = 15,
        /// <summary>
        /// 知名企业或著名商标所有者+2
        /// </summary>
        [Description("知名企业或著名商标所有者+2")]
        FamousEnterpriseOrTrademarkOwner = 16,
        /// <summary>
        /// 持有国家或国际质量、环境、安全管理体系认证证书+1
        /// </summary>
        [Description("持有国家或国际质量、环境、安全管理体系认证证书+1")]
        litteraeCredentiales = 17,
        /// <summary>
        /// 能适应并快速响应平台规则或管理变更+1
        /// </summary>
        [Description("能适应并快速响应平台规则或管理变更+1")]
        AdaptQuickly = 18,
        /// <summary>
        /// 不能适应并快速响应平台规则或管理变更-2
        /// </summary>
        [Description("不能适应并快速响应平台规则或管理变更-2")]
        AdaptSlow = 19,
        /// <summary>
        /// 服务水平高+0.5
        /// </summary>
        [Description("服务水平高+0.5")]
        GoodService = 20,
        /// <summary>
        /// 保证金达到5万+3
        /// </summary>
        [Description("保证金达到5万+3")]
        BailOver50Thousand = 21,
        /// <summary>
        /// 保证金达到10万+5
        /// </summary>
        [Description("保证金达到10万+5")]
        BailOver100Thousand = 22,
        /// <summary>
        /// 保证金达到50万+8
        /// </summary>
        [Description("保证金达到50万+8")]
        BailOver500Thousand = 23,
        /// <summary>
        /// 保证金达到100万+10
        /// </summary>
        [Description("保证金达到100万+10")]
        BailOver1Million = 24,
        /// <summary>
        /// 预留款达到5万+3
        /// </summary>
        [Description("预留款达到5万+3")]
        ReserveMoneyOver50Thousand = 25,
        /// <summary>
        /// 预留款达到10万+5
        /// </summary>
        [Description("预留款达到10万+5")]
        ReserveMoneyOver100Thousand = 26,
        /// <summary>
        /// 预留款达到50万+8
        /// </summary>
        [Description("预留款达到50万+8")]
        ReserveMoneyOver500Thousand = 27,
        /// <summary>
        /// 预留款达到100万+10
        /// </summary>
        [Description("预留款达到100万+10")]
        ReserveMoneyOver1Million = 28,
        /// <summary>
        /// 签订协议的方式将物资在平台指定仓库存储+6
        /// </summary>
        [Description("签订协议的方式将物资在平台指定仓库存储+6")]
        SignAndDesignatedWarehouse = 29,
        /// <summary>
        /// 签订协议的方式将物资配送运输业务交予积微运网+6
        /// </summary>
        [Description("签订协议的方式将物资配送运输业务交予积微运网+6")]
        SignAndGiveJwell = 30
    }
}
