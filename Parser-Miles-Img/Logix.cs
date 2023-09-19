using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace ParserImg
{
    internal class Logix
    {
        public void Work(IWebDriver driver, string sufix, out string linkMain, out string addLinks)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(@$"https://www.miles-auto.com/product/?artnr={sufix}");

            bool check = true;
            try
            {
                driver.FindElement(By.ClassName("catalog-detail__img")).Click();
            } catch { check = false; }

            IWebElement? SearchInputNext = null;
            IWebElement? SearchInputPrev = null;
            try
            {
                SearchInputNext = driver.FindElement(By.ClassName("light-gallery__next"));
            }
            catch { }

            try
            {
                SearchInputPrev = driver.FindElement(By.ClassName("light-gallery__prev"));
            }
            catch { }

            if (SearchInputNext != null)
            {
                SearchInputNext = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until<IWebElement>(_driver =>
                {
                    IWebElement tempElement = driver.FindElement(By.ClassName("light-gallery__next"));
                    return (tempElement.Displayed && tempElement.Enabled) ? tempElement : null;
                });

                SearchInputNext.Click();
            }

            if (SearchInputPrev != null)
            {
                SearchInputPrev = new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until<IWebElement>(_driver =>
                {
                    IWebElement tempElement = driver.FindElement(By.ClassName("light-gallery__prev"));
                    return (tempElement.Displayed && tempElement.Enabled) ? tempElement : null;
                });

                SearchInputPrev.Click();

                try
                {
                    SearchInputPrev.Click();
                }
                catch { }
            }

            addLinks = ""; linkMain = "";
            if (check) 
            {
                List<IWebElement> gallery = driver.FindElement(By.ClassName("light-gallery__container")).FindElements(By.TagName("img")).ToList();

                bool first = true;
                foreach (IWebElement img in gallery)
                {
                    if (first)
                    {
                        linkMain = img.GetAttribute("src");
                        first = false;
                    }
                    else
                    {
                        addLinks = addLinks + $"{img.GetAttribute("src")};";
                    }
                }

                addLinks = addLinks.TrimEnd(';');
            }
            //_ = WriteFile(img.GetAttribute("src"), savePath, sufix);

            //SearchInput.SendKeys($"Это были мо деньги Тони мои деньги GTA VICE CITY{OpenQA.Selenium.Keys.Enter}");
        }

        public void WriteFile(string mainUrl, string addUrl, string savePath, string sufix)
        {
            _ = Request(mainUrl, savePath, sufix);

            string[] urlMas = addUrl.Split(';');

            int counter = 1;
            foreach (var url in urlMas)
            {
                if (url != "")
                {
                    _ = Request(url, savePath, $"{sufix}=MLS_{counter}");
                    counter++;
                }
            }
        }

        private async Task Request(string url, string savePath, string sufix)
        {
            HttpClient httpClient = new HttpClient();

            try
            {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await httpClient.SendAsync(request);

                using (FileStream stream = File.Open(@$"{savePath}\{sufix}.jpg", FileMode.Create))
                {
                    byte[] data = await response.Content.ReadAsByteArrayAsync();

                    if (data.Length > 0)
                    {
                        BinaryWriter binWriter = new BinaryWriter(stream);
                        binWriter.Write(data);
                        binWriter.Close();
                    }

                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("err: " + ex.Message);
            }
        }
    }
}
