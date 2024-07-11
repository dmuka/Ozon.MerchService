namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class Status(int id, string name) : Enumeration(id, name)
{
    public static Status Created = new(1, nameof(Created));
    public static Status Declined = new(2, nameof(Declined));   
    public static Status Quequed = new(3, nameof(Quequed));
    public static Status Issued = new(4, nameof(Issued));
}