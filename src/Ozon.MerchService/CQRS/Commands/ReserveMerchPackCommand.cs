using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Reserve merch pack command
/// </summary>
public class ReserveMerchPackCommand : IRequest<Status>
{
    public ReserveMerchPackCommand(
        EmployeeEventType? eventType,
        long employeeId,
        string employeeFirstName,
        string employeeLastName,
        string employeeEmail,
        string hrEmail,
        ClothingSize employeeClothingSize,
        MerchType merchPackType)
    {
        EmployeeId = employeeId;
        EventType = eventType;
        EmployeeFirstName = employeeFirstName;
        EmployeeLastName = employeeLastName;
        EmployeeEmail = employeeEmail;
        HrEmail = hrEmail;
        EmployeeClothingSize = employeeClothingSize;
        MerchPackType = merchPackType;
    }
    
    /// <summary>
    /// Employee id
    /// </summary>
    public long EmployeeId { get; set; }

    public EmployeeEventType? EventType { get; private set; }

    /// <summary>
    /// Employee email
    /// </summary>
    public string EmployeeFirstName { get; set;  }
    /// <summary>
    /// Employee email
    /// </summary>
    public string EmployeeLastName { get; set;  }
    /// <summary>
    /// Employee email
    /// </summary>
    public string EmployeeEmail { get; set;  }
    /// <summary>
    /// Employee clothing size
    /// </summary>
    public ClothingSize EmployeeClothingSize { get; set;  }
    /// <summary>
    /// Hr email
    /// </summary>
    public string HrEmail { get; set;  }
    /// <summary>
    /// Merch pack type id
    /// </summary>
    public MerchType MerchPackType { get; set; }
}