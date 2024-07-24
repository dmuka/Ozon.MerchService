using CSharpCourse.Core.Lib.Enums;
using Ozon.MerchService.Domain.Models.MerchItemAggregate;

namespace Ozon.MerchService.Domain.Models.MerchPackAggregate;

public class MerchPack : Item, IAggregationRoot
{
    public MerchPack(MerchType merchPackType, IEnumerable<MerchItem> merchItems, ClothingSize clothingSize)
    {
        MerchPackType = merchPackType;

        _merchPackItems = merchItems.ToList();
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
        new MerchItem(1, 1000000, ItemType.TShirtAfterProbation, "TShirtAfterProbation"),
        new MerchItem(5, 1000005, ItemType.SweatshirtAfterProbation,"SweatshirtAfterProbation")
    ];

    private static List<MerchItem> WelcomePackItems { get; } =
    [
        new MerchItem(1, 1000000, ItemType.TShirtStarter, "TShirtStarter"),
        new MerchItem(5, 1000005, ItemType.NotepadStarter,"NotepadStarter"),
        new MerchItem(5, 1000005, ItemType.PenStarter,"PenStarter"),
        new MerchItem(5, 1000005, ItemType.SocksStarter,"SocksStarter")
    ];

    private static List<MerchItem> ConferenceListenerPackItems { get; } =
    [
        new MerchItem(1, 1000000, ItemType.NotepadConferenceListener, "NotepadConferenceListener"),
        new MerchItem(5, 1000005, ItemType.PenConferenceListener,"PenConferenceListener"),
        new MerchItem(5, 1000005, ItemType.TShirtСonferenceListener,"TShirtСonferenceListener")
    ];

    private static List<MerchItem> ConferenceSpeakerPackItems { get; } =
    [
        new MerchItem(1, 1000000, ItemType.NotepadConferenceSpeaker, "NotepadConferenceSpeaker"),
        new MerchItem(5, 1000005, ItemType.PenConferenceSpeaker,"PenConferenceSpeaker"),
        new MerchItem(5, 1000005, ItemType.SweatshirtConferenceSpeaker,"SweatshirtConferenceSpeaker")
    ];

    private static List<MerchItem> VeteranPackItems { get; } =
    [
        new MerchItem(1, 1000000, ItemType.TShirtVeteran, "TShirtVeteran"),
        new MerchItem(5, 1000005, ItemType.NotepadVeteran,"NotepadVeteran"),
        new MerchItem(5, 1000005, ItemType.CardHolderVeteran,"CardHolderVeteran"),
        new MerchItem(5, 1000005, ItemType.PenVeteran,"PenVeteran"),
        new MerchItem(5, 1000005, ItemType.SweatshirtVeteran,"SweatshirtVeteran")
    ];

    #endregion
}