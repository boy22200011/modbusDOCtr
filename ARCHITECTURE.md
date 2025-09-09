# DO 控制台 - 三層式架構說明

## 專案結構

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

## 三層式架構說明

### 1. 表示層 (Presentation Layer)
- **職責**: 處理使用者介面和輸入輸出
- **檔案**: `Presentation/` 資料夾
- **主要類別**: `ConsoleInterface`
- **功能**:
  - 顯示歡迎訊息和指令說明
  - 處理命令列輸入
  - 格式化輸出結果
  - 指令解析和分派

### 2. 業務邏輯層 (Business Logic Layer)
- **職責**: 實作核心業務邏輯和規則
- **檔案**: `BusinessLogic/` 資料夾
- **主要類別**: `DoControlService`
- **功能**:
  - DO 開關控制邏輯
  - DO 脈衝控制邏輯
  - 通道映射管理
  - 反相設定管理
  - 連線設定管理
  - 狀態查詢邏輯

### 3. 資料存取層 (Data Access Layer)
- **職責**: 處理資料存取和外部服務通訊
- **檔案**: `DataAccess/` 資料夾
- **主要類別**: `ConfigRepository`, `ModbusService`
- **功能**:
  - 設定檔讀寫
  - Modbus TCP 通訊
  - 連線管理
  - 重試機制

### 4. 資料模型層 (Models)
- **職責**: 定義資料結構和 DTO
- **檔案**: `Models/` 資料夾
- **主要類別**: `AppConfig`, `DoControlRequest`, `DoStatus`
- **功能**:
  - 應用程式設定模型
  - 請求/回應模型
  - 狀態模型

## 依賴關係

```
Program.cs
    ↓
Presentation Layer (ConsoleInterface)
    ↓
Business Logic Layer (DoControlService)
    ↓
Data Access Layer (ConfigRepository, ModbusService)
    ↓
Models (AppConfig, DoControlRequest, DoStatus)
```

## 優點

1. **關注點分離**: 每層都有明確的職責
2. **可維護性**: 程式碼結構清晰，易於維護
3. **可測試性**: 每層都可以獨立測試
4. **可擴展性**: 容易新增功能或修改現有功能
5. **重用性**: 各層可以獨立重用

## 使用方式

程式使用方式與原本完全相同，所有指令都保持不變：

- `on <1|2>` - 開啟 DO
- `off <1|2>` - 關閉 DO
- `pulse <1|2> <ms>` - DO 脈衝
- `inv <1|2>` - 切換反相
- `map <ch> <coil>` - 設定映射
- `cfg <ip> <port> <uid>` - 設定連線
- `status` - 顯示狀態
- `showcfg` - 顯示設定
- `exit` - 離開

## 編譯和執行

```bash
dotnet build
dotnet run
```
