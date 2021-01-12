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

namespace Inventurprogramm
{
    /// <summary>
    /// Interaktionslogik für Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        #region Default Constructor  
        public MainWindow mw;
        public Search()
        {
            //string ComboSelected = SearchBarCB.Text;
            try
            {
                // Initialization.  
                InitializeComponent();
                SuggestionBox.TextChanged += SuggestionBoxOnTextChanged;
            }
            catch (Exception ex)
            {
                // Info.  
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(ex);
            }
        }

        #endregion

        //Je nach Auswahl alle Erstellernamen / Artikel Nr. / Anzahl etc.
        //Wie löse ich das? Vielleicht mit einer Rüberleitung zu Main.Cs und von der Excel alle Daten von z.B. Erstellernamen hineinspeichern und wieder hierher rüberleiten

        private static readonly string[] SuggestionValues = {
            
            "Desktop",
            "Paul",
            "ETest",
            "England",
            "USA",
            "France",
            "Estonia"
        };

        private string _currentInput = "";
        private string _currentSuggestion = "";
        private string _currentText = "";

        private int _selectionStart;
        private int _selectionLength;
        private void SuggestionBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            
            var input = SuggestionBox.Text;
            if (input.Length > _currentInput.Length && input != _currentSuggestion)
            {
                _currentSuggestion = SuggestionValues.FirstOrDefault(x => x.StartsWith(input));
                if (_currentSuggestion != null)
                {
                    _currentText = _currentSuggestion;
                    _selectionStart = input.Length;
                    _selectionLength = _currentSuggestion.Length - input.Length;

                    SuggestionBox.Text = _currentText;
                    SuggestionBox.Select(_selectionStart, _selectionLength);
                }
            }
            _currentInput = input;
        }


        //if (SearchBarCB.SelectedItem == null)
        //{
        //    string message = "Please select an item from the Suggestion Box!";
        //    string caption = "Error 02";
        //    MessageBoxButton buttons = MessageBoxButton.OK;
        //    MessageBoxImage icon = MessageBoxImage.Error;
        //    MessageBox.Show(message, caption, buttons, icon);
        //}

        string[] suggestionarray;
        private void SuggestionBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBarCB.Text))
            {
                MessageBox.Show("No Item is Selected");
            }
            else
            {
                //MessageBox.Show("Item Selected is:" + SearchBarCB.Text);
                MainWindow wnd = (MainWindow)Application.Current.MainWindow;
                //suggestionarray = wnd.SearchCs_search(SearchBarCB.Text);
            }
        }




        /// <summary>
        /// Ab hier funktioniert es
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            //Welche Column
            //Was genau?
            Excel.Application excel = new Excel.Application();
            Excel.Workbook sheet = excel.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
            Excel.Worksheet x = excel.ActiveSheet as Excel.Worksheet;
            Excel.Range range;
            range = x.UsedRange;
            int cl = range.Columns.Count;
            int i = 1; int forarray = 0;
            int[] saverows = new int[range.Rows.Count];
            for (i = 2; i <= range.Rows.Count + 1; i++)//i <= 6
            {
                forarray++;
                if (SearchBarCB.Text == "Artikel Art" && x.Range["B" + i].Value == SuggestionBox.Text)  //for example 'N', but it works quiet well
                {
                    //Den Index einlesen welche 
                    saverows[forarray-1] = i-2;
                }
            }
            MainWindow wnd = (MainWindow)Application.Current.MainWindow;
            wnd.SearchCs_search(saverows);
            //MessageBox.Show(saverows[0].ToString() + "&" + saverows[1].ToString());
            Window.GetWindow(this).Close();
        }
    }
}