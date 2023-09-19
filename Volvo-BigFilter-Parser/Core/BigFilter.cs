using OpenQA.Selenium;
using ParserVolvo;
using ParserVolvo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserVolvo.Core
{
    internal class BigFilter : Parse
    {
        private IWebDriver driver;
        private Form form;

        public BigFilter(IWebDriver driver, Form form) : base(driver)
        {
            this.driver = driver;
            this.form = form;
        }

        internal override void Work(string[] sufixs)
        {
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(@$"https://bigfilter.com/products/search/");
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
                if(!string.IsNullOrEmpty(sufix) && (!Form._cancel))
                {
                    List<string[,]> listInf = new List<string[,]>();

                    try
                    {
                        AddElement(listInf, sufix, "Артикул Студии");

                        IWebElement searchInput = TryWait("oemnum", "id", 5);
                        searchInput?.Clear();
                        searchInput?.SendKeys($"{sufix}{OpenQA.Selenium.Keys.Enter}");

                        var searchResult = TryWait("div.filter-search-page div.filter-search-table", "css", 9);

                        if (searchResult == null)
                        {
                            throw new Exception("Нет данных для показа");
                        }

                        searchResult.FindElement(By.CssSelector("div.tr div.td.filter a")).Click();

                        var cardList = TryFindeByCss("div.production-card-description table.card-table tbody").FindElements(By.TagName("tr")).ToList();

                        foreach (var element in cardList)
                        {
                            var bufList = element.FindElements(By.TagName("td")).ToList();
                            AddElement(listInf, bufList[1].Text, bufList[0].Text);
                        }

                        var name = TryFindeByCss("div.container h1").Text.Split(' ');
                        AddElement(listInf, name[name.Count() - 1], "Артикул");

                        var navList = TryFindeByCss("div.application ul.nav.nav-tabs")?.FindElements(By.TagName("li")).ToList();
                        if(navList?.Count() == 4)
                        {
                            navList[3].Click();
                        }

                        var analogList = TryFindeByCss("div#zamena table.card-table tbody")?.FindElements(By.TagName("tr")).ToList();

                        if (analogList is not null)
                        {
                            string analogRow = "";
                            foreach(var element in analogList)
                            {
                                analogRow += $"{element.Text};";
                            }

                            AddElement(listInf, analogRow.TrimEnd(';'), "Аналоги(артикли)");
                        }

                        var imgList = TryFindeByCss("div.carousel.carousel-stage.production-card-img-img")?.FindElements(By.CssSelector("a.fancybox")).ToList();

                        if (imgList is not null)
                        {
                            string imgRow = "";
                            foreach(var element in imgList)
                            {
                                imgRow += $"{element.GetAttribute("href")};";
                            }

                            AddElement(listInf, imgRow.TrimEnd(';'), "Img");
                        }

                        navList = TryFindeByCss("div.application ul.nav.nav-tabs")?.FindElements(By.TagName("li")).ToList();
                        try
                        {
                            navList[1]?.Click();
                        }
                        catch { }

                        var oemList = TryFindeByCss("div.application div.tab-content div#directly table.card-table tbody")?.FindElements(By.TagName("tr")).ToList();

                        if (oemList is not null)
                        {
                            string oemRow = ""; string markRow = "";
                            foreach(var element in oemList)
                            {
                                var bufList = element.FindElements(By.TagName("td")).ToList();
                                markRow += $"{bufList[0].Text};";
                                oemRow += $"{bufList[1].Text};";
                            }

                            AddElement(listInf, markRow.TrimEnd(';'), "Марка");
                            AddElement(listInf, oemRow.TrimEnd(';'), "OEM-номер");
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
            string[,] mas = { { title, text } };
            listInf.Add(mas);
        }
    }
}
