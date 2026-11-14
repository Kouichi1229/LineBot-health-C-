# 專案技術回顧：LINE 健康資訊問答聊天機器人

## 1. 專案定位

這是大學專題作品，以 C#、ASP.NET Web API 與 LINE Bot SDK 建立健康資訊問答聊天機器人。使用者透過 LINE 傳送訊息，後端接收 Webhook 事件，再依照訊息內容提供互動選單或呼叫問答服務。

專案主要整合 LINE Messaging API 與 Azure QnA Maker；另外保留獨立的 LUIS 測試 Controller，探索自然語言意圖與 Entity 辨識。

本文件不記載學校名稱、指導單位或其他不必要的機構資訊。

## 2. 核心架構

### 2.1 LINE Webhook 與 ASP.NET Web API

`Main_V1Controller.cs` 繼承 LINE Webhook Controller，透過 HTTP POST endpoint 接收 LINE 傳入的事件。

主要處理流程包括：

- 解析收到的 LINE Event。
- 判斷訊息事件與文字訊息類型。
- 取得傳訊者的 LINE User ID 與顯示名稱。
- 依照訊息內容分派功能。
- 使用 LINE Bot SDK 的 `ReplyMessage()` 回覆訊息。
- 對 LINE Webhook 驗證事件回傳成功狀態。

這個部分呈現了使用 C# 建立 Web API、接收第三方平台 Webhook，以及串接外部 Messaging API 的經驗。

### 2.2 LINE 互動式選單

主要 Controller 使用 LINE Bot SDK 建立 `ButtonsTemplate` 與 `MessageAction`，將聊天指令整理成分層選單。

主選單包含：

- 關於熱量
- 代謝率計算
- 疾病查詢
- 認識毒品

其中可確認已實作的選單互動包括熱量相關選項、疾病查詢提示及毒品知識分類選單。部分選單文字雖然保留「代謝率計算」入口，但對應的資料蒐集與計算程式目前位於註解區段，不應宣稱為已完成的可用功能。

### 2.3 Azure QnA Maker 問答整合

主要 Controller 建立 `isRock.MsQnAMaker.Client`，使用設定的 Endpoint 與 API Key 將使用者文字問題送往知識庫。

程式接著：

1. 取得 QnA Maker 回傳的 answers。
2. 依照 score 由高至低排序。
3. 取出分數最高的答案。
4. 在答案分數大於 0 時，將答案透過 LINE 回覆。

這個整合讓聊天機器人可以依照知識庫內容回答使用者輸入的問題，而不是只依賴硬編碼的關鍵字回覆。

專案另有 `TestQnAController.cs`，以獨立 endpoint 展示基本的 QnA Maker 呼叫流程。

### 2.4 LUIS 自然語言理解測試

`TestLUISController.cs` 使用 Microsoft Cognitive LUIS Client，將使用者輸入文字送至 LUIS 服務，讀取辨識結果中的：

- Intents：判斷使用者輸入可能代表的意圖。
- Entities：擷取輸入中的實體資訊。
- Top Scoring Intent：取得最高分的意圖結果。

程式依照辨識結果回覆使用者，並處理沒有辨識到意圖或意圖為 None 的情況。

這是獨立測試 Controller 的實作證據；不應將它描述成主要聊天流程已全面使用 LUIS 進行意圖路由。

### 2.5 基礎代謝率與 BMI 程式草稿

主要 Controller 中保留了使用 LINE Conversation InformationCollector 蒐集身高、體重等資料，並計算 BMI 的程式草稿。不過該區段目前已被註解，沒有接入有效的訊息處理流程。

因此履歷或作品介紹應將它定位為開發過程中的功能構想／程式草稿，而不是已完成並可使用的計算功能。

## 3. 技術能力盤點

| 能力 | 程式證據 | 能力定位 |
|---|---|---|
| C# 後端開發 | Controller、類別、例外處理與 LINQ | 能以 C# 實作 API 後端邏輯 |
| ASP.NET Web API | HTTP POST、Route、IHttpActionResult | 能建立 Web API endpoint |
| Webhook 整合 | 接收與解析 LINE Event | 具備第三方事件回呼整合經驗 |
| LINE Bot 開發 | LINE Bot SDK、ReplyMessage、ButtonsTemplate | 能建立訊息回覆與互動選單 |
| 雲端問答服務整合 | Azure QnA Maker Client、Endpoint、答案分數排序 | 能串接外部知識庫問答 API |
| 自然語言理解 | Microsoft Cognitive LUIS、Intent、Entity | 曾實作意圖與實體辨識測試 |
| 對話流程設計 | 主選單、分層選項、訊息分派 | 能將多項功能組織成聊天互動流程 |

## 4. 功能邊界與靜態程式審查

以下內容是根據版本庫程式碼進行的靜態整理，不代表重新建置或執行後的測試結果。

### 4.1 已停用的外部服務

此專題使用的 LINE 與 Azure 服務已停止，相關 API 已失效。原本的 Endpoint 與服務憑證不再具備可用性，因此本專案定位為歷史專題與技術經驗展示，不宣稱目前可部署或正常運作。

### 4.2 憑證遮蔽

主要 Controller 原先將 LINE Channel Access Token、Channel Secret 與 Azure QnA Maker Key 直接寫在原始碼中。本次整理已將這三項憑證字串替換為 `APIKey` 佔位符，以避免在目前版本繼續公開原始憑證內容。

`APIKey` 只是遮蔽標記，並非有效金鑰。其他測試 Controller 原本已使用提示文字作為憑證佔位符。

本次只修改目前分支上的原始碼內容，沒有改寫 Git 歷史紀錄；舊憑證字串仍可能存在於先前的 commit 歷史中。由於使用者確認相關服務已停用，這次沒有進行歷史重寫。

### 4.3 部分功能仍停留在草稿或註解

- 基礎代謝率／BMI 對話蒐集與計算程式目前被註解。
- 部分疾病分類選單的程式碼也保留在註解中。
- LUIS 是獨立測試 Controller，不能直接視為主要聊天流程的意圖辨識模組。

### 4.4 舊版技術環境

專案檔顯示目標框架為 .NET Framework 4.5.2，並使用舊版 LINE Bot SDK、ASP.NET Web API 與 Microsoft Cognitive LUIS 套件。這些是當時的專案依賴，不代表目前仍受到支援或能在現代環境直接建置。

## 5. 履歷與面試描述

### 履歷精簡版

以 C# 與 ASP.NET Web API 開發 LINE 健康資訊問答聊天機器人，整合 LINE Messaging API Webhook、互動式訊息選單與 Azure QnA Maker 知識庫問答，並透過獨立測試 Controller 串接 Microsoft LUIS，實作使用者意圖與 Entity 辨識。

### 面試說明版

這是大學專題作品，我負責以 C# 和 ASP.NET Web API 開發 LINE Bot 後端。使用者從 LINE 傳送訊息後，Webhook 會將事件送到後端，再依照訊息內容提供不同的功能選單。問答部分串接 Azure QnA Maker，將使用者問題送到知識庫，並依照答案分數挑選回覆。另外也做過 LUIS 的整合測試，讓系統能辨識輸入文字的 Intent 和 Entity。

如果被問到目前是否還能使用，可以說明這是已結束的歷史專題，當時使用的 LINE 與 Azure 服務已停用，因此目前保留的是程式與技術實作紀錄，而不是仍在運作的線上服務。

## 6. 整理結論

這個作品最有價值的部分，是將 C# Web API、LINE Webhook、聊天互動流程與雲端問答服務整合在一起。它能證明曾經實作第三方 API 串接、事件接收、訊息解析、互動式選單及自然語言服務整合。

對外介紹時應聚焦在上述已由程式碼確認的整合能力；將未完成的基礎代謝率計算功能定位為程式草稿，並明確說明外部服務已停用。
