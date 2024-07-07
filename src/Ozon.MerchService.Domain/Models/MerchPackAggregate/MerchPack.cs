using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;
using Ozon.MerchService.Domain.Root;
using RequestedAt = Ozon.MerchService.Domain.Models.ValueObjects.RequestedAt;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class MerchPack : Item, IAggregationRoot
{
    public MerchPack(MerchType merchPackType)
    {
        MerchPackType = merchPackType;
        Items = MerchPackType switch
        {
            MerchType.WelcomePack => WelcomePackItems,
            MerchType.ProbationPeriodEndingPack => StarterPackItems,
            MerchType.ConferenceListenerPack => ConferenceListenerPackItems,
            MerchType.ConferenceSpeakerPack => ConferenceSpeakerPackItems,
            MerchType.VeteranPack => VeteranPackItems,
            _ => throw new ArgumentException("Wrong or unknown type of merch pack")
        };
    }

    public MerchType MerchPackType { get; }

    public List<MerchItem> Items { get; }

    #region Predefined merch packs

    private static List<MerchItem> StarterPackItems { get; } =
    [
        new MerchItem { Id = 1, Name = "Cap", StockKeepingUnit = 1000000 },
        new MerchItem { Id = 5, Name = "Bottle", StockKeepingUnit = 1000005 }
    ];

    private static List<MerchItem> WelcomePackItems { get; } =
    [
        new MerchItem { Id = 2, Name = "Backpack", StockKeepingUnit = 1000002 },
        new MerchItem { Id = 6, Name = "Mug", StockKeepingUnit = 1000006 }
    ];

    private static List<MerchItem> ConferenceListenerPackItems { get; } =
    [
        new MerchItem { Id = 3, Name = "Pen", StockKeepingUnit = 1000003 },
        new MerchItem { Id = 4, Name = "Notebook", StockKeepingUnit = 1000004 }
    ];

    private static List<MerchItem> ConferenceSpeakerPackItems { get; } =
    [
        new MerchItem { Id = 3, Name = "Pen", StockKeepingUnit = 1000003 },
        new MerchItem { Id = 4, Name = "Notebook", StockKeepingUnit = 1000004 },
        new MerchItem { Id = 7, Name = "Marker", StockKeepingUnit = 1000007 }
    ];

    private static List<MerchItem> VeteranPackItems { get; } =
    [
        new MerchItem { Id = 8, Name = "Laptop", StockKeepingUnit = 1000008 },
        new MerchItem { Id = 9, Name = "Mouse", StockKeepingUnit = 1000009 },
        new MerchItem { Id = 10, Name = "Keyboard", StockKeepingUnit = 1000010 }
    ];

    #endregion
}