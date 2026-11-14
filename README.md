# LINE 健康資訊問答聊天機器人

![專題畫面](https://user-images.githubusercontent.com/24285484/159651393-6b20b55b-911a-4be2-b765-8bf2e63b4a5b.png)

這是以 C# 與 ASP.NET Web API 開發的 LINE 聊天機器人，大學專題作品。透過 LINE Messaging API 接收使用者訊息，提供互動式功能選單，並整合 Azure QnA Maker 回答健康資訊相關問題。

專案也包含獨立的 LUIS 意圖辨識測試 Controller，呈現將自然語言理解服務接入聊天機器人的實作探索。

## 主要功能

- 透過 LINE Webhook 接收訊息並回覆使用者。
- 使用 LINE Buttons Template 建立主選單及分層選項。
- 提供食物熱量查詢提示與熱量相關問答入口。
- 提供疾病／症狀相關問題的問答入口，透過 Azure QnA Maker 取得答案。
- 提供毒品與藥物知識選單，依使用者選擇回覆相應內容。
- 另有 LUIS 測試 Controller，使用 Microsoft Cognitive LUIS 分析使用者輸入的意圖與 Entity。
- 程式中保留基礎代謝率／BMI 資料蒐集計算的早期程式草稿，但目前該區段已註解，不能視為主流程已完成的功能。

## 技術組成

| 技術 | 用途 |
|---|---|
| C# | 後端主要開發語言 |
| ASP.NET Web API | 建立 LINE Webhook HTTP API |
| LINE Messaging API / LINE Bot SDK | 接收訊息、回覆文字及互動式訊息模板 |
| Azure QnA Maker | 依使用者問題查詢知識庫並回傳答案 |
| Microsoft Cognitive LUIS | 自然語言意圖與 Entity 辨識測試 |
| .NET Framework 4.5.2 | 專案目標執行框架 |
| Visual Studio | 專案開發環境 |

## 訊息處理流程

1. 使用者在 LINE 傳送文字訊息。
2. LINE 將事件送至 ASP.NET Web API 的 Webhook endpoint。
3. Controller 解析收到的 LINE Event。
4. 依訊息內容判斷使用者選擇的功能或問答類型。
5. 需要知識庫回答時，呼叫 Azure QnA Maker，依答案分數選取回覆內容。
6. 透過 LINE Bot SDK 將文字或 Template Message 回覆給使用者。

## 專案結構

| 檔案 | 說明 |
|---|---|
| `Controllers/Main_V1Controller.cs` | 主要 LINE Webhook、選單互動及 QnA Maker 問答流程 |
| `Controllers/TestQnAController.cs` | 獨立的 QnA Maker 測試 Controller |
| `Controllers/TestLUISController.cs` | LUIS 意圖與 Entity 辨識測試 Controller |
| `App_Start/WebApiConfig.cs` | ASP.NET Web API 路由設定 |
| `ShuMIS25th_V3.csproj` | C# Web 專案與相依套件設定 |

## API 與執行狀態

- 此專題使用的 LINE 與 Azure 服務已停止，相關 API 不再提供服務。
- 原始程式中的憑證已改以 `APIKey` 作為遮蔽標記；這些值是文件化佔位符，不是可用的 API 金鑰。
- 主要 Controller 中的 LINE Channel Access Token、Channel Secret 與 Azure QnA Maker Key 已以佔位符取代。
- 目前不應將此專案描述為可直接部署或仍可連線使用的服務。
- 本次整理以版本庫程式碼進行靜態回顧，沒有重新建置、部署或呼叫已停用的 API。

## 技術回顧

開發脈絡、程式能力盤點、功能邊界與履歷描述請參閱 [PROJECT_RETROSPECTIVE.md](PROJECT_RETROSPECTIVE.md)。
