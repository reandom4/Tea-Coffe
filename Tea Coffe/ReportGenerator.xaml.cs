﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для ReportGenerator.xaml
    /// </summary>
    public partial class ReportGenerator : System.Windows.Window
    {
        readonly DataBase dataBase = new DataBase();
        readonly WordHelper wordHelper = new WordHelper();
        readonly WordDocCreator wordDocCreator = new WordDocCreator();
        public ReportGenerator()
        {
            InitializeComponent();
            Init();

        }
        // Инициализация элементов выбора даты с установкой диапазонов дат
        private void Init()
        {
            startDatePicker.DisplayDateStart = DateTime.Now.AddYears(-2);
            startDatePicker.DisplayDateEnd = DateTime.Now;
            endDatePicker.DisplayDateEnd = DateTime.Now;
            endDatePicker.DisplayDateStart = DateTime.Now.AddYears(-2);
        }
        // Генерация отчетов на основе выбранного типа отчета и диапазона дат
        private void Generate(object sender, RoutedEventArgs e)
        {
            wordHelper.GetSettings();
            
            try
            {
                if (startDatePicker.SelectedDate.Value == null || endDatePicker.SelectedDate.Value == null)
                {
                    MessageBox.Show("Укажите даты");
                }
                string startdate = startDatePicker.SelectedDate?.ToString("yyyy-MM-dd");
                string enddate = endDatePicker.SelectedDate?.ToString("yyyy-MM-dd");
                if (DateTime.Parse(startdate) > DateTime.Parse(enddate))
                {
                    MessageBox.Show("Ошибка: Дата начала не может быть позже даты окончания.");
                    return; // Возврат из метода, чтобы документ не создавался
                }
                if (reportTypeComboBox.Text == "Отчет о среднем чеке")
                {
                    double avg = dataBase.AverageBill(startdate, enddate);

                    var wwrd = new Dictionary<string, string>
                        {
                                { "{avgsum}" , avg.ToString("0.00") },
                                { "{datestart}" , startdate },
                                { "{dateend}" , enddate }

                        };
                    wordHelper.Process(wwrd);
                    //wordHelper.createAvg(avg);


                }
                if (reportTypeComboBox.Text == "Отчет о выручке")
                {
                    System.Data.DataTable dt = dataBase.BillProduct(startdate, enddate);
                    List<ProductItem> productList = new List<ProductItem>();


                    foreach (DataRow row in dt.Rows)
                    {

                        // Создание нового объекта ProductItem
                        ProductItem item = new ProductItem
                        {
                            // Присвоение значения свойствам из данных строки таблицы
                            Id = Convert.ToInt32(row["idProducts"]),
                            Name = row["name"].ToString(),
                            ImageData = System.IO.Directory.GetCurrentDirectory() + "\\image\\" + row["photo"].ToString(),
                            Cost = Convert.ToInt32(row["cost"]),
                            DefaultCost = Convert.ToInt32(row["cost"]),
                            Unit = row["Products_unitname"].ToString(),
                            MinUnit = Convert.ToInt32(row["products_unitcol"]),
                            Quantity = Convert.ToInt32(row["products_unitcol"]),
                            QuantityInStock = Convert.ToInt32(row["quantity"]),
                            Category = row["Product_categoryname"].ToString(),
                            Total_quantity = Convert.ToInt32(row["total_Position"]),
                            AllowChange = "Collapsed"

                        };

                        // Добавление объекта ProductItem в список
                        productList.Add(item);


                    }
                    //wordDocCreator.CreateBill2();
                    wordHelper.CreateBill(productList);
                }
                if (reportTypeComboBox.Text == "Отчет о наиболее популярных товарах")
                {
                    System.Data.DataTable dt = dataBase.BillProduct(startdate, enddate);
                    List<ProductItem> productList = new List<ProductItem>();

                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {

                        // Создание нового объекта ProductItem
                        ProductItem item = new ProductItem
                        {
                            // Присвоение значения свойствам из данных строки таблицы
                            Id = Convert.ToInt32(row["idProducts"]),
                            Name = row["name"].ToString(),
                            ImageData = System.IO.Directory.GetCurrentDirectory() + "\\image\\" + row["photo"].ToString(),
                            Cost = Convert.ToInt32(row["cost"]),
                            DefaultCost = Convert.ToInt32(row["cost"]),
                            Unit = row["Products_unitname"].ToString(),
                            MinUnit = Convert.ToInt32(row["products_unitcol"]),
                            Quantity = Convert.ToInt32(row["products_unitcol"]),
                            QuantityInStock = Convert.ToInt32(row["quantity"]),
                            Category = row["Product_categoryname"].ToString(),
                            Total_quantity = Convert.ToInt32(row["total_Position"]),
                            AllowChange = "Collapsed"

                        };

                        // Добавление объекта ProductItem в список
                        productList.Add(item);
                        i++;
                        if (i == 11)
                        {
                            break;
                        }
                    }
                    wordHelper.Createpopular(productList);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // Устанавливает начальную дату для элемента выбора конечной даты, чтобы предотвратить недопустимые диапазоны дат
        private void StartDatePicker_MouseLeave(object sender, MouseEventArgs e)
        {
            if (startDatePicker.SelectedDate != null)
                endDatePicker.DisplayDateStart = startDatePicker.SelectedDate.Value;
        }
        // Устанавливает конечную дату для элемента выбора начальной даты, чтобы предотвратить недопустимые диапазоны дат
        private void EndDatePicker_MouseLeave(object sender, MouseEventArgs e)
        {
            if (endDatePicker.SelectedDate != null)
                startDatePicker.DisplayDateEnd = endDatePicker.SelectedDate.Value;
        }

        
    }
}
