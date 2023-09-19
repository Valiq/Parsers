using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using ParserStellox.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumUndetectedChromeDriver;

namespace ParserStellox
{
    public partial class Form
    {
        IWebDriver? driver;
        List<IWebDriver> driverCollection;

        private string[] TextBoxEdit()
        {
            string[] lines = textBox.Lines;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("SX") is false)
                {
                    lines[i] = $"{lines[i]}SX";
                }
            }

            return lines;
        }

        private async void Parsing(ChromeOptions options, string[] lines)
        {
            List<string[]> divList = Parser_Prepear(lines, double.Parse(comboBox.Text));

            driverCollection = new List<IWebDriver>();

            var tasks = divList.Select(async part =>
            {
                await Task.Run(() =>
                {
                    driver = new ChromeDriver(options);
                    driverCollection.Add(driver);

                    ParserWorker worker = new ParserWorker(driver, this);

                    worker.Work(part);
                });
            });

            await Task.WhenAll(tasks);

            foreach (var driver in driverCollection)
            {
                driver.Quit();
            }

            #region testParse
            //await Task.Run(() =>
            //{
            //    using (driver = new ChromeDriver()) 
            //    {
            //        ParserWorker worker = new ParserWorker(driver, this);
            //        worker.Work(lines);
            //    }
            //});
            #endregion

            _cancel = true;
            EnabledButtons();
            labelBot.Text = "Готово";
            MessageBox.Show("Parsing: Jobs Done");
        }

        private async void ParsingName(ChromeOptions options)
        {
            DataTable dataTableName = new DataTable();
            CreateCollumns(dataTableName);

            await Task.Run(() =>
            {
                using (driver = UndetectedChromeDriver.Create(options: options, driverExecutablePath: @"chromedriver.exe"))
                {
                    ParserOzonWorker worker = new ParserOzonWorker(driver, this);
                    Dictionary<string, List<string>> inf = worker.Work(TextBoxEdit());

                    try
                    {
                        ComplateDataTable(inf, dataTableName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            });

            CompleteExcel(dataTableName);

            _cancel = true;
            EnabledButtons();
            MessageBox.Show("Parsing: Jobs Done");
        }

        private async void CompleteExcel(DataTable dataTableName)
        {
            labelBot.Text = $"Формирование документа...";

            await Task.Run(() =>
            {
                ExcelWorker excel = new ExcelWorker();
                excel.FullUpload(dataTableName);
            });

            labelBot.Text = "Готово";
        }

        private static void ComplateDataTable(Dictionary<string, List<string>> inf, DataTable dataTableName)
        {
            foreach (var elem in inf)
            {
                DataRow row = dataTableName.NewRow();

                row[0] = elem.Key;
                elem.Value.ForEach(x => row[elem.Value.IndexOf(x) + 1] = x);

                dataTableName.Rows.Add(row);
            }
        }

        private static void CreateCollumns(DataTable dataTableName)
        {
            dataTableName.Columns.Add("article");
            for (int i = 1; i < 5; i++)
            {
                dataTableName.Columns.Add($"var{i}");
            }
        }

        private List<string[]> Parser_Prepear(string[] lines, double threadNumber)
        {
            List<string[]> list = new List<string[]>();

            int integ = (int)Math.Round(lines.Count() / threadNumber);
            int count = lines.Count();

            if (integ == 0)
                integ++;

            for (int i = 0; i < lines.Count(); i += integ)
            {
                string[] mas;
                int last;
                if ((count - integ > integ) || (count > integ))
                {
                    mas = new string[integ];
                    last = integ;
                }
                else
                {
                    mas = new string[count];
                    last = count;
                }

                int buf = lines.Count() - count;
                for (int k = 0; k < last; k++)
                {
                    mas[k] = lines[k + buf];
                }
                list.Add(mas);

                count -= integ;
            }
            return list;
        }

        internal void Parser_OnNewData(List<string[,]> listInf)
        {
            foreach (var element in listInf)
            {
                bool flag = dataTable.Columns.Contains(element[0, 0]);

                if (!flag)
                {
                    dataTable.Columns.Add($"{element[0, 0]}");
                }
            }

            DataRow row = dataTable.NewRow();

            string buf = "";
            List<string> checkList = new List<string>();

            foreach (var first in listInf)
            {
                buf = first[0, 1]?.TrimEnd(' ');

                if (!checkList.Contains(first[0, 0]))
                {
                    for (int j = 1 + listInf.IndexOf(first); j < listInf.Count(); j++)
                    {
                        if (first[0, 0] == listInf[j][0, 0])
                        {
                            buf = $"{buf}; {listInf[j][0, 1].Trim(' ')}";
                        }
                    }

                    try
                    {
                        row[dataTable.Columns.IndexOf(first[0, 0])] = buf;
                        checkList.Add(first[0, 0]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка в артикуле {listInf[0][0, 1]}\n{ex}");
                    }
                }
            }

            dataTable.Rows.Add(row);
        }

        private void EnabledButtons()
        {
            Start.Enabled = true;
            FillEmptyStripMenu.Enabled = true;
            CheckNameToolStrip.Enabled = true;
        }

        private void DisabledButtons()
        {
            Start.Enabled = false;
            FillEmptyStripMenu.Enabled = false;
            CheckNameToolStrip.Enabled = false;
        }

        internal static bool isCansel()
        {
            return _cancel;
        }
    }
}
