using MicroTasks.CompanyApi.Dtos;

namespace MicroTasks.CompanyApi.Tests.Dtos;

public class TagDtoTests
{
    [Fact]
    public void TagDto_CanBeConstructedAndAssigned()
    {
        TagDto tag = new TagDto { Value = "demo" };
        Assert.Equal("demo", tag.Value);
    }
}
