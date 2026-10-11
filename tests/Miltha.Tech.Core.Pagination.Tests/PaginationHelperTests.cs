namespace Miltha.Tech.Core.Pagination.Tests;

public class PaginationHelperTests
{
    private static readonly int[] Data = [1, 2, 3];

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(-1, 10, 0)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 10, 10)]
    [InlineData(101, 10, 11)]
    [InlineData(99, 10, 10)]
    [InlineData(1, 10, 1)]
    public void CalculateTotalPages_ShouldReturnExpectedValue(long totalRecords, int pageSize, int expected)
    {
        // Arrange

        // Act
        var pages = PaginationHelper.CalculateTotalPages(totalRecords, pageSize);

        // Assert
        Assert.Equal(expected, pages);
    }

    [Fact]
    public void CreatePagedResponse_ShouldPopulateMetadata()
    {
        // Arrange
        var filter = new PaginationFilter(2, 10);

        // Act
        var response = PaginationHelper.CreatePagedResponse(Data, 55, filter);

        // Assert
        Assert.Same(Data, response.Data);
        Assert.Equal(2, response.Number);
        Assert.Equal(10, response.Size);
        Assert.Equal(55, response.TotalRecords);
        Assert.Equal(6, response.Pages);
    }

    [Fact]
    public void CreatePagedResponse_ShouldLeaveLinksEmpty_WhenNoUriServiceIsProvided()
    {
        // Arrange
        var filter = new PaginationFilter(1, 10);

        // Act
        var response = PaginationHelper.CreatePagedResponse(Data, 55, filter);

        // Assert
        Assert.Null(response.EndpointPath.FirstPage);
        Assert.Null(response.EndpointPath.NextPage);
        Assert.Null(response.EndpointPath.PreviousPage);
        Assert.Null(response.EndpointPath.LastPage);
    }

    [Fact]
    public void CreatePagedResponse_ShouldBuildLinks_WhenUriServiceIsProvided()
    {
        // Arrange
        var filter = new PaginationFilter(2, 10);
        var uriService = new FakeUriService();

        // Act
        var response = PaginationHelper.CreatePagedResponse(Data, 55, filter, uriService);

        // Assert
        Assert.Equal("https://api.test/items?PageNumber=1&PageSize=10", response.EndpointPath.FirstPage);
        Assert.Equal("https://api.test/items?PageNumber=3&PageSize=10", response.EndpointPath.NextPage);
        Assert.Equal("https://api.test/items?PageNumber=1&PageSize=10", response.EndpointPath.PreviousPage);
        Assert.Equal("https://api.test/items?PageNumber=6&PageSize=10", response.EndpointPath.LastPage);
    }

    [Fact]
    public void CreatePagedResponse_ShouldNotSetNextPage_WhenOnLastPage()
    {
        // Arrange
        var filter = new PaginationFilter(6, 10);
        var uriService = new FakeUriService();

        // Act
        var response = PaginationHelper.CreatePagedResponse(Data, 55, filter, uriService);

        // Assert
        Assert.Null(response.EndpointPath.NextPage);
        Assert.NotNull(response.EndpointPath.PreviousPage);
    }

    [Fact]
    public void CreatePagedResponse_ShouldNotSetPreviousPage_WhenOnFirstPage()
    {
        // Arrange
        var filter = new PaginationFilter(1, 10);
        var uriService = new FakeUriService();

        // Act
        var response = PaginationHelper.CreatePagedResponse(Data, 55, filter, uriService);

        // Assert
        Assert.Null(response.EndpointPath.PreviousPage);
        Assert.NotNull(response.EndpointPath.NextPage);
    }

    [Fact]
    public void CreatePagedResponse_ShouldThrow_WhenPaginationIsNull()
    {
        // Arrange

        // Act
        var action = () => PaginationHelper.CreatePagedResponse(Data, 55, null);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void CreatePagedResponse_ShouldThrow_WhenDataIsNull()
    {
        // Arrange
        var filter = new PaginationFilter(1, 10);

        // Act
        var action = () => PaginationHelper.CreatePagedResponse<int>(null, 55, filter);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    private sealed class FakeUriService : IUriService
    {
        public string GetEndpointPath(PaginationFilter paginationFilter) =>
            $"https://api.test/items?PageNumber={paginationFilter.PageNumber}&PageSize={paginationFilter.PageSize}";
    }
}