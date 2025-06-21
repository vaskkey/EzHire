using System.Security.Claims;
using System.Security.Principal;
using ezhire_api.Controllers;
using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ezhire_api_unit_tests;

public class RecruitmentCampaignsControllerTests
{
    [Fact]
    public async Task GetAllCampaigns_ReturnsOkWithCampaigns()
    {
        var mockCampaigns = new Mock<IRecruitmentCampaignsService>();
        var mockPostings = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var expected = new List<RecruitmentCampaignGetDto>
        {
            new() { Id = 1, Name = "Spring Hiring" },
            new() { Id = 2, Name = "Summer Interns" }
        };
        mockCampaigns.Setup(s => s.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new RecruitmentCampaignsController(mockCampaigns.Object, mockPostings.Object, mockAuth.Object);

        var result = await controller.GetAllCampaigns(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task GetCampaign_ReturnsOkWithCampaign()
    {
        var mockCampaigns = new Mock<IRecruitmentCampaignsService>();
        var mockPostings = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var campaign = new RecruitmentCampaignGetDto { Id = 5, Name = "Fall Drive" };
        mockCampaigns.Setup(s => s.GetById(It.IsAny<CancellationToken>(), 5)).ReturnsAsync(campaign);

        var controller = new RecruitmentCampaignsController(mockCampaigns.Object, mockPostings.Object, mockAuth.Object);

        var result = await controller.GetCampaign(CancellationToken.None, 5);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(campaign, okResult.Value);
    }

    [Fact]
    public async Task CreateCampaign_ReturnsCreatedAtActionWithCampaign()
    {
        var mockCampaigns = new Mock<IRecruitmentCampaignsService>();
        var mockPostings = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var createDto = new RecruitmentCampaignCreateDto { Name = "Winter Blitz" };
        var userObj = new UserGetDto();
        var response = new RecruitmentCampaignGetDto { Id = 100, Name = "Winter Blitz" };

        // Create a ClaimsIdentity with desired claims
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        }, "TestAuthType");

        var user = new ClaimsPrincipal(identity);

        mockAuth.Setup(a => a.GetUser(It.IsAny<CancellationToken>(), It.IsAny<IIdentity>()))
            .ReturnsAsync(userObj);
        mockAuth.Setup(a => a.ValidateRole(userObj, UserType.HIRING_MANAGER));
        mockCampaigns.Setup(s => s.Create(It.IsAny<CancellationToken>(), createDto, userObj))
            .ReturnsAsync(response);

        var controller = new RecruitmentCampaignsController(mockCampaigns.Object, mockPostings.Object, mockAuth.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await controller.CreateCampaign(CancellationToken.None, createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(controller.GetCampaign), createdResult.ActionName);
        Assert.Equal(response.Id, ((RecruitmentCampaignGetDto)createdResult.Value).Id);
    }

    [Fact]
    public async Task AddPosting_ReturnsCreatedWithPosting()
    {
        var mockCampaigns = new Mock<IRecruitmentCampaignsService>();
        var mockPostings = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var postingCreateDto = new CampaignPostingCreateDto { JobName = "Junior Developer" };
        var campaign = new RecruitmentCampaignGetDto { Id = 3, Name = "Summer Interns" };
        var posting = new JobPostingGetDto { Id = 50, JobName = "Junior Developer" };

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        }, "TestAuthType");

        var user = new ClaimsPrincipal(identity);

        mockAuth.Setup(a =>
            a.ValidateRole(It.IsAny<CancellationToken>(), It.IsAny<IIdentity>(), UserType.HIRING_MANAGER));
        mockCampaigns.Setup(s => s.GetById(It.IsAny<CancellationToken>(), 3)).ReturnsAsync(campaign);
        mockPostings.Setup(p => p.AddPosting(It.IsAny<CancellationToken>(), 3, postingCreateDto)).ReturnsAsync(posting);

        var controller = new RecruitmentCampaignsController(mockCampaigns.Object, mockPostings.Object, mockAuth.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = await controller.AddPosting(CancellationToken.None, 3, postingCreateDto);

        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal($"/api/postings/{posting.Id}", createdResult.Location);
        Assert.Equal(posting, createdResult.Value);
    }

    [Fact]
    public async Task CreateCampaign_InvalidRole_ThrowsOrReturnsError()
    {
        var mockCampaigns = new Mock<IRecruitmentCampaignsService>();
        var mockPostings = new Mock<IJobPostingsService>();
        var mockAuth = new Mock<IAuthService>();
        var createDto = new RecruitmentCampaignCreateDto { Name = "Winter Blitz" };
        var userObj = new UserGetDto();

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.NameIdentifier, "123")
        }, "TestAuthType");

        var user = new ClaimsPrincipal(identity);

        mockAuth.Setup(a => a.GetUser(It.IsAny<CancellationToken>(), It.IsAny<IIdentity>()))
            .ReturnsAsync(userObj);
        mockAuth.Setup(a => a.ValidateRole(userObj, UserType.HIRING_MANAGER))
            .Throws(new UnauthorizedAccessException());

        var controller = new RecruitmentCampaignsController(mockCampaigns.Object, mockPostings.Object, mockAuth.Object);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await controller.CreateCampaign(CancellationToken.None, createDto));
    }
}