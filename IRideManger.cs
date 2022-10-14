using OpenQA.Selenium;

namespace whatapp_ride_joiner
{
    public interface IRideManger
    {
        RideManger ChoseMassage();
        RideManger LoadMassages();
        IWebElement? PredicBestRideMassage(IList<IWebElement> massages);
        bool ReplayToChosenMassage();
    }
}