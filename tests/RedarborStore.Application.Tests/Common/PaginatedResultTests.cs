using FluentAssertions;
using RedarborStore.Application.Common;

namespace RedarborStore.Application.Tests.Common;

public class PaginatedResultTests
{
    [Fact]
    public void TotalPages_WithExactDivision_ShouldCalculateCorrectly()
    {
        var result = new PaginatedResult<string>
        {
            Items = new[] { "a", "b" },
            PageNumber = 1,
            PageSize = 5,
            TotalCount = 20
        };

        result.TotalPages.Should().Be(4);
    }

    [Fact]
    public void TotalPages_WithRemainder_ShouldRoundUp()
    {
        var result = new PaginatedResult<string>
        {
            Items = new[] { "a" },
            PageNumber = 1,
            PageSize = 3,
            TotalCount = 7
        };

        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public void TotalPages_WithZeroItems_ShouldBeZero()
    {
        var result = new PaginatedResult<string>
        {
            Items = Enumerable.Empty<string>(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0
        };

        result.TotalPages.Should().Be(0);
    }

    [Fact]
    public void TotalPages_WithSingleItem_ShouldBeOne()
    {
        var result = new PaginatedResult<string>
        {
            Items = new[] { "a" },
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 1
        };

        result.TotalPages.Should().Be(1);
    }

    [Fact]
    public void HasPreviousPage_OnFirstPage_ShouldBeFalse()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 50
        };

        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_OnSecondPage_ShouldBeTrue()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 2,
            PageSize = 10,
            TotalCount = 50
        };

        result.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_OnLastPage_ShouldBeFalse()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 5,
            PageSize = 10,
            TotalCount = 50
        };

        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void HasNextPage_NotOnLastPage_ShouldBeTrue()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 3,
            PageSize = 10,
            TotalCount = 50
        };

        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_WithSinglePage_ShouldBeFalse()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 5
        };

        result.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public void HasPreviousPage_WithSinglePage_ShouldBeFalse()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 5
        };

        result.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public void HasBothNavigation_OnMiddlePage_ShouldBeTrue()
    {
        var result = new PaginatedResult<string>
        {
            PageNumber = 3,
            PageSize = 10,
            TotalCount = 50
        };

        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
    }
}