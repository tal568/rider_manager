using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using rider_manager.Interfaces;

namespace rider_manager.servises;

public class RideManger : IRideManger
{
    private IList<IWebElement>? massages;
    private readonly ILogger<RideManger> _log;
    private readonly IConfiguration _config;
    private readonly IMyWebDriver _webDriver;
    private readonly INlpMessages _nlpMessages;
    private IWebElement? chosen_massage;


    public RideManger(ILogger<RideManger> log, IConfiguration config, IMyWebDriver webDriver, INlpMessages nlpMessages)
    {
        _log = log;
        _config = config;
        _webDriver = webDriver;
        _nlpMessages = nlpMessages;
    }
    public RideManger LoadMassages()
    {
        massages = _webDriver.GetMassages();
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
        _webDriver.Replay(chosen_massage);
        _webDriver.Close();
        _log.LogInformation("Replayed to ride");
        return true;

    }

    public IWebElement? PredicBestRideMassage(IList<IWebElement> massages)
    {
        float best_ride = 0.6f;
        IWebElement? best_message = null;
        foreach (IWebElement message in massages)
        {
            _nlpMessages.NlpLoadMessage(message.Text);
            if (_nlpMessages.Label == 2 && _nlpMessages.Confidnes.Max() > best_ride)
            {
                best_message = message;
                best_ride = _nlpMessages.Confidnes[2];
            }
            string save = "\nfor message:" + message.Text + "the results where" + _nlpMessages.Label + "  % was: " + _nlpMessages.Confidnes.Max();
            _log.LogInformation(save);
        }
        return best_message;
    }





}