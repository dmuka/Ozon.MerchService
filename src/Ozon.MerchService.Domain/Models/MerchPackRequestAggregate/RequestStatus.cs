namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class RequestStatus(int id, string name) : Enumeration(id, name)
{
    public static RequestStatus Created = new(1, nameof(Created));
    public static RequestStatus Declined = new(2, nameof(Declined));   
    public static RequestStatus Queued = new(3, nameof(Queued));
    public static RequestStatus Issued = new(4, nameof(Issued));
}