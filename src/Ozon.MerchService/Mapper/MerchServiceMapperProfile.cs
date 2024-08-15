using System.Text.Json;
using AutoMapper;
using Ozon.MerchService.Domain.Aggregates.MerchPackRequestAggregate;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Infrastructure.Repositories.DTOs;

namespace Ozon.MerchService.Mapper;

public class MerchServiceMapperProfile : Profile
{
    public MerchServiceMapperProfile()
    {
        UpdateMerchPackRequest();
    }

    private void UpdateMerchPackRequest()
    {
        CreateMap<MerchPackRequest, MerchPackRequestDto>(MemberList.Destination)
            .ForMember(destination => destination.Id, options => options.MapFrom(source => source.Id))
            .ForMember(destination => destination.EmployeeId, options => options.MapFrom(source => source.Employee.Id))
            .ForMember(destination => destination.MerchpackTypeId, options => options.MapFrom(source => (int)source.MerchPack.MerchPackType))
            .ForMember(destination => destination.MerchPackItems, options => options.MapFrom(source => GetMerchPackItemsJson(source.MerchPack.Items)))
            .ForMember(destination => destination.HrEmail, options => options.MapFrom(source => source.HrEmail.Value))
            .ForMember(destination => destination.ClothingSizeId, options => options.MapFrom(source => (int)source.ClothingSize))
            .ForMember(destination => destination.RequestedAt, options => options.MapFrom(source => source.RequestedAt.Value))
            .ForMember(destination => destination.Issued, options => options.MapFrom(source => source.Issued.Value))
            .ForMember(destination => destination.RequestStatusId, options => options.MapFrom(source => source.RequestStatus.Id))
            .ForMember(destination => destination.RequestTypeId, options => options.MapFrom(source => source.RequestType.Id));
    }

    private string GetMerchPackItemsJson(IList<MerchItem> merchItems)
    {
        var json = JsonSerializer.Serialize(merchItems.Select(item => new
        {
            item.Type.ItemTypeId,
            item.Type.ItemTypeName,
            Sku = item.Sku.Value,
            Quantity = item.Quantity.Value
        }));

        return json;
    }
}