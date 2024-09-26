using FluentValidation.TestHelper;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.CQRS.Validators;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.Tests;

public class CreateMerchPackCommandValidatorTests
{
    private readonly CreateMerchPackCommandValidator _validator = new();
    private const string ValidMerchPackName = "Merch pack";
    private List<MerchItemDto> _merchItems = [new MerchItemDto { ItemTypeId = 1, ItemTypeName = "Item", Quantity = 1 }];

    [Fact]
    public void CreateMerchPackCommand_IfAllDataValid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateMerchPackCommand(ValidMerchPackName, _merchItems);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateMerchPackCommand_IfMerchPackNameInvalid_ShouldHaveValidationErrors(string merchPackName)
    {
        // Arrange
        var command = new CreateMerchPackCommand(merchPackName, _merchItems);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Arrange
        result.ShouldHaveValidationErrorFor(c => c.MerchPackName);
    }
    
    [Theory]
    [MemberData(nameof(MerchItems))]
    public void CreateMerchPackCommand_IfMerchItemsCollectionInvalid_ShouldHaveValidationErrors(List<MerchItemDto> merchItems)
    {
        // Arrange
        var command = new CreateMerchPackCommand(ValidMerchPackName, merchItems);
        
        // Act
        var result = _validator.TestValidate(command);
        
        // Arrange
        result.ShouldHaveValidationErrorFor(c => c.MerchItems);
    }
    
    public static IEnumerable<object?[]> MerchItems(){
        yield return new object[] { new List<MerchItemDto> {} };
        yield return new object[] { null };
    }
}