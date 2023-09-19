using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserVolvo.Core
{
    internal abstract class Parse
    {
        private IWebDriver driver;

        internal Parse(IWebDriver driver)
        {
            this.driver = driver;
        }

        internal abstract void Work(string[] sufixs);

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


        private protected IWebElement TryFindeByClass(string name)
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

        private protected IWebElement TryFindeByClass(IWebElement parent, string name)
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

        private protected IWebElement TryFindeById(string id)
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

        private protected IWebElement TryFindeByTag(string tag)
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

        private protected IWebElement TryFindeByTag(IWebElement parent, string name)
        {
            IWebElement element;

            try
            {
                element = parent.FindElement(By.TagName(name));
            }
            catch
            {
                element = null;
            }

            return element;
        }

        private protected IWebElement TryFindeByCss(string css)
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
    }
}
