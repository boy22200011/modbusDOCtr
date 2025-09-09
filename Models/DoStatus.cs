namespace ConsoleApp1.Models
{
    /// <summary>
    /// DO 狀態資訊模型
    /// </summary>
    public class DoStatus
    {
        /// <summary>
        /// 所有 Coil 狀態陣列
        /// </summary>
        public bool[] CoilStates { get; set; } = Array.Empty<bool>();

        /// <summary>
        /// 通道 1 對應的 Coil 編號
        /// </summary>
        public int Channel1Coil { get; set; }

        /// <summary>
        /// 通道 2 對應的 Coil 編號
        /// </summary>
        public int Channel2Coil { get; set; }

        /// <summary>
        /// 通道 1 是否反相
        /// </summary>
        public bool Channel1Inverted { get; set; }

        /// <summary>
        /// 通道 2 是否反相
        /// </summary>
        public bool Channel2Inverted { get; set; }

        /// <summary>
        /// 連線 IP 位址
        /// </summary>
        public string Ip { get; set; } = string.Empty;

        /// <summary>
        /// 連線連接埠
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 單元 ID
        /// </summary>
        public byte UnitId { get; set; }
    }
}
