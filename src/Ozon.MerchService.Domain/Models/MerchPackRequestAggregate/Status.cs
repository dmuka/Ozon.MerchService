namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class Status(int id, string name) : Enumeration(id, name)
{
    public static Status Created = new(1, nameof(Created));
    public static Status Declined = new(2, nameof(Declined));
    public static Status Processing = new(3, nameof(Processing));    
    public static Status Quequed = new(4, nameof(Quequed));   
    public static Status Reserved = new(5, nameof(Reserved));
    public static Status Notificated = new(6, nameof(Notificated));
    public static Status Issued = new(7, nameof(Issued));
}