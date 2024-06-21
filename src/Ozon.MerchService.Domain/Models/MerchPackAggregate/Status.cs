namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class Status(int id, string name) : Enumeration(id, name)
{
    public static Status Created = new(1, nameof(Created));
    public static Status Quequed = new(2, nameof(Quequed)); 
    public static Status Unquequed = new(3, nameof(Unquequed));    
    public static Status Reserved = new(4, nameof(Reserved));
    public static Status Notificated = new(5, nameof(Notificated));
    public static Status Issued = new(6, nameof(Issued));
}