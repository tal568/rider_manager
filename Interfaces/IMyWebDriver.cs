using OpenQA.Selenium;

namespace rider_manager.Interfaces
{
    public interface IMyWebDriver
    {
        void choose_group(string group);
        void Close();
        IList<IWebElement> GetMassages();
        string GetPage();
        void Replay(IWebElement element);
    }
}