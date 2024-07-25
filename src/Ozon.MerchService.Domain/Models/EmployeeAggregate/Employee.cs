using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

public class Employee(
    FullName fullName,
    Email email)
    : Entity, IAggregationRoot
{
    public FullName FullName { get; private set; } = fullName;

    public Email Email { get; private set; } = email;

    private List<MerchPackRequest> _merchPackRequests = new List<MerchPackRequest>();
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
                    && Equals(pack.RequestStatus, RequestStatus.Issued) 
                    && (DateTimeOffset.UtcNow - pack.Issued.Value).Value.Days <= 365);

            result = !previousActiveMerchPacks.Any();
        }
        else
        {
            result = MerchPacksRequests.All(pack => pack.MerchPackType != merchType);
        }
        
        return result;
    }

    public static Employee CreateInstance(
        long id, 
        string fullName, 
        string email)
    {
        var employee = new Employee(new FullName(fullName), new Email(email));

        employee.Id = id;

        return employee;
    }

    public void AddMerchPackRequest(MerchPackRequest merchPackRequest)
    {
        _merchPackRequests.Add(merchPackRequest);
    }
}