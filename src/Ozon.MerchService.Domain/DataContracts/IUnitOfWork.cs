namespace Ozon.MerchService.Domain.DataContracts;

public interface IUnitOfWork
{
    ValueTask StartTransaction(CancellationToken token);
        
    Task SaveChangesAsync(CancellationToken cancellationToken);
}