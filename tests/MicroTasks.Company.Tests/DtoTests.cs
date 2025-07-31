using MicroTasks.CompanyApi.Dtos;

namespace MicroTasks.CompanyApi.Tests;

public class DtoTests
{
    [Fact]
    public void CompanyDto_Defaults_AreValid()
    {
        var dto = new CompanyDto();
        Assert.Equal(string.Empty, dto.Name);
        Assert.NotNull(dto.Tags);
        Assert.Empty(dto.Tags);
    }

    [Fact]
    public void TagDto_CanBeConstructedAndAssigned()
    {
        var tag = new TagDto { Value = "demo" };
        Assert.Equal("demo", tag.Value);
    }

    [Fact]
    public void CompanyDto_CanAssignTags()
    {
        var dto = new CompanyDto
        {
            Name = "TestCo",
            Tags = new() { new TagDto { Value = "enterprise" }, new TagDto { Value = "technology" } }
        };
        Assert.Equal("TestCo", dto.Name);
        Assert.Equal(2, dto.Tags.Count);
        Assert.Contains(dto.Tags, t => t.Value == "enterprise");
        Assert.Contains(dto.Tags, t => t.Value == "technology");
    }
}
