using ConsoleApp1.Models;

namespace ConsoleApp1.BusinessLogic
{
    /// <summary>
    /// DO 控制服務介面
    /// </summary>
    public interface IDoControlService
    {
        /// <summary>
        /// 控制 DO 開關
        /// </summary>
        /// <param name="request">DO 控制請求</param>
        void ControlDo(DoControlRequest request);

        /// <summary>
        /// 執行 DO 脈衝
        /// </summary>
        /// <param name="request">DO 脈衝請求</param>
        void PulseDo(DoPulseRequest request);

        /// <summary>
        /// 切換通道反相設定
        /// </summary>
        /// <param name="channel">通道編號</param>
        void ToggleChannelInvert(int channel);

        /// <summary>
        /// 設定通道映射
        /// </summary>
        /// <param name="request">通道映射請求</param>
        void SetChannelMapping(ChannelMappingRequest request);

        /// <summary>
        /// 更新連線設定
        /// </summary>
        /// <param name="request">連線設定請求</param>
        void UpdateConnectionConfig(ConnectionConfigRequest request);

        /// <summary>
        /// 取得 DO 狀態
        /// </summary>
        /// <returns>DO 狀態資訊</returns>
        DoStatus GetDoStatus();

        /// <summary>
        /// 取得應用程式設定
        /// </summary>
        /// <returns>應用程式設定</returns>
        AppConfig GetConfig();
    }
}
