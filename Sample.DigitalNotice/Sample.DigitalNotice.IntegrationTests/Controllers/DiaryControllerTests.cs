using NUnit.Framework;
using Sample.DigitalNotice.Common.Entities;
using Sample.DigitalNotice.Common.Requests;
using System.Net;
using Sample.DigitalNotice.IntegrationTests.Utilities;
using FluentAssertions;
using System.Text;
using System.Text.Json;

namespace Sample.DigitalNotice.IntegrationTests.Controllers;

[TestFixture]
internal class DiaryControllerTests : IntegrationTestBase
{
    [Test]
    public async Task Create_ValidRequest_ReturnsCreatedAtAction()
    {
        var model = new DiaryRequestModel 
        { 
            Name = "New diary", 
            Description = "Description about new diary"
        };

        // Act
        using var response = await httpClient.SendRequestAsync(HttpMethod.Post, "/api/diaries", model);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

        var diary = await response.DeserializeContentAsync<Diary>();
        diary.Should().NotBeNull();
    }

    [Test]
    public async Task Create_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new DiaryRequestModel();

        // Act
        using var response = await httpClient.SendRequestAsync(HttpMethod.Post, "/api/diaries", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetByPage_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var expectedResult = new[]
        {
            new Diary 
            {
                Name = "Diary 1",
                Description = "Description about diary 1"
            },
            new Diary 
            { 
                Name = "Diary 2", 
                Description = "Description about diary 2"
            },
            new Diary 
            { 
                Name = "Diary 3",
                Description = "Description about diary 3"
            },
            new Diary 
            { 
                Name = "Diary 4", 
                Description = "Description about diary 4"
            },
            new Diary 
            { 
                Name = "Diary 5", 
                Description = "Description about diary 5"
            },
        };

        DiaryAccessor.Push(expectedResult);

        var model = new GetByPageQueryModel
        {
            PageSize = 10,
        };

        // Act
        using var response = await httpClient.GetAsync($"/api/diaries{model.ToQueryString()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

        var diaries = await response.DeserializeContentAsync<IEnumerable<Diary>>();
        diaries.Should().NotBeEmpty(); // Ensure the result set is not empty

        // Assert that all expected diaries are present in the result set
        expectedResult.Should().OnlyContain(expectedDiary =>
            diaries.Any(actualDiary =>
                actualDiary.Id == expectedDiary.Id &&
                actualDiary.Name == expectedDiary.Name &&
                actualDiary.Description == expectedDiary.Description
            )
        );
        // Additional assertions for the retrieved diaries
    }

    [Test]
    public async Task GetById_ExistingId_ReturnsOkResult()
    {
        // Arrange
        var expectedResult = new Diary
        {
            Name = "Existing diary 1 for retrieving by id",
            Description = "Description about following diary",
        };

        DiaryAccessor.Push(expectedResult);

        // Act
        using var response = await httpClient.GetAsync($"/api/diaries/{expectedResult.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.MediaType.Should().Be("application/json");

        var diary = await response.DeserializeContentAsync<Diary>();
        diary.Should().BeEquivalentTo(expectedResult);
        // Additional assertions for the retrieved diary
    }

    [Test]
    public async Task GetById_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = "123456";

        // Act
        using var response = await httpClient.GetAsync($"/api/diaries/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        using var response = await httpClient.GetAsync($"/api/diaries/{nonExistingId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Update_ExistingId_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var existingDiary = new Diary
        {
            Name = "Existing diary 1 for updation by id",
            Description = "Description about following diary",
        };

        DiaryAccessor.Push(existingDiary);

        var model = new DiaryRequestModel
        {
            Name = "New diary name",
            Description = "New diary description about following diary",
        };

        var expectedResult = new Diary
        {
            Id = existingDiary.Id,
            Name = model.Name,
            Description = model.Description,
        };

        // Act
        using var response = await httpClient.SendRequestAsync(HttpMethod.Put, $"/api/diaries/{existingDiary.Id}", model);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        DiaryAccessor.GetById(existingDiary.Id).Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task Update_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidId = Guid.Empty;
        var model = new DiaryRequestModel
        {
            Name = "New diary name",
            Description = "New diary description about following diary",
        };

        // Act
        using var response = await httpClient.SendRequestAsync(HttpMethod.Put, $"/api/diaries/{invalidId}", model);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var nonExistingDiaryId = Guid.NewGuid();
        var model = new DiaryRequestModel
        {
            Name = "New diary name",
            Description = "New diary description about following diary",
        };

        // Act
        using var response = await httpClient.SendRequestAsync(HttpMethod.Put, $"/api/diaries/{nonExistingDiaryId}", model);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var diary = new Diary
        {
            Name = "Diary for deleting",
            Description = "Description about following diary",
        };

        DiaryAccessor.Push(diary);

        // Act
        using var response = await httpClient.DeleteAsync($"/api/diaries/{diary.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Delete_InvalidId_ReturnsBadRequest()
    {
        // Arrange
        var invalidDiaryId = Guid.Empty;

        // Act
        using var response = await httpClient.DeleteAsync($"/api/diaries/{invalidDiaryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var nonExistingDiaryId = Guid.NewGuid();

        // Act
        using var response = await httpClient.DeleteAsync($"/api/diaries/{nonExistingDiaryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
