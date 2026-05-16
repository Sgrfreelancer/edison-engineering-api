using FluentAssertions;

using Moq;

using EdisonEngineering.Application.DTOs;
using EdisonEngineering.Application.Interfaces;
using EdisonEngineering.Application.Services;
using EdisonEngineering.Application.Common;

using EdisonEngineering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EdisonEngineering.Tests.Services;

public class BlogServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnBlogs()
    {
        // =====================================
        // ARRANGE
        // =====================================

        var blogs =
            new List<Blog>
            {
                new Blog
                {
                    Title = "Solar Blog",

                    Slug = "solar-blog",

                    Content = "Content"
                }
            };

        var repoMock =
            new Mock<IBlogRepository>();

        var loggerMock =
            new Mock<ILogger<BlogService>>();

        repoMock
            .Setup(x => x.GetPagedAsync(It.IsAny<BlogFilterDto>()))
            .ReturnsAsync((blogs, 1));

        var service =
            new BlogService(
                repoMock.Object,
                loggerMock.Object);

        var query = new BlogQueryDto
        {
            Page = 1,
            PageSize = 10,
            Search = null
        };

        // =====================================
        // ACT
        // =====================================

        var result =
            await service.GetAllAsync(query);

        // =====================================
        // ASSERT
        // =====================================

        result.Should().NotBeNull();

        result.Items.Count.Should().Be(1);

        result.Items[0].Title
            .Should().Be("Solar Blog");
    }
}