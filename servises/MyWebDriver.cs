using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using rider_manager.Interfaces;

namespace whatsapp_ride_joiner;

public class MyWebDriver : IMyWebDriver
{
    IWebDriver driver;
    private readonly ILogger<MyWebDriver> _log;
    private readonly IConfiguration _config;

#pragma warning disable CS8618 // Non-nullable field 'driver' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    public MyWebDriver(ILogger<MyWebDriver> log, IConfiguration config)
#pragma warning restore CS8618 // Non-nullable field 'driver' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
    {
        _log = log;
        _config = config;
        initWebDriver();
    }
    private bool StartingWhatappAuthentication(int tries, int maxTries)
    {
        try
        {
            string btn_start_xpath = _config.GetValue<string>("btn_start_xpath");
            Thread.Sleep(5000);
            _log.LogInformation("starting whatapp authentication ");
            IWebElement start = driver.FindElement(By.XPath(btn_start_xpath));
            start.Click();
            Thread.Sleep(100);
            return true;
        }
        catch (Exception e)
        {
            _log.LogError("error while tring to authentication to whatsapp  try number {tries} of {maxTries}", tries, maxTries);
            if (tries < maxTries)
            {
                return false;

            }
            else
            {
                _log.LogError("could not authentication to whatapp {max_times} times pleas chek what wrong and if whatapp loaded pres enter to countinue", maxTries);
                Console.ReadKey();
                return true;
            }

        }
        return false;
    }
    public void initWebDriver()
    {
        ChromeOptions options = new();
        options.AddArguments("--user-data-dir=" + @"C:\Users\tal\AppData\Local\Google\Chrome\User Data\tal_ride");

        driver = new ChromeDriver(@"C:\My Projects", options);
        driver.Url = "https://web.whatsapp.com";
        int tries = 0, maxTries = _config.GetValue<int>("maxtriesToAuthentication");
        bool worked = false;
        while (tries < maxTries && !worked)
        {
            tries++;
            StartingWhatappAuthentication(tries, maxTries);
        }

        try
        {
            choose_group(_config.GetValue<string>("Groupname"));
        }
        catch (Exception e)
        {
            _log.LogError("could not find the groupname pleas change it in the setting file \n" + "exption:" + e.Message);

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
