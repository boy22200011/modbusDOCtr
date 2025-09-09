// dotnet add package NModbus4
// .NET 6+ 建置即可
using ConsoleApp1.DataAccess;
using ConsoleApp1.BusinessLogic;
using ConsoleApp1.Presentation;

namespace ConsoleApp1
{
	/// <summary>
	/// 主程式類別
	/// </summary>
	class Program
	{
		/// <summary>
		/// 主程式進入點
		/// </summary>
		static void Main()
		{
			// 設定檔路徑
			var configPath = Path.Combine(AppContext.BaseDirectory, "config.json");

			// 建立依賴注入容器（簡化版）
			var configRepository = new ConfigRepository(configPath);
			var config = configRepository.LoadConfig();
			var modbusService = new ModbusService(config);
			var doControlService = new DoControlService(configRepository, modbusService);
			var consoleInterface = new ConsoleInterface(doControlService);

			try
			{
				// 顯示歡迎訊息和指令說明
				consoleInterface.ShowWelcomeMessage();
				consoleInterface.ShowCommandHelp();
				Console.WriteLine($"設定檔：{configPath}");
				consoleInterface.ShowConfigSummary();

				// 確保連線
				modbusService.EnsureConnected();

				// 執行主迴圈
				consoleInterface.RunMainLoop();
			}
			finally
			{
				// 清理資源
				modbusService.Cleanup();
			}
		}
	}
}
