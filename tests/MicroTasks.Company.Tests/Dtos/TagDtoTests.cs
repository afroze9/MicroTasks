using MicroTasks.CompanyApi.Dtos;

namespace MicroTasks.CompanyApi.Tests.Dtos;

public class TagDtoTests
{
    [Fact]
    public void TagDto_CanBeConstructedAndAssigned()
    {
        var tag = new TagDto { Value = "demo" };
        Assert.Equal("demo", tag.Value);
    }
}
