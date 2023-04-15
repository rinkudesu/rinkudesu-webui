using OpenQA.Selenium;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

public class LinksTests : RinkudesuDataFilledTest
{
    public LinksTests()
    {
        CreateTag("test1");
        CreateTag("test2");
        GoTo("links");
        WaitForIndexLoad();
    }

    [Fact]
    public void NoLinks_EmptyListLoadsCorrectly()
    {
        var contentText = GetDriver().FindElement("content".AsId()).Text;

        Assert.Empty(contentText);
    }

    [Theory]
    [InlineData("test", "url", "", new string[0])]
    [InlineData("test", "url", "description", new string[0])]
    [InlineData("test", "url", "description", new [] {"test1"})]
    [InlineData("test", "url", "description", new [] {"test1", "test2"})]
    public void CreateNewLink_NewLinkAdded(string linkName, string linkUrl, string linkDescription, string[] tagNames)
    {
        CreateLink(linkName, linkUrl, linkDescription, tagNames);

        WaitForIndexLoad();
        var linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Single(linkRows);
        var linkColumns = linkRows.Single().FindElements(By.ClassName("index-data-row-col"));
        Assert.Equal(linkName, linkColumns.First().Text);
        var splitTags = linkColumns[1].Text.Split();
        foreach (var tagName in tagNames)
        {
            Assert.Contains(tagName, splitTags);
        }
        linkColumns[^2].FindElement(By.TagName("a")).Click();
        Assert.Equal(linkUrl, GetDriver().FindElement("LinkUrl".AsId()).GetAttribute("value"));
        Assert.Equal(linkDescription, GetDriver().FindElement("Description".AsId()).GetAttribute("value"));
    }

    [Theory]
    [InlineData("this is a test")]
    public void CreateNewLink_DuplicateName(string linkName)
    {
        CreateLink(linkName, linkName);

        CreateLink(linkName, linkName);

        Assert.Equal("Bad request", GetDriver().FindElement(By.TagName("h1")).Text);
    }

    [Theory]
    [InlineData("test1", "test2", "url1", "url2")]
    public void EditLink_NameAndUrlChanged(string oldName, string newName, string oldUrl, string newUrl)
    {
        CreateLink(oldName, oldUrl);

        OpenFirstLinkEdit();
        FillTextBox("Title".AsId(), newName, true);
        FillTextBox("LinkUrl".AsId(), newUrl, true);
        GetDriver().FindElements(By.ClassName("btn-primary")).Last().Click();
        WaitForIndexLoad();
        var linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Single(linkRows);
        var linkColumns = linkRows.Single().FindElements(By.ClassName("index-data-row-col"));
        Assert.Equal(newName, linkColumns.First().Text);
        linkColumns[^2].FindElement(By.TagName("a")).Click();
        Assert.Equal(newUrl, GetDriver().FindElement("LinkUrl".AsId()).GetAttribute("value"));
    }

    [Theory]
    [InlineData("test")]
    public void DeleteLink_DeletedProperly(string linkData)
    {
        CreateLink(linkData, linkData);
        WaitForIndexLoad();

        var linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        var linkColumns = linkRows.Single().FindElements(By.ClassName("index-data-row-col"));
        linkColumns[^1].FindElement(By.TagName("button")).Click();
        WaitUntilVisible(By.CssSelector(".modal.fade.show"));
        var reallyDeleteBtn = GetDriver().FindElement(By.CssSelector(".modal.fade.show")).FindElements(By.TagName("input")).First(i => i.GetAttribute("type") == "submit");
        reallyDeleteBtn?.Click();
        WaitForIndexLoad();
        linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Empty(linkRows);
    }

    [Fact]
    public void LinkIndexPagination_AbleToGoForwardsBackwards()
    {
        for (var i = 0; i < 30; i++)
        {
            CreateLink($"{i}", $"{i}");
        }
        WaitForIndexLoad();

        var linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal("0", linkRows.First().FindElement(By.ClassName("index-data-row-col")).Text);
        var prevPage = GetDriver().FindElement("page-prev".AsId());
        Assert.False(prevPage.Displayed);
        var nextPage = GetDriver().FindElement("page-next".AsId());
        ScrollTo(nextPage);
        nextPage.Click();
        WaitUntilNotVisible("page-next".AsId());
        linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        // I'm not 100% sure why this is 20, so if it ever fails, just change this value to something sane as this test doesn't cover if it's sorted in a proper way
        Assert.Equal("20", linkRows.First().FindElement(By.ClassName("index-data-row-col")).Text);
        nextPage = GetDriver().FindElement("page-next".AsId());
        Assert.False(nextPage.Displayed);
        prevPage = GetDriver().FindElement("page-prev".AsId());
        ScrollTo(prevPage);
        prevPage.Click();
        WaitUntilNotVisible("page-prev".AsId());
        WaitForIndexLoad();
        linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal("0", linkRows.First().FindElement(By.ClassName("index-data-row-col")).Text);
    }

    private void OpenFirstLinkEdit()
    {
        WaitForIndexLoad();
        var linkRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        var linkColumns = linkRows.Single().FindElements(By.ClassName("index-data-row-col"));
        linkColumns[^2].FindElement(By.TagName("a")).Click();
    }

    private void WaitForIndexLoad() => WaitUntilNotVisible("loading_notice".AsId());
}
