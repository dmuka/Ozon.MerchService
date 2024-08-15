using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Domain.Tests;

public class MerchPackRequestAggregateTests
{
    [Fact]
    public void CreateMerchPackRequestInstance_AllDataValid_Success()
    {
        //Arrange
        var employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new MerchItem[] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });
        
        
        //Act
        var instance = new MerchPackRequest(
            employee, 
            merchPack, 
            ClothingSize.L, 
            new Email("hr@email.com"), 
            RequestType.Auto, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            RequestStatus.Created);

        //Assert
        Assert.NotNull(instance);
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_EmployeeIsNull_ThrowException()
    {
        //Arrange
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            null, 
            merchPack, 
            ClothingSize.L, 
            new Email("hr@email.com"), 
            RequestType.Auto, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            RequestStatus.Created));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_MerchPackIsNull_ThrowException()
    {
        //Arrange
        var employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            employee, 
            null, 
            ClothingSize.L, 
            new Email("hr@email.com"), 
            RequestType.Auto, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            RequestStatus.Created));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_RequestTypeIsNull_ThrowException()
    {
        //Arrange
        var employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            employee, 
            merchPack, 
            ClothingSize.L, 
            new Email("hr@email.com"), 
            null, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            RequestStatus.Created));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_HrEmailIsNull_ThrowException()
    {
        //Arrange
        var employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            employee, 
            merchPack, 
            ClothingSize.L, 
            null, 
            RequestType.Auto, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            RequestStatus.Created));
    }
    
    [Fact]
    public void CreateMerchPackRequestInstance_RequestStatusIsNull_ThrowException()
    {
        //Arrange
        var employee = new Employee(new FullName("First", "Last"), new Email("employee@email.com"));
        var merchPack = new MerchPack(MerchType.WelcomePack,
            new [] { new MerchItem(1, new ItemType(1, "ItemTypeName"), 1) });

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new MerchPackRequest(
            employee, 
            merchPack, 
            ClothingSize.L, 
            new Email("hr@email.com"), 
            RequestType.Auto, 
            new DateTimeOffset(2024,08,15,10,45,00, new TimeSpan(0,0,0,0)), 
            null));
    }
}