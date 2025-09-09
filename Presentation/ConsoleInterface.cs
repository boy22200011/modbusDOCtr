using System.Text.Json;
using ConsoleApp1.BusinessLogic;
using ConsoleApp1.Models;

namespace ConsoleApp1.Presentation
{
    /// <summary>
    /// 命令列介面服務實作
    /// </summary>
    public class ConsoleInterface : IConsoleInterface
    {
        private readonly IDoControlService _doControlService;

        /// <summary>
        /// 初始化命令列介面
        /// </summary>
        /// <param name="doControlService">DO 控制服務</param>
        public ConsoleInterface(IDoControlService doControlService)
        {
            _doControlService = doControlService;
        }

        /// <summary>
        /// 顯示歡迎訊息
        /// </summary>
        public void ShowWelcomeMessage()
        {
            Console.WriteLine("=== DO 控制台（長連線＋自動重連／支援反相＋設定檔）===");
        }

        /// <summary>
        /// 顯示指令說明
        /// </summary>
        public void ShowCommandHelp()
        {
            Console.WriteLine("指令：");
            Console.WriteLine("  on <1|2>              將指定 DO 置為 ON（套用反相）");
            Console.WriteLine("  off <1|2>             將指定 DO 置為 OFF（套用反相）");
            Console.WriteLine("  pulse <1|2> <ms>      DO 脈衝（毫秒）");
            Console.WriteLine("  inv <1|2>             切換指定 DO 反相（toggle），並自動存檔");
            Console.WriteLine("  map <ch> <coil>       設定 DO→coil 映射（例：map 1 0），並自動存檔");
            Console.WriteLine("  cfg <ip> <port> <uid> 設定連線參數，並自動存檔");
            Console.WriteLine("  status                顯示 8 顆 coil 狀態與映射/反相設定");
            Console.WriteLine("  showcfg               顯示目前設定（JSON）");
            Console.WriteLine("  exit                  離開");
        }

        /// <summary>
        /// 顯示設定摘要
        /// </summary>
        public void ShowConfigSummary()
        {
            var config = _doControlService.GetConfig();
            Console.WriteLine($"IP={config.Ip} Port={config.Port} UnitId={config.UnitId}");
            Console.WriteLine($"映射：DO1→coil{config.Ch2Coil[1]}, DO2→coil{config.Ch2Coil[2]}");
            Console.WriteLine($"反相：DO1={config.Invert[1]}, DO2={config.Invert[2]}");
            Console.WriteLine();
        }

        /// <summary>
        /// 顯示 DO 狀態
        /// </summary>
        public void ShowDoStatus()
        {
            var status = _doControlService.GetDoStatus();
            Console.WriteLine("Coils: " + string.Join(",", status.CoilStates));
            Console.WriteLine($"IP={status.Ip} Port={status.Port} UnitId={status.UnitId}");
            Console.WriteLine($"映射：DO1→coil{status.Channel1Coil}, DO2→coil{status.Channel2Coil}");
            Console.WriteLine($"反相：DO1={status.Channel1Inverted}, DO2={status.Channel2Inverted}");
        }

        /// <summary>
        /// 顯示設定檔內容
        /// </summary>
        public void ShowConfig()
        {
            var config = _doControlService.GetConfig();
            Console.WriteLine(JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// 執行主迴圈
        /// </summary>
        public void RunMainLoop()
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLowerInvariant();

                try
                {
                    if (command == "exit") break;

                    switch (command)
                    {
                        case "cfg":
                            HandleConfigCommand(parts);
                            break;
                        case "map":
                            HandleMapCommand(parts);
                            break;
                        case "on":
                        case "off":
                            HandleOnOffCommand(command, parts);
                            break;
                        case "pulse":
                            HandlePulseCommand(parts);
                            break;
                        case "inv":
                            HandleInvertCommand(parts);
                            break;
                        case "status":
                            ShowDoStatus();
                            break;
                        case "showcfg":
                            ShowConfig();
                            break;
                        default:
                            Console.WriteLine("未知指令");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERROR] " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 處理設定指令
        /// </summary>
        /// <param name="parts">指令參數</param>
        private void HandleConfigCommand(string[] parts)
        {
            var request = new ConnectionConfigRequest();
            if (parts.Length >= 2) request.Ip = parts[1];
            if (parts.Length >= 3 && int.TryParse(parts[2], out var port)) request.Port = port;
            if (parts.Length >= 4 && byte.TryParse(parts[3], out var unitId)) request.UnitId = unitId;

            _doControlService.UpdateConnectionConfig(request);
        }

        /// <summary>
        /// 處理映射指令
        /// </summary>
        /// <param name="parts">指令參數</param>
        private void HandleMapCommand(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("用法：map <ch:1|2> <coil:>=0>");
                return;
            }

            if (!int.TryParse(parts[1], out var channel) || channel < 1 || channel > 2)
            {
                Console.WriteLine("ch 僅支援 1 或 2");
                return;
            }

            if (!int.TryParse(parts[2], out var coil) || coil < 0)
            {
                Console.WriteLine("coil 需 >= 0");
                return;
            }

            var request = new ChannelMappingRequest { Channel = channel, CoilNumber = coil };
            _doControlService.SetChannelMapping(request);
        }

        /// <summary>
        /// 處理開關指令
        /// </summary>
        /// <param name="command">指令名稱</param>
        /// <param name="parts">指令參數</param>
        private void HandleOnOffCommand(string command, string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("用法：on/off <1|2>");
                return;
            }

            if (!int.TryParse(parts[1], out var channel) || channel < 1 || channel > 2)
            {
                Console.WriteLine("通道僅支援 1 或 2");
                return;
            }

            var request = new DoControlRequest
            {
                Channel = channel,
                IsOn = command == "on"
            };

            _doControlService.ControlDo(request);
        }

        /// <summary>
        /// 處理脈衝指令
        /// </summary>
        /// <param name="parts">指令參數</param>
        private void HandlePulseCommand(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("用法：pulse <1|2> <ms>");
                return;
            }

            if (!int.TryParse(parts[1], out var channel) || channel < 1 || channel > 2)
            {
                Console.WriteLine("通道僅支援 1 或 2");
                return;
            }

            if (!int.TryParse(parts[2], out var duration) || duration <= 0)
            {
                Console.WriteLine("ms 需為正整數");
                return;
            }

            var request = new DoPulseRequest
            {
                Channel = channel,
                DurationMs = duration
            };

            _doControlService.PulseDo(request);
        }

        /// <summary>
        /// 處理反相指令
        /// </summary>
        /// <param name="parts">指令參數</param>
        private void HandleInvertCommand(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("用法：inv <1|2>");
                return;
            }

            if (!int.TryParse(parts[1], out var channel) || channel < 1 || channel > 2)
            {
                Console.WriteLine("通道僅支援 1 或 2");
                return;
            }

            _doControlService.ToggleChannelInvert(channel);
        }
    }
}
