using MediatR;
using Npgsql;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Infrastructure.Repositories.Exceptions;
using Ozon.MerchService.Infrastructure.Repositories.Interfaces;

namespace Ozon.MerchService.Infrastructure.Repositories.Implementations;

    public class UnitOfWork<T>(
        IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
        IPublisher publisher,
        ITracker<T> tracker)
        : IUnitOfWork, IDisposable
        where T : IEquatable<T>
    {
        private NpgsqlTransaction? _npgsqlTransaction;
        
        private readonly IDbConnectionFactory<NpgsqlConnection>? _dbConnectionFactory = dbConnectionFactory;

        public async ValueTask StartTransaction(CancellationToken token)
        {
            if (!(_npgsqlTransaction is null && _dbConnectionFactory is not null)) return;
            
            var connection = await _dbConnectionFactory.Create(token);
            
            _npgsqlTransaction = await connection.BeginTransactionAsync(token);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (_npgsqlTransaction is null)
            {
                throw new NoActiveTransactionsException();
            }

            var domainEvents = new Queue<INotification>(
                tracker.TrackedEntities
                    .SelectMany(entity =>
                    {
                        var events = entity.DomainEvents.ToList();
                        entity.ClearDomainEvents();
                        
                        return events;
                    }));
            
            while (domainEvents.TryDequeue(out var notification))
            {
                await publisher.Publish(notification, cancellationToken);
            }

            await _npgsqlTransaction.CommitAsync(cancellationToken);
        }

        void IDisposable.Dispose()
        {
            _npgsqlTransaction?.Dispose();
            _dbConnectionFactory?.Dispose();
            
            GC.SuppressFinalize(this);
        }
    }