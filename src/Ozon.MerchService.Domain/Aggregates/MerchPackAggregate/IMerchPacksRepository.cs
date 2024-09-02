using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public interface IMerchPacksRepository : IRepository
{
    Task<MerchPack> GetMerchPackById(int merchPackId, CancellationToken cancellationToken);
}