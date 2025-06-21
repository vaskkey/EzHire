using System.Security.Claims;
using ezhire_api.Controllers;
using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ezhire_api_unit_tests;

public class JobApplicationsControllerTests
{
    private (JobApplicationsController, Mock<IJobApplicationsService>, Mock<IAuthService>) CreateControllerWithUser()
    {
        var mockService = new Mock<IJobApplicationsService>();
        var mockAuth = new Mock<IAuthService>();
        var controller = new JobApplicationsController(mockService.Object, mockAuth.Object);

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
    public async Task GetAllApplications_ReturnsOkWithApplications()
    {
        var (controller, mockService, _) = CreateControllerWithUser();
        var expected = new List<PostingApplicationDto>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        mockService.Setup(s => s.GetAllForPosting(It.IsAny<CancellationToken>(), 5))
            .ReturnsAsync(expected);

        var result = await controller.GetApplications(CancellationToken.None, 5);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetApplication_ReturnsOkWithApplication()
    {
        var (controller, mockService, _) = CreateControllerWithUser();
        var expected = new JobApplicationGetDto { Id = 12 };

        mockService.Setup(s => s.GetById(It.IsAny<CancellationToken>(), 12))
            .ReturnsAsync(expected);

        var result = await controller.GetApplication(CancellationToken.None, 12);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task RejectApplication_ReturnsOkWithApplication()
    {
        var (controller, mockService, mockAuth) = CreateControllerWithUser();
        var expected = new JobApplicationGetDto { Id = 13 };

        mockAuth.Setup(a =>
            a.ValidateRole(It.IsAny<CancellationToken>(), It.IsAny<ClaimsIdentity>(), UserType.RECRUITER));
        mockService.Setup(s => s.Reject(It.IsAny<CancellationToken>(), 13))
            .ReturnsAsync(expected);

        var result = await controller.RejectApplication(CancellationToken.None, 13);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task AcceptApplication_ReturnsOkWithApplication()
    {
        var (controller, mockService, mockAuth) = CreateControllerWithUser();
        var expected = new JobApplicationGetDto { Id = 14 };

        mockAuth.Setup(a =>
            a.ValidateRole(It.IsAny<CancellationToken>(), It.IsAny<ClaimsIdentity>(), UserType.RECRUITER));
        mockService.Setup(s => s.Accept(It.IsAny<CancellationToken>(), 14))
            .ReturnsAsync(expected);

        var result = await controller.AcceptApplication(CancellationToken.None, 14);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task PlanMeeting_ReturnsOkWithMeeting()
    {
        var (controller, mockService, mockAuth) = CreateControllerWithUser();
        var planDto = new ApplicationMeetingPlanDto();
        var expected = new RecruitmentStageMeetingGetDto { Id = 33 };

        mockAuth.Setup(a =>
            a.ValidateRole(It.IsAny<CancellationToken>(), It.IsAny<ClaimsIdentity>(), UserType.RECRUITER));
        mockService.Setup(s => s.PlanMeeting(It.IsAny<CancellationToken>(), 22, planDto))
            .ReturnsAsync(expected);

        var result = await controller.PlanMeeting(CancellationToken.None, 22, planDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }
}