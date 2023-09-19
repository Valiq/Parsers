using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ParserStellox.Core
{
    internal class ExcelWorker
    {
        static Excel.Application excel = new Excel.Application();

        internal ExcelWorker()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
        }

        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        private static void KillExcel(Excel.Application theApp)
        {
            int id = 0;
            IntPtr intptr = new IntPtr(theApp.Hwnd);
            System.Diagnostics.Process p = null;
            try
            {
                GetWindowThreadProcessId(intptr, out id);
                p = System.Diagnostics.Process.GetProcessById(id);
                if (p != null)
                {
                    p.Kill();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("KillExcel:" + ex.Message);
            }
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                KillExcel(excel);
            }
            catch { }
        }

        public void FullUpload(System.Data.DataTable dataTable)
        {
            Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
            xcelApp.Application.Workbooks.Add(System.Type.Missing);

            for (int i = 1; i < dataTable.Columns.Count + 1; i++)
            {
                xcelApp.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
            }

            xcelApp.Range[xcelApp.Cells[1, 1], xcelApp.Cells[1, dataTable.Columns.Count]].Font.Bold = true;
            xcelApp.Range[xcelApp.Cells[1, 1], xcelApp.Cells[1, dataTable.Columns.Count]].Font.Color = ColorTranslator.ToOle(Color.Blue);

            // xcelApp.Cells.Font.Color = ColorTranslator.ToOle(Color.White);

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string[] RowItems = dataTable.Rows[i].ItemArray.Select(x => x.ToString()).ToArray();

                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    xcelApp.Cells[i + 2, j + 1] = RowItems[j];

                    if (RowItems[j] == "Нет данных для показа")
                    {
                        xcelApp.Range[xcelApp.Cells[i + 2, j + 1], xcelApp.Cells[i + 2, j + 1]].Font.Bold = true;
                        xcelApp.Range[xcelApp.Cells[i + 2, j + 1], xcelApp.Cells[i + 2, j + 1]].Font.Color = ColorTranslator.ToOle(Color.Red);
                        xcelApp.Range[xcelApp.Cells[i + 2, j + 1], xcelApp.Cells[i + 2, j + 1]].Interior.Color = ColorTranslator.ToOle(Color.LightPink);
                    }
                }
            }

            xcelApp.Range[xcelApp.Cells[1, 1], xcelApp.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count]].Borders.Color = ColorTranslator.ToOle(Color.Black);
            xcelApp.Range[xcelApp.Cells[1, 1], xcelApp.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count]].Borders.LineStyle = XlLineStyle.xlContinuous;
            xcelApp.Range[xcelApp.Cells[1, 1], xcelApp.Cells[dataTable.Rows.Count + 1, dataTable.Columns.Count]].Borders.Weight = XlBorderWeight.xlMedium;

            xcelApp.Columns.AutoFit();
            xcelApp.Visible = true;
        }
        public void DocGeneration(System.Data.DataTable dataTable, string filename)
        {
            System.Data.DataTable tempDataTable = dataTable.Copy();
            DelError(tempDataTable);

            excel.Workbooks.Open(filename);

            try
            {
                for (int i = 1; i < 70; i++)
                {
                    int step = 4;
                    Excel.Range rng = excel.Sheets[5].Cells[2, i] as Excel.Range;
                    if (rng.Value != null)
                    {
                        switch (rng.Value)
                        {
                            case "Артикул*":
                                Articl(tempDataTable, i, step);
                                break;
                            case "Название товара":
                                Name(tempDataTable, i, step);
                                break;
                            case "НДС, %*":
                                NDS(tempDataTable, i, step);
                                break;
                            case "Штрихкод (Серийный номер / EAN)":
                                EAN(dataTable, i, step);
                                break;
                            case "Ссылка на главное фото*":
                                MainPhoto(tempDataTable, i, step);
                                break;
                            case "Ссылки на дополнительные фото":
                                SecondPhoto(tempDataTable, i, step);
                                break;
                            case "Название модели (для объединения в одну карточку)*":
                                Association(tempDataTable, i, step);
                                break;
                            case "Бренд*":
                                Brend(tempDataTable, i, step);
                                break;
                            case "Вид техники":
                                Type(tempDataTable, i, step);
                                break;  
                            case "OEM-номер":
                                OEM(tempDataTable, i, step);
                                break;
                            case "Rich-контент JSON":
                                Rich(tempDataTable, i, step);
                                break;
                            case "Партномер (артикул производителя)":
                            case "Партномер (артикул производителя)*":
                                PartNumber(tempDataTable, i, step);
                                break;
                            case "Аннотация":
                                Annotation(tempDataTable, i, step);
                                break;
                            case "Ключевые слова":
                                KeyWords(tempDataTable, i, step);
                                break;
                            case "Страна-изготовитель":
                                Country(tempDataTable, i, step);
                                break;
                        }
                    }
                }

                excel.ActiveWorkbook.Close(true);

                MessageBox.Show("Шаблон изменён");
            }
            catch (Exception ex)
            {
                KillExcel(excel);
                MessageBox.Show(ex.ToString());
            }
        }

        private static void EAN(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("Штрих-код/EAN");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                (excel.Sheets[5].Cells[j + step, i] as Excel.Range).NumberFormat = "#";
                excel.Sheets[5].Cells[j + step, i] = dataTable.Rows[j][id];
            }
        }

        private static void DelError(System.Data.DataTable dataTable)
        {
            int id = dataTable.Columns.IndexOf("Ошибка");
            if (id == -1)
            {
                return;
            }

            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                if (!string.IsNullOrEmpty(dataTable.Rows[j][id].ToString()))
                {
                    dataTable.Rows[j].Delete();
                }
            }
        }

        private static void Association(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("Артикул Студии");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i] =
                    $"{dataTable.Rows[j][id]}".Replace("-", "").Replace(" ", "").Replace(".", "");
            }
        }

        private static void Name(System.Data.DataTable dataTable, int i, int step)
        {
            int idName = dataTable.Columns.IndexOf("Товарные группы");
            int idArt = dataTable.Columns.IndexOf("Номер артикула");
            int idBrend = dataTable.Columns.IndexOf("Бренд");

            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value =
                    $"{dataTable.Rows[j][idName]} {dataTable.Rows[j][idArt]} {dataTable.Rows[j][idBrend]}";
            }
        }

        private static void Rich(System.Data.DataTable dataTable, int i, int step)
        {
            for (int k = 0; k < dataTable.Rows.Count; k++)
            {
                string charRow = "", oemRow = "";
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    string value = dataTable.Rows[k][j].ToString();
                    string colName = dataTable.Columns[j].ColumnName;

                    if (value != "" && (colName != "Марка" && colName != "OEM-номер" &&
                                        colName != "Артикул Студии" && colName != "img"))
                    {
                        charRow = charRow + $"[[\"{colName}\"],[\"{value.Replace('\t', ' ').Replace('\"', ' ')}\"]],\n";
                    }
                }

                if (dataTable.Columns.IndexOf("OEM-номер") != -1)
                {
                    string oemNumberCell = dataTable.Rows[k][dataTable.Columns.IndexOf("OEM-номер")].ToString();
                    string oemNameCell = dataTable.Rows[k][dataTable.Columns.IndexOf("Марка")].ToString();

                    string[] oemNumbers = oemNumberCell.Split(';');
                    string[] oemNames = oemNameCell.Split(';');

                    if (oemNumbers.Count() == oemNames.Count())
                    {
                        for (int j = 0; j < oemNumbers.Count(); j++)
                        {
                            oemRow = oemRow + $"[[\"{oemNames[j]}\"],[\"{oemNumbers[j].Trim()}\"]],\n";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка в формировании Rich-Contenta OEM, количество совпадений не верно.");
                    }
                }

                string resultRow = $"{Description.richFirst}{charRow.TrimEnd(',', '\n')}{Description.rischSecond}{oemRow.TrimEnd(',', '\n')}{Description.rischThird}";

                excel.Sheets[5].Cells[k + step, i].Value = resultRow;
                excel.Sheets[5].Cells[k + step, i].WrapText = false;
            }
        }

        private static void KeyWords(System.Data.DataTable dataTable, int i, int step)
        {
            for (int k = 0; k < dataTable.Rows.Count; k++)
            {
                string row = "";
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    Regex regex = new Regex(@"\d+");
                    bool result = regex.IsMatch(dataTable.Rows[k][j].ToString());

                    if ((!result))
                    {
                        string str = dataTable.Rows[k][j].ToString();

                        if(dataTable.Columns[j].ColumnName == "Марка")
                        {
                            string[] marks = str.Split(';');

                            str = "";
                            foreach(var mark in marks)
                            {
                                if(!str.Contains(mark))
                                {
                                    str += $"{mark};";
                                }
                            }
                            str = str.TrimEnd(';');
                        }

                        if ((str != ""))
                            row += $"{str};";
                    }
                }
                excel.Sheets[5].Cells[k + step, i].Value = row.TrimEnd(';');
            }
        }

        private static void Annotation(System.Data.DataTable dataTable, int i, int step)
        {
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value = Description.description;
                excel.Sheets[5].Cells[j + step, i].WrapText = false;
            }
        }

        private static void PartNumber(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("Номер артикула");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i] = dataTable.Rows[j][id].ToString().Replace(".","");
            }
        }

        private static void Country(System.Data.DataTable dataTable, int i, int step)
        {
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value = "Германия";
            }
        }

        private static void Type(System.Data.DataTable dataTable, int i, int step)
        {
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value = "Легковые автомобили";
            }
        }

        private static void OEM(System.Data.DataTable dataTable, int i, int step)
        {
            if (dataTable.Columns.Contains("OEM-номер"))
            {
                int id = dataTable.Columns.IndexOf("OEM-номер");
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    (excel.Sheets[5].Cells[j + step, i] as Excel.Range).NumberFormat = "#";
                    excel.Sheets[5].Cells[j + step, i] = dataTable.Rows[j][id].ToString();
                }
            }
        }

        private static void Brend(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("Бренд");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value = dataTable.Rows[j][id].ToString();
            }
        }

        private static void SecondPhoto(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("img");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                if (!string.IsNullOrEmpty(dataTable.Rows[j][id].ToString())) 
                {
                    excel.Sheets[5].Cells[j + step, i].Value = Description.imgLinkSec;
                }
            }
        }

        private static void MainPhoto(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("img");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                if (string.IsNullOrEmpty(dataTable.Rows[j][id].ToString()))
                {
                    excel.Sheets[5].Cells[j + step, i].Value = Description.imgLinkMain;
                }
                else
                {
                    excel.Sheets[5].Cells[j + step, i].Value = dataTable.Rows[j][id].ToString();
                }
            }
        }

        private static void NDS(System.Data.DataTable dataTable, int i, int step)
        {
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i].Value = "Не облагается";
            }
        }

        private static void Articl(System.Data.DataTable dataTable, int i, int step)
        {
            int id = dataTable.Columns.IndexOf("Артикул Студии");
            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                excel.Sheets[5].Cells[j + step, i] =
                    $"{dataTable.Rows[j][id]}=SLX".Replace("-", "").Replace(" ","").Replace(".","");
            }
        }
            

    }
}
