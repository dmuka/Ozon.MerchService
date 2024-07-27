using System.ComponentModel.DataAnnotations.Schema;
using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.DataContracts.Attributes;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

[Table("merchpacks")]
public class MerchPack : Item, IAggregationRoot
{
    public MerchPack(MerchType merchPackType, IEnumerable<MerchItem> merchItems)
    {
        MerchPackType = merchPackType;

        _merchPackItems = merchItems.ToList();
    }

    [ColumnExclude]
    public MerchType MerchPackType { get; }

    private List<MerchItem> _merchPackItems;
    public IEnumerable<MerchItem> Items => _merchPackItems.AsReadOnly();

    public static MerchPack CreateInstance(long id, MerchPack merchPack)
    {
        merchPack.Id = id;

        return merchPack;
    }
}