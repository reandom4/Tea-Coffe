using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using static Tea_Coffe.Window1;
using Word = Microsoft.Office.Interop.Word;

namespace Tea_Coffe
{
    internal class WordHelper
    {
        public void Creatcheque(List<ProductItem> items, DateTime dateTime)
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
            
            

            Word.Paragraph textAfterTable = doc.Paragraphs.Add();
            textAfterTable.Format.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft; // Выравнивание по правому краю
            textAfterTable.Range.Font.Bold = 0;
            textAfterTable.Range.Font.Size = 8;
            textAfterTable.Format.SpaceAfter = 0;
            textAfterTable.Range.Text = "Адрес: Московская обл. Балашиха г." +
                              "Место расчета: г. Заводская ул. Бажанова д. 14\n" +
                              "КАССИР: Иванов С.В.\n" +
                              "Сайт ФНС: www.nalog.ru\n" +
                              "СНО: ОСН\n" +
                              "ЗН ККТ: 0123456789\n" +
                              "Чек №: 9876543210\n" +
                              $"Дата и время: {dateTime:dd.MM.yyyy HH.mm.ss}" +
                              "ИНН: 123456789012\n" +
                              "РК ККТ: 456789\n" +
                              "ФН № 987654321098\n" +
                              "ФД № example.ofd.com\n" +
                              "ФП: 123123123\n";


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

        public void CreateAvg(double avg)
        {
            // Создаем объект приложения Word
            Word.Application wordApp = new Word.Application
            {
                Visible = true
            };
            // Создаем новый документ
            Document doc = wordApp.Documents.Add();

            // Устанавливаем размер страницы документа на минимальное значение
            doc.PageSetup.PaperSize = WdPaperSize.wdPaperA5;
            doc.PageSetup.Orientation = WdOrientation.wdOrientPortrait;

            // Добавляем заголовок
            Paragraph title = doc.Paragraphs.Add();
            title.Range.Text = "Магазин чая и кофе";
            title.Range.Font.Size = 24;
            title.Range.Font.Bold = 1; // Жирный шрифт
            title.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; // Выравнивание по центру
            title.Range.InsertParagraphAfter();

            // Получаем средний чек из базы данных
            double averageCheck = avg;

            // Добавляем информацию о среднем чеке
            Paragraph averageCheckParagraph = doc.Paragraphs.Add();
            averageCheckParagraph.Range.Text = $"Средний чек = {averageCheck:0.00}";
            averageCheckParagraph.Range.Font.Size = 16;
            averageCheckParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            averageCheckParagraph.Range.InsertParagraphAfter();
            
            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "Средний чек");
            string filePath = Path.Combine(chequesFolderPath, $"Отчет о средним чеке{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и выходим из Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine($"Документ успешно создан и сохранен по пути: {filePath}");
        }

        public void Createpopular(List<ProductItem> items )
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
            Word.Table table = doc.Tables.Add(paradoc.Range, items.Count+1, 5);

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
                table.Cell(i + 2, 1).Range.Text = (i+1).ToString();

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
    }
}
