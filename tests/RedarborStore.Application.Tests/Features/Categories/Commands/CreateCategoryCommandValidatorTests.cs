using FluentAssertions;
using RedarborStore.Application.Features.Categories.Commands.CreateCategory;

namespace RedarborStore.Application.Tests.Features.Categories.Commands;

public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandValidatorTests()
    {
        _validator = new CreateCategoryCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldPass()
    {
        var command = new CreateCategoryCommand { Name = "Electronics", Description = "Devices" };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptyName_ShouldFail()
    {
        var command = new CreateCategoryCommand { Name = "", Description = "Test" };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WithNameExceeding100Chars_ShouldFail()
    {
        var command = new CreateCategoryCommand
        {
            Name = new string('A', 101),
            Description = "Test"
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_WithDescriptionExceeding500Chars_ShouldFail()
    {
        var command = new CreateCategoryCommand
        {
            Name = "Valid Name",
            Description = new string('B', 501)
        };
        var result = await _validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task Validate_WithNullDescription_ShouldPass()
    {
        var command = new CreateCategoryCommand { Name = "Valid", Description = null };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithNameExactly100Chars_ShouldPass()
    {
        var command = new CreateCategoryCommand
        {
            Name = new string('A', 100),
            Description = "Test"
        };
        var result = await _validator.ValidateAsync(command);
        result.IsValid.Should().BeTrue();
    }
}