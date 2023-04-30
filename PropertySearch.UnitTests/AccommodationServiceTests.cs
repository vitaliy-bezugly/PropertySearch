using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PropertySearchApp.Common;
using PropertySearchApp.Common.Constants;
using PropertySearchApp.Domain;
using PropertySearchApp.Entities;
using PropertySearchApp.Repositories.Abstract;
using PropertySearchApp.Services;
using PropertySearchApp.Services.Abstract;
using PropertySearchApp.Validations;

namespace PropertySearch.UnitTests;

public class AccommodationServiceTests
{
    private readonly AccommodationService _sut;
    private readonly IUserValidatorService _userValidator;

    private readonly IAccommodationRepository _accommodationRepository = Substitute.For<IAccommodationRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUserReceiverRepository _userReceiver = Substitute.For<IUserReceiverRepository>();
    private readonly ILogger<UserValidatorService> _logger = Substitute.For<ILogger<UserValidatorService>>();
    private readonly IValidator<AccommodationDomain> _accommodationValidator = new AccommodationDomainValidator();
    private readonly IValidator<LocationDomain> _locationValidator = new LocationDomainValidator();

    public AccommodationServiceTests()
    {
        _userValidator = new UserValidatorService(_userReceiver, _logger);
        _sut = new AccommodationService(_accommodationRepository, 
            _mapper, 
            _userValidator,
            _accommodationValidator,
            _locationValidator);
    }

    [Fact]
    public async Task CreateAccommodation_ShouldCreateAccommodation_WhenAllParametersAreValid()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity { Id = userId, IsLandlord = true });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _accommodationRepository.Received(1).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    [Fact]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_LocationIsNull()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;

        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = null, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };

        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity { Id = userId, IsLandlord = true });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);

        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().Contain(ErrorMessages.Accommodation.Validation.NullLocation);
    }
    [Fact]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_UserDoesNotExist()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };

        _userReceiver.GetByIdAsync(userId).ReturnsNull();
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.NotFound);
        await _accommodationRepository.Received(0).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    [Fact]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_UserIsNotLandlord()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Test title", description = "Some description";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };

        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity {Id = userId, IsLandlord = false });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);
        
        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.NotLandlord);
        await _accommodationRepository.Received(0).CreateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    [Theory]
    [InlineData("0f8fad5b-d9cb-469f-a165-70867728950e", "7c9e6679-7425-40de-944b-e07fc1f90ae7", "Invalid test", "User exists", -980)]
    [InlineData("0f8fad5b-d9cb-469f-a165-70867728950e", "7c9e6679-7425-40de-944b-e07fc1f90ae7", "", "Invalid test", 980)]
    public async Task CreateAccommodation_ShouldNotCreateAccommodation_WhenParametersAreInvalid(
        string accommodationIdentifier, string userIdentifier, string title, string description, int price)
    {
        // Arrange
        Guid accommodationId = Guid.Parse(accommodationIdentifier), userId = Guid.Parse(userIdentifier);

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdAsync(userId).Returns(new UserEntity {Id = userId, IsLandlord = true });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.CreateAsync(accommodationEntity, CancellationToken.None).Returns(true);

        // Act
        var actual = await _sut.CreateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
    }
    
    [Fact]
    public async Task UpdateAccommodation_ShouldUpdateAccommodation_WhenAllParametersAreValid()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Updating test", updatedDescription = "UpdatedDescription";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = true, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = accommodationId, UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.UpdateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _accommodationRepository.Received(1).UpdateAsync(Arg.Any<AccommodationEntity>(), CancellationToken.None);
    }
    [Fact]
    public async Task UpdateAccommodation_ShouldNotUpdateAccommodation_UserDoesNotOwnAccommodation()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Updating test", updatedDescription = "UpdatedDescription";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = true, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = Guid.NewGuid(), UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.UpdateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.HasNoAccess);
    }
    [Fact]
    public async Task UpdateAccommodation_ShouldNotUpdateAccommodation_UserIsNotLandlord()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Updating test", updatedDescription = "UpdatedDescription";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = false, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = Guid.NewGuid(), UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.UpdateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.NotLandlord);
    }
    [Theory]
    [InlineData("0f8fad5b-d9cb-469f-a165-70867728950e", "7c9e6679-7425-40de-944b-e07fc1f90ae7", "Invalid test", "User exists", -980)]
    [InlineData("0f8fad5b-d9cb-469f-a165-70867728950e", "7c9e6679-7425-40de-944b-e07fc1f90ae7", "", "Invalid test", 980)]
    public async Task UpdateAccommodation_ShouldNotCreateAccommodation_WhenParametersAreInvalid(
        string accommodationIdentifier, string userIdentifier, string title, string description, int price)
    {
        // Arrange
        Guid accommodationId = Guid.Parse(accommodationIdentifier), userId = Guid.Parse(userIdentifier);

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = true, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = accommodationId, UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());

        // Act
        var actual = await _sut.UpdateAccommodationAsync(accommodation, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
    }
    
    [Fact]
    public async Task DeleteAccommodation_ShouldDeleteAccommodation_WhenAllParametersAreValid()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Deleting test";
        const int price = 980;

        var location = new LocationDomain { Id = Guid.NewGuid(), Country = "UA", Region = "Kyiv City", City = "Kyiv", Address = "Yavornitskogo 28" };
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price, Location = location, PhotoUri = null };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = true, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = accommodationId, UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.DeleteAsync(accommodationId, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.DeleteAccommodationAsync(userId, accommodationId, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(true);
        await _accommodationRepository.Received(1).DeleteAsync(Arg.Any<Guid>(), CancellationToken.None);
    }
    [Fact]
    public async Task DeleteAccommodation_ShouldNotDeleteAccommodation_UserDoesNotHaveAccommodation()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Updating test";
        const int price = 980;
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = true, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = Guid.NewGuid(), UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.DeleteAccommodationAsync(userId, accommodationId, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.HasNoAccess);
    }
    [Fact]
    public async Task DeleteAccommodation_ShouldNotDeleteAccommodation_UserIsNotLandlord()
    {
        // Arrange
        Guid accommodationId = Guid.NewGuid(), userId = Guid.NewGuid();
        const string title = "Unit test", description = "Updating test";
        const int price = 980;
        
        var accommodation = new AccommodationDomain { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        var accommodationEntity = new AccommodationEntity { Id = accommodationId, Title = title, Description = description, UserId = userId, Price = price };
        
        _userReceiver.GetByIdWithAccommodationsAsync(userId).Returns(new UserEntity 
        { 
            Id = userId, 
            IsLandlord = false, 
            Accommodations = new List<AccommodationEntity>
            {
                new AccommodationEntity { Id = accommodationId, UserId = userId }
            }
        });
        _mapper.Map<AccommodationEntity>(accommodation).Returns(accommodationEntity);
        _accommodationRepository.UpdateAsync(accommodationEntity, CancellationToken.None).Returns(new OperationResult());
        
        // Act
        var actual = await _sut.DeleteAccommodationAsync(userId, accommodationId, CancellationToken.None);

        // Assert
        actual.Succeeded.Should().Be(false);
        actual.ErrorMessage.Should().BeEquivalentTo(ErrorMessages.User.NotLandlord);
    }
}