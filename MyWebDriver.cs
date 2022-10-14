using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace whatapp_ride_joiner
{

    class MyWebDriver
    {
        IWebDriver driver;
        private readonly ILogger _log;
        private readonly IConfiguration _config;

        public MyWebDriver(ILogger log, IConfiguration config)
        {
            _log = log;
            _config = config;
            driver = new ChromeDriver(@"C:\My Projects");
            driver.Url = "https://web.whatsapp.com";
            string btn_start_xpath = _config.GetValue<string>("btn_start_xpath");
            Console.WriteLine("scan QR code");
            Console.ReadLine();
            IWebElement start = driver.FindElement(By.XPath(btn_start_xpath));
            start.Click();
            Thread.Sleep(100);
            try
            {
                choose_group(_config.GetValue<string>("Groupname"));
            }
            catch (Exception e)
            {
                _log.LogError("could not find the groupname pleas change it in the setting file \n" + "exption:" + e.Message);
                throw;
            }



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
            string chat_xpath = String.Format(_config.GetValue<string>("chat_xpath"), group);
            IWebElement chat = driver.FindElement(By.XPath(chat_xpath));
            chat.Click();
        }
        public IList<IWebElement> GetMassages()
        {
            DateTime today = DateTime.Now;
            today = today.AddMinutes(-1);
            string format_today = today.ToString(_config.GetValue<string>("Timeformate"));
            //HH:mm, dd/MM/yyyy
            string get_messages_by_xpath = String.Format(_config.GetValue<string>("get_messages_by_xpath"), format_today);
            //  Console.WriteLine("loading messages pres enter when done");
            //  Console.ReadLine();

            IList<IWebElement> messages = driver.FindElements(By.XPath(get_messages_by_xpath));
            return messages;





        }


        public void Replay(IWebElement element)
        {
            string text = "אני";
            Actions a = new Actions(driver);
            a.MoveToElement(element).Perform();
            string menue_xpath = _config.GetValue<string>("menue_xpath");
            IWebElement menue = driver.FindElement(By.XPath(menue_xpath));
            menue.Click();
            string reply_btn_xpath = _config.GetValue<string>("reply_btn_xpath");
            IWebElement reply = driver.FindElement(By.XPath(reply_btn_xpath));
            reply.Click();
            string input_text_xpath = _config.GetValue<string>("input_text_xpath");
            IWebElement input_box = driver.FindElement(By.XPath(input_text_xpath));
            input_box.SendKeys(text + Keys.Enter);

        }


    }
}
