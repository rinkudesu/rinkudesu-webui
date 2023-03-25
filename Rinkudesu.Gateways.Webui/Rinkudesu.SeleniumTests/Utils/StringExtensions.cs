using OpenQA.Selenium;

namespace Rinkudesu.SeleniumTests.Utils;

public static class StringExtensions
{
    public static By AsId(this string value) => By.Id(value);
}
