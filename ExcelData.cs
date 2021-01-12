using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Inventurprogramm
{
    public class ExcelData
    {                               
        //Für Create.Cs
        public int rowco;


        //Für Edit.Cs
        public static int whichrowisselected = 0;
        public DataTable dt = new DataTable();


       
        public DataView Data        
        {
            get
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook;
                Excel.Worksheet worksheet;
                Excel.Range range;
                workbook = excelApp.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
                /*worksheet = (Excel.Worksheet)workbook.Sheets["Test Sheet"];*///.get_Item(1);
                worksheet = excelApp.ActiveSheet as Excel.Worksheet;

                int column = 0;
                int row = 1;

                range = worksheet.UsedRange;
                //DataTable dt = new DataTable();
                dt.Columns.Add("Artikel");
                dt.Columns.Add("Artikel Nr.");
                dt.Columns.Add("Anzahl");
                dt.Columns.Add("Lagerort");
                dt.Columns.Add("Ersteller");
                dt.Columns.Add("Datum");

                int row1 = 0;
                //if (rowco != 0) //<= 0 
                //    row1 = 2;// = rowco
                //else
                    row1 = 1;
                //if (rowco != 0)
                //    row1 = 1;
                MessageBox.Show(row1.ToString());
                DataRow dr;              
                for (row = row1; row <= range.Rows.Count; row++)
                {//ging ja ei
                        dr = dt.NewRow();
                        for (column = 1; column <= range.Columns.Count; column++)
                    {
                        // dr[column - 1] = (range.Cells[row, column] as Excel.Range).Value2 != null ? (range.Cells[row, column] as Excel.Range).Value2.ToString() : "";
                        if ((range.Cells[row, column] as Excel.Range).Value2 != null)
                        {
                            dr[column - 1] = (range.Cells[row, column] as Excel.Range).Value2.ToString();
                        }
                        else
                        {
                            dr[column - 1] = "";
                        }
                        //dt.Columns.Add((range.Cells[1, column] as Excel.Range).Value2.ToString());
                    }

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                DataRowCollection itemColumns = dt.Rows;
                //itemColumns[0].Delete();

                //if (rowco <= 1)
                //{
                //    MessageBox.Show(itemColumns[1]["Artikel"].ToString());
                //}
                //MessageBox.Show(itemColumns[10]["Artikel Nr."].ToString());
                //itemColumns[2]["Artikel Nr."] = "Deleted";

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(worksheet);

                //close and release
                workbook.Close(true, Missing.Value, Missing.Value);//
                //quit and release
                excelApp.Quit(); //
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(excelApp);

                return dt.DefaultView;
            }
        }
        public void ExcelData_AddData(string ArtikelArt, string Artikel_Nr, string Anzahl, string Lagerort, string Ersteller, string Datum)
        {
            dt.Rows.Add(ArtikelArt, Artikel_Nr, Anzahl, Lagerort, Ersteller, Datum);
        }
        public void ExcelData_RowDelete(int whichcountisselected)
        {
            //Warum befinde ich mich bei neu Hinzugefügtem in 'out of Range?' | Er löscht es zwar in der Excel, aber nicht in der DataGrid, generell, sobald nicht neu funktioniert es
            DataRowCollection itemColumns = dt.Rows;
            itemColumns[whichcountisselected].Delete();
            //Console.WriteLine(itemColumns[3].RowState.ToString());
        }
        public void ExcelData_Edit(int whichcountisselected1, bool ispressed, bool OKpressed, string Artikel_Art, string Artikel_Nr, string Anzahl, string Lagerort, string Name)
        {
            DataRowCollection itemColumns = dt.Rows;
            //einlesen
            int a = 0;
            for (int i = 1; i < 7; i++ )
            {
                //Geht komischerweise nicht, sobald in der Session gerade etwas hinzugefügt wurde, warum erkennt er sie nicht?
                if (ispressed)
                    Edit.readolddata[a++] = itemColumns[whichcountisselected1][i-1].ToString();
            }

            if (OKpressed == true && whichrowisselected > 0)
            {
                itemColumns[whichcountisselected1]["Artikel"] = Artikel_Art;
                itemColumns[whichcountisselected1]["Artikel Nr."] = Artikel_Nr;
                itemColumns[whichcountisselected1]["Anzahl"] = Anzahl;
                itemColumns[whichcountisselected1]["Lagerort"] = Lagerort;
                itemColumns[whichcountisselected1]["Ersteller"] = Name;
            }
        }
        public void SearchCs_search(int[] saverows)
        {
            DataRowCollection itemColumns = dt.Rows;
            int getthelastnumber = saverows.Length;

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(@"E:\Nur hier Dateien\Hoffentlic_nicht_Schreibgeschützt.xlsx");
            /*worksheet = (Excel.Worksheet)workbook.Sheets["Test Sheet"];*///.get_Item(1);
            worksheet = excelApp.ActiveSheet as Excel.Worksheet;
            range = worksheet.UsedRange;
            int columscount = range.Columns.Count;
            columscount = columscount - 3;
            MessageBox.Show(columscount.ToString());
            for (int i = 0; i <= columscount; i++)
            {
                //Delete every single row
                itemColumns[i].Delete();
            }
            // Reject some specific changes
            foreach (int counter in saverows)
            {
                //System.Console.Write("{0} ", i);
                itemColumns[counter].RejectChanges();
            }
            MessageBox.Show(saverows[0].ToString() + "&" + saverows[1].ToString());

            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(worksheet);

            //close and release
            workbook.Close(true, Missing.Value, Missing.Value);//
                                                               //quit and release
            excelApp.Quit(); //
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excelApp);
        }
    }
}
