using OpenQA.Selenium;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

public class AccountManagementTests : ChromeDriverTest
{
    [Fact]
    public void AccountManagement_LogsOutEverywhere()
    {
        LogIn();
        GoToManagement();
        Click("log-out-everywhere-submit".AsId());

        var loginBtn = GetDriver().FindElement("login_btn".AsId());

        Assert.NotNull(loginBtn);
    }

    [Theory]
    [InlineData("test2@example.com", "qwertyuiop[]", "qqwertyuiop[]")]
    public void AccountManagement_NewAccount_ChangePassword_DeleteAccount(string email, string initialPassword, string newPassword)
    {
        //yes, this test is too big, but that's the only way to test this without making tests dependent on one another

        //register initial account
        Click("register_btn".AsId());
        FillTextBox("Email".AsId(), email);
        FillTextBox("Password".AsId(), initialPassword);
        FillTextBox("PasswordRepeat".AsId(), initialPassword);
        GetDriver().FindElement(By.ClassName("btn-success")).Click();

        //at this point, the user should see "account created" screen and be able to log in
        //note that selenium tests must have account confirmation disabled as we're unable to receive emails here
        Assert.Equal("Account created", GetDriver().FindElement(By.TagName("h2")).Text);
        GetDriver().FindElements(By.ClassName("btn-primary")).Last().Click();
        LogIn(email, initialPassword);
        Assert.Contains(email, GetDriver().FindElement(By.Id("account-management-btn")).Text);

        //now try changing the password
        GoToManagement();
        FillTextBox("OldPassword".AsId(), initialPassword);
        FillTextBox("NewPassword".AsId(), newPassword);
        FillTextBox("NewPasswordRepeat".AsId(), newPassword);
        GetDriver().FindElement(By.Id("password-change-form")).FindElement(By.ClassName("btn-primary")).Click();

        //alert should display confirming change
        Assert.Equal("Changes saved successfully", GetDriver().FindElement(By.ClassName("alert")).Text);
        //now log out and try logging in again with new password
        Click("log-out-submit".AsId());
        LogIn(email, newPassword);
        Assert.Contains(email, GetDriver().FindElement(By.Id("account-management-btn")).Text);

        //now try deleting account
        GoToManagement();
        FillTextBox("Password".AsId(), newPassword);
        GetDriver().FindElement(By.ClassName("btn-danger")).Click();
        //make sure you're not logged in after that
        var loginBtn = GetDriver().FindElement("login_btn".AsId());
        Assert.NotNull(loginBtn);
        //attempt to log in
        LogIn(email, newPassword);
        //fail successfully
        Assert.Equal("Page not found", GetDriver().FindElement(By.TagName("h1")).Text);
    }

    private void GoToManagement() => Click("account-management-btn".AsId());
}
