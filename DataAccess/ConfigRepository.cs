using System.Text.Json;
using ConsoleApp1.Models;

namespace ConsoleApp1.DataAccess
{
    /// <summary>
    /// 設定檔存取實作
    /// </summary>
    public class ConfigRepository : IConfigRepository
    {
        private readonly string _configPath;

        /// <summary>
        /// 初始化設定檔存取器
        /// </summary>
        /// <param name="configPath">設定檔路徑</param>
        public ConfigRepository(string configPath)
        {
            _configPath = configPath;
        }

        /// <summary>
        /// 載入設定檔
        /// </summary>
        /// <returns>應用程式設定</returns>
        public AppConfig LoadConfig()
        {
            try
            {
                if (File.Exists(_configPath))
                {
                    var json = File.ReadAllText(_configPath);
                    var loaded = JsonSerializer.Deserialize<AppConfig>(json);
                    if (loaded != null)
                    {
                        // 保底長度
                        if (loaded.Ch2Coil.Length < 3)
                            loaded.Ch2Coil = new[] { -1, 0, 1 };
                        if (loaded.Invert.Length < 3)
                            loaded.Invert = new[] { false, false, false };

                        return loaded;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] 讀取設定失敗，使用預設。原因: {ex.Message}");
            }

            // 如果讀取失敗，建立預設設定並儲存
            var defaultConfig = new AppConfig();
            SaveConfig(defaultConfig);
            return defaultConfig;
        }

        /// <summary>
        /// 儲存設定檔
        /// </summary>
        /// <param name="config">要儲存的設定</param>
        public void SaveConfig(AppConfig config)
        {
            try
            {
                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_configPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARN] 寫入設定失敗：{ex.Message}");
            }
        }
    }
}
