using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ovan_P1
{
    class Constant
    {
        public string storeName = "ラーメン世代";
        public string dbName = "obanp1";
        public string yesStr = "はい";
        public string noStr = "いいえ";
        public string unit = "円";
        public string amountUnit = "枚";
        public string amountUnit1 = "点";
        public string yearLabel = "年";
        public string monthLabel = "月";
        public string dayLabel = "日";
        public string hourLabel = "時";
        public string minuteLabel = "分";
        public string cancelLabel = "キャンセル";
        public string cancelRun = "取消実行";
        public string categoryListPrintMessage = "全てのカテゴリーを印刷 しても宜しいですか？";
        public string categoryListPrintTitle = "カテゴリー一覧印刷";
        public string categoryListTitleLabel = "表示位置/カテゴリーNo";
        public string categoryDiplayLabel = "表示位置";
        public string categoryLabel = "カテゴリー";
        public string groupListTitleLabel = "グループ名";
        public string groupTitleLabel = "グループ";
        public string groupListPrintMessage = "全てのグループを印刷 しても宜しいですか？";
        public string groupListPrintTitle = "グループ一覧";
        public string TimeLabel = "販売時間";
        public string SaleTimeLabel = "販売時刻";
        public string prevButtonLabel = "プレビュー";
        public string printButtonLabel = "一覧印刷";
        public string printProductNameField = "印刷品目名";
        public string salePriceField = "販売価格";
        public string saleLimitField = "限定数";
        public string saleStatusLabel = "販売中";
        public string saleStopLabel = "中止";
        public string saleStopText = "利用停止";
        public string saleStopStatusText = "販売停止";
        public string currentDateLabel = "現在の日付";
        public string currentTimeLabel = "現在の時刻";
        public string timeSettingLabel = "時刻設定";
        public string dateSettingTitle = "日付設定";
        public string timeSettingTitle = "時間設定";
        public string passwordSettingLabel = "暗証番号設定";
        public string oldPasswordLabel = "旧暗証番号";
        public string newPasswordLabel = "新暗証番号";
        public string confirmPasswordLabel = "新暗証番号(確認用)";
        public string charClearLabel = "一文字クリア";
        public string allClearLabel = "全クリア";
        public string settingLabel = "設定";
        public string passwordInputTitle = "パスワードを入力";
        public string receiptionTitle = "領収書発行一覧";
        public string receiptionField = "印刷日時";
        public string dailyReportTitle = "売上日報";
        public string priceField = "金額";
        public string closingProcessTitle = "締め処理";
        public string timeRangeLabel = "時台";
        public string logReportLabel = "ログ表示";
        public string falsePurchaseTitle = "誤購入取消";
        public string falsePurchaseSubTitle1 = "誤購入取消";
        public string falsePurchaseSubContent1 = "誤購入の取消を行う場合は下記のボタンを\nタッチしてください。";
        public string falsePurchaseSubTitle2 = "誤購入一覧表示";
        public string falsePurchaseButton = "誤購入取消";
        public string falsePurchaseStartLabel = "開始";
        public string falsePurchaseEndLabel = "終了";
        public string falsePurchaseListLabel = "一覧表示";
        public string falsePurchasePageTitle = "取り消す注文を選択";
        public string orderTimeField = "注文日付";
        public string prodNameField = "品名";
        public string saleNumberField = "売上連番";
        public string openTimeChangeTitle = "営業変更";
        public string dayType = "曜日タイプ";
        public string startTimeLabel = "営業開始時刻";
        public string endTimeLabel = "営業終了時刻";
        public string menuReadingTitle = "メニュー読込";
        public string menuReadingSubContent1 = "USBメモリをセットしてメニュー読込ボタンを押して、別ウィンドウが開いたら読み込むメニューを選択してください。";
        public string menuReadingSubContent2 = "中止する場合は、取消ボタンを押してください。";
        public string menuReadingButton = "メニュー読込";
        public string menuReadingErrorTitle = "データに問題があります。";
        public string menuReadingErrorContent = "設定ソフトウェアより「USBメモリへ の書込」を行った後に再度試してください。";
        public string restEmptyMessage = "注文商品の在庫がありません。";
        public string orderDialogRunText = "注文内容確認";
        public string sumLabel = "合計";
        public string receiptInstruction = "上記金額正に領収しました。";

        public string soldoutSettingTitle = "売り切れ設定";
        public string categorylistLabel = "カテゴリー選択";
        public string prdNameField = "品目名";
        public string saleStateSettingField = "状態";

        public string sumProgressAlert = "締め処理中です。終わるまでお待ちください";

        public string orderCancelDialogTitle = "取消確認";
        public string orderDate = "注文日";
        public string orderTime = "注文時間";
        public string orderProductList = "品目";
        public string orderSumPrice = "合計金額";



        public string cancelErrorMessage = "締め処理を行った後は取消出来ません";
        public string cancelResultErrorMessage = "データが無いか、日付が誤っています";

        public string prdCategoryField = "所属カテゴリー";
        public string prdPriceFieldIncludTax = "販売価格(税込)";
        public string prdSaleTimeField = "販売時刻設定";
        public string prdScreenText = "画面メッセージ";
        public string prdPrintText = "印刷メッセージ";

        public string errorMsgTitle = "係員をお呼びください。";
        public string systemErrorMsg = "システムエラーが発生しました。";
        public string systemSubErrorMsg = "マニュアルに従ってサービスをご依頼ください。";
        public string printErrorMsg = "レシート用紙切れです。\nロール紙を補充してください。";
        public string printSubErrorMsg = "完了しましたらエラー解除ボタンを押してください。";

        public string unChanged = "未変更";
        public string dayTypeBefore = "デフォルトの曜日タイプ";
        public string dayTypeAfter = "変更後の曜日タイプ";
        public string startTimeBefore = "デフォルトの開始時間";
        public string startTimeAfter = "変更御の開始時間";
        public string endTimeBefore = "デフォルトの終了時間";
        public string endTimeAfter = "変更後の終了時間";
        public string openTimeInstruction1 = "曜日タイプが変更された場合、指定された曜日の販売時間設定、\n商品構成に切り替わります。";
        public string openTimeInstruction2 = "デフォルトの開始時間より前に設定された場合、\nデフォルトの販売開始時間に販売される商品が早期開始対象になります。";
        public string openTimeInstruction3 = "デフォルトの終了時間より後に変更された場合、\nデフォルトの販売終了時間の直前に販売されている商品が延長対象になります。\n（売り切れ商品はそのまま）";
        public string openTimeSettingMessageTitle = "営業変更";
        public string openTimeSettingMessageContent = "設定内容を保存し前の画面に戻ります。変更内容は即時反映されます。宜しいですか？";
        public string openTimeCancelMessageTitle = "営業変更取消";
        public string openTimeCancelMessageContent = "設定内容を元に戻して前の画面に戻ります。宜しいですか？";

        public string gettingLabel = "了解";

        public int singleticketPrintPaperWidth = 500;
        public int singleticketPrintPaperHeight = 35 * 9;
        public int multiticketPrintPaperWidth = 500;
        public int multiticketPrintPaperHeight = 35 * 8;
        public int receiptPrintPaperWidth = 500;
        public int receiptPrintPaperHeight = 35 * 9;

        public int dailyReportPrintPaperWidth = 500;
        public int dailyReportPrintPaperHeight = 800;
        public int receiptReportPrintPaperWidth = 500;
        public int receiptReportPrintPaperHeight = 800;

        public int categorylistPrintPaperWidth = 500;
        public int categorylistPrintPaperHeight = 800;
        public int grouplistPrintPaperWidth = 500;
        public int grouplistPrintPaperHeight = 800;

        public int fontSizeBig = 14;
        public int fontSizeMedium = 10;
        public int fontSizeSmall = 8;

        public string[] tbNames = new string[] { "CategoryTB", "ProductTB", "CategoryDetailTB", "SaleTB", "TableSetTicket", "TableSetReceipt", "TableSetStore", "DaySaleTB", "ReceiptTB", "CancelOrderTB", "TableGroupName", "TableGroupDetail", "GeneralTB", "TableSetAudio" };

        public string[] dayTypeValue = new string[] { "平日", "土曜", "日曜" };

        public string[] months1 = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        public string[] dates1 = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
        public string[] months2 = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        public string[] dates2 = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
        public string[] times = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        public string[] end_times = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        public string[] minutes = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
        public string[] end_minutes = new string[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
        public string[] main_Menu = new string[3] { "メンテナンス", "販売画面", "メニュー読込" };
        public string[] main_Menu_Name = new string[3] { "maintenance", "salescreen", "readingmenu" };
        public string[] saleCategories = new string[] { "定番ラーメン", "替わり唾ラーメン", "トッピング", "ご飯、餃子", "ドリンク" };
        public int[] saleCategoryLayout = new int[] { 16, 25, 9, 13, 21 };
        public string[] saleCategories_btnName = new string[] { "category_1", "category_2", "category_3", "category_4", "category_5" };
        public string[] transactionLabelName = new string[] { "投入 金額", "購入 金額", "釣銭" };
        public string[] productAmount = new string[] { "1 枚", "2 枚", "3 枚", "4 枚", "5 枚", "6 枚", "7 枚", "8 枚", "9 枚" };

        public string dialogTitle = "注文メニュー";
        public string dialogInstruction = "複数注文時はプルダウンで選ん\n決定ボタンを押して下さい。";

        public string saleScreenTopTitle = "いらっしゃいませ\n定番メニューがおすすめです。";
        public string main_Menu_Title = "処理を選択して下さい。";
        public string upButtonName = "upButton";
        public string downButtonName = "downButton";
        public string ticketingButtonText = "発券";
        public string cancelButtonText = "取消";
        public string receiptButtonText = "領収書";
        public string deleteText = "削除";
        public string backText = "戻る";
        public string[] maintanenceLabel = new string[] { "各種処理", "内容表示", "設定" };
        public string[][] maintanenceButton = new string[][]
        {
            new string[] {  "売切れ設定", "締め処理", "誤購入取消" },
            new string[] { "商品品目", "カテゴリー", "グループ" },
            new string[] { "時刻設定", "暗証番号", "営業変更" }
        };

        public string[] maitanenceButtonImage = new string[]
        {
            @"D:\\ovan\\Ovan_P1\\images\\menubutton1.png",
            @"D:\\ovan\\Ovan_P1\\images\\menubutton2.png",
            @"D:\\ovan\\Ovan_P1\\images\\menubutton3.png"
        };

        public string[] closingProcessLabel = new string[] { "手動での締め処理", "日報の処理", "領収書の処理", "ログ表示" };
        public string[][] closingProcessButton = new string[][]
        {
            new string[] {  "手動締め処理開始", "締め処理解除" },
            new string[] { "表示", "印刷" },
            new string[] { "表示", "印刷" },
            new string[] { "表示", "" }
        };
        public string[] closingProcessButtonImage = new string[]
        {
            @"D:\\ovan\\Ovan_P1\\images\\menubutton1.png",
            @"D:\\ovan\\Ovan_P1\\images\\menubutton2.png",
            @"D:\\ovan\\Ovan_P1\\images\\menubutton3.png",
            @"D:\\ovan\\Ovan_P1\\images\\menubutton1.png"
       };



        public string dropdownArrowUpIcon = @"D:\\ovan\\Ovan_P1\\images\\arrow_up.png";
     //   public string dropdownArrowDownIcon = @"D:\\ovan\\Ovan_P1\\images\\arrow_down_icon.png";
        public string dropdownArrowDownIcon = @"D:\\ovan\\Ovan_P1\\images\\arrow_down.png";
        public string backButton = @"D:\\ovan\\Ovan_P1\\images\\back_new.png";
        public string powerButton = @"D:\\ovan\\Ovan_P1\\images\\power_button.png";
        public string soldoutBadge = @"D:\\ovan\\Ovan_P1\\images\\soldout.png";


        Color[] saleCategoryButtonColor = new Color[5] {
            Color.FromArgb(255, 255, 192, 0),
            Color.FromArgb(255, 255, 204, 255),
            Color.FromArgb(255, 204, 255, 153),
            Color.FromArgb(255, 204, 255, 255),
            Color.FromArgb(255, 255, 255, 204)
        };
        Color[] saleCategoryButtonBorderColor = new Color[5] {
            Color.FromArgb(255, 255, 148, 0),
            Color.FromArgb(255, 255, 153, 204),
            Color.FromArgb(255, 51, 204, 51),
            Color.FromArgb(255, 0, 176, 240),
            Color.FromArgb(255, 255, 192, 0)
        };

        public Color[][] pattern_Clr = new Color[][]
        {
            new Color[] { Color.FromArgb(255, 255, 192, 0), Color.FromArgb(255, 255, 153, 204), Color.FromArgb(255, 50, 204, 50),Color.FromArgb(255, 0, 176, 240),Color.FromArgb(255, 255, 255, 204),Color.FromArgb(255, 255, 204, 255),Color.FromArgb(255, 204, 255, 153),Color.FromArgb(255, 204, 255, 255)},
            new Color[] { Color.FromArgb(255, 255, 153, 51), Color.FromArgb(255, 255, 102, 153), Color.FromArgb(255, 0, 153,0),Color.FromArgb(255, 0, 0, 255),Color.FromArgb(255, 255, 192, 0),Color.FromArgb(255, 255, 153, 204),Color.FromArgb(255, 51, 204, 51),Color.FromArgb(255, 0, 176, 240)},
            new Color[] { Color.FromArgb(255, 0, 0, 255), Color.FromArgb(255, 0, 176, 240), Color.FromArgb(255, 0, 0, 255),Color.FromArgb(255, 0, 204, 255),Color.FromArgb(255, 147, 219, 255),Color.FromArgb(255, 204, 255, 255),Color.FromArgb(255, 147, 219, 255),Color.FromArgb(255, 204, 255, 255)},
            new Color[] { Color.FromArgb(255, 255, 102, 0), Color.FromArgb(255, 251, 151, 0), Color.FromArgb(255, 255, 102, 0),Color.FromArgb(255, 251, 151, 0),Color.FromArgb(255, 253, 232, 141),Color.FromArgb(255, 255, 255, 204),Color.FromArgb(255, 253, 232, 141),Color.FromArgb(255, 255, 255, 204)},
            new Color[] { Color.FromArgb(255, 0, 176, 240), Color.FromArgb(255, 253, 241, 0), Color.FromArgb(255, 0, 204, 255),Color.FromArgb(255, 253, 241, 0),Color.FromArgb(255, 204, 255, 255),Color.FromArgb(255, 255, 255, 204),Color.FromArgb(255, 204, 255, 255),Color.FromArgb(255, 255, 255, 204)},
            new Color[] { Color.FromArgb(255, 51, 204, 51), Color.FromArgb(255, 253, 241, 0), Color.FromArgb(255, 50, 204, 50),Color.FromArgb(255, 253, 241, 0),Color.FromArgb(255, 204, 255, 153),Color.FromArgb(255, 255, 255, 204),Color.FromArgb(255, 204, 255, 153),Color.FromArgb(255, 255, 255, 204)},
            new Color[] { Color.FromArgb(255, 192, 0, 0), Color.FromArgb(255, 120, 147, 60), Color.FromArgb(255, 228, 108, 10),Color.FromArgb(255, 55, 96, 146),Color.FromArgb(255, 242, 220, 220),Color.FromArgb(255, 215, 228, 190),Color.FromArgb(255, 252, 213, 181),Color.FromArgb(255, 142, 180, 227)}
        };

        public Color[] getSaleCategoryButtonColor()
        {
            return saleCategoryButtonColor;
        }
        public Color[] getSaleCategoryButtonBorderColor()
        {
            return saleCategoryButtonBorderColor;
        }

        public DateTime sumDayTimeStart(string storeEndTime)
        {
            DateTime sumDayTime = DateTime.Now;
            if (String.Compare("00:00", storeEndTime) == 0)
            {
                sumDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), 00, 00, 00);
            }
            else
            {
                if(int.Parse(DateTime.Now.ToString("HH")) < int.Parse(storeEndTime.Split(':')[0]))
                {
                    sumDayTime = new DateTime(int.Parse(DateTime.Now.AddDays(-1).ToString("yyyy")), int.Parse(DateTime.Now.AddDays(-1).ToString("MM")), int.Parse(DateTime.Now.AddDays(-1).ToString("dd")), int.Parse(storeEndTime.Split(':')[0]), 00, 00);
                }
                else
                {
                    sumDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), int.Parse(storeEndTime.Split(':')[0]), 00, 00);
                }
            }

            return sumDayTime;

        }

        public DateTime sumDayTimeEnd(string storeEndTime)
        {
            DateTime sumDayTime = DateTime.Now;
            sumDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), int.Parse(storeEndTime.Split(':')[0]), 00, 00);
            if(storeEndTime.Split(':')[0] == "00")
            {
                sumDayTime = sumDayTime.AddDays(1).AddSeconds(-1);
            }
            else
            {
                sumDayTime = sumDayTime.AddSeconds(-1);
            }

            return sumDayTime;
        }

        public DateTime currentDateTimeFromTime(string time)
        {
            DateTime sumDayTime = DateTime.Now;
            sumDayTime = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("dd")), int.Parse(time.Split(':')[0]), 00, 00);
            sumDayTime = sumDayTime.AddSeconds(-1);

            return sumDayTime;
        }

        public string sumDate(string storeEndTime)
        {
            DateTime now = DateTime.Now;
            string sumDate = now.ToString("yyyy-MM-dd");
            if (String.Compare("00:00", storeEndTime) <= 0 && String.Compare("06:00", storeEndTime) >= 0)
            {
                if (int.Parse(DateTime.Now.ToString("HH")) < int.Parse(storeEndTime.Split(':')[0]))
                {
                    sumDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                else
                {
                    sumDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                if (int.Parse(DateTime.Now.ToString("HH")) < int.Parse(storeEndTime.Split(':')[0]))
                {
                    sumDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    sumDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                }
            }

            return sumDate;
        }

    }
}
