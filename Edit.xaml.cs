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
using System.Drawing;

namespace Inventurprogramm
{
    /// <summary>
    /// Interaktionslogik für Edit.xaml
    /// </summary>
    public partial class Edit : UserControl
    {
        public MainWindow mw;
        public static string[] readolddata = new string[]{"error","error","error","error","error","error"};
        public Edit()
        {
            InitializeComponent();
            ArtikelArt_input.Text = readolddata[0];
            Artikelnr_input.Text = readolddata[1];
            Anzahl_input.Text = readolddata[2];
            Lagerort_input.Text = readolddata[3];
            Name_input.Text = readolddata[4];

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

        private void btn_OK_Click (object sender, RoutedEventArgs e)
        {
            //Integrate input variables into the class
            //ExcelData exceldata = new ExcelData
            //{
            //    Okpressed = true
            //    //rowco = Convert.ToInt32(readolddata[2])
            //};
            //ExcelData excelData = new ExcelData();
            //excelData.ExcelData_Edit(ExcelData.whichrowisselected, false);
            //mw.dataGrid1.DataContext = exceldata;
            //Close Child Window

            //Artikel Nr. : Gucken, ob Artikel Nr. schon vorhanden ist, wenn ja MessageBox öffnen, wenn nein, dann Zeile in die entsprechende Excel Reihe schreiben. 
            //In ExcelData sagen, dass alle leere Zeilen/Rows gelöscht werden. Allerdings sollen alle Spalten, mit Außnahme von Datum
            string[] str1 = new string[] { ArtikelArt_input.Text, Artikelnr_input.Text, Anzahl_input.Text, Lagerort_input.Text, Name_input.Text };

            //New
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            bool OkisPressed = false;
            bool returnValue = wnd.EditCs_edit(ArtikelArt_input.Text, Artikelnr_input.Text, "1", "2", "3", OkisPressed);
            if (returnValue == false)
            {
                wnd.EditCs_edit(ArtikelArt_input.Text, Artikelnr_input.Text, Anzahl_input.Text, Lagerort_input.Text, Name_input.Text, true);
                Window.GetWindow(this).Close();
            }
            else
            {
                Artikelnr_input.FontWeight = FontWeights.Bold;

                //1. Read Data in DataTable
                //2. Write Data in Excel
            }
               
        }
        private void btn_close_Click (object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}

