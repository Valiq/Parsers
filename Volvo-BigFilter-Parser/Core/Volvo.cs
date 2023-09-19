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
using ParserVolvo.Core;

namespace ParserVolvo.Core
{
    internal class Volvo : Parse
    {
        private IWebDriver driver;
        private Form form;

        internal Volvo(IWebDriver driver, Form form):base(driver) 
        {
            this.driver = driver;
            this.form = form;
        }

        internal override void Work(string[] sufixs)
        {
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(@$"https://www.volvopartswebstore.com/");
            }
            catch (Exception ex)
            {
                List<string[,]> lisErr = new List<string[,]>();
                AddElement(lisErr, ex.Message, "Ошибка");

                form.Invoke((MethodInvoker)delegate
                {
                    form.Parser_OnNewData(lisErr);
                });
            }

            foreach (var sufix in sufixs)
            {
                if (!string.IsNullOrEmpty(sufix) && (!Form._cancel))
                {
                    List<string[,]> listInf = new List<string[,]>();

                    try
                    {
                        AddElement(listInf, sufix, "Артикул Студии");

                        IWebElement searchInput = TryWait("SearchInput", "id", 5);
                        searchInput?.SendKeys($"{sufix}{OpenQA.Selenium.Keys.Enter}");

                        IWebElement searchResult = TryWait("div.panel-heading.product-info-heading", "css", 10);

                        if (searchResult == null)
                        {
                            throw new Exception("Нет данных для показа");
                        }

                        List<IWebElement> listElements;

                        AddElement(listInf, TryFindeByCss("span.prodDescriptH2")?.Text, "Title");
                        AddElement(listInf, TryFindeByCss("span.body-3.stock-code-text")?.Text, "Part Number");
                        AddElement(listInf, TryFindeByCss("span.body-3.alt-stock-code-text")?.Text, "Supersession(s)");

                        var pList = TryWait("div.item-desc", "css", 1)?.FindElements(By.TagName("p")).ToList();

                        if (pList is not null)
                        {
                            foreach (var element in pList)
                            {
                                if ((pList.IndexOf(element) == 0) && (element.Text.Count() > 10))
                                {
                                    AddElement(listInf, element.Text, "Description");
                                }

                                if (element.Text.Contains("Keyword"))
                                {
                                    int id = element.Text.IndexOf(':');
                                    AddElement(listInf, element.Text.Substring(id + 2), "KeyWords");
                                }
                            }
                        }

                        var imgList = TryWait("div.img-gallery-wrapper.isSingleImage", "css", 1)?.FindElements(By.TagName("img")).ToList();

                        if (imgList is not null)
                        {
                            string imgUrl = "";
                            foreach (var element in imgList)
                            {
                                element.Click();
                                imgUrl += $"{TryWait("img.img-responsive.img-thumbnail", "css", 3)?.GetAttribute("src")};";
                            }

                            AddElement(listInf, imgUrl.TrimEnd(';'), "Images");
                        }

                        var markButton = TryWait("ctl00_Content_PageBody_ProductTabsLegacy_fitmentTabLI", "id", 3);

                        if (markButton is not null)
                        {
                            markButton.Click();

                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("document.getElementById(\"ctl00_Content_PageBody_ProductTabsLegacy_showAllApplications\").click()");

                            TryWait("ctl00_Content_PageBody_ProductTabsLegacy_lblYearSelection", "id", 10);
                            var markList = TryWait("ctl00_Content_PageBody_ProductTabsLegacy_div_applicationListContainer", "id", 5)?.FindElements(By.TagName("tr")).ToList();

                            string markRow = "";
                            foreach (var element in markList)
                            {
                                string bufRow = "";
                                bufRow += $"{element.FindElement(By.CssSelector("td.whatThisFitsFitment")).Text} ";

                                var yearList = element.FindElement(By.CssSelector("td.whatThisFitsYears")).FindElements(By.TagName("a")).ToList();

                                foreach (var year in yearList)
                                {
                                    bufRow += $"{year.Text}, ";
                                }

                                markRow += $"{bufRow.TrimEnd(',', ' ')};";
                            }

                            AddElement(listInf, markRow.TrimEnd(';'), "Fits");
                            markButton.Click();
                        }

                        var diagramButton = TryWait("ctl00_Content_PageBody_ProductTabsLegacy_assembliesTabLI", "id", 3);

                        if (diagramButton is not null)
                        {
                            diagramButton.Click();
                            TryWait("ctl00_Content_PageBody_ProductTabsLegacy_ToggleAssemblyButton", "id", 3)?.Click();
                            var panelList = TryWait("assembliesUpdate", "id", 3)?.FindElements(By.CssSelector("div.panel-body"));

                            string diagramUrl = "";
                            for (int i = 0; i < panelList.Count - 1; i++)
                            {
                                var diagram = panelList[i].FindElement(By.CssSelector("a.assemblyPreviewAssemblyName.col-md-8.col-xs-6"));
                                string name = diagram.Text;

                                diagram.Click();

                                diagramUrl += $"{TryWait("div.img-container", "css", 6)?.FindElement(By.Id("ctl00_Content_PageBody_fullsizeImgAssembly")).GetAttribute("src")};";

                                driver.Navigate().Back();

                                TryWait("ctl00_Content_PageBody_ProductTabsLegacy_ToggleAssemblyButton", "id", 3)?.Click();
                                panelList = TryWait("assembliesUpdate", "id", 3)?.FindElements(By.CssSelector("div.panel-body"));
                                //driver.SwitchTo().Window(driver.WindowHandles.Last()).Close();
                            }

                            AddElement(listInf, diagramUrl.TrimEnd(';'), "Diagrams");
                        }
                    }
                    catch (Exception ex)
                    {
                        AddElement(listInf, ex.Message.Substring(0, 21), "Ошибка");
                    }

                    form.Invoke((MethodInvoker)delegate
                    {
                        form.Parser_OnNewData(listInf);
                    });
                }
            }
        }

        private void AddElement(List<string[,]> listInf, string? text, string title)
        {
            string[,] mas = { { title,  text} };
            listInf.Add(mas);
        }

       
        
    }
}
