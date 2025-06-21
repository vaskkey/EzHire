using System.Security.Claims;
using System.Security.Principal;
using ezhire_api.Controllers;
using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ezhire_api_unit_tests;

public class RecruitmentStagesControllerTests
{
     [Fact]
        public async Task GetAllStages_ReturnsOkWithStages()
        {
            // Arrange
            var mockStages = new Mock<IRecruitmentStagesService>();
            var mockAuth = new Mock<IAuthService>();
            var expectedStages = new List<RecruitmentStageGetDto>
            {
                new CultureMeetingGetDto(),
                new TeamMeetingGetDto()
            };

            mockStages.Setup(s => s.GetAllForId(It.IsAny<CancellationToken>(), 10))
                .ReturnsAsync(expectedStages);

            var controller = new RecruitmentStagesController(mockStages.Object, mockAuth.Object);

            // Act
            var result = await controller.GetAllStages(CancellationToken.None, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedStages, okResult.Value);
        }

        [Fact]
        public async Task GetStage_ReturnsOkWithStage()
        {
            // Arrange
            var mockStages = new Mock<IRecruitmentStagesService>();
            var mockAuth = new Mock<IAuthService>();
            var expectedStage = new CultureMeetingGetDto();

            mockStages.Setup(s => s.GetById(It.IsAny<CancellationToken>(), 5))
                .ReturnsAsync(expectedStage);

            var controller = new RecruitmentStagesController(mockStages.Object, mockAuth.Object);

            // Act
            var result = await controller.GetStage(CancellationToken.None, 5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedStage, okResult.Value);
        }

        [Fact]
        public async Task AddStage_ReturnsOkWithCreatedStage()
        {
            
            // Arrange
            var mockStages = new Mock<IRecruitmentStagesService>();
            var mockAuth = new Mock<IAuthService>();
            var createDto = new GenericRecruitmentStageCreateDto {};
            var userObj = new UserGetDto();
            var createdStage = new TechnicalMeetingGetDto
            {
                Posting = new CampaignPostingGetDto(),
            };
            
            // Create a ClaimsIdentity with desired claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "TestAuthType");

            var user = new ClaimsPrincipal(identity);

            mockAuth.Setup(a => a.GetUser(It.IsAny<CancellationToken>(), It.IsAny<IIdentity>()))
                .ReturnsAsync(userObj);
            mockAuth.Setup(a => a.ValidateRole(userObj, UserType.RECRUITER));
            mockStages.Setup(s => s.Create(It.IsAny<CancellationToken>(), createDto, userObj))
                .ReturnsAsync(createdStage);

            var controller = new RecruitmentStagesController(mockStages.Object, mockAuth.Object);
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };


            // Act
            var result = await controller.AddStage(CancellationToken.None, createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(createdStage, okResult.Value);
        }
}