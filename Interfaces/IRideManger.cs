using OpenQA.Selenium;
using rider_manager.servises;

namespace rider_manager.Interfaces
{
    public interface IRideManger
    {
        RideManger ChoseMassage();
        RideManger LoadMassages();
        IWebElement? PredicBestRideMassage(IList<IWebElement> massages);
        bool ReplayToChosenMassage();
    }
}