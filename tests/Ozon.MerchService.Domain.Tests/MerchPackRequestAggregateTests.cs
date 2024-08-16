using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Tests;

public class MerchPackRequestAggregateTests
{
    private Employee _employee;
    private MerchPack _merchPack;
    private DateTimeOffset _requestedAt;
    private ClothingSize _clothingSize;
    private Email _hrEmail;
    private RequestType _requestType;
    private RequestStatus _requestStatus;

    public MerchPackRequestAggregateTests()
    {
        _employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        _merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });
        _requestedAt = new DateTimeOffset(2024, 08, 15, 10, 45, 00,
            new TimeSpan(0, 0, 0, 0));
        _clothingSize = ClothingSize.L;
        _hrEmail = new Email("hr@email.com");
        _requestType = RequestType.Manual;
        _requestStatus = RequestStatus.Created;
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_AllDataValid_Success()
    {
        //Arrange - Act
        var instance = new MerchPackRequest(
            _employee, 
            _merchPack, 
            _clothingSize, 
            _hrEmail, 
            _requestType, 
            _requestedAt, 
            _requestStatus);

        //Assert
        Assert.NotNull(instance);
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_EmployeeIsNull_ThrowException()
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            null, 
            _merchPack, 
            _clothingSize, 
            _hrEmail, 
            _requestType, 
            _requestedAt, 
            _requestStatus));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_MerchPackIsNull_ThrowException()
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            _employee, 
            null, 
            _clothingSize, 
            _hrEmail, 
            _requestType, 
            _requestedAt, 
            _requestStatus));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_RequestTypeIsNull_ThrowException()
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            _employee, 
            _merchPack, 
            _clothingSize, 
            _hrEmail, 
            null, 
            _requestedAt, 
            _requestStatus));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_HrEmailIsNull_ThrowException()
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            _employee, 
            _merchPack, 
            _clothingSize, 
            null, 
            _requestType, 
            _requestedAt, 
            _requestStatus));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_RequestStatusIsNull_ThrowException()
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            _employee, 
            _merchPack, 
            _clothingSize, 
            _hrEmail, 
            _requestType, 
            _requestedAt, 
            null));
    }
}