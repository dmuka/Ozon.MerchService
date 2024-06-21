using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class Employee : Entity<long>, IAggregationRoot
{
    public List<MerchPack> MerchPacks { get; } = [];

    public bool CanReceiveMerchPack(MerchType merchType)
    {
        bool result;
        
        if (merchType is MerchType.VeteranPack
            or MerchType.ConferenceListenerPack
            or MerchType.ConferenceSpeakerPack)
        {
            var previousActiveMerchPacks = MerchPacks
                .Where(pack =>
                    pack.MerchPackType == merchType 
                    && Equals(pack.Status, Status.Issued) 
                    && (DateTimeOffset.UtcNow - pack.Issued.Value).Value.Days <= 365);

            result = !previousActiveMerchPacks.Any();
        }
        else
        {
            result = MerchPacks.All(pack => pack.MerchPackType != merchType);
        }
        
        return result;
    } 
}