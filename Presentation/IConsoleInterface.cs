namespace ConsoleApp1.Presentation
{
    /// <summary>
    /// 命令列介面服務介面
    /// </summary>
    public interface IConsoleInterface
    {
        /// <summary>
        /// 顯示歡迎訊息
        /// </summary>
        void ShowWelcomeMessage();

        /// <summary>
        /// 顯示指令說明
        /// </summary>
        void ShowCommandHelp();

        /// <summary>
        /// 顯示設定摘要
        /// </summary>
        void ShowConfigSummary();

        /// <summary>
        /// 顯示 DO 狀態
        /// </summary>
        void ShowDoStatus();

        /// <summary>
        /// 顯示設定檔內容
        /// </summary>
        void ShowConfig();

        /// <summary>
        /// 執行主迴圈
        /// </summary>
        void RunMainLoop();
    }
}
