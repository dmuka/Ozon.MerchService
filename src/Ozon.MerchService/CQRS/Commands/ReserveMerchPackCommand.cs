using CSharpCourse.Core.Lib.Enums;
using MediatR;

namespace Ozon.MerchService.CQRS.Commands;

/// <summary>
/// Reserve merch pack command
/// </summary>
public class ReserveMerchPackCommand : IRequest
{
    /// <summary>
    /// Employee id
    /// </summary>
    public long EmployeeId { get; set; }
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