using ClosedXML.Excel;
using Microsoft.Build.BuildEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        public static bool isUpdate = false;
        public static bool isSimplified = false;
        private int numberOfRecPerPage;
        private static Paging PagedTable = new Paging();
        private static Parser parser = new Parser(); 
        private IList<Threat> myListOfThreats = null;
        public ObservableCollection<Threat> previousThreats = new ObservableCollection<Threat>();
        public IList<Threat> modifiedThreats = null;
        public ObservableCollection<ModifiedThreat> changesThreats = new ObservableCollection<ModifiedThreat>();

        public MainWindow()
        {
            InitializeComponent();
            if (!File.Exists("thrlist.xlsx"))
            {
                Window1 initialMessage = new Window1();
                initialMessage.ShowDialog();
            }
            myListOfThreats = parser.GetData();
            modifiedThreats = myListOfThreats;
            PagedTable.PageIndex = 1;
            int[] RecordsToShow = { 15, 25, 50};

            foreach (int RecordGroup in RecordsToShow)
            {
                NumberOfRecords.Items.Add(RecordGroup);
            }

            NumberOfRecords.SelectedItem = 15;

            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);

            DataTable firstTable = PagedTable.SetPaging(myListOfThreats, numberOfRecPerPage); 

            dataGrid.ItemsSource = firstTable.DefaultView;
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
		{
            dataGrid.ItemsSource = PagedTable.Next(myListOfThreats, numberOfRecPerPage).DefaultView;
            SimpleView();
        }

		private void Backwards_Click(object sender, RoutedEventArgs e)
		{
            dataGrid.ItemsSource = PagedTable.Previous(myListOfThreats, numberOfRecPerPage).DefaultView;
            SimpleView();
        }

		private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e) 
		{                                                                                        
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            dataGrid.ItemsSource = PagedTable.First(myListOfThreats, numberOfRecPerPage).DefaultView;
            SimpleView();
        }

        private void First_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = PagedTable.First(myListOfThreats, numberOfRecPerPage).DefaultView;
            SimpleView();
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = PagedTable.Last(myListOfThreats, numberOfRecPerPage).DefaultView;
            SimpleView();
        }

        private void ViewMode_Click(object sender, RoutedEventArgs e)
        {
            isSimplified = !isSimplified;
            SimpleView();
        }

        private void SimpleView()
        {
            if (isSimplified)
            {
                for (int i = 2; i < dataGrid.Columns.Count; i++)
                {
                    dataGrid.Columns[i].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                for (int i = 2; i < dataGrid.Columns.Count; i++)
                {
                    dataGrid.Columns[i].Visibility = Visibility.Visible;
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Document";
            dialog.DefaultExt = ".xlsx";
            dialog.Filter = "Excel|*.xlsx";
            try
            {
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    string FileName = dialog.FileName;
                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Sheet");
                    worksheet.ColumnWidth = 38;
                    worksheet.RowHeight = 15;
                    worksheet.Cell("A" + 1).Style.Font.Bold = true;
                    worksheet.Cell("A" + 1).Value = "Идентификатор УБИ";
                    worksheet.Cell("B" + 1).Style.Font.Bold = true;
                    worksheet.Cell("B" + 1).Value = "Наименование УБИ";
                    worksheet.Cell("C" + 1).Style.Font.Bold = true;
                    worksheet.Cell("C" + 1).Value = "Описание";
                    worksheet.Cell("D" + 1).Style.Font.Bold = true;
                    worksheet.Cell("D" + 1).Value = "Источник угрозы (характеристика и потенциал нарушителя)";
                    worksheet.Cell("E" + 1).Style.Font.Bold = true;
                    worksheet.Cell("E" + 1).Value = "Объект воздействия";
                    worksheet.Cell("F" + 1).Style.Font.Bold = true;
                    worksheet.Cell("F" + 1).Value = "Нарушение конфиденциальности";
                    worksheet.Cell("G" + 1).Style.Font.Bold = true;
                    worksheet.Cell("G" + 1).Value = "Нарушение целостности";
                    worksheet.Cell("H" + 1).Style.Font.Bold = true;
                    worksheet.Cell("H" + 1).Value = "Нарушение доступности";
                    int row = 2;
                    foreach(var item in myListOfThreats)
                    {
                        worksheet.Cell("A"  + row).Value = item.Id;
                        worksheet.Cell("B"  + row).Value = item.Name;
                        worksheet.Cell("C"  + row).Value = item.Description;
                        worksheet.Cell("D"  + row).Value = item.Source;
                        worksheet.Cell("E"  + row).Value = item.Target;
                        worksheet.Cell("F"  + row).Value = item.ConfidentialityBreach == true ? 1 : 0;
                        worksheet.Cell("G"  + row).Value = item.IntegrityViolation == true ? 1 : 0;
                        worksheet.Cell("H"  + row).Value = item.AccessViolation == true ? 1 : 0;
                        row++;
                    }
                    workbook.SaveAs(FileName);
                }
                else
                {
                    throw new Exception("File not selected!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
            private void RefreshFile_Click(object sender, RoutedEventArgs e)
        {
            isUpdate = true;
            foreach (DataRowView table in dataGrid.ItemsSource)
            {
                Threat thrt = new Threat();
                thrt.Id = Convert.ToInt32(table.Row[0]);
                thrt.Name = Convert.ToString(table.Row[1]);
                thrt.Description = Convert.ToString(table.Row[2]);
                thrt.Source = Convert.ToString(table.Row[3]);
                thrt.Target = Convert.ToString(table.Row[4]);
                thrt.ConfidentialityBreach = Convert.ToString(table.Row[5]) == "1" ? true : false;
                thrt.IntegrityViolation = Convert.ToString(table.Row[6]) == "1" ? true : false;
                thrt.AccessViolation = Convert.ToString(table.Row[7]) == "1" ? true : false;
                previousThreats.Add(thrt);
            }
            myListOfThreats = null;
            Window1 initialMessage = new Window1();
            initialMessage.ShowDialog();
            myListOfThreats = parser.GetData();
            DataTable firstTable = PagedTable.SetPaging(myListOfThreats, numberOfRecPerPage);
            dataGrid.ItemsSource = firstTable.DefaultView;
            MessageBox.Show("Updated!");
            int totalThreatsChanged = 0;
            int i = 0, id = 1;
            foreach (var item in previousThreats)
            {
                if (item.Id != modifiedThreats[i].Id)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = Convert.ToString(item.Id);
                    mt.Became = Convert.ToString(modifiedThreats[i].Id);
                    mt.ColumnName = "ID";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.Name != modifiedThreats[i].Name)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = item.Name;
                    mt.Became = modifiedThreats[i].Name;
                    mt.ColumnName = "Наименование";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.Description != modifiedThreats[i].Description)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = item.Description;
                    mt.Became = modifiedThreats[i].Description;
                    mt.ColumnName = "Описание";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.Source != modifiedThreats[i].Source)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = item.Source;
                    mt.Became = modifiedThreats[i].Source;
                    mt.ColumnName = "Источник";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.Target != modifiedThreats[i].Target)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = item.Target;
                    mt.Became = modifiedThreats[i].Target;
                    mt.ColumnName = "Объект";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.ConfidentialityBreach != modifiedThreats[i].ConfidentialityBreach)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = Convert.ToString(item.ConfidentialityBreach);
                    mt.Became = Convert.ToString(modifiedThreats[i].ConfidentialityBreach);
                    mt.ColumnName = "Нарушение конфиденциальности";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.IntegrityViolation != modifiedThreats[i].IntegrityViolation)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = Convert.ToString(item.IntegrityViolation);
                    mt.Became = Convert.ToString(modifiedThreats[i].IntegrityViolation);
                    mt.ColumnName = "Нарушение целосности";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                if (item.AccessViolation != modifiedThreats[i].AccessViolation)
                {
                    ModifiedThreat mt = new ModifiedThreat();
                    mt.Id = id++;
                    mt.Was = Convert.ToString(item.AccessViolation);
                    mt.Became = Convert.ToString(modifiedThreats[i].AccessViolation);
                    mt.ColumnName = "Нарушение доступности";
                    totalThreatsChanged++;
                    changesThreats.Add(mt);
                }
                i++;
            }
            ModifiedThreatsMessage update = new ModifiedThreatsMessage();
            update.Show();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
