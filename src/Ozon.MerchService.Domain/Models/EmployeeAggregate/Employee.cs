using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Root;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class Employee : Entity<long>, IAggregationRoot
{
    public Employee(
        int id,
        FullName fullName,
        Email email,
        Email hrEmail,
        ClothingSize clothingSize
    )
    {
        Id = id;
        FullName = fullName;
        Email = email;
        HREmail = hrEmail;
        ClothingSize = clothingSize;
    }
    
    public FullName FullName { get; }
    
    public Email Email { get; }
    
    public Email HREmail { get; }
    
    public ClothingSize ClothingSize { get; }
    
    public List<MerchPackRequest> MerchPacksRequests { get; } = [];

    public bool CanReceiveMerchPack(MerchType merchType)
    {
        bool result;
        
        if (merchType is MerchType.VeteranPack
            or MerchType.ConferenceListenerPack
            or MerchType.ConferenceSpeakerPack)
        {
            var previousActiveMerchPacks = MerchPacksRequests
                .Where(pack =>
                    pack.MerchPackType == merchType 
                    && Equals(pack.Status, Status.Issued) 
                    && (DateTimeOffset.UtcNow - pack.Issued.Value).Value.Days <= 365);

            result = !previousActiveMerchPacks.Any();
        }
        else
        {
            result = MerchPacksRequests.All(pack => pack.MerchPackType != merchType);
        }
        
        return result;
    } 
}