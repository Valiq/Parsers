using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;

namespace ParserStellox.Core
{
    internal class ParserWorker
    {
        internal IWebDriver driver;
        private Form form;

        internal ParserWorker(IWebDriver driver, Form form) 
        {
            this.driver = driver;
            this.form = form;
        }

        internal void Work(string[] sufixs)
        {
            string exEnter = null;
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(@$"https://web.tecalliance.net/stellox/ru/home");

                TryWait("ppms_cm_popup_wrapper", "class", 15)?.FindElement(By.Id("ppms_cm_reject-all")).Click();
                TryWait("exact-search-checkbox", "tag", 10)?.FindElement(By.TagName("a")).Click();
            }
            catch (Exception ex)
            {
                exEnter = ex.Message;
            }

            foreach (var sufix in sufixs)
            {
                if (!string.IsNullOrEmpty(sufix) && !Form.isCansel())
                {
                    List<string[,]> listInf = new List<string[,]>();

                    string[,] mainArticl = { { "Артикул Студии", sufix } };
                    listInf.Add(mainArticl);

                    try
                    {
                        if (!string.IsNullOrEmpty(exEnter))
                        {
                            throw new Exception(exEnter);
                        }

                        IWebElement searchInput = TryWait("part-search-input", "id", 10);
                        searchInput?.SendKeys($"{sufix}{OpenQA.Selenium.Keys.Enter}");

                        var searchResult = TryWait("div.ag-center-cols-container", "css", 20)?.FindElements(By.CssSelector("div[role=row]")).ToList();

                        if ((searchResult is not null) && (searchResult?.Count != 0))
                        {
                            if (searchResult.Count == 1)
                            {
                                TryWait("div.ag-center-cols-container div", "css", 10)?.Click();
                            }
                            else
                            {
                                for(int i = 0; i < searchResult.Count; i++)
                                {
                                    if (TryWait("div.text-truncate.text-primary a", "css", 5) is not null) 
                                    {
                                        string article = TryFindeByCss("div.text-truncate.text-primary a", searchResult[i])?.Text ?? "";

                                        if (article.Replace("-", "").Equals(sufix.Replace("-", "")))
                                        {
                                            searchResult[i].Click();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Нет данных для показа");
                        }

                        List<IWebElement> listElements;

                        listElements = TryWait("part-detail-v2-summary-table", "tag", 10).FindElements(By.TagName("tr")).ToList();
                        textExtract(listElements, listInf);

                        string[,] mas = { { "img", TryFindeByClass("image-center")?.GetAttribute("src") } };
                        listInf.Add(mas);

                        if (TryFindeByTag("part-detail-v2-criteria-tab") != null)
                        {
                            listElements = TryFindeByTag("part-detail-v2-criteria-tab").FindElement(By.TagName("table")).FindElements(By.TagName("tr")).ToList();
                            textExtract(listElements, listInf);
                        }


                        //  OEM PARSING


                        string oeFinal = ""; string markFinal = "";
                        var oeCheckpoint = TryFindeByTag("part-detail-v2-oe-table");

                        if (TryFindeByTag("button", oeCheckpoint) != null)
                        {
                            bool enabledButton = TryFindeByTag("button", oeCheckpoint).Enabled;

                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("document.querySelector(\"p-autocomplete span input\").removeAttribute(\"disabled\")");
                            js.ExecuteScript("document.querySelector(\"p-autocomplete span button\").removeAttribute(\"disabled\")");

                            //oeCheckpoint = TryFindeByTag("part-detail-v2-oe-table");
                            TryFindeByTag("button", oeCheckpoint)?.Click();

                            oeCheckpoint = TryWait("part-detail-v2-oe-table", "tag", 10);
                            WaitMarkList(oeCheckpoint);

                            List<string> markList = oeCheckpoint.FindElements(By.TagName("li")).ToList().ConvertAll(x =>
                            {
                                return x.Text;
                            });

                            foreach (var elem in markList)
                            {
                                oeCheckpoint = TryWait("part-detail-v2-oe-table", "tag", 10);

                                if (enabledButton)
                                {
                                    oeCheckpoint.FindElement(By.TagName("input")).Clear();
                                    oeCheckpoint.FindElement(By.TagName("input")).SendKeys($"{elem}");
                                }

                                WaitMarkList(oeCheckpoint);

                                TryFindeByTag("li", oeCheckpoint)?.Click();

                                ExtractOem(ref oeFinal, ref markFinal, oeCheckpoint, elem);
                            }
                            string[,] masMark = { { "Марка", markFinal.TrimEnd(';') } };
                            string[,] masOEM = { { "OEM-номер", oeFinal.TrimEnd(';',' ') } };

                            listInf.Add(masMark);
                            listInf.Add(masOEM);
                        }
                    }
                    catch (Exception ex)
                    {
                        string[,] exception = { { "Ошибка", ex.Message.Substring(0, 21) } };
                        listInf.Add(exception);
                    }

                    form.dataGrid.Invoke((MethodInvoker)delegate
                    {
                        form.Parser_OnNewData(listInf);
                    });
                }
            }
        }

        private void WaitMarkList(IWebElement oeCheckpoint)
        {
            IWebElement bufElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until<IWebElement>(_driver =>
            {
                //IWebElement tempFirst = TryFindeByTag("part-detail-v2-oe-table");
                IWebElement? tempSecond = TryFindeByTag("li", oeCheckpoint);

                if (tempSecond is not null)
                    return (tempSecond.Displayed && tempSecond.Enabled) ? tempSecond : null;
                else
                    return null;
            });
        }

        private void ExtractOem(ref string oeFinal, ref string markFinal, IWebElement oeCheckpoint, string mark)
        {
            TryFindButton(oeCheckpoint);
            List<IWebElement>? oeList = TryFindeByTag("tbody", oeCheckpoint)?.FindElements(By.TagName("tr")).ToList();

            foreach (var number in oeList)
            {
                string? value = TryFindeByTag("strong", number)?.Text;

                if (!string.IsNullOrEmpty(value))
                {
                    oeFinal += $"{value.Replace(" ","").Replace(".","").Replace(",","").Replace("/", "").Replace("-","")}; ";
                    markFinal += $"{mark};";
                }
            }
        }

        private void TryFindButton(IWebElement oeCheckpoint)
        {
            try
            {
                oeCheckpoint.FindElements(By.TagName("button")).ToList()[1].Click();
            }
            catch { }
        }

        private void textExtract(List<IWebElement> listElements, List<string[,]> list)
        {
            foreach (var element in listElements)
            {
                string? name = TryFindeByTag("th", element)?.Text;
                string? value = TryFindeByTag("td", element)?.Text;

                if ((!string.IsNullOrEmpty(name)))
                {
                    string[,] mas = { { name, value?.Replace("­","") } };
                    list.Add(mas);
                }
            }
        }


        protected IWebElement TryWait(string name, string mod, double time)
        {
            try
            {
                IWebElement webElement = new WebDriverWait(driver, TimeSpan.FromSeconds(time)).Until<IWebElement>(_driver =>
                {
                    IWebElement tempElement = null;
                    switch (mod)
                    {
                        case "class":
                            tempElement = TryFindeByClass(name);
                            break;
                        case "id":
                            tempElement = TryFindeById(name);
                            break;
                        case "tag":
                            tempElement = TryFindeByTag(name);
                            break;
                        case "css":
                            tempElement = TryFindeByCss(name);
                            break;
                    }

                    if (tempElement is not null)
                    {
                        try
                        {
                            return (tempElement.Displayed && tempElement.Enabled) ? tempElement : null;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                });

                return webElement;
            }
            catch
            {
                return null;
            }
        }


        protected IWebElement TryFindeByClass(string name)
        {
            IWebElement element;

            try
            {
                element = driver.FindElement(By.ClassName(name));
            }
            catch 
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeByClass(string name, IWebElement parent)
        {
            IWebElement element;

            try
            {
                element = parent.FindElement(By.Name(name));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeById(string id)
        {
            IWebElement element;

            try
            {
                element = driver.FindElement(By.Id(id));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeByTag(string tag)
        {
            IWebElement element;

            try
            {
                element = driver.FindElement(By.TagName(tag));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeByTag(string tag, IWebElement parent)
        {
            IWebElement element;

            try
            {
                element = parent.FindElement(By.TagName(tag));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeByCss(string css)
        {
            IWebElement element;

            try
            {
                element = driver.FindElement(By.CssSelector(css));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        protected IWebElement TryFindeByCss(string css, IWebElement parent)
        {
            IWebElement element;

            try
            {
                element = parent.FindElement(By.CssSelector(css));
            }
            catch
            {
                element = null;
            }

            return element;
        }

    }
}
