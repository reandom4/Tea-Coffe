using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime;
using static Tea_Coffe.Window1;

using Word = Microsoft.Office.Interop.Word;

namespace Tea_Coffe
{
    internal class WordHelper
    {
        readonly DataBase dataBase = new DataBase();
        // Создание чека
        public void Creatcheque(List<ProductItem> items, DateTime dateTime,int userid)
        {
            Word.Application wordApp = new Word.Application
            {
                Visible = true
            };

            // Создаем новый документ
            Word.Document doc = wordApp.Documents.Add();

            doc.PageSetup.PageWidth = wordApp.InchesToPoints(3.5f);  // Ширина в дюймах
            doc.PageSetup.PageHeight = wordApp.InchesToPoints(11f);

            doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1f);


            // Создаем новый параграф и добавляем текст перед таблицей
            Word.Paragraph titlePara2 = doc.Paragraphs.Add();
            titlePara2.Range.Text = "Магазин чая и кофе";
            titlePara2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            titlePara2.Format.SpaceAfter = 12; // 12 единиц - это расстояние в точках
            titlePara2.Range.Font.Bold = 1;
            titlePara2.Range.Font.Size = 12;
            titlePara2.Format.SpaceAfter = 0;
            doc.Paragraphs.Add();
            Word.Paragraph paradoc = doc.Paragraphs.Add();

            // Добавляем таблицу
            Word.Table table = doc.Tables.Add(paradoc.Range, items.Count + 1, 3);

            table.Borders.Enable = 1;
            table.Range.Font.Size = 8; // Размер шрифта для всей таблицы
            table.Range.Font.Bold = 0; // Не жирный для всей таблицы
            table.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Columns[1].Width = 3.49f * 28.35f;
            table.Columns[2].Width = 1.65f * 28.35f;
            table.Columns[3].Width = 1.65f * 28.35f;
            int fullcost = 0;
            for (int i = 0; i < items.Count; i++)
            {
                table.Cell(i + 1, 1).Range.Text = items[i].Name;
                table.Cell(i + 1, 1).Range.ParagraphFormat.SpaceAfter = 0;
                table.Cell(i + 1, 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                table.Cell(i + 1, 2).Range.Text = items[i].BasketQuantity.ToString() + items[i].Unit;
                table.Cell(i + 1, 2).Range.ParagraphFormat.SpaceAfter = 0;
                table.Cell(i + 1, 2).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                table.Cell(i + 1, 3).Range.Text = items[i].BasketCost.ToString() + "₽";
                table.Cell(i + 1, 3).Range.ParagraphFormat.SpaceAfter = 0;
                table.Cell(i + 1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                fullcost += items[i].BasketCost;
            }
            table.Cell(items.Count + 1, 1).Range.Text = "Итого";
            table.Cell(items.Count + 1, 3).Range.Text = fullcost.ToString() + "₽";
            table.Cell(items.Count + 1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Cell(items.Count + 1, 1).Range.ParagraphFormat.SpaceAfter = 0;
            table.Cell(items.Count + 1, 2).Range.ParagraphFormat.SpaceAfter = 0;
            table.Cell(items.Count + 1, 3).Range.ParagraphFormat.SpaceAfter = 0;

            
            string name = dataBase.GetUserName(userid);
            int checknumber = dataBase.GetLastOrderId();
            CompanyInfo inf = GetSettings();
            Word.Paragraph textAfterTable = doc.Paragraphs.Add();
            textAfterTable.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; // Выравнивание по правому краю
            textAfterTable.Range.Font.Bold = 0;
            textAfterTable.Range.Font.Size = 8;
            textAfterTable.Format.SpaceAfter = 0;
            textAfterTable.Range.Text =  $"Адрес: {inf.Adres}\n" +
                              $"Место расчета: г. {inf.Place}\n" +
                              $"КАССИР: {name}\n" +
                              $"Сайт ФНС: {inf.FNS}\n" +
                              $"СНО: {inf.SNO}\n" +
                              $"ЗН ККТ: {inf.KKT}\n" +
                              $"Чек №: {checknumber}\n" +
                              $"Дата и время: {dateTime:dd.MM.yyyy HH.mm.ss}" +
                              $"ИНН: {inf.INN}\n" +
                              $"РК ККТ:{inf.RKKKT}\n" +
                              $"ФН № {inf.FN}\n" +
                              $"ФД № {inf.FD}\n" +
                              $"ФП: {inf.FP}\n";


            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "чеки");
            string filePath = Path.Combine(chequesFolderPath, $"чек{dateTime:dd.MM.yyyy HH.mm.ss}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и приложение Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine("Чек успешно создан.");
        }
        // Создание отчета о популярных товарах
        public void Createpopular(List<ProductItem> items)
        {
            Word.Application wordApp = new Word.Application
            {
                Visible = true
            };

            // Создаем новый документ
            Word.Document doc = wordApp.Documents.Add();

            doc.PageSetup.PageWidth = wordApp.InchesToPoints(4.2f);  // Ширина в дюймах
            doc.PageSetup.PageHeight = wordApp.InchesToPoints(11f);

            doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1f);


            // Создаем новый параграф и добавляем текст перед таблицей
            Word.Paragraph titlePara2 = doc.Paragraphs.Add();
            titlePara2.Range.Text = "Магазин чая и кофе";
            titlePara2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            titlePara2.Format.SpaceAfter = 12; // 12 единиц - это расстояние в точках
            titlePara2.Range.Font.Bold = 1;
            titlePara2.Range.Font.Size = 12;
            titlePara2.Format.SpaceAfter = 0;
            doc.Paragraphs.Add();
            Word.Paragraph paradoc = doc.Paragraphs.Add();

            // Добавляем таблицу
            Word.Table table = doc.Tables.Add(paradoc.Range, items.Count + 1, 5);

            table.Borders.Enable = 1;
            table.Range.Font.Size = 8; // Размер шрифта для всей таблицы
            table.Range.Font.Bold = 0; // Не жирный для всей таблицы
            table.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Columns[1].Width = 0.7f * 28.35f;
            table.Columns[2].Width = 1.49f * 28.35f;
            table.Columns[3].Width = 3.49f * 28.35f;
            table.Columns[4].Width = 1.5f * 28.35f;
            table.Columns[5].Width = 1.8f * 28.35f;


            table.Cell(1, 1).Range.Text = "№";
            table.Cell(1, 2).Range.Text = "Группа";
            table.Cell(1, 3).Range.Text = "Товар";
            table.Cell(1, 4).Range.Text = "Заказано";
            table.Cell(1, 5).Range.Text = "Сумма";

            for (int i = 0; i < items.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = (i + 1).ToString();

                table.Cell(i + 2, 2).Range.Text = items[i].Category;

                table.Cell(i + 2, 3).Range.Text = items[i].Name;

                table.Cell(i + 2, 4).Range.Text = items[i].Total_quantity.ToString() + items[i].Unit;

                table.Cell(i + 2, 5).Range.Text = (items[i].Cost * items[i].Total_quantity / items[i].MinUnit).ToString("0.00") + "₽";

            }



            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "чеки");
            string filePath = Path.Combine(chequesFolderPath, $"Отчет о наиболее популярных товарах{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и приложение Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine("Чек успешно создан.");
        }
        // Создание отчета отчета о выручке
        public void CreateBill(List<ProductItem> items)
        {
            Word.Application wordApp = new Word.Application
            {
                Visible = true
            };

            // Создаем новый документ
            Word.Document doc = wordApp.Documents.Add();

            doc.PageSetup.PageWidth = wordApp.InchesToPoints(4.2f);  // Ширина в дюймах
            doc.PageSetup.PageHeight = wordApp.InchesToPoints(11f);

            doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1f);


            // Создаем новый параграф и добавляем текст перед таблицей
            Word.Paragraph titlePara2 = doc.Paragraphs.Add();
            titlePara2.Range.Text = "Магазин чая и кофе";
            titlePara2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            titlePara2.Format.SpaceAfter = 12; // 12 единиц - это расстояние в точках
            titlePara2.Range.Font.Bold = 1;
            titlePara2.Range.Font.Size = 12;
            titlePara2.Format.SpaceAfter = 0;
            doc.Paragraphs.Add();
            Word.Paragraph paradoc = doc.Paragraphs.Add();

            // Добавляем таблицу
            Word.Table table = doc.Tables.Add(paradoc.Range, items.Count + 1, 5);

            table.Borders.Enable = 1;
            table.Range.Font.Size = 8; // Размер шрифта для всей таблицы
            table.Range.Font.Bold = 0; // Не жирный для всей таблицы
            table.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Columns[1].Width = 0.7f * 28.35f;
            table.Columns[2].Width = 1.49f * 28.35f;
            table.Columns[3].Width = 3.49f * 28.35f;
            table.Columns[4].Width = 1.5f * 28.35f;
            table.Columns[5].Width = 1.8f * 28.35f;


            table.Cell(1, 1).Range.Text = "№";
            table.Cell(1, 2).Range.Text = "Группа";
            table.Cell(1, 3).Range.Text = "Товар";
            table.Cell(1, 4).Range.Text = "Заказано";
            table.Cell(1, 5).Range.Text = "Сумма";
            double fulcost = 0;
            for (int i = 0; i < items.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = (i + 1).ToString();

                table.Cell(i + 2, 2).Range.Text = items[i].Category;

                table.Cell(i + 2, 3).Range.Text = items[i].Name;

                table.Cell(i + 2, 4).Range.Text = items[i].Total_quantity.ToString() + items[i].Unit;

                table.Cell(i + 2, 5).Range.Text = (items[i].Cost * items[i].Total_quantity / items[i].MinUnit).ToString("0.00") + "₽";
                fulcost += (items[i].Cost * items[i].Total_quantity / items[i].MinUnit);
            }

            Word.Paragraph textAfterTable = doc.Paragraphs.Add();
            textAfterTable.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; // Выравнивание по правому краю
            textAfterTable.Range.Font.Bold = 0;
            textAfterTable.Range.Font.Size = 8;
            textAfterTable.Format.SpaceAfter = 0;
            textAfterTable.Range.Text = $"Общая выручка {fulcost:0.00}₽";
            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "Популярные товары");
            string filePath = Path.Combine(chequesFolderPath, $"Отчет о выручке{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и приложение Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine("Чек успешно создан.");
        }
        // Создание отчета о среднем чеке
        public void Process(Dictionary<string, string> items)
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string fileInfo = Path.Combine(programPath, "avg.docx");
            Word.Application app = new Word.Application();


            //создание экземпляра класса
            app = new Word.Application();
            object fille = fileInfo;
            object missing = Type.Missing;
            app.Documents.Open(fille);

            app.Visible = true;

            //замена всех вхождений
            foreach (var item in items)
            {
                Word.Find find = app.Selection.Find;
                find.Text = item.Key;
                find.Replacement.Text = item.Value;

                object wrap = Word.WdFindWrap.wdFindContinue;
                object replace = Word.WdReplace.wdReplaceAll;

                //параметры документа
                find.Execute(FindText: Type.Missing,
                    MatchCase: false,
                    MatchWholeWord: false,
                    MatchWildcards: false,
                    MatchSoundsLike: missing,
                    MatchAllWordForms: false,
                    Forward: true,
                    Wrap: wrap,
                    Format: false,
                    ReplaceWith: missing, Replace: replace);
            }
            //сохранение в новый файл
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "Средний чек");
            string filePath = Path.Combine(chequesFolderPath, $"Средний чек{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");
            app.ActiveDocument.SaveAs(filePath);
            //app.ActiveDocument.Close();
            //app.Quit();

        }
        // Создание отчета инвентаризации
        public void CreateStorage(List<ProductItem> items)
        {
            Word.Application wordApp = new Word.Application
            {
                Visible = true
            };

            // Создаем новый документ
            Word.Document doc = wordApp.Documents.Add();

            doc.PageSetup.PageWidth = wordApp.InchesToPoints(4.2f);  // Ширина в дюймах
            doc.PageSetup.PageHeight = wordApp.InchesToPoints(11f);

            doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1f);


            // Создаем новый параграф и добавляем текст перед таблицей
            Word.Paragraph titlePara2 = doc.Paragraphs.Add();
            titlePara2.Range.Text = "Магазин чая и кофе";
            titlePara2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            titlePara2.Format.SpaceAfter = 12; // 12 единиц - это расстояние в точках
            titlePara2.Range.Font.Bold = 1;
            titlePara2.Range.Font.Size = 12;
            titlePara2.Format.SpaceAfter = 0;
            doc.Paragraphs.Add();
            Word.Paragraph paradoc = doc.Paragraphs.Add();

            // Добавляем таблицу
            Word.Table table = doc.Tables.Add(paradoc.Range, items.Count + 1, 3);

            table.Borders.Enable = 1;
            table.Range.Font.Size = 8; // Размер шрифта для всей таблицы
            table.Range.Font.Bold = 0; // Не жирный для всей таблицы
            table.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Columns[1].Width = 3.50f * 28.35f;
            table.Columns[2].Width = 1.9f * 28.35f;
            table.Columns[3].Width = 1.9f * 28.35f;


            table.Cell(1, 1).Range.Text = "Товар";
            table.Cell(1, 2).Range.Text = "Количество на складе";
            table.Cell(1, 3).Range.Text = "Реальное количество";

            for (int i = 0; i < items.Count; i++)
            {
                table.Cell(i + 2, 1).Range.Text = items[i].Name;

                table.Cell(i + 2, 2).Range.Text = items[i].Quantity.ToString() + items[i].Unit.ToString();
            }

            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "Инвентаризация");
            string filePath = Path.Combine(chequesFolderPath, $"Инвентаризация{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и приложение Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine("Чек успешно создан.");
        }
        // Получение настроек
        public CompanyInfo GetSettings()
        {
            string configFile = "setting.ini";

            // Проверка существования файла

            // Чтение содержимого файла и разбивка его на строки
            string[] lines = File.ReadAllLines(configFile);

            // Создание словаря для хранения переменных конфигурации
            Dictionary<string, string> config = new Dictionary<string, string>();

            // Обработка каждой строки файла
            foreach (string line in lines)
            {
                // Разделение строки на ключ и значение
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    // Добавление в словарь
                    config[parts[0].Trim()] = parts[1].Trim();
                }
            }

            // Использование переменных конфигурации
            CompanyInfo info = new CompanyInfo();
            if (config.TryGetValue("Адрес", out string dbString))
            {
                info.Adres = dbString;
            }
            if (config.TryGetValue("Место_расчета", out dbString))
            {
                info.Place = dbString;
            }
            if (config.TryGetValue("КАССИР", out dbString))
            {
                info.Kassir = dbString;
            }
            if (config.TryGetValue("Сайт_ФНС", out dbString))
            {
                info.FNS = dbString;
            }
            if (config.TryGetValue("СНО", out dbString))
            {
                info.SNO = dbString;
            }
            if (config.TryGetValue("ЗНККТ", out dbString))
            {
                info.KKT = dbString;
            }
            if (config.TryGetValue("ИНН", out dbString))
            {
                info.INN = dbString;
            }
            if (config.TryGetValue("РКККТ", out dbString))
            {
                info.RKKKT = dbString;
            }
            if (config.TryGetValue("ФН", out dbString))
            {
                info.FN = dbString;
            }
            if (config.TryGetValue("ФД", out dbString))
            {
                info.FD = dbString;
            }
            if (config.TryGetValue("ФП", out dbString))
            {
                info.FP = dbString;
            }
            return info;

        }
        public class CompanyInfo
        {
            public string Adres { get; set; }
            public string Place { get; set; }
            public string Kassir { get; set; }
            public string FNS { get; set; }
            public string SNO { get; set; }
            public string KKT { get; set; }
            public string INN { get; set; }
            public string RKKKT { get; set; }
            public string FN { get; set; }
            public string FD { get; set; }
            public string FP { get; set; }
        }
        }
}
