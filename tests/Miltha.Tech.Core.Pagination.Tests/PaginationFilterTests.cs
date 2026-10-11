using Miltha.Tech.Core.Pagination;

namespace Miltha.Tech.Core.Pagination.Tests;

public class PaginationFilterTests
{
    [Fact]
    public void Default_ShouldUseDefaultPageNumberAndSize()
    {
        // Arrange

        // Act
        var filter = PaginationFilter.Default();

        // Assert
        Assert.Equal(PaginationFilter.DefaultPageNumber, filter.PageNumber);
        Assert.Equal(PaginationFilter.DefaultPageSize, filter.PageSize);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_ShouldNormalizePageNumber_WhenLowerThanDefault(int pageNumber)
    {
        // Arrange

        // Act
        var filter = new PaginationFilter(pageNumber, 10);

        // Assert
        Assert.Equal(PaginationFilter.DefaultPageNumber, filter.PageNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ShouldUseDefaultPageSize_WhenLowerThanOne(int pageSize)
    {
        // Arrange

        // Act
        var filter = new PaginationFilter(1, pageSize);

        // Assert
        Assert.Equal(PaginationFilter.DefaultPageSize, filter.PageSize);
    }

    [Fact]
    public void Constructor_ShouldClampPageSize_WhenGreaterThanMax()
    {
        // Arrange
        var requested = PaginationFilter.MaxPageSize + 1000;

        // Act
        var filter = new PaginationFilter(1, requested);

        // Assert
        Assert.Equal(PaginationFilter.MaxPageSize, filter.PageSize);
    }

    [Fact]
    public void Constructor_ShouldKeepPageSize_WhenWithinRange()
    {
        // Arrange
        var requested = 50;

        // Act
        var filter = new PaginationFilter(3, requested);

        // Assert
        Assert.Equal(requested, filter.PageSize);
        Assert.Equal(3, filter.PageNumber);
    }

    [Fact]
    public void Skip_ShouldReturnRecordsToSkip_ForCurrentPage()
    {
        // Arrange
        var filter = new PaginationFilter(3, 10);

        // Act
        var skip = filter.Skip;

        // Assert
        Assert.Equal(20, skip);
    }

    [Fact]
    public void Take_ShouldReturnPageSize()
    {
        // Arrange
        var filter = new PaginationFilter(2, 25);

        // Act
        var take = filter.Take;

        // Assert
        Assert.Equal(25, take);
    }

    [Fact]
    public void DefaultConstructor_ShouldMatchDefaultFilter()
    {
        // Arrange

        // Act
        var filter = new PaginationFilter();

        // Assert
        Assert.Equal(PaginationFilter.DefaultPageNumber, filter.PageNumber);
        Assert.Equal(PaginationFilter.DefaultPageSize, filter.PageSize);
    }
}
