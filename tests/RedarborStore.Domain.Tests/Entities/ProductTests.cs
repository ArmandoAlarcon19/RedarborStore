using FluentAssertions;
using RedarborStore.Domain.Entities;
using RedarborStore.Domain.Tests.Builders;

namespace RedarborStore.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_WhenCreated_ShouldHaveDefaultValues()
    {
        var product = new Product();
        product.Id.Should().Be(0);
        product.Name.Should().BeEmpty();
        product.Description.Should().BeNull();
        product.Price.Should().Be(0);
        product.Stock.Should().Be(0);
        product.CategoryId.Should().Be(0);
        product.UpdatedDate.Should().BeNull();
        product.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Product_WhenCreatedWithBuilder_ShouldHaveCorrectValues()
    {
        var product = new ProductBuilder()
            .WithId(1)
            .WithName("Laptop")
            .WithDescription("Gaming laptop")
            .WithPrice(1299.99m)
            .WithStock(50)
            .WithCategoryId(1)
            .Build();
        product.Id.Should().Be(1);
        product.Name.Should().Be("Laptop");
        product.Description.Should().Be("Gaming laptop");
        product.Price.Should().Be(1299.99m);
        product.Stock.Should().Be(50);
        product.CategoryId.Should().Be(1);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(9.99)]
    [InlineData(99.99)]
    [InlineData(9999.99)]
    [InlineData(99999.99)]
    public void Product_Price_ShouldAcceptPositiveValues(decimal price)
    {
        var product = new ProductBuilder()
            .WithPrice(price)
            .Build();
        product.Price.Should().Be(price);
        product.Price.Should().BePositive();
    }

    [Fact]
    public void Product_Price_ShouldHandleZero()
    {
        var product = new ProductBuilder()
            .WithPrice(0)
            .Build();
        product.Price.Should().Be(0);
    }

    [Fact]
    public void Product_Price_ShouldHandleNegative()
    {
        var product = new ProductBuilder()
            .WithPrice(-10.50m)
            .Build();
        product.Price.Should().BeNegative();
    }

    [Fact]
    public void Product_Price_ShouldMaintainDecimalPrecision()
    {
        var product = new ProductBuilder()
            .WithPrice(1299.99m)
            .Build();
        product.Price.Should().Be(1299.99m);
        product.Price.Should().NotBe(1299.98m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(10000)]
    public void Product_Stock_ShouldAcceptNonNegativeValues(int stock)
    {
        var product = new ProductBuilder()
            .WithStock(stock)
            .Build();
        product.Stock.Should().Be(stock);
        product.Stock.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void Product_Stock_CanBeModified()
    {
        var product = new ProductBuilder()
            .WithStock(100)
            .Build();
        product.Stock += 50;
        product.Stock.Should().Be(150);
    }

    [Fact]
    public void Product_Stock_CanBeReduced()
    {
        var product = new ProductBuilder()
            .WithStock(100)
            .Build();
        product.Stock -= 30;
        product.Stock.Should().Be(70);
    }

    [Fact]
    public void Product_Stock_ShouldAllowZeroAfterFullExit()
    {
        var product = new ProductBuilder()
            .WithStock(50)
            .Build();
        product.Stock -= 50;
        product.Stock.Should().Be(0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Product_CategoryId_ShouldStoreCorrectValue(int categoryId)
    {
        var product = new ProductBuilder()
            .WithCategoryId(categoryId)
            .Build();
        product.CategoryId.Should().Be(categoryId);
    }

    [Fact]
    public void Product_CategoryId_CanBeChanged()
    {
        var product = new ProductBuilder()
            .WithCategoryId(1)
            .Build();
        product.CategoryId = 2;
        product.CategoryId.Should().Be(2);
    }

    [Fact]
    public void Product_UpdatedAt_ShouldBeNullOnCreation()
    {
        var product = new Product();
        product.UpdatedDate.Should().BeNull();
    }

    [Fact]
    public void Product_UpdatedAt_CanBeSet()
    {
        var product = new ProductBuilder().Build();
        var updateTime = DateTime.UtcNow;
        product.UpdatedDate = updateTime;
        product.UpdatedDate.Should().NotBeNull();
        product.UpdatedDate.Should().Be(updateTime);
    }

    [Fact]
    public void Product_WhenModified_ShouldTrackUpdateTime()
    {
        var product = new ProductBuilder()
            .WithPrice(100m)
            .Build();
        product.UpdatedDate.Should().BeNull();
        product.Price = 150m;
        product.UpdatedDate = DateTime.UtcNow;
        product.Price.Should().Be(150m);
        product.UpdatedDate.Should().NotBeNull();
        product.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}