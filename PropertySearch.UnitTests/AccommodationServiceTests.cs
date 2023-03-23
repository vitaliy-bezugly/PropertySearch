using AutoMapper;
using FluentAssertions;
using LanguageExt.Common;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using PropertySearchApp.Common.Exceptions;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;

namespace PropertySearch.UnitTests;

public class AccommodationServiceTests
{
    private readonly AccommodationService _sut;
    private readonly IUserValidatorService _userValidator;

    private readonly IAccommodationRepository _accommodationRepository = Substitute.For<IAccommodationRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUserReceiverRepository _userReceiver = Substitute.For<IUserReceiverRepository>();
    private readonly ILogger<UserValidatorService> _logger = Substitute.For<ILogger<UserValidatorService>>();

    public AccommodationServiceTests()
    {
        _userValidator = new UserValidatorService(_userReceiver, _logger);
        _sut = new AccommodationService(_accommodationRepository, _mapper, _userValidator);
    }

    [Fact]
    public async Task CreateAccommodation_ShouldCreateAccommodation_WhenAllParametersAreValid()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity { Id = userId, IsLandlord = true });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.IsSuccess.Should().Be(true);
        actual.Should().Be(actual.Match(result => result, exception => throw exception) == true);
        await _accommodationRepository.Received(1).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    
    [Fact]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_UserDoesNotExist()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };

        _userReceiver.GetByIdAsync(userId).ReturnsNull();
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.IsFaulted.Should().Be(true);
        actual.Invoking((x) =>
        {
            x.IfFail((exception) =>
            {
                throw exception;
            });
        }).Should().Throws<UserNotFoundException>();
        await _accommodationRepository.Received(0).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    
    [Fact]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_UserIsNotLandlord()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };

        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity {Id = userId, IsLandlord = false });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.IsFaulted.Should().Be(true);
        actual.Invoking((x) => x.IfFail((exception) => throw exception))
            .Should()
            .Throw<UserValidationException>()
            .WithMessage("Regular user can not create accommodation's offers");
        await _accommodationRepository.Received(0).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    
    [Theory]
    [InlineData("0f8fad5b-d9cb-469f-a165-70867728950e", "00000000-0000-0000-0000-000000000000", "Invalid test", "User does not exist", 980)]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_WhenParametersAreInvalid(
        string accommodationIdentifier, string userIdentifier, string title, string description, int price)
    {
        // Arrange
        Guid accommodationId = Guid.Parse(accommodationIdentifier), userId = Guid.Parse(userIdentifier);
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userValidator.ValidateAsync(userId).Returns(new Result<bool>(true));
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.IsFaulted.Should().Be(true);
    }
}