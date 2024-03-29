﻿using OpenQA.Selenium;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

[CollectionDefinition(nameof(RinkudesuDataFilledTest), DisableParallelization = true)]
public abstract class RinkudesuDataFilledTest : ChromeDriverTest
{
    protected RinkudesuDataFilledTest(string? baseUrl = null) : base(baseUrl)
    {
        LogIn();
        RemoveData();
    }

    protected void CreateTag(string tagName)
    {
        GoTo("tags");
        FillTextBox("Name".AsId(), tagName);
        Click("tag_create_quick".AsId());
    }

    protected void CreateLink(string linkName, string linkUrl, string? linkDescription = null, IEnumerable<string>? tagNames = null)
    {
        GoTo("links/create");
        FillTextBox("Title".AsId(), linkName);
        FillTextBox("LinkUrl".AsId(), linkUrl);
        if (!string.IsNullOrEmpty(linkDescription))
            FillTextBox("Description".AsId(), linkDescription);
        if (tagNames is not null)
            SelectFromDropDown("TagIds", tagNames);
        Click("link_create_submit".AsId());
    }

    // Note that right now there's no way to actually remove new users, so use this method with caution until then.
    protected void RegisterUser(string email, string password = "qwertyuiop[]")
    {
        LogOut();
        GoTo("/");

        Click("register_btn".AsId());
        FillTextBox("Email".AsId(), email);
        FillTextBox("Password".AsId(), password);
        FillTextBox("PasswordRepeat".AsId(), password);
        GetDriver().FindElement(By.ClassName("btn-success")).Click();

        if (GetDriver().FindElement(By.TagName("h2")).Text != "Account created")
            throw new InvalidOperationException("User creation has failed");
    }

    protected void RemoveData()
    {
        // remove all links
        GoTo("links");
        // wait for initial link batch to load
        while (true)
        {
            WaitUntilNotVisible("loading_notice".AsId());
            var deleteBtn = GetDriver().FindElements(By.CssSelector(".btn.btn-danger")).FirstOrDefault(b => b.GetAttribute("data-bs-target").StartsWith("#delete"));
            if (deleteBtn is null)
                break;
            deleteBtn.Click();
            // a wild modal appeared
            WaitUntilVisible(By.CssSelector(".modal.fade.show"));
            var reallyDeleteBtn = GetDriver().FindElement(By.CssSelector(".modal.fade.show")).FindElements(By.TagName("input")).First(i => i.GetAttribute("type") == "submit");
            reallyDeleteBtn?.Click();
        }
        // remove all tags
        GoTo("tags");
        while (true)
        {
            WaitUntilNotVisible("loading_notice".AsId());
            var deleteBtn = GetDriver().FindElements(By.CssSelector(".btn.btn-danger")).FirstOrDefault(b => b.GetAttribute("data-bs-target").StartsWith("#delete"));
            if (deleteBtn is null)
                break;
            deleteBtn.Click();
            // a wild modal appeared
            WaitUntilVisible(By.CssSelector(".modal.fade.show"));
            var reallyDeleteBtn = GetDriver().FindElement(By.CssSelector(".modal.fade.show")).FindElements(By.TagName("input")).First(i => i.GetAttribute("type") == "submit");
            reallyDeleteBtn?.Click();
        }
        GoTo("AccountAdmin");
        while (true)
        {
            WaitUntilNotVisible("loading_notice".AsId());
            var deleteBtn = GetDriver().FindElements(By.ClassName("index-data-row")).FirstOrDefault(r => !r.GetAttribute("class").Contains("my-account"))?.FindElements(By.CssSelector(".btn.btn-danger")).FirstOrDefault(b => b.GetAttribute("data-bs-target").StartsWith("#delete"));
            if (deleteBtn is null)
                break;
            deleteBtn.Click();
            WaitUntilVisible(By.CssSelector(".modal.fade.show"));
            var reallyDeleteBtn = GetDriver().FindElement(By.CssSelector(".modal.fade.show")).FindElements(By.TagName("input")).First(i => i.GetAttribute("type") == "submit");
            reallyDeleteBtn?.Click();
        }
    }
}
