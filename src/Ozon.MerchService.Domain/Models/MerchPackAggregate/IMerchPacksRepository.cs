using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public interface IMerchPacksRepository : IRepository<MerchPack>
{
    Task<MerchPack> CreateAsync(MerchPack merchPack, CancellationToken cancellationToken);
    Task<MerchPack> UpdateAsync(MerchPack merchPack, CancellationToken cancellationToken);
    Task<MerchPack> FindByTypeAsync(MerchType merchType, CancellationToken cancellationToken);
    Task<IReadOnlyList<MerchPack>> FindByTypesAsync(IReadOnlyList<MerchType> merchTypes, CancellationToken cancellationToken);
    Task<IReadOnlyList<MerchPack>> GetAllAsync(CancellationToken cancellationToken);
}