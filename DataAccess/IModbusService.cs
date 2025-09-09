using ConsoleApp1.Models;

namespace ConsoleApp1.DataAccess
{
    /// <summary>
    /// Modbus 通訊服務介面
    /// </summary>
    public interface IModbusService
    {
        /// <summary>
        /// 確保連線正常
        /// </summary>
        void EnsureConnected();

        /// <summary>
        /// 重新連線
        /// </summary>
        void Reconnect();

        /// <summary>
        /// 寫入單一 Coil
        /// </summary>
        /// <param name="coilAddress">Coil 位址</param>
        /// <param name="value">要寫入的值</param>
        void WriteSingleCoil(ushort coilAddress, bool value);

        /// <summary>
        /// 讀取多個 Coils
        /// </summary>
        /// <param name="startAddress">起始位址</param>
        /// <param name="numberOfPoints">讀取數量</param>
        /// <returns>Coil 狀態陣列</returns>
        bool[] ReadCoils(ushort startAddress, ushort numberOfPoints);

        /// <summary>
        /// 執行帶重試機制的操作
        /// </summary>
        /// <param name="action">要執行的操作</param>
        void ExecuteWithRetry(Action action);

        /// <summary>
        /// 清理資源
        /// </summary>
        void Cleanup();
    }
}
