using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

/// <summary>
/// Get merch pack query
/// </summary>
/// <param name="merchPackType">Merch pack type</param>
/// <param name="clothingSize">Clothing size</param>
public class GetMerchPackQuery(MerchType merchPackType, ClothingSize clothingSize) : IRequest<MerchPack>
{
    /// <summary>
    /// Merch pack type
    /// </summary>
    public MerchType MerchPackType { get; } = merchPackType;
    /// <summary>
    /// Clothing size
    /// </summary>
    public ClothingSize ClothingSize { get; } = clothingSize;
}