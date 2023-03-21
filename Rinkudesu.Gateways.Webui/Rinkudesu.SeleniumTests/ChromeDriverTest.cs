using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Rinkudesu.SeleniumTests;

public abstract class ChromeDriverTest
{
    private readonly ChromeDriver _driver;
    private readonly string _baseUrl;

    //todo: add some base data in each test, so prep another abstract class with methods like "add link", "add tag" and so on
    protected ChromeDriverTest(string baseUrl)
    {
        _baseUrl = baseUrl;
        var options = new ChromeOptions
        {
            AcceptInsecureCertificates = true,
            LeaveBrowserRunning = false
        };
        //don't display any window if debugger attached
        if (!Debugger.IsAttached)
        {
            options.AddArgument("--headless");
            options.AddArgument("--window-size=1920,1080");
        }
        _driver = new ChromeDriver(options);
        GoTo(_baseUrl);
    }

    protected ChromeDriver GetDriver() => _driver;

    protected void GoTo(string url) => _driver.Navigate().GoToUrl(url);

    protected void Click(By locator) => _driver.FindElement(locator).Click();

    protected void FillTextBox(By locator, string keys) => _driver.FindElement(locator).SendKeys(keys);

    protected void LogIn(string username = "test", string password = "test")
    {
        GoTo(_baseUrl);
        Click(By.Id("login_submit"));
        FillTextBox(By.Id("username"), username);
        FillTextBox(By.Id("password"), password);
        Click(By.Id("kc-login"));
    }
}
