using AttendanceSystem.Application.Contracts.Persistence;
using AttendanceSystem.Application.Exceptions;
using AttendanceSystem.Application.Features.Members.Commands.AddMember;
using AttendanceSystem.Application.Utilities;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Enums;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Test
{
    public class AddMemberCommandHandlerTests
    {
        private readonly Mock<IAsyncRepository<Member>> _memberRepositoryMock = new();
        private readonly Mock<IAsyncRepository<Fellowship>> _fellowshipRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILogger<AddMemberCommandHandler>> _loggerMock = new();

        private readonly Guid _testUserId = Guid.NewGuid();
        private readonly Guid _testGroupId = Guid.NewGuid();
        private readonly Guid _testMemberId = Guid.NewGuid();
        private readonly Guid _testFellowshipId = Guid.Parse("F62A5D9D-1672-4B68-A69D-8D5F70E5A5FF");

        private readonly AddMemberCommandHandler _handler;

        public AddMemberCommandHandlerTests()
        {
            // Set up IUserService mock to return expected user details
            _userServiceMock.Setup(x => x.UserDetails()).Returns(new TokenUserData
            {
                UserId = _testUserId,
                FullName = "Test Discipler",
                GroupId = _testGroupId,
                GroupName = "Test Fellowship"
            });

            _handler = new AddMemberCommandHandler(
                _loggerMock.Object,
                _memberRepositoryMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object,
                _fellowshipRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldAddMemberAndReturnSuccess()
        {
            // GIVEN: A valid request
            var request = new AddMemberCommand
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "08180084896",
                Email = "john@example.com",
                FellowshipId = _testFellowshipId,
                Gender = GenderEnum.Male,
                MemberType = MemberType.WorkersInTraining
            };


            var mappedMember = new Member
            {
                Id = _testMemberId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "08180084896",
                Gender = GenderEnum.Male,
                FellowshipId = _testFellowshipId
            };

            _mapperMock.Setup(m => m.Map<Member>(It.IsAny<AddMemberCommand>())).Returns(mappedMember);
            _memberRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Member>())).ReturnsAsync(mappedMember);

            // Mock the Fellowship Repository to return 1 (fellowship exists)
            _fellowshipRepositoryMock.Setup(r => r.CountAsync(It.IsAny<Expression<Func<Fellowship, bool>>>(), false))
                .ReturnsAsync(1);

            // When
            var result = await _handler.Handle(request, CancellationToken.None);

            // Then
            Assert.True(result.Success);
            //Assert.
            Assert.Equal("Completed successfully", result.Message);
            Assert.Equal(_testMemberId, result.Result.Id);
            Assert.Equal("Test Discipler", result.Result.DisciplerFullName);
            Assert.Equal(_userServiceMock.Object.UserDetails().GroupId, result.Result.FellowshipId);

        }

        [Fact]
        public async Task Handle_InvalidRequest_ShouldReturnValidationErrors()
        {
            // GIVEN: An invalid request (missing required fields)
            var request = new AddMemberCommand();

            // Use the real validator instance
            var validator = new AddMemberCommandValidator(_memberRepositoryMock.Object, _fellowshipRepositoryMock.Object);
            var validationResult = await validator.ValidateAsync(request, CancellationToken.None);

            // Ensure validation fails before proceeding with Handle method
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain(x => x.PropertyName == "FirstName" && x.ErrorMessage == "First name is required");

            // WHEN: Handle method is called
            var response = await _handler.Handle(request, CancellationToken.None);

            // THEN: Ensure validation fails
            response.Success.Should().BeFalse();
            response.ValidationErrors.Should().Contain(x => x.Contains("First name is required"));
        }


        [Fact]
        public async Task Handle_ThrowsCustomException_ShouldReturnCustomErrorMessage()
        {
            // GIVEN: Exception in repository
            var request = new AddMemberCommand { FirstName = "John" };

            _memberRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Member>()))
                .ThrowsAsync(new CustomException("Custom error"));

            // WHEN: Handle method is called
            var response = await _handler.Handle(request, CancellationToken.None);

            // THEN: Ensure custom exception is caught
            response.Success.Should().BeFalse();
            response.Message.Should().Be("Custom error");
        }

        [Fact]
        public async Task Handle_ThrowsUnexpectedException_ShouldReturnGenericErrorMessage()
        {
            // GIVEN: Unexpected exception
            var request = new AddMemberCommand { FirstName = "John" };

            _memberRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Member>()))
                .ThrowsAsync(new Exception("Database failure"));

            // WHEN: Handle method is called
            var response = await _handler.Handle(request, CancellationToken.None);

            // THEN: Ensure exception is logged and generic error is returned
            response.Success.Should().BeFalse();
            response.Message.Should().Be(Constants.ErrorResponse);

            _loggerMock.Verify(
                x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()),
                Times.Once);
        }
    }
}