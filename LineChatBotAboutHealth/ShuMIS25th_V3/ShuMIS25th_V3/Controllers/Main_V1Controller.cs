using isRock.LineBot.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShuMIS25th_V3.Controllers
{
    public class Main_V1Controller : isRock.LineBot.LineWebHookControllerBase
    {
        const string channelAccessToken = "bZzW/iWV9Q0Ems+uIyguUQEM58LV0wbUkawztCiUkI1rU2MYeNCyg0S8Fyv+B4S73r7epM9yuJWMNNzH6hWSq7T6Pdv2iZZnXRAcoy35AobVT8LsCqmBVaGm1PN1kxP6FTLvK6yxBLjQmHPWPiXdeAdB04t89/1O/w1cDnyilFU=";
        const string AdminUserId = "Ud9cc0a2f1d572a98e58bf03adebefc77";
        //--short token--//
        const string channelShortID = "1594416612";
        const string channelShortSecret = "2a2cda417e69f881db7b27af675fcb07";

        /*
         QAmaker EndPoint  =  Host + post
                   Key =  endpointKey
           建立好的WEB 要跟 QAWEB 同個資源
       */
        const string DatabasesQAEpoint = "https://testbot33.azurewebsites.net/qnamaker/knowledgebases/695ce9f6-c08e-46fb-b861-73b75738d051/generateAnswer";
        const string DatabasesQAKey = "43492666-bcc0-4d53-a32a-23331f16d325";


        const string UnknowAnswer = "我的資料庫裡沒有這方面的資料,你可以問我這些東西:\n";



        [Route("api/ShuMIS25thV3")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                var NewToken = isRock.LineBot.Utility.IssueChannelAccessToken(channelShortID,channelShortSecret);

                //this.ChannelAccessToken = channelAccessToken;
                this.ChannelAccessToken = NewToken.access_token;
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault(); //JSON轉成物件
                //isRock.LineBot.Bot bot = new isRock.LineBot.Bot(channelAccessToken);
                //short channel
                isRock.LineBot.Bot bot = new isRock.LineBot.Bot(NewToken.access_token);

                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();
                //回覆訊息

                string lineID = ReceivedMessage.events.FirstOrDefault().source.userId;//使用者的發送資源碼
                var userid = bot.GetUserInfo(lineID).displayName;//取得使用者的名稱

                #region 回復訊息 功能

                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {

                        #region 建立主選單
                        if (LineEvent.message.text.ToLower() == "主選單")
                        {
                            //建立actions, 作為ButtonTemplate的用戶回覆行為
                            var actionMain = new List<isRock.LineBot.TemplateActionBase>();
                            actionMain.Add(new isRock.LineBot.MessageAction() { label = "關於熱量", text = "關於熱量" });
                            actionMain.Add(new isRock.LineBot.MessageAction() { label = "代謝率計算", text = "代謝率計算" });
                            actionMain.Add(new isRock.LineBot.MessageAction() { label = "疾病查詢", text = "疾病查詢" });
                            actionMain.Add(new isRock.LineBot.MessageAction() { label = "認識毒品", text = "毒品" });

                            var BtnMain = new isRock.LineBot.ButtonsTemplate();
                            {
                                BtnMain.thumbnailImageUrl = new Uri("https://scontent.ftpe8-4.fna.fbcdn.net/v/t1.0-9/42989840_279619006211005_2212480676580556800_n.jpg?_nc_cat=110&oh=827a424e40ab2efa52b280c3674ba26d&oe=5C5EFB31");
                                BtnMain.text = "功能選單";
                                BtnMain.title = "請選擇以下功能: ";
                                //加入動作
                                BtnMain.actions = actionMain;
                            }//上面文字 

                            this.ReplyMessage(LineEvent.replyToken, BtnMain);


                        }// end of 主選單 tempalte MSG
                        #endregion

                        #region 認識毒品(( 改 完成
                        else if (LineEvent.message.text.ToLower() == "毒品")
                        {
                            var actions_Drugs = new List<isRock.LineBot.TemplateActionBase>();
                            actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "一級毒品", text = "一級毒品" });
                            actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "二級毒品", text = "二級毒品" });
                            actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "三級 & 四級毒品", text = "三級 & 四級毒品" });

                            var BtnTemplateMsg_Drugs = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                            {

                                //----功能選擇 文字
                                BtnTemplateMsg_Drugs.thumbnailImageUrl = new Uri("https://i.screenshot.net/0gq8bx1");//照片
                                BtnTemplateMsg_Drugs.text = "認識毒品/藥物";
                                BtnTemplateMsg_Drugs.title = "請選以下選項";
                                //add action
                                BtnTemplateMsg_Drugs.actions = actions_Drugs;
                            };
                            this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs);

                        }//end if 認識毒品
                        /*
                         一級毒品 進度底下
                         所有包含類別的底下
                         送出對印的圖片
                         */
                        if (LineEvent.message.text.ToLower() == "一級毒品")
                        {
                            var actions_Drugs1 = new List<isRock.LineBot.TemplateActionBase>();
                            actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "古柯鹼", text = "古柯鹼" });
                            actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "海洛因", text = "海洛因" });
                            actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "嗎啡", text = "嗎啡" });
                            actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "鴉片", text = "鴉片" });


                            var BtnTemplateMsg_Drugs1 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                            {

                                //----功能選擇 文字
                                BtnTemplateMsg_Drugs1.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                                BtnTemplateMsg_Drugs1.text = "一級毒品";
                                BtnTemplateMsg_Drugs1.title = "包含:";
                                //add action
                                BtnTemplateMsg_Drugs1.actions = actions_Drugs1;
                            };
                            this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs1);
                        }//進入一級毒品底下

                        if (LineEvent.message.text.ToLower() == "古柯鹼" || LineEvent.message.text.ToLower() == "海洛因" || LineEvent.message.text.ToLower() == "嗎啡" || LineEvent.message.text.ToLower() == "鴉片")
                        {
                            if (LineEvent.message.text == "古柯鹼")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-4.fna.fbcdn.net/v/t1.0-9/44512417_289710488535190_8327672820775518208_n.jpg?_nc_cat=107&_nc_ht=scontent.ftpe7-4.fna&oh=43592844c9d185eb089863932dc00d8b&oe=5C4B9525"));
                            }
                            if (LineEvent.message.text == "海洛因")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-1.fna.fbcdn.net/v/t1.0-9/44543147_289710641868508_8472298113527185408_n.jpg?_nc_cat=110&_nc_ht=scontent.ftpe7-1.fna&oh=b35547ec597bed827be370b523a1099d&oe=5C3D6DF3"));
                            }
                            if (LineEvent.message.text == "嗎啡")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44755107_289710495201856_2598789976715952128_n.jpg?_nc_cat=109&_nc_ht=scontent.ftpe7-2.fna&oh=d354ec18b75fa7413966149b0647c441&oe=5C4EBED9"));
                            }
                            if (LineEvent.message.text == "鴉片")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-1.fna.fbcdn.net/v/t1.0-9/44474645_289710588535180_2885157204644593664_n.jpg?_nc_cat=110&_nc_ht=scontent.ftpe7-1.fna&oh=778233fa67c2d19d5181956a3442ff4b&oe=5C55C2B4"));
                            }
                        }//LineEvent.message.text.ToLower() == "古柯鹼" || LineEvent.message.text.ToLower() == "海洛因" || LineEvent.message.text.ToLower() == "嗎啡" || LineEvent.message.text.ToLower() == "鴉片"


                        //二類
                        if (LineEvent.message.text.ToLower() == "二級毒品")
                        {
                            var actions_Drugs2 = new List<isRock.LineBot.TemplateActionBase>();
                            actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "大麻", text = "大麻" });
                            actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "安非他命", text = "安非他命" });
                            actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "搖頭丸", text = "搖頭丸" });
                            actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "魔菇", text = "魔菇" });


                            var BtnTemplateMsg_Drugs2 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                            {

                                //----功能選擇 文字
                                BtnTemplateMsg_Drugs2.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                                BtnTemplateMsg_Drugs2.text = "二級毒品";
                                BtnTemplateMsg_Drugs2.title = "包含:";
                                //add action
                                BtnTemplateMsg_Drugs2.actions = actions_Drugs2;
                            };

                            this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs2);
                        }//end if 2

                        if (LineEvent.message.text.ToLower() == "大麻" || LineEvent.message.text.ToLower() == "安非他命" || LineEvent.message.text.ToLower() == "搖頭丸" || LineEvent.message.text.ToLower() == "魔菇")
                        {
                            if (LineEvent.message.text == "大麻")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-4.fna.fbcdn.net/v/t1.0-9/44532974_289710661868506_723348900114595840_n.jpg?_nc_cat=105&_nc_ht=scontent.ftpe7-4.fna&oh=d31afb7b26f0ed3f6d4be0b35c9cbb1b&oe=5C40D05B"));
                            }
                            if (LineEvent.message.text == "安非他命")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-3.fna.fbcdn.net/v/t1.0-9/44600272_289710511868521_4920973721583222784_n.jpg?_nc_cat=102&_nc_ht=scontent.ftpe7-3.fna&oh=21dd7ae8d74629f896e4134968959fdf&oe=5C8AC861"));
                            }
                            if (LineEvent.message.text == "搖頭丸")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44442011_289710581868514_2693626711492263936_n.jpg?_nc_cat=109&_nc_ht=scontent.ftpe7-2.fna&oh=c81b10c33dd39dfe150b707052de1c0d&oe=5C478727"));
                            }
                            if (LineEvent.message.text == "魔菇")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44468789_289710571868515_1123661202384224256_n.jpg?_nc_cat=109&_nc_ht=scontent.ftpe7-2.fna&oh=63136c2f2a7f2f7cbb2dd68f27e95bb7&oe=5C499DC6"));
                            }
                        }

                        //三類&四類
                        if (LineEvent.message.text.ToLower() == "三級 & 四級毒品")
                        {
                            var actions_Drugs34 = new List<isRock.LineBot.TemplateActionBase>();
                            actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "3級 K他命(氯胺酮)", text = "愷他命" });
                            actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "3級 FM2", text = "強暴丸" });
                            actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "4級 蝴蝶片", text = "蝴蝶片" });


                            var BtnTemplateMsg_Drugs34 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                            {

                                //----功能選擇 文字
                                BtnTemplateMsg_Drugs34.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                                BtnTemplateMsg_Drugs34.text = "三級 & 四級毒品";
                                BtnTemplateMsg_Drugs34.title = "包含:";
                                //add action
                                BtnTemplateMsg_Drugs34.actions = actions_Drugs34;
                            };

                            this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs34);
                        }//end of 3&4

                        if (LineEvent.message.text.ToLower() == "愷他命" || LineEvent.message.text.ToLower() == "強暴丸" || LineEvent.message.text.ToLower() == "蝴蝶片")
                        {
                            if (LineEvent.message.text == "愷他命")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44654928_289710501868522_8126295144434499584_n.jpg?_nc_cat=104&_nc_ht=scontent.ftpe7-2.fna&oh=7019110757b0d7f22369d838959794cb&oe=5C3C309C"));
                            }
                            if (LineEvent.message.text == "強暴丸")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44471505_289710628535176_1379139201613692928_n.jpg?_nc_cat=104&_nc_ht=scontent.ftpe7-2.fna&oh=1dbf504de3bbb248b19d38d0423d37df&oe=5C4D8773"));
                            }
                            if (LineEvent.message.text == "蝴蝶片")
                            {
                                this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-2.fna.fbcdn.net/v/t1.0-9/44487736_289710565201849_6465331643535065088_n.jpg?_nc_cat=104&_nc_ht=scontent.ftpe7-2.fna&oh=8c3ebc28aa54828d6c95589b79896813&oe=5C3EC72A"));
                            }
                        }


                        #endregion

                        /*
                         問題!!!!!!!!!
                         LineEvent.message.type == "text" 會戶擋
                         擺到外投沒關係
                         解決方法: 想法一 :大雜燴 放到外投  
                         */

                        /*
                         四大功能 
                         關於熱量  代謝率計算 疾病查詢 認識毒品                        
                         先進入選項 
                         在建立選項表單                
                        */

                        /*
                         * 關於熱量
                         1.建立表單 
                         2.進入選項 和選項的平行選擇條件
                         */

                        #region QA 關於熱量 和 疾病
                        else if (LineEvent.message.text.ToLower() == "關於熱量")
                        {


                            var actionCal = new List<isRock.LineBot.TemplateActionBase>();
                            actionCal.Add(new isRock.LineBot.MessageAction() { label = "查詢食物熱量", text = "查詢食物熱量" });
                            actionCal.Add(new isRock.LineBot.MessageAction() { label = "熱量問答", text = "熱量問答" });

                            var BtnCal = new isRock.LineBot.ButtonsTemplate();
                            {
                                BtnCal.thumbnailImageUrl = new Uri("https://scontent.ftpe7-3.fna.fbcdn.net/v/t1.0-9/40685223_264561371050102_1948310784530448384_n.jpg?_nc_fx=ftpe7-3&_nc_cat=0&oh=c148736b5a0996f8406d85fada463570&oe=5C25CA17");
                                BtnCal.text = "功能選單";
                                BtnCal.title = "請選擇以下功能: ";
                                //加入動作
                                BtnCal.actions = actionCal;
                            }//上面文字 

                            this.ReplyMessage(LineEvent.replyToken, BtnCal);

                        }//進入關於熱量的底層

                        if (LineEvent.message.text.ToLower() == "查詢食物熱量" || LineEvent.message.text.ToLower() == "熱量問答")
                        {

                            if (LineEvent.message.text == "查詢食物熱量")
                            {
                                this.ReplyMessage(LineEvent.replyToken, "您可以輸入以下這些: 雞肉 牛肉 豬肉 羊肉 蝦肉 魚肉 白飯 雞蛋 白吐司 麵 番薯 馬鈴薯 起司 香蕉 豆腐 奶油 牛奶 豆漿 油");
                            }//查詢食物熱量

                            else if (LineEvent.message.text == "熱量問答")
                            {
                                this.ReplyMessage(LineEvent.replyToken, "您可以輸入以下這些:\n 成年人一天需要多少熱量?\n兒童一天需要多少熱量?\n青少年一天需要多少熱量? \n基礎代謝\n");
                            }

                        }//進入熱量 食物 QA



                        else if (LineEvent.message.text.ToLower() == "疾病查詢")
                        {

                            this.ReplyMessage(LineEvent.replyToken, "您可以問我: 直接輸入疾病(ex:肺炎,糖尿病,大腸癌,高血壓,中風.....)\n或是輸入症狀(兩個以上 1ex:眼睛紅腫 有分泌物 睜不開 2ex:胸痛 胸悶 ) ");
                            //var actions_Sick = new List<isRock.LineBot.TemplateActionBase>();
                            //actions_Sick.Add(new isRock.LineBot.MessageAction() { label = "疾病症狀查詢", text = "疾病症狀查詢" });
                            //actions_Sick.Add(new isRock.LineBot.MessageAction() { label = "疾病的分類", text = "疾病的分類" });


                            //var BtnTemplateMsg_Sick = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                            //{

                            //    //----功能選擇 文字
                            //    BtnTemplateMsg_Sick.thumbnailImageUrl = new Uri("https://scontent.ftpe7-3.fna.fbcdn.net/v/t1.0-9/40912906_265044184335154_3090929470048043008_n.jpg?_nc_fx=ftpe7-3&_nc_cat=0&oh=480ce3da7f50cd926a57678c636ea6a0&oe=5C3A5C69");//照片
                            //    BtnTemplateMsg_Sick.text = "查詢";
                            //    BtnTemplateMsg_Sick.title = "請選以下選項";
                            //    //add action
                            //    BtnTemplateMsg_Sick.actions = actions_Sick;
                            //};

                            //this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Sick);
                        }// 疾病查詢 

                        //if (LineEvent.message.text.ToLower() == "疾病症狀查詢" || LineEvent.message.text.ToLower() == "疾病的分類")
                        //{
                        //    if (LineEvent.message.text== "疾病症狀查詢")
                        //    {
                        //        this.ReplyMessage(LineEvent.replyToken,"您可以打:疾病or病狀");
                        //    }

                        //    if (LineEvent.message.text == "疾病的分類")
                        //    {
                        //        this.ReplyMessage(LineEvent.replyToken, "您可以直接打:疾病的名稱");
                        //    }
                        //}

                        if (ReceivedMessage.events[0].message.type == "text") //收到文字
                        {

                            //建立 MsQnAMaker Client
                            var helper = new isRock.MsQnAMaker.Client(new Uri(DatabasesQAEpoint), DatabasesQAKey);
                            var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                            var ret = (from c in QnAResponse.answers
                                       orderby c.score descending
                                       select c
                                    ).Take(1);
                            //var data = "雞肉 牛肉 豬肉 羊肉 蝦肉 魚肉 白飯 雞蛋 白吐司 麵 番薯 馬鈴薯 起司 香蕉 豆腐 奶油 牛奶 豆漿 油 ";
                            var responseText = "";// UnknowAnswer + data;
                            if (ret.FirstOrDefault().score > 0)
                                responseText = ret.FirstOrDefault().answer;

                            //回覆
                            this.ReplyMessage(LineEvent.replyToken, responseText);
                        }//if

                        #endregion



               

                        #region 基礎代謝率計算(( 改 
                       
                        
                          //  //收集定義者資料 switch 那一塊
                          //  isRock.LineBot.Conversation.InformationCollector<LeaveRequest> CIC = new isRock.LineBot.Conversation.InformationCollector<LeaveRequest>(channelAccessToken);

                          //  var responseMsg = "";

                          //  //取得 http post rawData
                          //  string postData = Request.Content.ReadAsStringAsync().Result;
                          //  var ReMSG = isRock.LineBot.Utility.Parsing(postData);
                          //  LeaveRequest lr = new LeaveRequest(); // 計算的list


                          //  //定義接收結果
                          //  ProcessResult<LeaveRequest> result;

                          //  //確認是否有進入
                          ////  this.ReplyMessage(LineEvent.replyToken, "請輸入'計算',就會開始計算基礎代謝率");

                          //  if ( LineEvent.message.type == "text" && ReceivedMessage.events[0].message.text == "算")
                          //  {
                          //      //把訊息丟給CIC 
                          //      result = CIC.Process(ReceivedMessage.events[0], true);
                          //      responseMsg = "開始計算\n";
                          //  }
                          //  else
                          //  {
                          //      //把訊息丟給CIC 
                          //      result = CIC.Process(ReceivedMessage.events[0]);
                          //  }

                          //  //處理 CIC回覆的結果
                          //  switch (result.ProcessResultStatus)
                          //  {
                          //      case ProcessResultStatus.Processed:
                          //          //取得候選訊息發送
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          break;
                          //      case ProcessResultStatus.Done:
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          responseMsg += $"蒐集到的資料有...\n";
                          //          double bmi = (lr.Hight) / 100 / Math.Pow(lr.Weight, 2);
                          //          responseMsg += $" Newtonsoft.Json.JsonConvert.SerializeObject(result.ConversationState.ConversationEntity)\n";
                          //          responseMsg += $"BMI為{bmi}";
                          //          break;
                          //      //case ProcessResultStatus.Pass:
                          //      //    responseMsg = $"你說的 '{ReceivedMessage.events[0].message.text}' 我看不懂，如果想要請假，請跟我說 : 『我要請假』";
                          //      //    break;
                          //      case ProcessResultStatus.Exception:
                          //          //取得候選訊息發送
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          break;
                          //      case ProcessResultStatus.Break:
                          //          //取得候選訊息發送
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          break;
                          //      case ProcessResultStatus.InputDataFitError:
                          //          responseMsg += "\n資料型態不合\n";
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          break;
                          //      default:
                          //          //取得候選訊息發送
                          //          responseMsg += result.ResponseMessageCandidate;
                          //          break;
                          //  }






                        //end of 代謝率計算






                        #endregion



                        #region 四個功能 
                        //if (LineEvent.message.text.ToLower() == "關於熱量" || LineEvent.message.text.ToLower() == "代謝率計算" || LineEvent.message.text.ToLower() == "疾病查詢" || LineEvent.message.text.ToLower() == "認識毒品")
                        //{


                        //    #region 關於熱量
                        //    if (LineEvent.message.text == "關於熱量")
                        //    {
                        //        var actionCal = new List<isRock.LineBot.TemplateActionBase>();
                        //        actionCal.Add(new isRock.LineBot.MessageAction() { label = "查詢食物熱量", text = "查詢食物熱量" });
                        //        actionCal.Add(new isRock.LineBot.MessageAction() { label = "熱量有關QA", text = "熱量有關QA" });

                        //        var BtnCal = new isRock.LineBot.ButtonsTemplate();
                        //        {
                        //            BtnCal.thumbnailImageUrl = new Uri("https://scontent.ftpe7-3.fna.fbcdn.net/v/t1.0-9/40685223_264561371050102_1948310784530448384_n.jpg?_nc_fx=ftpe7-3&_nc_cat=0&oh=c148736b5a0996f8406d85fada463570&oe=5C25CA17");
                        //            BtnCal.text = "功能選單";
                        //            BtnCal.title = "請選擇以下功能: ";
                        //            //加入動作
                        //            BtnCal.actions = actionCal;
                        //        }//上面文字 

                        //        this.ReplyMessage(LineEvent.replyToken, BtnCal);

                        //        if (LineEvent.message.text.ToLower() == "查詢食物熱量" || LineEvent.message.text.ToLower() == "熱量有關QA")
                        //        {
                        //            if (LineEvent.message.text == "查詢食物熱量")
                        //            {
                        //                // var repmsg = "";
                        //                // if (LineEvent.message.type== "text") //收到文字
                        //                //{
                        //                this.ReplyMessage(LineEvent.replyToken, "您可以輸入以下這些: 雞肉 牛肉 豬肉 羊肉 蝦肉 魚肉 白飯 雞蛋 白吐司 麵 番薯 馬鈴薯 起司 香蕉 豆腐 奶油 牛奶 豆漿 油");

                        //                //建立 MsQnAMaker Client
                        //                var helper = new isRock.MsQnAMaker.Client(new Uri("CalQAEpoint"), "CalQAKey");
                        //                var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        //                var ret = (from c in QnAResponse.answers
                        //                           orderby c.score descending
                        //                           select c
                        //                        ).Take(1);
                        //                var data = "雞肉 牛肉 豬肉 羊肉 蝦肉 魚肉 白飯 雞蛋 白吐司 麵 番薯 馬鈴薯 起司 香蕉 豆腐 奶油 牛奶 豆漿 油 ";
                        //                var responseText = UnknowAnswer + data;
                        //                if (ret.FirstOrDefault().score > 0)
                        //                    responseText = ret.FirstOrDefault().answer;
                        //                //回覆
                        //                this.ReplyMessage(LineEvent.replyToken, responseText);
                        //                //}//if
                        //            }//end of 查詢食物熱量


                        //            if (LineEvent.message.text == "熱量有關QA")
                        //            {
                        //                // var repmsg = "";
                        //                // if (LineEvent.type == "message") //收到文字
                        //                //{

                        //                this.ReplyMessage(LineEvent.replyToken, "您可以輸入以下這些: 成年人一天需要多少熱量?\n兒童一天需要多少熱量?\n青少年一天需要多少熱量? \n基礎代謝\n");

                        //                //建立 MsQnAMaker Client
                        //                var helper = new isRock.MsQnAMaker.Client(new Uri(CalQAEpoint), CalQAKey);
                        //                var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        //                var ret = (from c in QnAResponse.answers
                        //                           orderby c.score descending
                        //                           select c
                        //                        ).Take(1);
                        //                var data = "成年人一天需要多少熱量?\n兒童一天需要多少熱量?\n青少年一天需要多少熱量? \n基礎代謝\n";
                        //                var responseText = UnknowAnswer + data;
                        //                if (ret.FirstOrDefault().score > 0)
                        //                    responseText = ret.FirstOrDefault().answer;
                        //                //回覆
                        //                this.ReplyMessage(LineEvent.replyToken, responseText);
                        //                //}//if lineevent
                        //            }//熱量有關QA 
                        //        }//end of LineEvent.message.text.ToLower() == "查詢食物熱量" || LineEvent.message.text.ToLower() == "熱量有關QA"
                        //    }//end of 關於熱量 

                        //    #endregion

                        //    #region 代謝率
                        //    if (LineEvent.message.text == "代謝率換算")
                        //    {


                        //        // 定義資訊收集者 list
                        //        isRock.LineBot.Conversation.InformationCollector<LeaveRequest> CIC =
                        //            new isRock.LineBot.Conversation.InformationCollector<LeaveRequest>(channelAccessToken);
                        //        LeaveRequest lr = new LeaveRequest();

                        //        this.ReplyMessage(LineEvent.replyToken, "只要說 : '計算' 就會開始計算基礎代謝率");
                        //        //取得 http post RawData Json
                        //        string postData = Request.Content.ReadAsStringAsync().Result;
                        //        //剖析 Json
                        //        var reMessage = isRock.LineBot.Utility.Parsing(postData);

                        //        //定義類別
                        //        ProcessResult<LeaveRequest> result;
                        //        //回復
                        //        var responseMsg = "";

                        //        if (reMessage.events[0].message.text == "計算")
                        //        {

                        //            result = CIC.Process(reMessage.events[0], true);
                        //            responseMsg = "開始計算\n";

                        //        }//計算代謝率

                        //        else { result = CIC.Process(reMessage.events[0]); }
                        //        //處理 CIC 處理結果
                        //        switch (result.ProcessResultStatus)
                        //        {
                        //            case ProcessResultStatus.Processed:
                        //                //取得候選訊息發送
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                break;

                        //            case ProcessResultStatus.Done:
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                responseMsg += $"蒐集到的資料有...\n";
                        //                double bmi = (lr.Hight) / 100 / Math.Pow(lr.Weight, 2);
                        //                responseMsg += $" Newtonsoft.Json.JsonConvert.SerializeObject(result.ConversationState.ConversationEntity)\n";
                        //                responseMsg += $"BMI為{bmi}";
                        //                break;
                        //            //case ProcessResultStatus.Pass:
                        //            //    responseMsg = $"你說的 '{ReceivedMessage.events[0].message.text}' 我看不懂，如果想要請假，請跟我說 : 『我要請假』";
                        //            //    break;
                        //            case ProcessResultStatus.Exception:
                        //                //取得候選訊息發送
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                break;
                        //            case ProcessResultStatus.Break:
                        //                //取得候選訊息發送
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                break;
                        //            case ProcessResultStatus.InputDataFitError:
                        //                responseMsg += "\n資料型態不合\n";
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                break;
                        //            default:
                        //                //取得候選訊息發送
                        //                responseMsg += result.ResponseMessageCandidate;
                        //                break;

                        //        }

                        //    }//end of 代謝率換算
                        //    #endregion

                        //    #region 疾病查詢
                        //    if (LineEvent.message.text == "疾病查詢")
                        //    {
                        //        var actions_Sick = new List<isRock.LineBot.TemplateActionBase>();
                        //        actions_Sick.Add(new isRock.LineBot.MessageAction() { label = "疾病症狀查詢", text = "疾病症狀查詢" });
                        //        actions_Sick.Add(new isRock.LineBot.MessageAction() { label = "疾病的分類", text = "疾病的分類" });


                        //        var BtnTemplateMsg_Sick = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                        //        {

                        //            //----功能選擇 文字
                        //            BtnTemplateMsg_Sick.thumbnailImageUrl = new Uri("https://scontent.ftpe7-3.fna.fbcdn.net/v/t1.0-9/40912906_265044184335154_3090929470048043008_n.jpg?_nc_fx=ftpe7-3&_nc_cat=0&oh=480ce3da7f50cd926a57678c636ea6a0&oe=5C3A5C69");//照片
                        //            BtnTemplateMsg_Sick.text = "查詢";
                        //            BtnTemplateMsg_Sick.title = "請選以下選項";
                        //            //add action
                        //            BtnTemplateMsg_Sick.actions = actions_Sick;
                        //        };

                        //        this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Sick);
                        //        if (LineEvent.message.text.ToLower() == "疾病症狀查詢" || LineEvent.message.text.ToLower() == "疾病的分類")
                        //        {

                        //        }// end of LineEvent.message.text.ToLower()== "疾病症狀查詢" || LineEvent.message.text.ToLower() == "疾病的分類"


                        //    }//end of 疾病查詢

                        //    #endregion

                        //    #region 毒品

                        //    if (LineEvent.message.text == "認識毒品")
                        //    {

                        //        /* 建立TemplateMsg
                        //         *選擇後 進入選項底下 
                        //         * 
                        //         * 
                        //         *
                        //         */

                        //        var actions_Drugs = new List<isRock.LineBot.TemplateActionBase>();
                        //        actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "一級毒品", text = "一級毒品" });
                        //        actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "二級毒品", text = "二級毒品" });
                        //        actions_Drugs.Add(new isRock.LineBot.MessageAction() { label = "三級 & 四級毒品", text = "三級 & 四級毒品" });

                        //        var BtnTemplateMsg_Drugs = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                        //        {

                        //            //----功能選擇 文字
                        //            BtnTemplateMsg_Drugs.thumbnailImageUrl = new Uri("https://i.screenshot.net/0gq8bx1");//照片
                        //            BtnTemplateMsg_Drugs.text = "認識毒品/藥物";
                        //            BtnTemplateMsg_Drugs.title = "請選以下選項";
                        //            //add action
                        //            BtnTemplateMsg_Drugs.actions = actions_Drugs;
                        //        };
                        //        this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs);


                        //        //---------drugs 

                        //        if (LineEvent.message.text.ToLower() == "一級毒品" || LineEvent.message.text.ToLower() == "二級毒品" || LineEvent.message.text.ToLower() == "三級 & 四級毒品")
                        //        {

                        //            if (LineEvent.message.text == "一級毒品")
                        //            {
                        //                //TemplateMsg
                        //                var actions_Drugs1 = new List<isRock.LineBot.TemplateActionBase>();
                        //                actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "古柯鹼", text = "古柯鹼" });
                        //                actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "海洛因", text = "海洛因" });
                        //                actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "嗎啡", text = "嗎啡" });
                        //                actions_Drugs1.Add(new isRock.LineBot.MessageAction() { label = "鴉片", text = "鴉片" });


                        //                var BtnTemplateMsg_Drugs1 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                        //                {

                        //                    //----功能選擇 文字
                        //                    BtnTemplateMsg_Drugs1.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                        //                    BtnTemplateMsg_Drugs1.text = "一級毒品";
                        //                    BtnTemplateMsg_Drugs1.title = "包含:";
                        //                    //add action
                        //                    BtnTemplateMsg_Drugs1.actions = actions_Drugs1;
                        //                };
                        //                this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs1);

                        //                if (LineEvent.message.text.ToLower() == "古柯鹼" || LineEvent.message.text.ToLower() == "海洛因" || LineEvent.message.text.ToLower() == "嗎啡" || LineEvent.message.text.ToLower() == "鴉片")
                        //                {
                        //                    if (LineEvent.message.text == "古柯鹼") { this.ReplyMessage(LineEvent.replyToken, new Uri("https://scontent.ftpe7-4.fna.fbcdn.net/v/t1.0-9/40784055_264535537719352_25188119175233536_n.jpg?_nc_cat=107&oh=f070a3ae0e0bb933a21c4000bac6c676&oe=5C53343D")); }
                        //                    if (LineEvent.message.text == "海洛因") { }
                        //                    if (LineEvent.message.text == "嗎啡") { }
                        //                    if (LineEvent.message.text == "鴉片") { }
                        //                }//LineEvent.message.text.ToLower() == "古柯鹼" || LineEvent.message.text.ToLower() == "海洛因" || LineEvent.message.text.ToLower() == "嗎啡" || LineEvent.message.text.ToLower() == "鴉片"





                        //            }//end of 一級毒品

                        //            if (LineEvent.message.text.ToLower() == "二級毒品")
                        //            {
                        //                //TemplateMsg
                        //                var actions_Drugs2 = new List<isRock.LineBot.TemplateActionBase>();
                        //                actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "大麻", text = "大麻" });
                        //                actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "安非他命", text = "安非他命" });
                        //                actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "搖頭丸", text = "搖頭丸" });
                        //                actions_Drugs2.Add(new isRock.LineBot.MessageAction() { label = "魔菇", text = "魔菇" });


                        //                var BtnTemplateMsg_Drugs2 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                        //                {

                        //                    //----功能選擇 文字
                        //                    BtnTemplateMsg_Drugs2.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                        //                    BtnTemplateMsg_Drugs2.text = "二級毒品";
                        //                    BtnTemplateMsg_Drugs2.title = "包含:";
                        //                    //add action
                        //                    BtnTemplateMsg_Drugs2.actions = actions_Drugs2;
                        //                };

                        //                this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs2);
                        //            }//end of 二級毒品

                        //            if (LineEvent.message.text.ToLower() == "三級 & 四級毒品")
                        //            {
                        //                //TemplateMsg
                        //                var actions_Drugs34 = new List<isRock.LineBot.TemplateActionBase>();
                        //                actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "3級 K他命(氯胺酮)", text = "K他命(氯胺酮)" });
                        //                actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "3級 FM2", text = "FM2" });
                        //                actions_Drugs34.Add(new isRock.LineBot.MessageAction() { label = "4級 蝴蝶片", text = "蝴蝶片" });


                        //                var BtnTemplateMsg_Drugs34 = new isRock.LineBot.ButtonsTemplate(); // 一開始的功能選擇
                        //                {

                        //                    //----功能選擇 文字
                        //                    BtnTemplateMsg_Drugs34.thumbnailImageUrl = new Uri("https://i.screenshot.net/pzertmn");//照片
                        //                    BtnTemplateMsg_Drugs34.text = "三級 & 四級毒品";
                        //                    BtnTemplateMsg_Drugs34.title = "包含:";
                        //                    //add action
                        //                    BtnTemplateMsg_Drugs34.actions = actions_Drugs34;
                        //                };

                        //                this.ReplyMessage(LineEvent.replyToken, BtnTemplateMsg_Drugs34);

                        //            }//end of 三四級毒品



                        //        }//end of 1234 drug tolower


                        //    }//end of 認識毒品

                        //    #endregion


                        //    //}// (LineEvent.message.text.ToLower() == "關於熱量" || LineEvent.message.text.ToLower() == "代謝率計算" || LineEvent.message.text.ToLower() == "疾病查詢" || LineEvent.message.text.ToLower() == "認識毒品
                        #endregion
                    }// end of message type is text 

                    if (LineEvent.message.type == "sticker")
                    {
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    }//收到貼圖


                    if (LineEvent.message.type == "location")
                    {
                        this.ReplyMessage(LineEvent.replyToken, $"你的位置在\n{LineEvent.message.latitude}, {LineEvent.message.longitude}");
                    }//收到位置

                }//end of  收到訊息(message)

                #endregion

                //response OK
                return Ok();
            }//end of try 
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }//end of public post


        /*
        連續對話 List Data
        get: 性別 年齡 身高 體重 活動量     
        */
        public class LeaveRequest : ConversationEntity
        {
            string sex = "";
            int age;
            double hight;
            double weight;
            [ButtonsTemplateQuestion("詢問", "請問您的性別是?", "", "男性", "女性")]
            [Order(1)]
            public string Sex { get { return sex; } set { sex = value; } }

            [Question("你問您的年齡大約是?")]
            [Order(2)]
            public int Age { get { return age; } set { age = value; } }

            [Question("請問您身高是?(CM)")]
            [Order(3)]
            public double Hight { get { return hight; } set { hight = value; } }

            [Question("請問你的體重是?(KG)")]
            [Order(4)]
            public double Weight { get { return weight; } set { weight = value; } }


            /*[Question("請問您要請幾小時?")]
            [Order(5)]
            public float 請假時數 { get; set; }*/

        }//end of class LeaveRequest


    }// end of class
}//end of nameSpace
