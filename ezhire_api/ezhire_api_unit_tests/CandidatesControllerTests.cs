using ezhire_api.Controllers;
using ezhire_api.DTO;
using ezhire_api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ezhire_api_unit_tests;

public class CandidatesControllerTests
{
    [Fact]
    public async Task GetAllCandidates_ReturnsOkWithCandidates()
    {
        // Arrange
        var mockService = new Mock<ICandidatesService>();
        var expectedCandidates = new List<CandidateGetDto>
        {
            new CandidateGetDto
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-1),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                Experiences = new List<CandidateExperienceGetDto>
                {
                    new CandidateExperienceGetDto
                    {
                        Id = 101,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        CompanyName = "Google",
                        JobName = "Software Engineer",
                        DateStarted = DateTime.UtcNow.AddYears(-3),
                        DateFinished = DateTime.UtcNow.AddYears(-1),

                    }
                }
            },
            new CandidateGetDto
            {
                Id = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                UpdatedAt = DateTime.UtcNow.AddDays(-5),
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                Experiences = new List<CandidateExperienceGetDto>
                {
                    new CandidateExperienceGetDto
                    {
                        Id = 102,
                        CreatedAt = DateTime.UtcNow.AddDays(-10),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1),
                        CompanyName = "Microsoft",
                        JobName = "Software Engineer",
                        DateStarted = DateTime.UtcNow.AddYears(-5),
                        DateFinished = DateTime.UtcNow.AddYears(-3),

                    }
                }
            }
        };
        mockService.Setup(s => s.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCandidates);

        var controller = new CandidatesController(mockService.Object);

        // Act
        var result = await controller.GetAllCandidates(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expectedCandidates, okResult.Value);
    }
    
     [Fact]
        public async Task GetAllCandidates_ReturnsOkWithEmptyList()
        {
            // Arrange
            var mockService = new Mock<ICandidatesService>();
            mockService.Setup(s => s.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CandidateGetDto>());
            var controller = new CandidatesController(mockService.Object);

            // Act
            var result = await controller.GetAllCandidates(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Empty((ICollection<CandidateGetDto>)okResult.Value);
        }

        [Fact]
        public async Task ExtendOffer_ReturnsOkWithOffer()
        {
            // Arrange
            var mockService = new Mock<ICandidatesService>();
            var candidateId = 7;
            var candidate = new CandidateGetDto
            {
                Id = candidateId,
                FirstName = "Emily",
                LastName = "Jones",
                Email = "emily.jones@example.com"
            };
            var offer = new OfferGetDto
            {
                Id = 42,
                Accepted = false,
                DateExtended = DateTime.UtcNow
            };
            mockService.Setup(s => s.GetById(It.IsAny<CancellationToken>(), candidateId))
                .ReturnsAsync(candidate);
            mockService.Setup(s => s.ExtendOffer(It.IsAny<CancellationToken>(), candidate))
                .ReturnsAsync(offer);

            var controller = new CandidatesController(mockService.Object);

            // Act
            var result = await controller.ExtendOffer(CancellationToken.None, candidateId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(offer, okResult.Value);
        }

        [Fact]
        public async Task ExtendOffer_CandidateNotFound_ThrowsExceptionOrReturnsError()
        {
            // Arrange
            var mockService = new Mock<ICandidatesService>();
            var candidateId = 999;
            mockService.Setup(s => s.GetById(It.IsAny<CancellationToken>(), candidateId))
                .ThrowsAsync(new Exception("NotFound")); // Simulate service throwing not found
            var controller = new CandidatesController(mockService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await controller.ExtendOffer(CancellationToken.None, candidateId));
        }
}