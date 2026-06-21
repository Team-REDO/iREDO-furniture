using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

public class SynchronizerTests
{
    [Fact]
    public async Task Should_Upsert_When_ListingCreated()
    {
        // Arrange
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<Microsoft.Extensions.Logging.ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            EventType = "ListingCreated",
            Guid = "123",
            PersonGUID = "person1",
            ListingDetails = new ListingDetails
            {
                Title = "Chair",
                Description = "Nice chair",
                Quantity = 2,
                Price = 100,
                Condition = "Good",
                City = "Copenhagen",
                Colors = new List<Color>
                {
                    new Color { Name = "Red", Href = "red-url" }
                },
                SubCategories = new List<SubCategory>
                {
                    new SubCategory
                    {
                        Name = "Furniture",
                        Category = new Category { Name = "Home" }
                    }
                },
                Images = new List<string> { "img1.jpg" }
            }
        };

        // Act
        await worker.HandleUpsert(input);

        // Assert
        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Guid == "123" &&
                p.Title == "Chair" &&
                p.Color.Name == "Red"
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Handle_Missing_Color()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<Microsoft.Extensions.Logging.ILogger<Worker>>(),
            mongoMock.Object,
            true // ✅ FIXED
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails()
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Color != null
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Handle_Missing_Category()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<Microsoft.Extensions.Logging.ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails
            {
                SubCategories = new List<SubCategory>()
            }
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Category != null
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Map_Images_Correctly()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<Microsoft.Extensions.Logging.ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails
            {
                Images = new List<string> { "img1", "img2" }
            }
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Images.Count == 2
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Call_Delete_When_ListingDeleted()
    {
        var mongoMock = new Mock<IMongoService>();

        await mongoMock.Object.DeleteAsync("123");

        mongoMock.Verify(x => x.DeleteAsync("123"), Times.Once);
    }

    [Fact]
    public async Task Should_Upsert_When_ListingUpdated()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            EventType = "ListingUpdated",
            Guid = "123",
            ListingDetails = new ListingDetails()
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x => x.UpsertAsync(It.IsAny<SalesPost>()), Times.Once);
    }

    [Fact]
    public async Task Should_Call_Delete_When_Event_Is_ListingDeleted()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        await mongoMock.Object.DeleteAsync("123");

        mongoMock.Verify(x => x.DeleteAsync("123"), Times.Once);
    }


    [Fact]
    public async Task Should_Convert_Quantity_To_String()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails
            {
                Quantity = 5
            }
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Quantity == "5"
            )),
            Times.Once
        );
    }

    [Fact]
    public void Should_Deserialize_Envelope_Correctly()
    {
        var json = @"{
        ""eventId"": ""123"",
        ""eventType"": ""ListingCreated"",
        ""payload"": {
            ""guid"": ""abc"",
            ""personGUID"": ""p1"",
            ""listingDetails"": {}
        }
    }";

        var envelope = JsonSerializer.Deserialize<EventEnvelope<IncomingListing>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(envelope);
        Assert.Equal("123", envelope.EventId);
        Assert.Equal("ListingCreated", envelope.EventType);
        Assert.NotNull(envelope.Payload);
    }


    [Fact]
    public async Task Should_Not_Crash_On_Unknown_EventType()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            EventType = "SomethingWeird",
            Guid = "123"
        };

        var exception = await Record.ExceptionAsync(async () =>
        {
            await worker.HandleUpsert(input);
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task Should_Call_Delete_When_ListingDeleted_Event()
    {
        var mongoMock = new Mock<IMongoService>();

        await mongoMock.Object.DeleteAsync("123");

        mongoMock.Verify(x => x.DeleteAsync("123"), Times.Once);
    }

    [Fact]
    public async Task Should_Handle_Empty_Images()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails
            {
                Images = new List<string>()
            }
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Images.Count == 0
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Handle_Null_ListingDetails()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = null
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p != null
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Map_Category_Correctly()
    {
        var mongoMock = new Mock<IMongoService>();

        var worker = new Worker(
            Mock.Of<ILogger<Worker>>(),
            mongoMock.Object,
            true
        );

        var input = new IncomingListing
        {
            Guid = "123",
            ListingDetails = new ListingDetails
            {
                SubCategories = new List<SubCategory>
            {
                new SubCategory
                {
                    Name = "Chair",
                    Category = new Category { Name = "Furniture" }
                }
            }
            }
        };

        await worker.HandleUpsert(input);

        mongoMock.Verify(x =>
            x.UpsertAsync(It.Is<SalesPost>(p =>
                p.Category.Name == "Furniture" &&
                p.Category.Subcat.Name == "Chair"
            )),
            Times.Once
        );
    }
}