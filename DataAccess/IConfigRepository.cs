using ConsoleApp1.Models;

namespace ConsoleApp1.DataAccess
{
    /// <summary>
    /// 設定檔存取介面
    /// </summary>
    public interface IConfigRepository
    {
        /// <summary>
        /// 載入設定檔
        /// </summary>
        /// <returns>應用程式設定</returns>
        AppConfig LoadConfig();

        /// <summary>
        /// 儲存設定檔
        /// </summary>
        /// <param name="config">要儲存的設定</param>
        void SaveConfig(AppConfig config);
    }
}
