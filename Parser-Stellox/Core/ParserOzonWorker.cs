using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserStellox.Core
{
    internal class ParserOzonWorker : ParserWorker
    {
        IWebDriver _driver;
        internal static bool cancel;
        private Form form;

        internal ParserOzonWorker(IWebDriver driver,Form form):base(driver,form)
        {
            _driver = driver;
            cancel = false;
            this.form = form;
        }

        internal Dictionary<string, List<string>> Work(string[] sufixs)
        {
            Dictionary<string, List<string>> resultList = new Dictionary<string, List<string>>();

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(@$"https://ozon.by/");

            int counter = 0;
            foreach (var sufix in sufixs)
            {
                if (!cancel) 
                {
                    List<string> nameList = new List<string>();

                    try
                    {
                        form.labBotSecond.Invoke((MethodInvoker)delegate
                        {
                            form.labBotSecond.Text = $"Обработано: {counter++}";
                        });

                        ParseAction(sufix, nameList);
                        resultList.Add(sufix, nameList);
                    }
                    catch (Exception ex)
                    {
                        nameList.Add(ex.Message.Substring(0, 21));
                        resultList.Add(sufix, nameList);
                    }
                }
            }
            
            return resultList;
        }

        private void ParseAction(string sufix, List<string> nameList)
        {
            IWebElement searchInput = TryWait("form input", "css", 12);
            clearWebField(searchInput);
            searchInput.SendKeys($"{sufix}{OpenQA.Selenium.Keys.Enter}");

            IWebElement searchResult = TryWait("a div span.tsBody500Medium", "css", 12);

            if (searchResult is not null)
            {
                var elementsList = _driver.FindElements(By.CssSelector("a div span.tsBody500Medium")).ToList();

                int count = 0;
                foreach (var element in elementsList)
                {
                    if (count < 4)
                    {
                        nameList.Add(element.Text);
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("Нет данных для показа");
            }
        }

        private static void clearWebField(IWebElement searchInput)
        {
            while (!searchInput.GetAttribute("value").Equals(""))
            {
                searchInput.SendKeys(OpenQA.Selenium.Keys.Backspace);
            }
        }
    }
}
