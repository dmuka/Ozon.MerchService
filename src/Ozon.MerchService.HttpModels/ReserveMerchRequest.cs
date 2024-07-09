using CSharpCourse.Core.Lib.Enums;

namespace Ozon.MerchService.HttpModels;

public class ReserveMerchRequest
{
    public int EmployeeId { get; set; }
    public MerchType MerchPackType { get; set; }
    
    public ClothingSize ClothingSize { get; set; }
}