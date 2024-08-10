using CSharpCourse.Core.Lib.Enums;

namespace Ozon.MerchService.HttpModels;

public class ReserveMerchRequest
{
    public int EmployeeId { get; set; }
 
    public string EmployeeFirstName { get; set;  }
 
    public string EmployeeLastName { get; set;  }
 
    public string EmployeeEmail { get; set;  }
 
    public string HrEmail { get; set;  }
 
    public string HrName { get; set;  }
 
    public MerchType MerchPackType { get; set; }
    public ClothingSize ClothingSize { get; set; }
}