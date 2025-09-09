using System.Text.Json;

namespace ConsoleApp1.Models
{
    /// <summary>
    /// 應用程式設定檔模型
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Modbus 伺服器 IP 位址
        /// </summary>
        public string Ip { get; set; } = "192.168.1.5";

        /// <summary>
        /// Modbus 伺服器連接埠
        /// </summary>
        public int Port { get; set; } = 502;

        /// <summary>
        /// Modbus 單元 ID
        /// </summary>
        public byte UnitId { get; set; } = 1;

        /// <summary>
        /// 通道對應 Coil 映射陣列 [不用, DO1→coil, DO2→coil]
        /// </summary>
        public int[] Ch2Coil { get; set; } = new[] { -1, 0, 1 };

        /// <summary>
        /// 通道反相設定陣列 [不用, DO1是否反相, DO2是否反相]
        /// </summary>
        public bool[] Invert { get; set; } = new[] { false, false, false };
    }
}
