using System.Text.Json;
using MediatR;
using Ozon.MerchService.CQRS.Commands;
using Ozon.MerchService.Domain.DataContracts;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.CQRS.Handlers;

/// <summary>
/// Handler for create merch pack request
/// </summary>
/// <param name="merchPacksRepository">Merch packs repository instance</param>
public class CreateMerchPackCommandHandler(IRepository repository) : IRequestHandler<CreateMerchPackCommand, int>
{
    /// <summary>
    /// Create merch pack
    /// </summary>
    /// <param name="command">Command with merch pack data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Id of created merch pack</returns>
    public async Task<int> Handle(CreateMerchPackCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.CreateAsync<MerchPackDto, int>(
            new
            {
                Name = command.MerchPackName, 
                Items = JsonSerializer.Serialize(
                    command.MerchItems
                        .Select(item => new
                        {
                            item.ItemTypeId,
                            item.ItemTypeName,
                            item.Quantity
                        }))
            },
            cancellationToken);

        return result;
    }
}