using FluentAssertions;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Tests.Builders;

namespace RedarborStore.Domain.Tests.Entities;

public class CategoryTests
{
    [Fact]
    public void Category_WhenCreated_ShouldHaveDefaultValues()
    {
        var category = new Category();
        category.Id.Should().Be(0);
        category.Name.Should().BeEmpty();
        category.Description.Should().BeNull();
        category.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Category_WhenCreatedWithBuilder_ShouldHaveCorrectValues()
    {
        var category = new CategoryBuilder()
            .WithId(1)
            .WithName("Electronics")
            .WithDescription("Electronic devices")
            .Build();
        category.Id.Should().Be(1);
        category.Name.Should().Be("Electronics");
        category.Description.Should().Be("Electronic devices");
    }

    [Theory]
    [InlineData("Electronics")]
    [InlineData("Clothing")]
    [InlineData("Food & Beverages")]
    [InlineData("Home & Garden")]
    public void Category_Name_ShouldAcceptValidNames(string name)
    {
        var category = new CategoryBuilder()
            .WithName(name)
            .Build();
        category.Name.Should().Be(name);
    }

    [Fact]
    public void Category_Description_ShouldBeNullable()
    {
        var category = new CategoryBuilder()
            .WithDescription(null)
            .Build();
        category.Description.Should().BeNull();
    }

    [Fact]
    public void Category_Description_ShouldAcceptValue()
    {
        var description = "This is a detailed description of the category.";
        var category = new CategoryBuilder()
            .WithDescription(description)
            .Build();

        category.Description.Should().Be(description);
    }
    [Fact]
    public void Categories_WithSameId_ShouldHaveSameId()
    {
        var category1 = new CategoryBuilder().WithId(5).Build();
        var category2 = new CategoryBuilder().WithId(5).Build();
        category1.Id.Should().Be(category2.Id);
    }

    [Fact]
    public void Categories_WithDifferentIds_ShouldHaveDifferentIds()
    {
        var category1 = new CategoryBuilder().WithId(1).Build();
        var category2 = new CategoryBuilder().WithId(2).Build();
        category1.Id.Should().NotBe(category2.Id);
    }
}