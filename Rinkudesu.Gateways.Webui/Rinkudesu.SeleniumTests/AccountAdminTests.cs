using OpenQA.Selenium;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

// this tests must assume some conditions, that otherwise would make this impossible:
// 1. the initial user created is test@example.com, is an admin and has email confirmed by default,
// 2. all other accounts are initially not confirmed and unlocked,
// 3. I don't think there's a reasonable way of testing 2FA, so it will remain always disabled for now.
public class AccountAdminTests : RinkudesuDataFilledTest
{
    private readonly List<string> _users = new List<string>
    {
        "test1@example.com",
        "aaabbb@example.com",
        "hello@example.com",
        "test2@example.com",
        // test@example.com will also exist
    };

    public AccountAdminTests()
    {
        foreach (var user in _users)
        {
            RegisterUser(user);
        }
        LogIn();
        GoTo("AccountAdmin");
    }

    [Fact]
    public void AccountAdmin_LoadsAllUsers()
    {
        WaitForIndexLoad();

        Assert.Equal(_users.Count + 1, GetUserCount());
    }

    [Fact]
    public void EmailContains_ReturnsOnlyValidEmails()
    {
        WaitForIndexLoad();

        FillTextBox("EmailContains".AsId(), "test");
        SubmitFilter();

        WaitForIndexLoad();
        Assert.Equal(3, GetUserCount());
        var emails = GetEmailsDisplayed();
        Assert.Contains("test@example.com", emails);
        Assert.Contains("test1@example.com", emails);
        Assert.Contains("test2@example.com", emails);
    }

    [Fact]
    public void AdminsOnly_OnlyTestExampleDisplayed()
    {
        WaitForIndexLoad();

        GetDriver().FindElement("IsAdmin".AsId()).Click();
        SubmitFilter();

        WaitForIndexLoad();
        Assert.Equal(1, GetUserCount());
        var emails = GetEmailsDisplayed();
        Assert.Equal("test@example.com", emails.Single());
    }

    [Fact]
    public void OnlyConfirmedEmail_OnlyTestExampleReturned()
    {
        WaitForIndexLoad();

        SelectFromGenericDropdown("EmailConfirmed".AsId(), e => e.SelectByValue("True"));
        SubmitFilter();

        WaitForIndexLoad();
        Assert.Equal(1, GetUserCount());
        var emails = GetEmailsDisplayed();
        Assert.Equal("test@example.com", emails.Single());
    }

    [Theory]
    [InlineData("userCreationTest@localhost")]
    public void CreateUserAccount_AccountAddedAndDisplayed(string email)
    {
        Click("showCreateAccount".AsId());

        WaitUntilVisible("Email".AsId());
        FillTextBox("Email".AsId(), email);
        Click("admin-account-create-submit".AsId());

        WaitForIndexLoad();
        var emails = GetEmailsDisplayed();
        Assert.Single(emails);
        Assert.Contains(email, emails);
    }

    private int GetUserCount()
        => GetDriver().FindElements(By.ClassName("index-data-row")).Count;

    private void WaitForIndexLoad()
        => WaitUntilNotVisible("loading_notice".AsId());

    private void SubmitFilter()
        => Click("filter-submit".AsId());

    private HashSet<string> GetEmailsDisplayed()
        => GetDriver().FindElements(By.TagName("strong")).Select(s => s.Text).ToHashSet();
}
