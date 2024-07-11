using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;
using Ozon.MerchService.Domain.Models.ValueObjects;
using RequestedAt = Ozon.MerchService.Domain.Models.ValueObjects.RequestedAt;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class MerchPack : Item, IAggregationRoot
{
    public MerchPack(MerchType merchPackType, ClothingSize clothingSize)
    {
        MerchPackType = merchPackType;
        _merchPackItems = MerchPackType switch
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

    private List<MerchItem> _merchPackItems;
    public IEnumerable<MerchItem> Items => _merchPackItems.AsReadOnly();

    public static MerchPack CreateInstance(long id, MerchPack merchPack)
    {
        merchPack.Id = id;

        return merchPack;
    }

    #region Predefined merch packs

    private static List<MerchItem> StarterPackItems { get; } =
    [
        new MerchItem(1, 1000000,"Cap"),
        new MerchItem(5, 1000005, "Bottle")
    ];

    private static List<MerchItem> WelcomePackItems { get; } =
    [
        new MerchItem(2, 1000002, "Backpack"),
        new MerchItem(6, 1000006, "Mug")
    ];

    private static List<MerchItem> ConferenceListenerPackItems { get; } =
    [
        new MerchItem (3, 1000003, "Pen"),
        new MerchItem (4, 1000004, "Notebook")
    ];

    private static List<MerchItem> ConferenceSpeakerPackItems { get; } =
    [
        new MerchItem (3, 1000003, "Pen"),
        new MerchItem (4, 1000004, "Notebook"),
        new MerchItem (7, 1000007, "Marker")
    ];

    private static List<MerchItem> VeteranPackItems { get; } =
    [
        new MerchItem (8, 1000008, "Laptop"),
        new MerchItem (9, 1000009, "Mouse"),
        new MerchItem (10, 1000010, "Keyboard")
    ];

    #endregion
}