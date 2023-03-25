using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

public abstract class ChromeDriverTest : IDisposable
{
    private readonly ChromeDriver _driver;
    private readonly string _baseUrl;

    protected ChromeDriverTest(string? baseUrl = null)
    {
        _baseUrl = baseUrl ?? Environment.GetEnvironmentVariable("RINKUDESU_SELENIUM_BASEURL") ?? "http://localhost:5000/";
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

    protected void GoTo(string path = "") => _driver.Navigate().GoToUrl(_baseUrl + path);

    protected void Click(By locator) => _driver.FindElement(locator).Click();

    protected void FillTextBox(By locator, string keys) => _driver.FindElement(locator).SendKeys(keys);

    protected void LogIn(string username = "test", string password = "test")
    {
        GoTo();
        Click(By.Id("login_submit"));
        FillTextBox(By.Id("username"), username);
        FillTextBox(By.Id("password"), password);
        Click(By.Id("kc-login"));
    }

    // controlId cannot be By as it needs to be appended before use
    protected void SelectFromDropDown(string controlId, IEnumerable<string> selectedValues)
    {
        var tsControlId = $"{controlId}-ts-control".AsId();
        foreach (var selectedValue in selectedValues)
        {
            UnfocusControls();
            FillTextBox(tsControlId, selectedValue);
            var validOptionSelector = By.CssSelector(".option.active");
            WaitUntilVisible(validOptionSelector);
            var validOption = _driver.FindElement(validOptionSelector);
            validOption?.Click();
        }
        UnfocusControls();
    }

    protected void WaitUntilVisible(By locator)
    {
        Wait(d => IsDisplayed(d, locator));
    }

    protected void WaitUntilNotVisible(By locator)
    {
        Wait(d => !IsDisplayed(d, locator));
    }

    protected static bool IsDisplayed(IWebDriver driver, By locator)
    {
        try
        {
            return driver.FindElement(locator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    protected void Wait<TResult>(Func<IWebDriver, TResult> until, TimeSpan? timeout = null)
    {
        var wait = new WebDriverWait(_driver, timeout ?? TimeSpan.FromMinutes(1));
        wait.Until(until);
    }

    protected void UnfocusControls() => _driver.FindElements(By.ClassName("container")).First().Click();

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _driver.Close();
            _driver.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
