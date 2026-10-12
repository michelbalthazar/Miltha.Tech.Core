using System.Net;
using Miltha.Tech.Data.Mongo.Extensions;
using Miltha.Tech.Data.Mongo.Settings;

namespace Miltha.Tech.Core.Data.Mongo.Tests;

public class MongoAbstractionsTests
{
    [Fact]
    public void BsonCollectionAttribute_ShouldStoreCollectionName()
    {
        // Arrange

        // Act
        var attribute = new BsonCollectionAttribute("users");

        // Assert
        Assert.Equal("users", attribute.CollectionName);
    }

    [Fact]
    public void BsonCollectionAttribute_ShouldThrow_WhenNameIsNull()
    {
        // Arrange

        // Act
        var action = () => new BsonCollectionAttribute(null);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void ConcurrencyConflictException_ShouldCarryConflictData()
    {
        // Arrange
        var id = Guid.NewGuid();
        const string expected = "stamp-1";

        // Act
        var exception = new ConcurrencyConflictException(id, expected);

        // Assert
        Assert.Equal(id, exception.Id);
        Assert.Equal(expected, exception.Expected);
        Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
        Assert.Contains(id.ToString(), exception.Message);
    }

    [Fact]
    public void MongoDbSettings_ShouldComposeConnectionString()
    {
        // Arrange
        var settings = new MongoDbSettings("miltha", "localhost", "27017");

        // Act
        var connectionString = settings.ConnectionString;

        // Assert
        Assert.Equal("mongodb://localhost:27017", connectionString);
    }
}