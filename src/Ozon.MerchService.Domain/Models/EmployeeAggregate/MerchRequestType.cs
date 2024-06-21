namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class MerchRequestType(int id, string name) : Enumeration(id, name)
{
    public static Status Hire = new(1, nameof(Hire));
    public static Status ConferenceListener = new(2, nameof(ConferenceListener)); 
    public static Status ConferenceSpeaker = new(3, nameof(ConferenceSpeaker));    
    public static Status ProbationPeriodEnd = new(4, nameof(ProbationPeriodEnd));
}