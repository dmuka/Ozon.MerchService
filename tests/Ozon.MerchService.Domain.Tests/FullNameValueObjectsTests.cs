using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;

namespace Ozon.MerchService.Domain.Tests;

public class FullNameValueObjectsTests
{
    [Fact]
    public void CreateFullNameInstance_ValidFullNameValue_Success()
    {
        //Arrange
        const string fullName = "First Last";
        
        //Act
        var instance = new FullName(fullName);

        //Assert
        Assert.NotNull(instance);
        Assert.Equal(fullName, instance.Value);
    }
    
    [Fact]
    public void CreateFullNameInstance_InvalidFullNameValue_Fail()
    {
        //Arrange
        const string fullName = "FirstLast";

        //Act - Assert
        Assert.Throws<ArgumentException>(() => new FullName(fullName));
    }
    
    [Fact]
    public void FullNameInstancesEquality_ValidFullNameValues_Success()
    {
        //Arrange
        const string fullName1 = "First Last";
        const string fullName2 = "First Last";
        
        //Act
        var instance1 = new FullName(fullName1);
        var instance2 = new FullName(fullName2);

        //Assert
        Assert.True(instance1.Equals(instance2));
    }
    
    [Fact]
    public void FullNameInstancesEquality_ValidNonEqualFullNameValues_Fail()
    {
        //Arrange
        const string fullName1 = "First Last";
        const string fullName2 = "FirstFirst LastLast";
        
        //Act
        var instance1 = new FullName(fullName1);
        var instance2 = new FullName(fullName2);

        //Assert
        Assert.False(instance1.Equals(instance2));
    }
}