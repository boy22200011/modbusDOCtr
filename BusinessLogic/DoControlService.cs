using ConsoleApp1.DataAccess;
using ConsoleApp1.Models;

namespace ConsoleApp1.BusinessLogic
{
    /// <summary>
    /// DO 控制服務實作
    /// </summary>
    public class DoControlService : IDoControlService
    {
        private readonly IConfigRepository _configRepository;
        private readonly IModbusService _modbusService;
        private AppConfig _config;

        /// <summary>
        /// 初始化 DO 控制服務
        /// </summary>
        /// <param name="configRepository">設定檔存取器</param>
        /// <param name="modbusService">Modbus 服務</param>
        public DoControlService(IConfigRepository configRepository, IModbusService modbusService)
        {
            _configRepository = configRepository;
            _modbusService = modbusService;
            _config = _configRepository.LoadConfig();
        }

        /// <summary>
        /// 控制 DO 開關
        /// </summary>
        /// <param name="request">DO 控制請求</param>
        public void ControlDo(DoControlRequest request)
        {
            ValidateChannel(request.Channel);
            int coil = ResolveCoil(request.Channel);
            bool toWrite = _config.Invert[request.Channel] ? !request.IsOn : request.IsOn;

            _modbusService.ExecuteWithRetry(() =>
            {
                _modbusService.WriteSingleCoil((ushort)coil, toWrite);
                Thread.Sleep(50);
                var states = _modbusService.ReadCoils(0, 8);
                Console.WriteLine($"DO{request.Channel} 要求={(request.IsOn ? "ON" : "OFF")} 實寫={(toWrite ? "True" : "False")}  狀態: {string.Join(",", states)}");
            });
        }

        /// <summary>
        /// 執行 DO 脈衝
        /// </summary>
        /// <param name="request">DO 脈衝請求</param>
        public void PulseDo(DoPulseRequest request)
        {
            ValidateChannel(request.Channel);
            int coil = ResolveCoil(request.Channel);
            bool onWrite = _config.Invert[request.Channel] ? false : true;  // 邏輯 ON 時要寫的值
            bool offWrite = !onWrite;

            _modbusService.ExecuteWithRetry(() =>
            {
                _modbusService.WriteSingleCoil((ushort)coil, onWrite);
                Thread.Sleep(request.DurationMs);
                _modbusService.WriteSingleCoil((ushort)coil, offWrite);
                Thread.Sleep(50);
                var states = _modbusService.ReadCoils(0, 8);
                Console.WriteLine($"DO{request.Channel} pulse {request.DurationMs}ms 完成  狀態: {string.Join(",", states)}");
            });
        }

        /// <summary>
        /// 切換通道反相設定
        /// </summary>
        /// <param name="channel">通道編號</param>
        public void ToggleChannelInvert(int channel)
        {
            ValidateChannel(channel);
            _config.Invert[channel] = !_config.Invert[channel];
            _configRepository.SaveConfig(_config);
            Console.WriteLine($"DO{channel} 反相 = {_config.Invert[channel]}");
        }

        /// <summary>
        /// 設定通道映射
        /// </summary>
        /// <param name="request">通道映射請求</param>
        public void SetChannelMapping(ChannelMappingRequest request)
        {
            ValidateChannel(request.Channel);
            if (request.CoilNumber < 0)
                throw new ArgumentException("coil 需 >= 0");

            _config.Ch2Coil[request.Channel] = request.CoilNumber;
            _configRepository.SaveConfig(_config);
            Console.WriteLine($"映射更新：DO{request.Channel}→coil{request.CoilNumber}");
        }

        /// <summary>
        /// 更新連線設定
        /// </summary>
        /// <param name="request">連線設定請求</param>
        public void UpdateConnectionConfig(ConnectionConfigRequest request)
        {
            if (!string.IsNullOrEmpty(request.Ip))
                _config.Ip = request.Ip;
            if (request.Port.HasValue)
                _config.Port = request.Port.Value;
            if (request.UnitId.HasValue)
                _config.UnitId = request.UnitId.Value;

            _configRepository.SaveConfig(_config);
            Console.WriteLine($"已設定：ip={_config.Ip} port={_config.Port} unitId={_config.UnitId}");
            _modbusService.Reconnect();
        }

        /// <summary>
        /// 取得 DO 狀態
        /// </summary>
        /// <returns>DO 狀態資訊</returns>
        public DoStatus GetDoStatus()
        {
            bool[] coilStates = Array.Empty<bool>();

            _modbusService.ExecuteWithRetry(() =>
            {
                coilStates = _modbusService.ReadCoils(0, 8);
            });

            return new DoStatus
            {
                CoilStates = coilStates,
                Channel1Coil = _config.Ch2Coil[1],
                Channel2Coil = _config.Ch2Coil[2],
                Channel1Inverted = _config.Invert[1],
                Channel2Inverted = _config.Invert[2],
                Ip = _config.Ip,
                Port = _config.Port,
                UnitId = _config.UnitId
            };
        }

        /// <summary>
        /// 取得應用程式設定
        /// </summary>
        /// <returns>應用程式設定</returns>
        public AppConfig GetConfig()
        {
            return _config;
        }

        /// <summary>
        /// 驗證通道編號
        /// </summary>
        /// <param name="channel">通道編號</param>
        private static void ValidateChannel(int channel)
        {
            if (channel < 1 || channel > 2)
                throw new ArgumentException("通道僅支援 1 或 2");
        }

        /// <summary>
        /// 解析通道對應的 Coil 編號
        /// </summary>
        /// <param name="channel">通道編號</param>
        /// <returns>Coil 編號</returns>
        private int ResolveCoil(int channel)
        {
            int coil = _config.Ch2Coil[channel];
            if (coil < 0)
                throw new InvalidOperationException($"DO{channel} 尚未設定對應的 coil");
            return coil;
        }
    }
}
