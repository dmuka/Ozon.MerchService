namespace Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

public class RequestType(int id, string name) : Enumeration(id, name)
{
    public static RequestType Auto = new(1, nameof(Auto));
    public static RequestType Manual = new(2, nameof(Manual));
}