using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class Employee(
    FullName fullName,
    Email email,
    Email hrEmail,
    ClothingSize clothingSize)
    : Entity, IAggregationRoot
{
    public FullName FullName { get; private set; } = fullName;

    public Email Email { get; private set; } = email;

    public Email HrEmail { get; private set; } = hrEmail;

    public ClothingSize ClothingSize { get; private set; } = clothingSize;


    private List<MerchPackRequest> _merchPackRequests;
    public IEnumerable<MerchPackRequest> MerchPacksRequests  => _merchPackRequests.AsReadOnly();

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

    public static Employee CreateInstance(long id, Employee employee)
    {
        employee.Id = id;

        return employee;
    }
}