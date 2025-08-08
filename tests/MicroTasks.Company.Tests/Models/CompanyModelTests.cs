using MicroTasks.CompanyApi.Models;

namespace MicroTasks.CompanyApi.Tests.Models;

public class CompanyModelTests
{
    [Fact]
    public void Company_CanBeConstructedWithTags()
    {
        Tag[] tags = new[] { new Tag { Value = "enterprise" }, new Tag { Value = "technology" } };
        Company company = new Company("Acme Corp", tags);
        Assert.Equal("Acme Corp", company.Name);
        Assert.Equal(2, company.Tags.Count);
        Assert.Contains(company.Tags, t => t.Value == "enterprise");
        Assert.Contains(company.Tags, t => t.Value == "technology");
        Assert.True(company.IsActive);
    }

    [Fact]
    public void Company_AddTag_AddsUniqueTag()
    {
        Company company = new Company("TestCo");
        company.AddTag(new Tag { Value = "demo" });
        company.AddTag(new Tag { Value = "demo" }); // duplicate
        Assert.Single(company.Tags);
    }

    [Fact]
    public void Company_AddTag_NullTag_DoesNothing()
    {
        Company company = new Company("TestCo");
        company.AddTag(null);
        Assert.Empty(company.Tags);
    }

    [Fact]
    public void Company_AddTags_AddsMultipleTags()
    {
        Company company = new Company("TestCo");
        Tag[] tags = new[] { new Tag { Value = "one" }, new Tag { Value = "two" } };
        company.AddTags(tags);
        Assert.Equal(2, company.Tags.Count);
        Assert.Contains(company.Tags, t => t.Value == "one");
        Assert.Contains(company.Tags, t => t.Value == "two");
    }

    [Fact]
    public void Company_AddTags_IgnoresDuplicatesAndNulls()
    {
        Company company = new Company("TestCo");
        Tag[] tags = new Tag?[] { new Tag { Value = "one" }, new Tag { Value = "one" }, null };
        company.AddTags(tags!);
        Assert.Single(company.Tags);
        Assert.Contains(company.Tags, t => t.Value == "one");
    }

    [Fact]
    public void Company_RemoveTag_RemovesTag()
    {
        Company company = new Company("TestCo", new[] { new Tag { Value = "demo" } });
        company.RemoveTag("demo");
        Assert.Empty(company.Tags);
    }

    [Fact]
    public void Company_ChangeName_UpdatesName()
    {
        Company company = new Company("OldName");
        company.ChangeName("NewName");
        Assert.Equal("NewName", company.Name);
    }

    [Fact]
    public void Company_Deactivate_And_Activate()
    {
        Company company = new Company("TestCo");
        company.Deactivate();
        Assert.False(company.IsActive);
        company.Activate();
        Assert.True(company.IsActive);
    }
}
