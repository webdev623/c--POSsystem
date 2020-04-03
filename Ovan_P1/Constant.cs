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
        public string[] months1 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        public string[] dates1 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
        public string[] months2 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        public string[] dates2 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
        public string[] main_Menu = new string[3] { "メンテナンス", "販売画面", "メニュー読込" };
        public string[] main_Menu_Name = new string[3] { "maintenance", "salescreen", "readingmenu" };
        public string[] saleCategories = new string[] { "定番ラーメン", "替わり唾ラーメン", "トッピング", "ご飯、餃子", "ドリンク" };
        public string[] saleCategories_btnName = new string[] { "category_1", "category_2", "category_3", "category_4", "category_5" };
        public string[] transactionLabelName = new string[] { "投入 金額", "購入 金額", "釣銭" };
        public string[] productAmount = new string[] { "1 枚", "2 枚", "3 枚", "4 枚", "5 枚", "6 枚", "7 枚", "8 枚", "9 枚" };
        public string[] productBigImageUrl = new string[]
        {
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png"
        };
        public string[] productSmallImageUrl = new string[]
        {
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png",
            @"D:\\ovan\\Ovan_P1\\images\\category1.png"
        };
        public string[] productBigBadgeImageUrl = new string[]
        {
            @"D:\\ovan\\Ovan_P1\\images\\badge1.png",
            "",
            "",
            @"D:\\ovan\\Ovan_P1\\images\\badge1.png"
        };
        public string[] productSmallBadgeImageUrl = new string[]
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };



        public string[][] productBigName = new string[][]{
            new string[] { "醤油ラーメン", "塩ラーメン", "味噌ラーメン", "つけ麺", "チャーシュー麺", "特製ラーメン", "全部乗せラーメン", "特製つけ麺", "油そば", "チャーシュー", "メンマ" },
            new string[] { "味噌ラーメン", "塩ラーメン", "つけ麺", "全部乗せラーメン", "チャーシュー麺", "醤油ラーメン", "特製ラーメン", "全部乗せラーメン", "特製つけ麺", "油そば", "チャーシュー", "メンマ"},
            new string[] {  "チャーシュー", "醤油ラーメン", "特製ラーメン", "塩ラーメン", "味噌ラーメン", "つけ麺", "チャーシュー麺", "全部乗せラーメン", "特製つけ麺", "メンマ" },
            new string[] { "塩ラーメン", "味噌ラーメン", "つけ麺", "チャーシュー麺", "醤油ラーメン", "特製ラーメン", "全部乗せラーメン", "特製つけ麺", "油そば", "チャーシュー", "メンマ"  },
            new string[] { "塩ラーメン", "醤油ラーメン", "味噌ラーメン", "メンマ", "チャーシュー麺", "特製ラーメン", "全部乗せラーメン", "つけ麺", "油そば", "チャーシュー" },
        };
        public string[] productSmallName = new string[] { "ご飯（小）", "ご飯（大)", "替え麺", "大盛り ", "コーン", "玉子", "メンマ", "焼き海苔", "焼き豚" };
        public int[][] productBigPrice = new int[][]{
           new int[] { 700, 600, 900, 700, 900, 900, 1100, 700, 800, 700, 600 },
           new int[] { 700, 900, 600, 900, 700, 900, 1100, 700, 800, 700, 600, 800 },
           new int[] { 600, 900, 700, 900, 900, 1100, 700, 800, 700, 600 },
           new int[] { 900, 700, 600, 700, 900, 900, 1100, 700, 800, 700, 600 },
           new int[] { 900, 900, 700, 900, 900, 1100, 700, 800, 700, 600 },
        };
        public string[][] productBigSaleAmount = new string[][]{
            new string[] { "0(0)", "0(0)", "10(20)", "10(0)", "1(2)", "10(2)", "1(12)", "10(2)", "3(2)", "15(101)", "0(20)" },
            new string[] { "10(20)", "0(0)", "1(2)", "0(0)", "12(0)", "0(103)", "3(2)", "9(1)", "10(2)", "10(3)", "0(20)", "2(15)" },
            new string[] { "0(3)", "10(20)", "3(0)", "3(12)", "2(12)", "11(4)", "10(2)", "2(3)", "1(5)", "0(20)" },
            new string[] { "10(2)", "0(0)", "0(0)", "10(20)", "3(10)", "1(2)", "21(121)", "14(5)", "2(31)", "1(5)", "0(20)" },
            new string[] { "0(20)", "0(0)", "10(20)", "3(0)", "1(2)", "27(13)", "15(6)", "10(2)", "31(7)", "1(5)" },
        };
        public string[][] productBigSaleStatus = new string[][]{
            new string[] { "販売中", "販売中", "販売中", "販売中", "中止", "販売中", "販売中", "販売中", "販売中", "中止", "販売中" },
            new string[] { "販売中", "販売中", "販売中", "中止", "販売中", "中止", "販売中", "販売中", "販売中", "販売中", "中止", "販売中" },
            new string[] { "販売中", "販売中", "販売中", "中止", "販売中", "販売中", "販売中", "販売中", "中止", "販売中" },
            new string[] { "販売中", "販売中", "販売中", "販売中", "販売中", "販売中", "中止", "販売中", "販売中", "中止", "販売中" },
            new string[] { "販売中", "販売中", "販売中", "販売中", "中止", "販売中", "販売中", "販売中", "中止", "販売中" },
        };
        public int[] productSmallPrice = new int[] { 100, 200, 150, 100, 100, 100, 200, 100, 100 };

        public int[] productSoldAmount = new int[] { 100, 20, 15, 10, 10, 14, 22, 10, 10, 30, 20, 10 };

        public int[] recieptIssuePrice = new int[] { 700, 900, 600, 900, 700, 900, 1100, 700, 800, 700, 600, 800 };
        public int[] recieptIssueAmount = new int[] { 10, 2, 15, 1, 1, 4, 2, 1, 1, 3, 2, 2 };

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



        public string dropdownArrowUpIcon = @"D:\\ovan\\Ovan_P1\\images\\arrow_up_icon.png";
        public string dropdownArrowDownIcon = @"D:\\ovan\\Ovan_P1\\images\\arrow_down_icon.png";


        Color[] saleCategoryButtonColor = new Color[5] {
            Color.FromArgb(255, 255, 192, 0),
            Color.FromArgb(255, 255, 204, 255),
            Color.FromArgb(255, 204, 255, 153),
            Color.FromArgb(255, 204, 255, 255),
            Color.FromArgb(255, 255, 255, 204)
        };
        Color[] saleCategoryButtonBorderColor = new Color[5] {
            Color.FromArgb(255, 255, 0, 0),
            Color.FromArgb(255, 255, 153, 204),
            Color.FromArgb(255, 51, 204, 51),
            Color.FromArgb(255, 0, 176, 240),
            Color.FromArgb(255, 255, 192, 0)
        };

        public Color[] getSaleCategoryButtonColor()
        {
            return saleCategoryButtonColor;
        }
        public Color[] getSaleCategoryButtonBorderColor()
        {
            return saleCategoryButtonBorderColor;
        }

    }
}
