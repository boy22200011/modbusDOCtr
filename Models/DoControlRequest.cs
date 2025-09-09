namespace ConsoleApp1.Models
{
    /// <summary>
    /// DO 控制請求模型
    /// </summary>
    public class DoControlRequest
    {
        /// <summary>
        /// 通道編號 (1 或 2)
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 是否開啟
        /// </summary>
        public bool IsOn { get; set; }
    }

    /// <summary>
    /// DO 脈衝請求模型
    /// </summary>
    public class DoPulseRequest
    {
        /// <summary>
        /// 通道編號 (1 或 2)
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 脈衝持續時間（毫秒）
        /// </summary>
        public int DurationMs { get; set; }
    }

    /// <summary>
    /// 通道映射請求模型
    /// </summary>
    public class ChannelMappingRequest
    {
        /// <summary>
        /// 通道編號 (1 或 2)
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 對應的 Coil 編號
        /// </summary>
        public int CoilNumber { get; set; }
    }

    /// <summary>
    /// 連線設定請求模型
    /// </summary>
    public class ConnectionConfigRequest
    {
        /// <summary>
        /// IP 位址
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// 連接埠
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// 單元 ID
        /// </summary>
        public byte? UnitId { get; set; }
    }
}
