using Ozon.MerchService.Domain.Models;

namespace Ozon.MerchService.HttpModels;

public class ReserveMerchRequest
{
    public int EmployeeId { get; set; }
    public MerchPackType MerchPackType { get; set; }
}