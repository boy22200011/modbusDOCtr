# Modbus DO Controller (modbusDOCtr)

一個基於 .NET 8 的 Modbus TCP DO (Digital Output) 控制台應用程式，採用三層式架構設計，提供直觀的命令列介面來控制 Modbus 設備的數位輸出。

## 🚀 功能特色

- **Modbus TCP 通訊**: 支援標準 Modbus TCP 協定
- **DO 控制**: 支援數位輸出的開關、脈衝控制
- **通道映射**: 靈活的通道到 Coil 位址映射設定
- **反相控制**: 支援通道反相設定
- **即時狀態**: 即時顯示 DO 狀態和連線資訊
- **設定管理**: JSON 格式的設定檔管理
- **重試機制**: 內建連線重試和錯誤處理
- **三層架構**: 清晰的程式碼結構，易於維護和擴展

## 📋 系統需求

- .NET 8.0 或更高版本
- Windows/Linux/macOS 作業系統
- 網路連線至 Modbus TCP 設備

## 🛠️ 安裝與建置

### 1. 複製專案
```bash
git clone <repository-url>
cd modbusDOCtr
```

### 2. 還原相依性
```bash
dotnet restore
```

### 3. 建置專案
```bash
dotnet build
```

### 4. 執行應用程式
```bash
dotnet run
```

## 📖 使用說明

### 基本指令

| 指令 | 語法 | 說明 |
|------|------|------|
| 開啟 DO | `on <1\|2>` | 開啟指定的 DO 通道 |
| 關閉 DO | `off <1\|2>` | 關閉指定的 DO 通道 |
| DO 脈衝 | `pulse <1\|2> <ms>` | 執行指定時間的脈衝輸出 |
| 切換反相 | `inv <1\|2>` | 切換指定通道的反相設定 |
| 設定映射 | `map <ch> <coil>` | 設定通道到 Coil 位址的映射 |
| 設定連線 | `cfg <ip> <port> <uid>` | 設定 Modbus 連線參數 |
| 顯示狀態 | `status` | 顯示當前 DO 狀態和連線資訊 |
| 顯示設定 | `showcfg` | 顯示當前設定檔內容 |
| 離開程式 | `exit` | 離開應用程式 |

### 使用範例

```bash
# 開啟 DO1
on 1

# 關閉 DO2
off 2

# DO1 執行 500ms 脈衝
pulse 1 500

# 切換 DO1 反相設定
inv 1

# 設定 DO1 對應到 Coil 位址 10
map 1 10

# 設定 Modbus 連線參數
cfg 192.168.1.100 502 1

# 查看狀態
status
```

## ⚙️ 設定檔

應用程式使用 `config.json` 檔案儲存設定，位於執行檔同目錄下：

```json
{
  "Ip": "192.168.1.5",
  "Port": 502,
  "UnitId": 1,
  "Ch2Coil": [-1, 0, 1],
  "Invert": [false, false, false]
}
```

### 設定說明

- **Ip**: Modbus 伺服器 IP 位址
- **Port**: Modbus 伺服器連接埠 (預設: 502)
- **UnitId**: Modbus 單元 ID (預設: 1)
- **Ch2Coil**: 通道到 Coil 位址映射陣列 `[不用, DO1→coil, DO2→coil]`
- **Invert**: 通道反相設定陣列 `[不用, DO1是否反相, DO2是否反相]`

## 🏗️ 專案架構

本專案採用三層式架構設計：

```
ConsoleApp1/
├── Models/                    # 資料模型層
│   ├── AppConfig.cs          # 應用程式設定模型
│   ├── DoControlRequest.cs   # DO 控制請求模型
│   └── DoStatus.cs           # DO 狀態模型
├── DataAccess/               # 資料存取層
│   ├── IConfigRepository.cs  # 設定檔存取介面
│   ├── ConfigRepository.cs   # 設定檔存取實作
│   ├── IModbusService.cs     # Modbus 通訊介面
│   └── ModbusService.cs      # Modbus 通訊實作
├── BusinessLogic/            # 業務邏輯層
│   ├── IDoControlService.cs  # DO 控制服務介面
│   └── DoControlService.cs   # DO 控制服務實作
├── Presentation/             # 表示層
│   ├── IConsoleInterface.cs  # 命令列介面介面
│   └── ConsoleInterface.cs   # 命令列介面實作
└── Program.cs                # 主程式進入點
```

### 架構說明

1. **表示層 (Presentation Layer)**: 處理使用者介面和命令列輸入
2. **業務邏輯層 (Business Logic Layer)**: 實作 DO 控制的核心邏輯
3. **資料存取層 (Data Access Layer)**: 處理 Modbus 通訊和設定檔存取
4. **資料模型層 (Models)**: 定義資料結構和 DTO

## 📦 相依性

- **NModbus4** (2.1.0): Modbus 通訊協定實作
- **System.IO.Ports** (9.0.8): 序列埠通訊支援

## 🔧 開發

### 建置專案
```bash
dotnet build
```

### 執行測試
```bash
dotnet test
```

### 發佈應用程式
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## 🤝 貢獻

歡迎提交 Issue 和 Pull Request 來改善這個專案。

## 📄 授權

本專案採用 MIT 授權條款，詳見 [LICENSE](LICENSE) 檔案。

## 📞 支援

如有問題或建議，請透過以下方式聯繫：

- 提交 Issue
- 發送 Pull Request
- 查看 [ARCHITECTURE.md](ARCHITECTURE.md) 了解詳細架構說明

---

**注意**: 使用前請確保 Modbus 設備的 IP 位址、連接埠和單元 ID 設定正確，並確保網路連線正常。
