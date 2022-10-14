using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using rider_manager;


namespace whatapp_ride_joiner
{
    public class RideManger : IRideManger
    {
        private IList<IWebElement>? massages;
        private MyWebDriver webdriver;
        private readonly ILogger<RideManger> _log;
        private readonly IConfiguration _config;
        private IWebElement? chosen_massage;

        public RideManger(ILogger<RideManger> log, IConfiguration config)
        {
            _log = log;
            _config = config;
            webdriver = new(_log, _config);
        }
        public RideManger LoadMassages()
        {


            massages = webdriver.GetMassages();
            if (massages != null)
                _log.LogInformation("found/saved group messages");
            else
                _log.LogWarning("no message was found ");
            return this;

        }
        public RideManger ChoseMassage()
        {
            if (massages == null)
                return this;

          

            chosen_massage = PredicBestRideMassage(massages);
            if (chosen_massage != null)
                _log.LogInformation("found ride");
            else
                _log.LogInformation("no ride found");
            return this;

        }

        public bool ReplayToChosenMassage()
        {
            if (chosen_massage == null)
                return false;
            webdriver.Replay(chosen_massage);
            webdriver.Close();
            _log.LogInformation("Replayed to ride");
            return true;

        }

        public IWebElement? PredicBestRideMassage(IList<IWebElement> massages)
        {
            float best_ride = 0.65f;
            IWebElement? best_message = null;
            foreach (IWebElement message in massages)
            {
                NlpMessages predicted_massages = new NlpMessages(message.Text);
                if (predicted_massages.Label == 2 && predicted_massages.Confidnes[2] > best_ride)
                {
                    best_message = message;
                    best_ride = predicted_massages.Confidnes[3];
                }
                string save = "for: " + message.Text + "\n % was: " + String.Join($"\n", predicted_massages.Confidnes);
                NlpMessages.SaveMassages(save);
            }
            return best_message;
        }





    }
}