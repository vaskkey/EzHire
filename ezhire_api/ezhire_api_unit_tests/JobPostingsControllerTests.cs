using System.Security.Claims;
using ezhire_api.Controllers;
using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ezhire_api_unit_tests;

public class JobPostingsControllerTests
{
    private (JobPostingsController, Mock<IJobPostingsService>, Mock<IAuthService>) CreateControllerWithUser()
    {
        var mockService = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var controller = new JobPostingsController(mockService.Object, mockAuth.Object);

        // Mock ClaimsIdentity and ClaimsPrincipal
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        }, "TestAuthType");
        var user = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return (controller, mockService, mockAuth);
    }

    [Fact]
    public async Task GetAllPostings_ReturnsOkWithPostings()
    {
        var (controller, mockService, _) = CreateControllerWithUser();
        var expected = new List<CampaignPostingGetDto>
        {
            new() { Id = 1, JobName = "Backend Developer" },
            new() { Id = 2, JobName = "QA Tester" }
        };

        mockService.Setup(s => s.GetAllForId(It.IsAny<CancellationToken>(), 7))
            .ReturnsAsync(expected);

        var result = await controller.GetAllPostings(CancellationToken.None, 7);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetPosting_ReturnsOkWithPosting()
    {
        var (controller, mockService, _) = CreateControllerWithUser();
        var expected = new JobPostingGetDto { Id = 11, JobName = "Frontend Developer" };

        mockService.Setup(s => s.GetById(It.IsAny<CancellationToken>(), 11))
            .ReturnsAsync(expected);

        var result = await controller.GetPosting(CancellationToken.None, 11);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task ApplyToPosting_ReturnsOkWithPosting()
    {
        var (controller, mockService, _) = CreateControllerWithUser();
        var application = new CandidateCreateDto { FirstName = "John", LastName = "Doe", Email = "john@example.com" };
        var expected = new JobApplicationGetDto { };

        mockService.Setup(s => s.Apply(It.IsAny<CancellationToken>(), 77, application))
            .ReturnsAsync(expected);

        var result = await controller.ApplyToPosting(CancellationToken.None, 77, application);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }
}