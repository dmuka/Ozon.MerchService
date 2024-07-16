using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public interface IMerchPacksRepository : IRepository<MerchPack, long>
{
    Task<MerchPack> GetMerchPackByMerchType(MerchType merchType, CancellationToken cancellationToken);
}