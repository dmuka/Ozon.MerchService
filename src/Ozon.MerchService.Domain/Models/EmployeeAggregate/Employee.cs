using System.ComponentModel.DataAnnotations.Schema;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Domain.Models.MerchPackRequestAggregate;

namespace Ozon.MerchService.Domain.Models.EmployeeAggregate;

[Table("employees")]
public class Employee(
    FullName fullName,
    Email email)
    : Entity, IAggregationRoot
{
    [Column("full_name")]
    public FullName FullName { get; private set; } = fullName;

    public Email Email { get; private set; } = email;

    private List<MerchPackRequest> _merchPackRequests = new List<MerchPackRequest>();
    [NotMapped]   
    public IEnumerable<MerchPackRequest> MerchPacksRequests  => _merchPackRequests.AsReadOnly();

    public bool CanReceiveMerchPack(MerchType merchType)
    {
        bool result;
        
        if (merchType is MerchType.VeteranPack
            or MerchType.ConferenceListenerPack
            or MerchType.ConferenceSpeakerPack)
        {
            var previousActiveMerchPacks = MerchPacksRequests
                .Where(request =>
                    request.MerchPack.MerchPackType == merchType 
                    && Equals(request.RequestStatus, RequestStatus.Issued) 
                    && (DateTimeOffset.UtcNow - request.Issued.Value).Value.Days <= 365);

            result = !previousActiveMerchPacks.Any();
        }
        else
        {
            result = MerchPacksRequests.All(request => request.MerchPack.MerchPackType != merchType);
        }
        
        return result;
    }

    public static Employee CreateInstance(
        long id, 
        string firstName,
        string lastName,
        string email)
    {
        var employee = new Employee(new FullName(firstName, lastName), new Email(email))
        {
            Id = id
        };

        return employee;
    }

    public void AddMerchPackRequest(MerchPackRequest merchPackRequest)
    {
        _merchPackRequests.Add(merchPackRequest);
    }

    public void SetMerchPackRequests(List<MerchPackRequest> merchPacksRequests)
    {
        _merchPackRequests = merchPacksRequests;
    }
}