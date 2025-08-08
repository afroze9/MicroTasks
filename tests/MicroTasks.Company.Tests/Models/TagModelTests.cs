using MicroTasks.CompanyApi.Models;
using System;

namespace MicroTasks.CompanyApi.Tests.Models;

public class TagModelTests
{
    [Fact]
    public void Tag_CanBeConstructedAndAssigned()
    {
        Tag tag = new Tag { Value = "demo" };
        Assert.Equal("demo", tag.Value);
        Tag tag2 = new Tag("test");
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
        Tag tag = new Tag("demo") { Id = Guid.NewGuid() };
        Assert.NotEqual(Guid.Empty, tag.Id);
    }
}
