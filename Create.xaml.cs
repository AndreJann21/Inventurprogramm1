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
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Runtime.InteropServices;

namespace Inventurprogramm
{
    /// <summary>
    /// Interaktionslogik für Create.xaml
    /// </summary>
    public partial class Create : UserControl
    {
        public MainWindow mw;
        public Create()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handling Delete, Backspace & Tab keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDown1(object sender, KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsDelOrBackspaceOrTabKey(e.Key);
        }
        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDelOrBackspaceOrTabKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab;
        }

        /// <summary>
        /// Handling Paste (strg/Ctrl + V) & Drag & Drop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlingPaste_Drag_Drop__TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            String tmp = tb.Text;  //keyname -> proberty
            foreach (char c in tb.Text.ToCharArray())
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(c.ToString(), "\\d"))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
            }
            tb.Text = tmp;
        }
        /// <summary>
        /// This will find the first ancestor of my UserControl that happens to be ChildWindow.
        /// This allows my UserControl to be placed at any depth in the child windows XAML,
        /// it would still find the correct object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private IEnumerable<DependencyObject> Ancestors()
        {
            DependencyObject current = VisualTreeHelper.GetParent(this);
            while (current != null)
            {
                yield return current;
                current = VisualTreeHelper.GetParent(current);
            }
        }
        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            //Soll sich alle Colums G(Datum) angucken und auf Inhalt prüfen
            //sobald kein Inhalt vorhanden ist, soll er diese ausgewertete Reihe (B:H) mit Inhalt füllen,
            //der durch den User Input gegeben wurde

            Excel.Application excel = new Excel.Application();
            Excel.Workbook sheet = excel.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");


            Excel.Worksheet x = excel.ActiveSheet as Excel.Worksheet;
            int i = 1;
            do
            {
                i++;
            } while (!(x.Range["G" + i].Value == null));

            x.Range["B" + i].Value = Artikel_Art_input.Text;
            x.Range["C" + i].Value = (i - 2);
            x.Range["D" + i].Value = Anzahl_input.Text;
            x.Range["E" + i].Value = Lagerort_input.Text;
            x.Range["F" + i].Value = Name_input.Text;
            x.Range["G" + i].Value = dateTime.ToString("dd/MM/yyyy");

            sheet.Close(true, Type.Missing, Type.Missing);
            //excel.Quit();
            //Marshal.ReleaseComObject(excel);


            //lese die Zeile in DataGrid aus, die ich gerade hinzugefügt habe!
            //ExcelData exceldata = new ExcelData
            //{
            //    rowco = i
            //};
            ////mw.dataGrid1.DataContext = exceldata;
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.CreateCs_create(Artikel_Art_input.Text, (i-2).ToString(), Anzahl_input.Text, Lagerort_input.Text, Name_input.Text, dateTime.ToString("dd/MM/yyyy"));
            //Hier soll er das User Controll bzw. das Child Window wieder schließen
            Window.GetWindow(this).Close();
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
