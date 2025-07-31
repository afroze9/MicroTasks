using MicroTasks.CompanyApi.Models;
using System;

namespace MicroTasks.CompanyApi.Tests.Models;

public class TagModelTests
{
    [Fact]
    public void Tag_CanBeConstructedAndAssigned()
    {
        var tag = new Tag { Value = "demo" };
        Assert.Equal("demo", tag.Value);
        var tag2 = new Tag("test");
        Assert.Equal("test", tag2.Value);
    }

    [Fact]
    public void Tag_Constructor_ThrowsOnEmptyValue()
    {
        Assert.Throws<ArgumentException>(() => new Tag(""));
        Assert.Throws<ArgumentException>(() => new Tag("   "));
    }

    [Fact]
    public void Tag_Id_CanBeAssigned()
    {
        var tag = new Tag("demo") { Id = Guid.NewGuid() };
        Assert.NotEqual(Guid.Empty, tag.Id);
    }
}
