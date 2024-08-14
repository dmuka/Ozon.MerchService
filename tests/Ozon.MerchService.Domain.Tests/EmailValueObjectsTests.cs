using Ozon.MerchService.Domain.Aggregates.EmployeeAggregate;
using Ozon.MerchService.Domain.Models.EmployeeAggregate;

namespace Ozon.MerchService.Domain.Tests;

public class EmailValueObjectsTests
{
    [Fact]
    public void CreateEmailInstance_ValidEmailValue_Success()
    {
        //Arrange
        const string email = "email@email.com";
        
        //Act
        var instance = new Email(email);

        //Assert
        Assert.NotNull(instance);
        Assert.Equal(email, instance.Value);
    }
    
    [Fact]
    public void CreateEmailInstance_InvalidEmailValue_Fail()
    {
        //Arrange
        const string email = "emailemail.com";

        //Act - Assert
        Assert.Throws<ArgumentException>(() => new Email(email));
    }
    
    [Fact]
    public void EmailInstancesEquality_ValidEqualEmailValues_Success()
    {
        //Arrange
        const string email1 = "email@email.com";
        const string email2 = "email@email.com";
        
        //Act
        var instance1 = new Email(email1);
        var instance2 = new Email(email2);

        //Assert
        Assert.True(instance1.Equals(instance2));
    }
    
    [Fact]
    public void EmailInstancesEquality_ValidNonEqualEmailValues_Fail()
    {
        //Arrange
        const string email1 = "email1@email.com";
        const string email2 = "email2@email.com";
        
        //Act
        var instance1 = new Email(email1);
        var instance2 = new Email(email2);

        //Assert
        Assert.False(instance1.Equals(instance2));
    }
}