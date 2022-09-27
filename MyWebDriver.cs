using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace whatapp_ride_joiner
{

    class MyWebDriver
    {
        IWebDriver driver;
        public MyWebDriver()
        {
            driver = new ChromeDriver(@"C:\My Projects");
            driver.Url = "https://web.whatsapp.com";
            string btn_start_xpath = "//button[@aria-label='Search or start new chat']";
            Console.WriteLine("scan QR code");
            Console.ReadLine();
            IWebElement start = driver.FindElement(By.XPath(btn_start_xpath));
            start.Click();
            Thread.Sleep(100);
        }
        public void Close()
        {
            this.driver.Quit();
        }
        public string GetPage()
        {
            return driver.PageSource;
        }
        public IList<IWebElement> FineElementsByXpath(string xpath)
        {
            return this.driver.FindElements(By.XPath(xpath));
        }
        public IWebElement FineElementByXpath(string xpath)
        {
            return this.driver.FindElement(By.XPath(xpath));
        }
        public void choose_group(string group)
        {
            string chat_xpath = $"//span[@title='{group}']";
            IWebElement chat = driver.FindElement(By.XPath(chat_xpath));
            chat.Click();
        }
        public IList<IWebElement> GetMassges()
        {
            DateTime today = DateTime.Today;
            string format_today = today.ToString("yyyy");
            string get_messages_by_xpath = $"//*[contains(@class,'message-in')]//div[@data-testid='msg-container']//*[contains(@data-pre-plain-text,'{format_today}')]";
            Console.WriteLine("loading messages pres enter when done");
            Console.ReadLine();
            IList<IWebElement> messages = driver.FindElements(By.XPath(get_messages_by_xpath));

            return messages;
        }

        public void Replay(IWebElement element)
        {
            string text = "אני";
            Actions a = new Actions(driver);
            a.MoveToElement(element).Perform();
            string menue_xpath = "//div[@data-testid='icon-down-context']";
            IWebElement menue = driver.FindElement(By.XPath(menue_xpath));
            menue.Click();
            string reply_xpath = "//div[@aria-label='Reply']";
            IWebElement reply = driver.FindElement(By.XPath(reply_xpath));
            reply.Click();
            string inp_xpath = "//p[@class='selectable-text copyable-text']";
            IWebElement input_box = driver.FindElement(By.XPath(inp_xpath));
            input_box.SendKeys(text);

        }


    }
}
