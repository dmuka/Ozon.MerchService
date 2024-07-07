using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(IUnitOfWork unitOfWork) : Repository<MerchPack, long>(unitOfWork), IMerchPacksRepository
{
}