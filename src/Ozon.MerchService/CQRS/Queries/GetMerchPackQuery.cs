using CSharpCourse.Core.Lib.Enums;
using MediatR;
using Ozon.MerchService.Domain.Models.MerchPackAggregate;

namespace Ozon.MerchService.CQRS.Queries;

public class GetMerchPackQuery(MerchType merchPackType, ClothingSize clothingSize) : IRequest<MerchPack>
{
    public MerchType MerchPackType { get; set; } = merchPackType;
    public ClothingSize ClothingSize { get; set; } = clothingSize;
}