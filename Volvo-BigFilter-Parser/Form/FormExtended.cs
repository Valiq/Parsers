using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using ParserVolvo.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace ParserVolvo 
{
    public partial class Form
    {
        List<IWebDriver> driverCollection;

        private async void Parsing(ChromeOptions options, string[] lines)
        {
            List<string[]> divList = Parser_Prepear(lines);

            driverCollection = new List<IWebDriver>();

            var tasks = divList.Select(async part =>
            {
                #region TaskVolvo
                //await Task.Run(() =>
                //{
                //    driver = new ChromeDriver("undetected_chromedriver.exe", options);
                //    driverCollection.Add(driver);
                //    Core.Volvo worker = new Core.Volvo(driver, this);

                //    worker.Work(part);
                //});
                #endregion

                #region BigFilter
                await Task.Run(() =>
                {
                    IWebDriver driver = new ChromeDriver(options);
                    driverCollection.Add(driver);
                    BigFilter worker = new BigFilter(driver, this);

                    worker.Work(part);
                });
                #endregion

                await Task.Run(() =>
                {

                });
            });

            await Task.WhenAll(tasks);

            foreach (var driver in driverCollection)
            {
                driver.Quit();
            }

            #region Test
            //await Task.Run(() =>
            //{
            //    IWebDriver driver = new ChromeDriver();
            //    BigFilter worker = new BigFilter(driver, this);
            //    worker.Work(lines);
            //});
            #endregion

            _cancel = true;
            EnabledButtons();
            labelBot.Text = "Готово";
            MessageBox.Show("Parsing: Jobs Done");
        }

        private List<string[]> Parser_Prepear(string[] lines)
        {
            List<string[]> list = new List<string[]>();

            int integ = (int)Math.Round(lines.Count() / 10.0);
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

        private void CheckDir(string name)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@$"{folderBrowserDialog.SelectedPath}\{name}");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }

        private void EnabledButtons()
        {
            Start.Enabled = true;
            FillEmptyStripMenu.Enabled = true;
        }

        private void DisabledButtons()
        {
            Start.Enabled = false;
            FillEmptyStripMenu.Enabled = false;
        }

        internal static bool isCansel()
        {
            return _cancel;
        }
    }
}
