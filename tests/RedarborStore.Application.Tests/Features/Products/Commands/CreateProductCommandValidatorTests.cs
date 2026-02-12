using FluentAssertions;
using RedarborStore.Application.Features.Products.Commands.CreateProduct;

namespace RedarborStore.Application.Tests.Features.Products.Commands;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldPass()
    {
        var command = new CreateProductCommand
        {
            Name = "Laptop",
            Price = 1299.99m,
            Stock = 50,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyName_ShouldFail()
    {
        var command = new CreateProductCommand
        {
            Name = "",
            Price = 10m,
            Stock = 1,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WithZeroPrice_ShouldFail()
    {
        var command = new CreateProductCommand
        {
            Name = "Product",
            Price = 0m,
            Stock = 1,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task Validate_WithNegativePrice_ShouldFail()
    {
        var command = new CreateProductCommand
        {
            Name = "Product",
            Price = -5m,
            Stock = 1,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task Validate_WithNegativeStock_ShouldFail()
    {
        var command = new CreateProductCommand
        {
            Name = "Product",
            Price = 10m,
            Stock = -1,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Stock");
    }

    [Fact]
    public async Task Validate_WithZeroStock_ShouldPass()
    {
        var command = new CreateProductCommand
        {
            Name = "Coming Soon",
            Price = 10m,
            Stock = 0,
            CategoryId = 1
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithZeroCategoryId_ShouldFail()
    {
        var command = new CreateProductCommand
        {
            Name = "Product",
            Price = 10m,
            Stock = 1,
            CategoryId = 0
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CategoryId");
    }

    [Fact]
    public async Task Validate_WithMultipleErrors_ShouldReportAll()
    {
        var command = new CreateProductCommand
        {
            Name = "",
            Price = -1m,
            Stock = -5,
            CategoryId = 0
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThanOrEqualTo(4);
    }
}