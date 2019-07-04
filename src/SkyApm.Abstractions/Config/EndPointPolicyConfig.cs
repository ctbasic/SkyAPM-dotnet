namespace SkyApm.Config
{
    /// <summary>
    /// 自定义配置
    /// </summary>
    [Config("SkyWalking", "Custom", "EndPointPolicy")]
    public class EndPointPolicyConfig
    {
        /// <summary>
        /// 终端白名单策略,只有符合策略的才会创建链路信息
        /// </summary>
        public string Policies { get; set; }

        /// <summary>
        /// 启用白名单策略
        /// </summary>
        public bool Enable { get; set; }
    }
}
