using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Tea_Coffe
{
    internal class WordDocCreator
    {

        public void CreateBill2()
        {
            // Путь к создаваемому документу
            string exePath = Assembly.GetExecutingAssembly().Location;
            string programPath = Path.GetDirectoryName(exePath);
            string chequesFolderPath = Path.Combine(programPath, "Отчеты", "Отчет о выручке");
            string filePath = Path.Combine(chequesFolderPath, $"Отчет о выручке{DateTime.Now:dd.MM.yyyy HH.mm.ss}.docx");


            // Создаем новый документ
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                // Добавляем основную часть документа
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = new Body();

                // Устанавливаем размеры страницы
                SectionProperties sectionProperties = new SectionProperties();
                PageSize pageSize = new PageSize
                {
                    Width = (UInt32Value)(29.7 * 567),  // ширина A4 в см
                    Height = (UInt32Value)(21.0 * 567), // высота A4 в см
                    Orient = PageOrientationValues.Landscape // Устанавливаем альбомную ориентацию
                };
                PageMargin pageMargin = new PageMargin
                {
                    Top = 567,      // 1 см сверху
                    Bottom = 567,   // 1 см снизу
                    Left = 567,     // 1 см слева
                    Right = 567,    // 1 см справа
                    Header = 720,   // 0.5 дюйма (по умолчанию)
                    Footer = 720,   // 0.5 дюйма (по умолчанию)
                    Gutter = 0      // Нет дополнительного отступа для переплета
                };

                sectionProperties.Append(pageSize);
                sectionProperties.Append(pageMargin);
                body.Append(sectionProperties);

                // Заголовок документа
                Paragraph titleParagraph1 = new Paragraph(new Run(new Text("Магазин чая и кофе")));
                titleParagraph1.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                body.AppendChild(titleParagraph1);

                Paragraph titleParagraph2 = new Paragraph(new Run(new Text("Отчет о выручке 01.06.2024 - 05.06.2024")));
                titleParagraph2.ParagraphProperties = new ParagraphProperties(new Justification() { Val = JustificationValues.Center });
                body.AppendChild(titleParagraph2);
                
                
                // Добавление таблицы
                Table table = new Table();
                TableGrid tableGrid = new TableGrid(
                    new GridColumn() { Width = "3cm" },   // Ширина первого столбца 0.75 см = 0.75 * 567 ≈ 425 Twips
                    new GridColumn() { Width = "3544" },  // Ширина второго столбца 6.25 см = 6.25 * 567 ≈ 3544 Twips
                    new GridColumn() { Width = "6662" },  // Ширина третьего столбца 11.75 см = 11.75 * 567 ≈ 6662 Twips
                    new GridColumn() { Width = "1701" },  // Ширина четвертого столбца 3 см = 3 * 567 ≈ 1701 Twips
                    new GridColumn() { Width = "2409" }   // Ширина пятого столбца 4.25 см = 4.25 * 567 ≈ 2409 Twips
                );
                table.AddChild(tableGrid);
                // Устанавливаем свойства таблицы
                TableProperties tblProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                    )
                );

                table.AppendChild(tblProperties);

                // Установка ширины столбцов
                

                // Заголовок таблицы
                TableRow headerRow = new TableRow();
                string[] headers = { "№", "Группа", "Товар", "Заказано", "Сумма" };
                foreach (string header in headers)
                {
                    TableCell cell = new TableCell(new Paragraph(new Run(new Text(header))));
                    headerRow.Append(cell);
                }
                table.Append(headerRow);
                // Данные таблицы
                List<(string, string, string, string, string)> data = new List<(string, string, string, string, string)>
            {
                ("1", "Улун", "Улун Да Хун Пао категория B", "4500г", "2304000₽"),
                ("2", "Улун", "Улун Манговый (Премиум)", "4400г", "2384800₽"),
                ("3", "Черный чай", "Чай черный Рубин Цейлона ОР1", "2700г", "993600₽"),
                ("4", "Улун", "Улун Жасминовый", "2400г", "1065600₽"),
                ("5", "Улун", "Улун Най Сян Цзинь Сюань (Молочный улун) кат. B", "2200г", "655600₽"),
                ("6", "Черный чай с добавками", "Чай черный Таежный с пуэром", "800г", "512000₽"),
                ("7", "Чай", "Чай КуЦяо (Тайваньский гречишный)", "400г", "81600₽"),
                ("8", "Черный чай", "Чай черный Ассам FOP", "400г", "132000₽"),
                ("9", "Кофе в зернах", "Кофе в зернах Бразилия Basic 1000 г", "3шт", "512400₽"),
                ("10", "Зеленый чай", "Чай Сенча", "200г", "42000₽"),
                ("11", "Черный чай", "Чай черный Таежный сбор (черные типсы) премиум", "200г", "188800₽"),
                ("12", "Чай пуэр", "Пуэр Шу вишневый", "200г", "116000₽"),
                ("13", "Какао растворимое", "Какао-напиток растворимый гранулированный ЧУККА 130 г", "1шт", "14100₽"),
                ("14", "Зеленый чай", "Чай зеленый Моли Хуа Ча (Классический с жасмином)", "100г", "35100₽"),
                ("15", "Черный чай", "Чай чёрный Айва с персиком премиум", "100г", "62400₽"),
                ("16", "Кофе в зернах", "Кофе в зернах Arabica Brazil Italco 375 г", "1шт", "51800₽"),
                ("17", "Черный чай", "Чай черный Сосновый лес", "100г", "69800₽")
            };

                foreach (var (num, group, product, ordered, sum) in data)
                {
                    TableRow row = new TableRow();
                    row.Append(new TableCell(new Paragraph(new Run(new Text(num)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(group)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(product)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(ordered)))));
                    row.Append(new TableCell(new Paragraph(new Run(new Text(sum)))));
                    table.Append(row);
                    
                }
                
                // Добавляем таблицу в документ
                body.Append(table);

                // Добавляем общую выручку
                Paragraph totalParagraph = new Paragraph(new Run(new Text("Общая выручка 9221600₽")));
                body.AppendChild(totalParagraph);

                // Завершаем создание документа
                mainPart.Document.Append(body);
                mainPart.Document.Save();
            }

            Console.WriteLine("Документ создан успешно.");
        }
    }
}
