using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static Tea_Coffe.Window1;
using Word = Microsoft.Office.Interop.Word;

namespace Tea_Coffe
{
    internal class WordHelper
    {
        public void creatcheque(List<ProductItem> items,DateTime dateTime)
        {
            Word.Application wordApp = new Word.Application();
            wordApp.Visible = true;

            // Создаем новый документ
            Word.Document doc = wordApp.Documents.Add();

            doc.PageSetup.PageWidth = wordApp.InchesToPoints(3.5f);  // Ширина в дюймах
            doc.PageSetup.PageHeight = wordApp.InchesToPoints(11f);

            doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(1f);
            doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1f);

            // Создаем новый параграф и добавляем текст перед таблицей
            Word.Paragraph titlePara = doc.Paragraphs.Add();
            titlePara.Range.Text = "Магазин чая и кофе";
            titlePara.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            titlePara.Format.SpaceAfter = 12; // 12 единиц - это расстояние в точках
            titlePara.Range.Font.Bold = 1;
            titlePara.Range.Font.Size = 12;

            // Добавляем пустой абзац перед таблицей
            doc.Paragraphs.Add();

            // Добавляем таблицу
            Word.Table table = doc.Tables.Add(doc.Paragraphs[doc.Paragraphs.Count].Range, items.Count + 1, 3);

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
            table.Cell(items.Count + 1, 3).Range.Text = fullcost.ToString();
            table.Cell(items.Count + 1, 3).Range.ParagraphFormat.SpaceAfter = 0;
            table.Cell(items.Count + 1, 3).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            table.Rows.Alignment = Word.WdRowAlignment.wdAlignRowCenter;
            table.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            Word.Paragraph textAfterTable = doc.Paragraphs.Add();
            textAfterTable.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight; // Выравнивание по правому краю
            textAfterTable.Range.Text = dateTime.ToString("dd.MM.yyyy HH.mm.ss");
            textAfterTable.Range.Font.Bold = 0;
            textAfterTable.Range.Font.Size = 8;

            // Сохраняем документ
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "чеки");
            string filePath = Path.Combine(chequesFolderPath, $"чек{dateTime.ToString("dd.MM.yyyy HH.mm.ss")}.docx");
            doc.SaveAs2(filePath);

            // Закрываем документ и приложение Word
            //doc.Close();
            //wordApp.Quit();

            Console.WriteLine("Чек успешно создан.");
        }
    }
}
