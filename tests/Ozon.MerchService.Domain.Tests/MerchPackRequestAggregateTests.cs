using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Tests;

public class MerchPackRequestAggregateTests
{
    private const long Id = 1;
    private const long MerchPackRequestId = 1;
    private const long EmployeeId = 1;
    private const string EmployeeFullName = "First Last";
    private const string EmployeeEmail = "employee@email.com";
    private readonly MerchType _merchType = MerchType.WelcomePack;
    private readonly MerchItem[] _merchPackItems = [new MerchItem(1, new ItemType(1, "First"), 1)];
    private readonly Employee _employee = new(new FullName("First", "Last"), new Email("employee@email.com"));
    private readonly MerchPack _merchPack = new(MerchType.WelcomePack,
        new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });
    private readonly DateTimeOffset _requestedAt = new(2024, 08, 15, 10, 45, 00,
        new TimeSpan(0, 0, 0, 0));
    private readonly DateTimeOffset _issued = new(2024, 08, 16, 9, 19, 00,
        new TimeSpan(0, 0, 0, 0));
    private readonly ClothingSize _clothingSize = ClothingSize.L;
    private const string HrEmail = "hr@email.com";
    private readonly Email _hrEmail = new(HrEmail);
    private const int RequestTypeId = 1;
    private const string RequestTypeName = "Manual";
    private readonly RequestType _requestType = RequestType.Manual;
    private const int RequestStatusId = 1;
    private const string RequestStatusName = "Created";
    private readonly RequestStatus _requestStatus = RequestStatus.Created;

    [Fact]
    public void CreateMerchPackRequest_AllDataValid_Success()
    {
        //Arrange - Act
        var request = new MerchPackRequest(
            _employee, 
            _merchPack, 
            _clothingSize, 
            _hrEmail, 
            _requestType, 
            _requestedAt, 
            _requestStatus);

        //Assert
        Assert.NotNull(request);
    }
    
    [Fact]
    public void CreateMerchPackRequest_EmployeeIsNull_ThrowException()
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
    public void CreateMerchPackRequest_MerchPackIsNull_ThrowException()
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
    public void CreateMerchPackRequest_RequestTypeIsNull_ThrowException()
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
    public void CreateMerchPackRequest_HrEmailIsNull_ThrowException()
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
    public void CreateMerchPackRequest_RequestStatusIsNull_ThrowException()
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
    
    [Fact]
    public void CreateMerchPackRequestInstance_AllDataIsValid_Success()
    {        
        //Arrange - Act
        var request = MerchPackRequest.CreateInstance(
            Id,
            _employee, 
            _merchPack,  
            _hrEmail, 
            _clothingSize,
            _requestType);

        //Assert
        Assert.NotNull(request);
        Assert.Equal(Id, request.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateMerchPackRequestInstance_IfIdIsEqualZeroOrNegative_ThrowsException(long id)
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => MerchPackRequest.CreateInstance(
            id,
            _employee, 
            _merchPack,  
            _hrEmail, 
            _clothingSize,
            _requestType));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance2_AllDataIsValid_Success()
    {        
        //Arrange - Act
        var request = MerchPackRequest.CreateInstance(
            MerchPackRequestId,
            _merchType, 
            _merchPackItems,
            EmployeeId,
            EmployeeFullName,
            EmployeeEmail, 
            _clothingSize,
            HrEmail,
            RequestTypeId,
            RequestTypeName,
            _requestedAt,
            _issued,
            RequestStatusId,
            RequestStatusName);

        //Assert
        Assert.NotNull(request);
        Assert.Equal(Id, request.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateMerchPackRequestInstance2_IfMerchPackRequestIdIsEqualZeroOrNegative_ThrowsException(int merchPackRequestId)
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => MerchPackRequest.CreateInstance(
            merchPackRequestId,
            _merchType, 
            _merchPackItems,
            EmployeeId,
            EmployeeFullName,
            EmployeeEmail, 
            _clothingSize,
            HrEmail,
            RequestTypeId,
            RequestTypeName,
            _requestedAt,
            _issued,
            RequestStatusId,
            RequestStatusName));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CreateMerchPackRequestInstance2_IfEmployeeIdIsEqualZeroOrNegative_ThrowsException(int employeeId)
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => MerchPackRequest.CreateInstance(
            MerchPackRequestId,
            _merchType, 
            _merchPackItems,
            employeeId,
            EmployeeFullName,
            EmployeeEmail, 
            _clothingSize,
            HrEmail,
            RequestTypeId,
            RequestTypeName,
            _requestedAt,
            _issued,
            RequestStatusId,
            RequestStatusName));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public void CreateMerchPackRequestInstance2_IfRequestTypeIdIsInvalid_ThrowsException(int requestTypeId)
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => MerchPackRequest.CreateInstance(
            MerchPackRequestId,
            _merchType, 
            _merchPackItems,
            EmployeeId,
            EmployeeFullName,
            EmployeeEmail, 
            _clothingSize,
            HrEmail,
            requestTypeId,
            RequestTypeName,
            _requestedAt,
            _issued,
            RequestStatusId,
            RequestStatusName));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(5)]
    public void CreateMerchPackRequestInstance2_IfRequestStatusIdIsInvalid_ThrowsException(int requestStatusId)
    {
        //Arrange - Act - Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => MerchPackRequest.CreateInstance(
            MerchPackRequestId,
            _merchType, 
            _merchPackItems,
            EmployeeId,
            EmployeeFullName,
            EmployeeEmail, 
            _clothingSize,
            HrEmail,
            RequestTypeId,
            RequestTypeName,
            _requestedAt,
            _issued,
            requestStatusId,
            RequestStatusName));
    }
}