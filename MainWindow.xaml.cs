using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using Syncfusion.XlsIO;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.AccessControl;
using Excel = Microsoft.Office.Interop.Excel;


namespace Inventurprogramm
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindow MW;
        ExcelData exceldata = new ExcelData();

        public MainWindow()
        {
            InitializeComponent();
           

            dataGrid1.DataContext = exceldata;
            //Test(2);
        }

        //private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.Column.Header.ToString() == "Ersteller")
        //        e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        //}
        private Inventar_Fenster inventar_Fenster;

        private void Row1_Btn(object sender, RoutedEventArgs e)
        {
            //Button btn = (Button)sender;
            //pressed_basebutton = (btn.Name.ToString());
            inventar_Fenster = new Inventar_Fenster();

            switch ((sender as Button).Name)
            {
                case "Erstellen_Btn":
                    Create create = new Create
                    {
                        mw = this
                    };
                    inventar_Fenster.Auswahl.Content = create;
                    inventar_Fenster.Show();
                    //table.Rows.Add(1, "abc", "100");
                    break;
                case "Bearbeiten_Btn":
                    bool isclicked = true;
                    EditData(isclicked);
                    break;
                case "Löschen_Btn":
                    DeleteData();
                    //Excel.Range range;
                    //range = x.UsedRange;
                    //int cl = range.Columns.Count; //Ok es gibt 6 Felder
                    //range.EntireRow.Delete(Excel.XlDirection.xlUp);
                    //Testend
                    //int i = 1;
                    //for (i = 1; i <= 6; i++)
                    //{
                    //    // Conditional Needed to check if column row has a "N"
                    //    if (x.Range["A" + i].Value == "N")  //for example 'N', but it works quiet well
                    //    {
                    //        (x.Rows[i] as Excel.Range).Delete();
                    //    }
                    //}
                    break;
                case "SucheStarten_btn":
                    Search search = new Search
                    {
                        mw = this
                    };
                    inventar_Fenster.Auswahl.Content = search;
                    inventar_Fenster.Show();
                    break;
            }
        }
        #region Connection between some UserControls and MainWindows
        public void CreateCs_create(string ArtikelArt, string Artikel_Nr, string Anzahl, string Lagerort, string Ersteller, string Datum)
        {
            //MessageBox.Show("Hat wohl funktioniert");
            exceldata.ExcelData_AddData(ArtikelArt, Artikel_Nr, Anzahl, Lagerort, Ersteller, Datum);
        }
        public bool EditCs_edit (string Artikel_Art, string Artikel_Nr, string Anzahl, string Lagerort, string Name, bool OkisPressed)
        {
            bool ExcistAlready = false;
            Excel.Application excel = new Excel.Application();
            Excel.Workbook sheet = excel.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
            Excel.Worksheet x = excel.ActiveSheet as Excel.Worksheet;
            Excel.Range range;
            range = x.UsedRange;
            int cl = range.Columns.Count; //Ok es gibt 6 Felder
           // range.EntireRow.Delete(Excel.XlDirection.xlUp);
            int i = 1;
            for (i = 1; i <= range.Rows.Count; i++)//i <= 6
            {
                // Conditional Needed to check if column row has a "N"
                if (x.Range["B" + i].Value == Artikel_Art && Convert.ToString(x.Range["C" + i].Value) == Artikel_Nr)  //for example 'N', but it works quiet well
                {
                        string message = "The item number '" + Artikel_Nr + "' is under the item type: '" + Artikel_Art + "' already assigned!";
                        string caption = "Error 01";
                        MessageBoxButton buttons = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        MessageBox.Show(message, caption, buttons, icon);
                        ExcistAlready = true;
                }
            }
            if(ExcistAlready == false)
            {
                int indexreader1 = ReadSelectedRow();
                exceldata.ExcelData_Edit(indexreader1, false,  OkisPressed, Artikel_Art, Artikel_Nr, Anzahl, Lagerort, Name);
            }
            return ExcistAlready;
        }
        //public string[] SearchCs_search(string SelectedItem)
        //{
        //    //Je nach SelectedItem die Daten aus der Row in einen string array einlesen
        //    Excel.Application excel = new Excel.Application();
        //    Excel.Workbook sheet = excel.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
        //    Excel.Worksheet x = excel.ActiveSheet as Excel.Worksheet;
        //    Excel.Range range;
        //    range = x.UsedRange;
        //    int cl = range.Columns.Count;
        //    string[] readcolumndata = new string[(range.Rows.Count)+1];
        //    int i = 1; int arrayindex = 0;
        //    for (i = 2; i <= range.Rows.Count; i++)//i <= 6
        //    {
        //        switch (SelectedItem)
        //        {
        //            case "Artikel Art":
        //                readcolumndata[arrayindex++] = x.Range["B" + i].Value;
        //                break;
        //            case "Artikel Nr.":
        //                readcolumndata[arrayindex++] = x.Range["C" + i].Value;
        //                break;
        //                case "Anzahl":
        //                readcolumndata[arrayindex++] = x.Range["D" + i].Value;
        //                break;
        //            case "Lagerort":
        //                readcolumndata[arrayindex++] = x.Range["E" + i].Value;
        //                break;
        //            case "Ersteller":
        //                readcolumndata[arrayindex++] = x.Range["F" + i].Value;
        //                break;
        //            case "Datum":
        //                readcolumndata[arrayindex++] = x.Range["G" + i].Value;
        //                break;
        //        }

        //    }
        //    return readcolumndata;
        //}
        public void SearchCs_search(int[] saverows)
        {
           int a = saverows[0];
            MessageBox.Show(saverows[0].ToString() + "&" + saverows[1].ToString());
            exceldata.SearchCs_search(saverows);
        }
        #endregion
        private void DeleteData()
        {
            if (rowselected)
            {
                string message = "Are you sure?";
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    //2. Löschen der Row aus DataGrid
                    int indexreader1 = ReadSelectedRow();
                    exceldata.ExcelData_RowDelete(indexreader1);

                    //3. Löschen der Zeile/n aus Excel

                    Excel.Application excel = new Excel.Application();
                    Excel.Workbook sheet = excel.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
                    //xlWorkSheet = (Excel.Worksheet)sheet.Worksheets.get_Item(1);

                    Excel.Worksheet x = excel.ActiveSheet as Excel.Worksheet;
                    x.Rows[indexreader1 + 1].Delete();
                    sheet.Close(true, Type.Missing, Type.Missing);
                    excel.Quit();
                }
            }
            rowselected = false;
        }
        //Daten an das UserControl geben
        private void EditData(bool isclicked1)
        {
            if (rowselected)
            {
                //1. Welche Row wurde geklickt?
                //2. Wie kann ich auf die einzelnen Werte zugreifen?
                //3. Werte in die Textboxen reinlesen

                //4. Wenn Ok gedrückt wurde. soll er die Row aktualisieren (In der DataGrid, wie in der excelDatei
                int indexreader2 = ReadSelectedRow();
                exceldata.ExcelData_Edit(indexreader2, isclicked1, false, "", "", "", "", "");
                ExcelData.whichrowisselected = indexreader2;
                //Opens the Edit Window
                Edit edit = new Edit
                {
                    mw = this
                };
                inventar_Fenster.Auswahl.Content = edit;
                inventar_Fenster.Show();
            }
        }
        private int ReadSelectedRow()
        {
            int index = 0;
            foreach (var row in dataGrid1.SelectedItems)
            {
                index = dataGrid1.Items.IndexOf(row);
            }
            return index;
        }
        /// <summary>
        /// Turns 'rowselected' to 'true' when selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool rowselected = false;
        private void DataGrid1_SelectionChanged(object sender, EventArgs e)
        {
            rowselected = true;
        }
    }
}
