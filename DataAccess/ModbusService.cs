using System.Net.Sockets;
using Modbus.Device;
using ConsoleApp1.Models;

namespace ConsoleApp1.DataAccess
{
    /// <summary>
    /// Modbus 通訊服務實作
    /// </summary>
    public class ModbusService : IModbusService
    {
        private readonly AppConfig _config;
        private TcpClient? _client;
        private IModbusMaster? _master;
        private readonly object _lock = new object();

        /// <summary>
        /// 初始化 Modbus 服務
        /// </summary>
        /// <param name="config">應用程式設定</param>
        public ModbusService(AppConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 確保連線正常
        /// </summary>
        public void EnsureConnected()
        {
            lock (_lock)
            {
                if (_client != null && _client.Connected) return;

                Cleanup();
                _client = new TcpClient();
                _client.NoDelay = true;
                _client.ReceiveTimeout = 3000;
                _client.SendTimeout = 3000;
                _client.Connect(_config.Ip, _config.Port);

                _master = ModbusIpMaster.CreateIp(_client);
                _master.Transport.ReadTimeout = 3000;
                _master.Transport.WriteTimeout = 3000;

                Thread.Sleep(100); // 某些盒子剛連上需要一點緩衝
            }
        }

        /// <summary>
        /// 重新連線
        /// </summary>
        public void Reconnect()
        {
            Cleanup();
            EnsureConnected();
        }

        /// <summary>
        /// 寫入單一 Coil
        /// </summary>
        /// <param name="coilAddress">Coil 位址</param>
        /// <param name="value">要寫入的值</param>
        public void WriteSingleCoil(ushort coilAddress, bool value)
        {
            lock (_lock)
            {
                _master!.WriteSingleCoil(_config.UnitId, coilAddress, value);
            }
        }

        /// <summary>
        /// 讀取多個 Coils
        /// </summary>
        /// <param name="startAddress">起始位址</param>
        /// <param name="numberOfPoints">讀取數量</param>
        /// <returns>Coil 狀態陣列</returns>
        public bool[] ReadCoils(ushort startAddress, ushort numberOfPoints)
        {
            lock (_lock)
            {
                return _master!.ReadCoils(_config.UnitId, startAddress, numberOfPoints);
            }
        }

        /// <summary>
        /// 執行帶重試機制的操作
        /// </summary>
        /// <param name="action">要執行的操作</param>
        public void ExecuteWithRetry(Action action)
        {
            int tries = 0;
            while (true)
            {
                try
                {
                    EnsureConnected();
                    action();
                    return;
                }
                catch (SocketException)
                {
                    if (++tries >= 3) throw;
                    Thread.Sleep(200);
                    Reconnect();
                }
                catch (IOException)
                {
                    if (++tries >= 3) throw;
                    Thread.Sleep(200);
                    Reconnect();
                }
            }
        }

        /// <summary>
        /// 清理資源
        /// </summary>
        public void Cleanup()
        {
            try { _master = null; } catch { }
            try { _client?.Close(); } catch { }
            _client = null;
        }
    }
}
