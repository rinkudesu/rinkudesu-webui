using OpenQA.Selenium;
using Rinkudesu.SeleniumTests.Utils;

namespace Rinkudesu.SeleniumTests;

public class TagsTests : RinkudesuDataFilledTest
{
    public TagsTests()
    {
        GoTo("tags");
        WaitForIndexLoad();
    }

    [Fact]
    public void NoTags_EmptyListLoadsCorrectly()
    {
        var contentText = GetDriver().FindElement("content".AsId()).Text;

        Assert.Empty(contentText);
    }

    [Theory]
    [InlineData("this is a test")]
    public void CreateNewTag_NewTagAdded(string tagName)
    {
        CreateTag(tagName);

        WaitForIndexLoad();
        var tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Single(tagRows);
        Assert.Equal(tagName, tagRows.Single().FindElement(By.ClassName("tag-background-pill-text")).Text);
    }

    [Theory]
    [InlineData("this is a test")]
    public void CreateNewTag_DuplicateName(string tagName)
    {
        CreateTag(tagName);

        CreateTag(tagName);

        Assert.Equal("Bad request", GetDriver().FindElement(By.TagName("h1")).Text);
    }

    [Theory]
    [InlineData("test1", "test2")]
    public void EditTag_NameChanged(string originalName, string newName)
    {
        CreateTag(originalName);
        WaitForIndexLoad();

        var tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        tagRows.First().FindElements(By.ClassName("col")).Last().FindElements(By.TagName("a")).First(a => a.Text == "Edit").Click();
        FillTextBox("Name".AsId(), newName, true);
        GetDriver().FindElements(By.ClassName("btn-primary")).Last().Click();

        WaitForIndexLoad();
        tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal(newName, tagRows.Single().FindElement(By.ClassName("tag-background-pill-text")).Text);
    }

    [Theory]
    [InlineData("this is a test")]
    public void DeleteTag_DeletedProperly(string tagName)
    {
        CreateTag(tagName);
        WaitForIndexLoad();

        var tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        tagRows.First().FindElements(By.ClassName("col")).Last().FindElements(By.TagName("button")).First(a => a.Text == "Delete").Click();
        WaitUntilVisible(By.CssSelector(".modal.fade.show"));
        var reallyDeleteBtn = GetDriver().FindElement(By.CssSelector(".modal.fade.show")).FindElements(By.TagName("input")).First(i => i.GetAttribute("type") == "submit");
        reallyDeleteBtn?.Click();
        WaitForIndexLoad();
        tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Empty(tagRows);
    }

    [Fact]
    public void TagIndexPagination_AbleToGoForwardsBackwards()
    {
        for (var i = 0; i < 30; i++)
        {
            CreateTag($"{i}");
        }
        WaitForIndexLoad();

        var tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal("0", tagRows.First().FindElement(By.ClassName("tag-background-pill-text")).Text);
        var prevPage = GetDriver().FindElement("page-prev".AsId());
        Assert.False(prevPage.Displayed);
        var nextPage = GetDriver().FindElement("page-next".AsId());
        ScrollTo(nextPage);
        nextPage.Click();
        WaitUntilNotVisible("page-next".AsId());
        tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal("27", tagRows.First().FindElement(By.ClassName("tag-background-pill-text")).Text);
        nextPage = GetDriver().FindElement("page-next".AsId());
        Assert.False(nextPage.Displayed);
        prevPage = GetDriver().FindElement("page-prev".AsId());
        ScrollTo(prevPage);
        prevPage.Click();
        WaitUntilNotVisible("page-prev".AsId());
        WaitForIndexLoad();
        tagRows = GetDriver().FindElement("content".AsId()).FindElements(By.ClassName("index-data-row"));
        Assert.Equal("0", tagRows.First().FindElement(By.ClassName("tag-background-pill-text")).Text);
    }

    private void WaitForIndexLoad() => WaitUntilNotVisible("loading_notice".AsId());
}
