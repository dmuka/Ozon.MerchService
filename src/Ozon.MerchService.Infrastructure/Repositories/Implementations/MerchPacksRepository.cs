using Npgsql;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;
using Ozon.MerchService.Infrastructure.Repositories.Infrastructure.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

public class MerchPacksRepository(IDbConnectionFactory<NpgsqlConnection> connectionFactory) : Repository<MerchPack, long>(connectionFactory), IMerchPacksRepository
{
}