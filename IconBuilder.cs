using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ExileCore.Shared;
using GameOffsets.Native;
using SharpDX;
using System;
using Color = System.Drawing.Color;
using static System.Net.Mime.MediaTypeNames;
using ExileCore.Shared.Cache;
using System.Diagnostics;

namespace MapIcons;

public class HudTexture
{
    public HudTexture() {
    }

    public HudTexture(string fileName) {
        FileName = fileName;
    }

    public string FileName { get; set; }
    public RectangleF UV { get; set; } = new RectangleF(0, 0, 1, 1);
    public float Size { get; set; } = 13;
    public System.Drawing.Color Color { get; set; } = Color.White;
}


public sealed class IconBuilder
{
    private readonly MapIcons _plugin;
    public IconBuilder(MapIcons plugin) { _plugin = plugin; }
    private MapIconsSettings Settings => _plugin.Settings;

    private static EntityType[] SkippedEntityTypes =>
    [
        EntityType.HideoutDecoration,
        EntityType.Effect,
        EntityType.Light,
        EntityType.ServerObject,
        EntityType.Daemon,
        EntityType.Error,
    ];
    private static string[] SkippedEntityPaths =>
    [
        "Metadata/NPC/Hideout",
    ];
    private List<string> UserSkippedEntityPaths { get; set; }

    private int RunCounter { get; set; }
    private int IconVersion;

    public void RebuildIcons() => IconVersion++;
    public void Initialise() => UpdateUserSkippedEntities();
    public void Tick() {
        RunCounter++;
        if (RunCounter % Settings.RunEveryXTicks != 0) return;

        foreach (var entity in _plugin.GameController.Entities) {
            if (entity.GetHudComponent<MapIcon>() is { Version: var version, } && version >= IconVersion) continue;
            MapIcon icon = CreateIcon(entity);
            if (icon == null) continue;
            icon.UpdateSettings();
            icon.Version = IconVersion;
            entity.SetHudComponent(icon);
        }
    }

    private MapIcon CreateMiscIcon(Entity entity) {
        var icon = new MapIcon(entity);
        icon.IconCategory = IconCategories.Misc;
        icon.Text = icon.RenderName;

        // Player
        if (entity.Type == EntityType.Player) {
            if (!entity.IsValid) return null;
            icon.Text = entity.GetComponent<Player>().PlayerName;
            // Local Player
            if (_plugin.GameController.IngameState.Data.LocalPlayer.Address == entity.Address) {
                //if (plugin.GameController.IngameState.Data.LocalPlayer.GetComponent<Player>().PlayerName == entity.GetComponent<Player>().PlayerName) return; 
                icon.IconRenderType = IconRenderTypes.Player;
                icon.IconType = IconTypes.LocalPlayer;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.LocalPlayer_Draw;
                    icon.DrawText = Settings.LocalPlayer_DrawText;
                    icon.Size = Settings.LocalPlayer_Size;
                    icon.Index = Settings.LocalPlayer_Index;
                    icon.Tint = Settings.LocalPlayer_Tint;
                };
            }
            // Player
            else {
                icon.IconRenderType = IconRenderTypes.Player;
                icon.IconType = IconTypes.Player;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.Player_Draw;
                    icon.DrawText = Settings.Player_DrawText;
                    icon.Size = Settings.Player_Size;
                    icon.Index = Settings.Player_Index;
                    icon.Tint = Settings.Player_Tint;
                };
            }
        }
        else if (entity.Path.StartsWith("Metadata/Terrain/Leagues/Sanctum/Objects/SanctumMote")) {
            icon.IconRenderType = IconRenderTypes.Chest;
            icon.IconType = IconTypes.SanctumMote;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.SanctumMote_Draw;
                icon.DrawText = Settings.SanctumMote_DrawText;
                icon.Size = Settings.SanctumMote_Size;
                icon.Index = Settings.SanctumMote_Index;
                icon.Tint = Settings.SanctumMote_Tint;
            };
        }
        else return null;

        DebugMiscIcon(icon);
        return icon;
    }
    private MapIcon CreateChestIcon(Entity entity) {
        var icon = new MapIcon(entity);
        icon.IconRenderType = IconRenderTypes.Chest;
        icon.IconCategory = IconCategories.Chest;
        icon.Text = icon.RenderName;
        icon.Show = () => !entity.IsOpened;

        switch (entity.Path) {
            // Breach Chests
            case string path when path.Contains("BreachChest"):
                if (entity.Path.Contains("Large")) {
                    icon.IconType = IconTypes.BreachChestLarge;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.BreachChestBoss_Draw;
                        icon.DrawText = Settings.BreachChestBoss_DrawText;
                        icon.Size = Settings.BreachChestBoss_Size;
                        icon.Index = Settings.BreachChestBoss_Index;
                        icon.Tint = Settings.BreachChestBoss_Tint;
                    };
                }
                else {
                    icon.IconType = IconTypes.BreachChest;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.BreachChest_Draw;
                        icon.DrawText = Settings.BreachChest_DrawText;
                        icon.Size = Settings.BreachChest_Size;
                        icon.Index = Settings.BreachChest_Index;
                        icon.Tint = Settings.BreachChest_Tint;
                    };                }
                break;
            // Expedition Chests
            case string path when path.StartsWith("Metadata/Chests/LeaguesExpedition/", StringComparison.Ordinal):
                if (entity.Rarity == MonsterRarity.White) {
                    icon.IconType = IconTypes.ExpeditionChestWhite;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.ExpeditionNormalChest_Draw;
                        icon.DrawText = Settings.ExpeditionNormalChest_DrawText;
                        icon.Size = Settings.ExpeditionNormalChest_Size;
                        icon.Index = Settings.ExpeditionNormalChest_Index;
                        icon.Tint = Settings.ExpeditionNormalChest_Tint;
                    };
                }
                else if (entity.Rarity == MonsterRarity.Magic) {
                    icon.IconType = IconTypes.ExpeditionChestMagic;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.ExpeditionMagicChest_Draw;
                        icon.DrawText = Settings.ExpeditionMagicChest_DrawText;
                        icon.Size = Settings.ExpeditionMagicChest_Size;
                        icon.Index = Settings.ExpeditionMagicChest_Index;
                        icon.Tint = Settings.ExpeditionMagicChest_Tint;
                    };
                }
                else if (entity.Rarity == MonsterRarity.Rare) {
                    icon.IconType = IconTypes.ExpeditionChestRare;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.ExpeditionRareChest_Draw;
                        icon.DrawText = Settings.ExpeditionRareChest_DrawText;
                        icon.Size = Settings.ExpeditionRareChest_Size;
                        icon.Index = Settings.ExpeditionRareChest_Index;
                        icon.Tint = Settings.ExpeditionRareChest_Tint;
                    };
                }
                else return null;
                break;
            // Delve Chests
            case string path when path.StartsWith("Metadata/Chests/DelveChests/", StringComparison.OrdinalIgnoreCase):
                if (path.Contains("Azurite", StringComparison.OrdinalIgnoreCase)) {
                    icon.IconType = IconTypes.DelveChestAzurite;
                    icon.UpdateSettingsAction = () => {
                        bool shouldDraw = false;
                        bool isChest = path.Contains("Chest", StringComparison.OrdinalIgnoreCase) || path.Contains("Encounter", StringComparison.OrdinalIgnoreCase);

                        if (path.EndsWith("1")) shouldDraw = isChest ? Settings.DelveAzurite_Chest1_Draw : Settings.DelveAzurite_Vein1_Draw;
                        else if (path.EndsWith("2")) shouldDraw = isChest ? Settings.DelveAzurite_Chest2_Draw : Settings.DelveAzurite_Vein2_Draw;
                        else if (path.EndsWith("3")) shouldDraw = isChest ? Settings.DelveAzurite_Chest3_Draw : Settings.DelveAzurite_Vein3_Draw;
                        else if (path.EndsWith("4")) shouldDraw = isChest ? Settings.DelveAzurite_Chest4_Draw : Settings.DelveAzurite_Vein4_Draw;
                        else shouldDraw = isChest ? Settings.DelveAzurite_Chest1_Draw : Settings.DelveAzurite_Vein1_Draw;

                        icon.Draw = shouldDraw;
                        icon.DrawText = Settings.DelveAzurite_DrawText;
                        icon.Size = Settings.DelveAzurite_Size;
                        icon.Index = Settings.DelveAzurite_Index;
                        icon.Tint = Settings.DelveAzurite_Tint;
                    };
                }
                else if (path.Contains("Fossil", StringComparison.OrdinalIgnoreCase)) {
                    icon.IconType = IconTypes.DelveChestFossil;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.DelveChestFossil_Draw;
                        icon.DrawText = Settings.DelveChestFossil_DrawText;
                        icon.Size = Settings.DelveChestFossil_Size;
                        icon.Index = Settings.DelveChestFossil_Index;
                        icon.Tint = Settings.DelveChestFossil_Tint;
                    };
                }
                else if (path.Contains("Resonator", StringComparison.OrdinalIgnoreCase)) {
                    icon.IconType = IconTypes.DelveChestResonator;
                    icon.UpdateSettingsAction = () => {
                        bool shouldDraw = false;
                        if (path.EndsWith("1")) shouldDraw = Settings.DelveResonator_Stash_Draw;
                        else if (path.EndsWith("2")) shouldDraw = Settings.DelveResonator_Cache_Draw;
                        else if (path.EndsWith("3")) shouldDraw = Settings.DelveResonator_Trove_Draw;
                        else shouldDraw = Settings.DelveResonator_Stash_Draw;

                        icon.Draw = shouldDraw;
                        icon.DrawText = Settings.DelveResonator_DrawText;
                        icon.Size = Settings.DelveResonator_Size;
                        icon.Index = Settings.DelveResonator_Index;
                        icon.Tint = Settings.DelveResonator_Tint;
                    };
                }
                else if (path.Contains("MiningSupplies", StringComparison.OrdinalIgnoreCase) || path.Contains("Dynamite", StringComparison.OrdinalIgnoreCase)) {
                    icon.IconType = IconTypes.DelveChestGeneric;
                    icon.UpdateSettingsAction = () => {
                        bool shouldDraw = false;
                        if (path.Contains("Flares", StringComparison.OrdinalIgnoreCase)) {
                            if (path.EndsWith("2")) shouldDraw = Settings.DelveSupplies_LargeFlare_Draw;
                            else shouldDraw = Settings.DelveSupplies_Flare_Draw;
                        } 
                        else if (path.Contains("Dynamite", StringComparison.OrdinalIgnoreCase)) {
                            shouldDraw = Settings.DelveSupplies_Dynamite_Draw;
                        }
                        else {
                            shouldDraw = Settings.DelveSupplies_Flare_Draw;
                        }

                        icon.Draw = shouldDraw;
                        icon.DrawText = Settings.DelveSupplies_DrawText;
                        icon.Size = Settings.DelveSupplies_Size;
                        icon.Index = Settings.DelveSupplies_Index;
                        icon.Tint = Settings.DelveSupplies_Tint;
                    };
                }
                else {
                    icon.IconType = IconTypes.DelveChestGeneric;
                    icon.UpdateSettingsAction = () => {
                        string n = icon.RenderName ?? "";
                        
                        // Default fallback
                        bool shouldDraw = Settings.DelveChestGeneric_Draw;
                        var tint = Settings.DelveChestGeneric_Tint;
                        var size = Settings.DelveChestGeneric_Size;
                        var index = Settings.DelveChestGeneric_Index;
                        bool drawText = Settings.DelveChestGeneric_DrawText;

                        // Check legacy routes first dynamically just in case name matching fails
                        bool isDynamite = path.Contains("Dynamite", StringComparison.OrdinalIgnoreCase);
                        bool isOffPath = path.Contains("OffPath", StringComparison.OrdinalIgnoreCase);
                        if (n == "Forgotten Loot") { shouldDraw = Settings.DelvePath_Loot_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Forgotten Armour") { shouldDraw = Settings.DelvePath_Armour_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Forgotten Armaments") { shouldDraw = Settings.DelvePath_Armaments_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Forgotten Jewellery") { shouldDraw = Settings.DelvePath_Jewellery_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Forgotten Wealth") { shouldDraw = Settings.DelvePath_Wealth_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Lost Loot") { shouldDraw = Settings.DelveOffPath_Loot_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Lost Armour") { shouldDraw = Settings.DelveOffPath_Armour_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Lost Armaments") { shouldDraw = Settings.DelveOffPath_Armaments_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Lost Jewellery") { shouldDraw = Settings.DelveOffPath_Jewellery_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Lost Wealth") { shouldDraw = Settings.DelveOffPath_Wealth_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Loot") { shouldDraw = Settings.DelveDynamite_Loot_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Armour") { shouldDraw = Settings.DelveDynamite_Armour_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Armaments") { shouldDraw = Settings.DelveDynamite_Armaments_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Jewellery") { shouldDraw = Settings.DelveDynamite_Jewellery_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Wealth") { shouldDraw = Settings.DelveDynamite_Wealth_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Riches") { shouldDraw = Settings.DelveDynamite_Riches_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }
                        else if (n == "Hidden Opulence") { shouldDraw = Settings.DelveDynamite_Opulence_Draw; tint = Settings.DelveRoute_Tint; size = Settings.DelveRoute_Size; index = Settings.DelveRoute_Index; drawText = Settings.DelveRoute_DrawText; }

                        // Generic
                        else if (n == "Sealed Lockbox") shouldDraw = Settings.DelveGeneric_Sealed_Draw;
                        else if (n == "Hidden Treasures") shouldDraw = Settings.DelveGeneric_HiddenTreasures_Draw;
                        else if (n == "Murmuring Lockbox") shouldDraw = Settings.DelveGeneric_Murmuring_Draw;
                        else if (n == "Bloodstained Lockbox") shouldDraw = Settings.DelveGeneric_Bloodstained_Draw;
                        else if (n == "Opulent Lockbox") shouldDraw = Settings.DelveGeneric_Opulent_Draw;
                        else if (n == "Encrusted Lockbox") shouldDraw = Settings.DelveGeneric_Encrusted_Draw;
                        else if (n == "Light Lockbox") shouldDraw = Settings.DelveGeneric_Light_Draw;
                        else if (n == "Enchanted Lockbox") shouldDraw = Settings.DelveGeneric_Enchanted_Draw;
                        else if (n == "Eldritch Lockbox") shouldDraw = Settings.DelveGeneric_Eldritch_Draw;
                        else if (n == "Astral Lockbox") shouldDraw = Settings.DelveGeneric_Astral_Draw;
                        else if (n == "Clean Lockbox") shouldDraw = Settings.DelveGeneric_Clean_Draw;
                        else if (n == "Offering to the Goddess") shouldDraw = Settings.DelveGeneric_Offering_Draw;
                        else if (n == "Treasured Lockbox") shouldDraw = Settings.DelveGeneric_Treasured_Draw;
                        else if (n == "Hidden Heirlooms") shouldDraw = Settings.DelveGeneric_HiddenHeirlooms_Draw;
                        else if (n == "Shadowstitch") shouldDraw = Settings.DelveGeneric_Shadowstitch_Draw;

                        // Armour
                        else if (n == "Abandoned Armour") { shouldDraw = Settings.DelveArmour_Abandoned_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Twisted Armour") { shouldDraw = Settings.DelveArmour_Twisted_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Traveller's Armour") { shouldDraw = Settings.DelveArmour_Traveller_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Vital Armour") { shouldDraw = Settings.DelveArmour_Vital_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Pocked Armour") { shouldDraw = Settings.DelveArmour_Pocked_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Resistant Armour") { shouldDraw = Settings.DelveArmour_Resistant_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Unusual Armour") { shouldDraw = Settings.DelveArmour_Unusual_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Superior Armour") { shouldDraw = Settings.DelveArmour_Superior_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Astral Armour") { shouldDraw = Settings.DelveArmour_Astral_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Eldritch Armour") { shouldDraw = Settings.DelveArmour_Eldritch_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Triumphant Armour") { shouldDraw = Settings.DelveArmour_Triumphant_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Intricate Armour") { shouldDraw = Settings.DelveArmour_Intricate_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Flexible Armour") { shouldDraw = Settings.DelveArmour_Flexible_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Exceptional Armour") { shouldDraw = Settings.DelveArmour_Exceptional_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Shattered Armour") { shouldDraw = Settings.DelveArmour_Shattered_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }
                        else if (n == "Amalgamated Armour") { shouldDraw = Settings.DelveArmour_Amalgamated_Draw; tint = Settings.DelveArmour_Tint; size = Settings.DelveArmour_Size; index = Settings.DelveArmour_Index; drawText = Settings.DelveArmour_DrawText; }

                        // Weapons
                        else if (n == "Abandoned Armaments") { shouldDraw = Settings.DelveWeapon_Abandoned_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Twisted Armaments") { shouldDraw = Settings.DelveWeapon_Twisted_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Heavy Armaments") { shouldDraw = Settings.DelveWeapon_Heavy_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Runed Armaments") { shouldDraw = Settings.DelveWeapon_Runed_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Light Armaments") { shouldDraw = Settings.DelveWeapon_Light_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Superior Armaments") { shouldDraw = Settings.DelveWeapon_Superior_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Astral Armaments") { shouldDraw = Settings.DelveWeapon_Astral_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Eldritch Armaments") { shouldDraw = Settings.DelveWeapon_Eldritch_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Triumphant Armaments") { shouldDraw = Settings.DelveWeapon_Triumphant_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Honed Armaments") { shouldDraw = Settings.DelveWeapon_Honed_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Hexed Armaments") { shouldDraw = Settings.DelveWeapon_Hexed_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Exceptional Armaments") { shouldDraw = Settings.DelveWeapon_Exceptional_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Masterwork Armaments") { shouldDraw = Settings.DelveWeapon_Masterwork_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Shattered Armaments") { shouldDraw = Settings.DelveWeapon_Shattered_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }
                        else if (n == "Amalgamated Armaments") { shouldDraw = Settings.DelveWeapon_Amalgamated_Draw; tint = Settings.DelveWeapon_Tint; size = Settings.DelveWeapon_Size; index = Settings.DelveWeapon_Index; drawText = Settings.DelveWeapon_DrawText; }

                        // Jewellery
                        else if (n == "Abandoned Jewellery") { shouldDraw = Settings.DelveJewelry_AbandonedJewellery_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Abandoned Amulets") { shouldDraw = Settings.DelveJewelry_AbandonedAmulets_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Abandoned Rings") { shouldDraw = Settings.DelveJewelry_AbandonedRings_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Abandoned Jewels") { shouldDraw = Settings.DelveJewelry_AbandonedJewels_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Twisted Jewellery") { shouldDraw = Settings.DelveJewelry_TwistedJewellery_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Flexible Jewellery") { shouldDraw = Settings.DelveJewelry_Flexible_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Light Jewellery") { shouldDraw = Settings.DelveJewelry_Light_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Astral Jewellery") { shouldDraw = Settings.DelveJewelry_Astral_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Eldritch Jewellery") { shouldDraw = Settings.DelveJewelry_Eldritch_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Triumphant Jewellery") { shouldDraw = Settings.DelveJewelry_Triumphant_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Unusual Jewellery") { shouldDraw = Settings.DelveJewelry_Unusual_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Ritualistic Jewellery") { shouldDraw = Settings.DelveJewelry_Ritualistic_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Superior Jewellery") { shouldDraw = Settings.DelveJewelry_Superior_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Shattered Jewellery") { shouldDraw = Settings.DelveJewelry_Shattered_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Amalgamated Jewellery") { shouldDraw = Settings.DelveJewelry_Amalgamated_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Twisted Jewels") { shouldDraw = Settings.DelveJewelry_TwistedJewels_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }
                        else if (n == "Compacted Jewels") { shouldDraw = Settings.DelveJewelry_Compacted_Draw; tint = Settings.DelveJewelry_Tint; size = Settings.DelveJewelry_Size; index = Settings.DelveJewelry_Index; drawText = Settings.DelveJewelry_DrawText; }

                        // Gem
                        else if (n == "Abandoned Gems") { shouldDraw = Settings.DelveGem_Abandoned_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        else if (n == "Gemcutter's Gems") { shouldDraw = Settings.DelveGem_Gemcutters_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        else if (n == "Superior Gems") { shouldDraw = Settings.DelveGem_Superior_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        else if (n == "Ancient Gems") { shouldDraw = Settings.DelveGem_Ancient_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        else if (n == "Inscribed Gems") { shouldDraw = Settings.DelveGem_Inscribed_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        else if (n == "Exceptional Gems") { shouldDraw = Settings.DelveGem_Exceptional_Draw; tint = Settings.DelveGem_Tint; size = Settings.DelveGem_Size; index = Settings.DelveGem_Index; drawText = Settings.DelveGem_DrawText; }
                        
                        // Currency
                        else if (n == "Abandoned Riches") { shouldDraw = Settings.DelveCurrency_AbandonedRiches_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Pocked Riches") { shouldDraw = Settings.DelveCurrency_Pocked_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Bloodstained Riches") { shouldDraw = Settings.DelveCurrency_Bloodstained_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Light Riches") { shouldDraw = Settings.DelveCurrency_Light_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Glimmering Riches") { shouldDraw = Settings.DelveCurrency_Glimmering_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Packed Riches") { shouldDraw = Settings.DelveCurrency_Packed_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Cartographer's Riches") { shouldDraw = Settings.DelveCurrency_Cartographers_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Shattered Riches") { shouldDraw = Settings.DelveCurrency_Shattered_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }
                        else if (n == "Abandoned Fortune") { shouldDraw = Settings.DelveCurrency_AbandonedFortune_Draw; tint = Settings.DelveCurrency_Tint; size = Settings.DelveCurrency_Size; index = Settings.DelveCurrency_Index; drawText = Settings.DelveCurrency_DrawText; }

                        // Maps
                        else if (n == "Abandoned Charts") { shouldDraw = Settings.DelveMap_Abandoned_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Cartographer's Charts") { shouldDraw = Settings.DelveMap_Cartographers_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Bloodstained Charts") { shouldDraw = Settings.DelveMap_Bloodstained_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Strange Charts") { shouldDraw = Settings.DelveMap_Strange_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Explorer's Charts") { shouldDraw = Settings.DelveMap_Explorers_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Beautiful Charts") { shouldDraw = Settings.DelveMap_Beautiful_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Puzzling Charts") { shouldDraw = Settings.DelveMap_Puzzling_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Maddening Charts") { shouldDraw = Settings.DelveMap_Maddening_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }
                        else if (n == "Remembered Charts") { shouldDraw = Settings.DelveMap_Remembered_Draw; tint = Settings.DelveMap_Tint; size = Settings.DelveMap_Size; index = Settings.DelveMap_Index; drawText = Settings.DelveMap_DrawText; }

                        // Special
                        else if (n == "Grim Armour") { shouldDraw = Settings.DelveSpecial_GrimArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Grim Jewellery") { shouldDraw = Settings.DelveSpecial_GrimJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Grim Loot") { shouldDraw = Settings.DelveSpecial_GrimLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Grim Treasures") { shouldDraw = Settings.DelveSpecial_GrimTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Heavy Armour") { shouldDraw = Settings.DelveSpecial_HeavyArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Heavy Jewellery") { shouldDraw = Settings.DelveSpecial_HeavyJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Heavy Loot") { shouldDraw = Settings.DelveSpecial_HeavyLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Heavy Treasures") { shouldDraw = Settings.DelveSpecial_HeavyTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Smouldering Armour") { shouldDraw = Settings.DelveSpecial_SmoulderingArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Smouldering Armaments") { shouldDraw = Settings.DelveSpecial_SmoulderingArmaments_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Smouldering Jewellery") { shouldDraw = Settings.DelveSpecial_SmoulderingJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Smouldering Loot") { shouldDraw = Settings.DelveSpecial_SmoulderingLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Smouldering Treasures") { shouldDraw = Settings.DelveSpecial_SmoulderingTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Frosty Armour") { shouldDraw = Settings.DelveSpecial_FrostyArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Frosty Armaments") { shouldDraw = Settings.DelveSpecial_FrostyArmaments_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Frosty Jewellery") { shouldDraw = Settings.DelveSpecial_FrostyJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Frosty Loot") { shouldDraw = Settings.DelveSpecial_FrostyLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Frosty Treasures") { shouldDraw = Settings.DelveSpecial_FrostyTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Charged Armour") { shouldDraw = Settings.DelveSpecial_ChargedArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Charged Armaments") { shouldDraw = Settings.DelveSpecial_ChargedArmaments_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Charged Jewellery") { shouldDraw = Settings.DelveSpecial_ChargedJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Charged Loot") { shouldDraw = Settings.DelveSpecial_ChargedLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Charged Treasures") { shouldDraw = Settings.DelveSpecial_ChargedTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Malignant Armour") { shouldDraw = Settings.DelveSpecial_MalignantArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Malignant Armaments") { shouldDraw = Settings.DelveSpecial_MalignantArmaments_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Malignant Jewellery") { shouldDraw = Settings.DelveSpecial_MalignantJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Malignant Loot") { shouldDraw = Settings.DelveSpecial_MalignantLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Malignant Treasures") { shouldDraw = Settings.DelveSpecial_MalignantTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Feral Talismans") { shouldDraw = Settings.DelveSpecial_FeralTalismans_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Feral Armour") { shouldDraw = Settings.DelveSpecial_FeralArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Feral Jewellery") { shouldDraw = Settings.DelveSpecial_FeralJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Whispering Coffer") { shouldDraw = Settings.DelveSpecial_WhisperingCoffer_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Inspiring Armour") { shouldDraw = Settings.DelveSpecial_InspiringArmour_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Inspiring Jewellery") { shouldDraw = Settings.DelveSpecial_InspiringJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Inspiring Loot") { shouldDraw = Settings.DelveSpecial_InspiringLoot_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Inspiring Treasures") { shouldDraw = Settings.DelveSpecial_InspiringTreasures_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Gazing Jewellery") { shouldDraw = Settings.DelveSpecial_GazingJewellery_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }
                        else if (n == "Prehistoric Riches") { shouldDraw = Settings.DelveSpecial_PrehistoricRiches_Draw; tint = Settings.DelveSpecial_Tint; size = Settings.DelveSpecial_Size; index = Settings.DelveSpecial_Index; drawText = Settings.DelveSpecial_DrawText; }

                        // City Biomes
                        else if (n == "Bloodstained Loot") { shouldDraw = Settings.DelveCity_BloodstainedLoot_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Handcrafted Antiques") { shouldDraw = Settings.DelveCity_HandcraftedAntiques_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Mythical Crate") { shouldDraw = Settings.DelveCity_MythicalCrate_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Disturbing Loot") { shouldDraw = Settings.DelveCity_DisturbingLoot_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Rattling Coffer") { shouldDraw = Settings.DelveCity_RattlingCoffer_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Abyssal Jewellery") { shouldDraw = Settings.DelveCity_AbyssalJewellery_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Abyssal Treasures") { shouldDraw = Settings.DelveCity_AbyssalTreasures_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Prehistoric Ore") { shouldDraw = Settings.DelveCity_PrehistoricOre_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Prehistoric Fossils") { shouldDraw = Settings.DelveCity_PrehistoricFossils_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Prehistoric Resonators") { shouldDraw = Settings.DelveCity_PrehistoricResonators_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Prehistoric Rings") { shouldDraw = Settings.DelveCity_PrehistoricRings_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Prehistoric Jewels") { shouldDraw = Settings.DelveCity_PrehistoricJewels_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Concealed Opulence") { shouldDraw = Settings.DelveCity_ConcealedOpulence_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Concealed Oddities") { shouldDraw = Settings.DelveCity_ConcealedOddities_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }
                        else if (n == "Concealed Adornments") { shouldDraw = Settings.DelveCity_ConcealedAdornments_Draw; tint = Settings.DelveCity_Tint; size = Settings.DelveCity_Size; index = Settings.DelveCity_Index; drawText = Settings.DelveCity_DrawText; }

                        icon.Draw = shouldDraw;
                        icon.DrawText = drawText;
                        icon.Size = size;
                        icon.Index = index;
                        icon.Tint = tint;
                    };
                }
                break;
            // Sanctum Chests
            case string path when path.StartsWith("Metadata/Chests/LeagueSanctum/", StringComparison.Ordinal):
                icon.IconType = IconTypes.SanctumChest;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.SanctumChest_Draw;
                    icon.DrawText = Settings.SanctumChest_DrawText;
                    icon.Size = Settings.SanctumChest_Size;
                    icon.Index = Settings.SanctumChest_Index;
                    icon.Tint = Settings.SanctumChest_Tint;
                };
                break;
            // Strongboxes
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/ArmourerStrongbox"):
            //    IconType = IconTypes.ArmourerStrongbox;
            //    Setting_Draw = () => Settings.ArmourerStrongbox_Draw;
            //    Setting_DrawText = () => Settings.ArmourerStrongbox_DrawText;
            //    Setting_Size = () => Settings.ArmourerStrongbox_Size;
            //    Setting_Index = () => Settings.ArmourerStrongbox_Index;
            //    Setting_Color = () => Settings.ArmourerStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/MartialStrongbox"):
            //    IconType = IconTypes.BlacksmithStrongbox;
            //    Setting_Draw = () => Settings.BlacksmithStrongbox_Draw;
            //    Setting_DrawText = () => Settings.BlacksmithStrongbox_DrawText;
            //    Setting_Size = () => Settings.BlacksmithStrongbox_Size;
            //    Setting_Index = () => Settings.BlacksmithStrongbox_Index;
            //    Setting_Color = () => Settings.BlacksmithStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Artisan"):
            //    IconType = IconTypes.ArtisanStrongbox;
            //    Setting_Draw = () => Settings.ArtisanStrongbox_Draw;
            //    Setting_DrawText = () => Settings.ArtisanStrongbox_DrawText;
            //    Setting_Size = () => Settings.ArtisanStrongbox_Size;
            //    Setting_Index = () => Settings.ArtisanStrongbox_Index;
            //    Setting_Color = () => Settings.ArtisanStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Cartographer"):
            //    IconType = IconTypes.CartographerStrongbox;
            //    Setting_Draw = () => Settings.CartographerStrongbox_Draw;
            //    Setting_DrawText = () => Settings.CartographerStrongbox_DrawText;
            //    Setting_Size = () => Settings.CartographerStrongbox_Size;
            //    Setting_Index = () => Settings.CartographerStrongbox_Index;
            //    Setting_Color = () => Settings.CartographerStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Chemist"):
            //    IconType = IconTypes.ChemistStrongbox;
            //    Setting_Draw = () => Settings.ChemistStrongbox_Draw;
            //    Setting_DrawText = () => Settings.ChemistStrongbox_DrawText;
            //    Setting_Size = () => Settings.ChemistStrongbox_Size;
            //    Setting_Index = () => Settings.ChemistStrongbox_Index;
            //    Setting_Color = () => Settings.ChemistStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Gemcutter"):
            //    IconType = IconTypes.GemcutterStrongbox;
            //    Setting_Draw = () => Settings.GemcutterStrongbox_Draw;
            //    Setting_DrawText = () => Settings.GemcutterStrongbox_DrawText;
            //    Setting_Size = () => Settings.GemcutterStrongbox_Size;
            //    Setting_Index = () => Settings.GemcutterStrongbox_Index;
            //    Setting_Color = () => Settings.GemcutterStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Jeweller"):
            //    IconType = IconTypes.JewellerStrongbox;
            //    Setting_Draw = () => Settings.JewellerStrongbox_Draw;
            //    Setting_DrawText = () => Settings.JewellerStrongbox_DrawText;
            //    Setting_Size = () => Settings.JewellerStrongbox_Size;
            //    Setting_Index = () => Settings.JewellerStrongbox_Index;
            //    Setting_Color = () => Settings.JewellerStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Large"):
            //    IconType = IconTypes.LargeStrongbox;
            //    Setting_Draw = () => Settings.LargeStrongbox_Draw;
            //    Setting_DrawText = () => Settings.LargeStrongbox_DrawText;
            //    Setting_Size = () => Settings.LargeStrongbox_Size;
            //    Setting_Index = () => Settings.LargeStrongbox_Index;
            //    Setting_Color = () => Settings.LargeStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Ornate"):
            //    IconType = IconTypes.OrnateStrongbox;
            //    Setting_Draw = () => Settings.OrnateStrongbox_Draw;
            //    Setting_DrawText = () => Settings.OrnateStrongbox_DrawText;
            //    Setting_Size = () => Settings.OrnateStrongbox_Size;
            //    Setting_Index = () => Settings.OrnateStrongbox_Index;
            //    Setting_Color = () => Settings.OrnateStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/StrongboxDivination"):
            //    IconType = IconTypes.DivinerStrongbox;
            //    Setting_Draw = () => Settings.DivinerStrongbox_Draw;
            //    Setting_DrawText = () => Settings.DivinerStrongbox_DrawText;
            //    Setting_Size = () => Settings.DivinerStrongbox_Size;
            //    Setting_Index = () => Settings.DivinerStrongbox_Index;
            //    Setting_Color = () => Settings.DivinerStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Operative"):
            //    IconType = IconTypes.OperativeStrongbox;
            //    Setting_Draw = () => Settings.OperativeStrongbox_Draw;
            //    Setting_DrawText = () => Settings.OperativeStrongbox_DrawText;
            //    Setting_Size = () => Settings.OperativeStrongbox_Size;
            //    Setting_Index = () => Settings.OperativeStrongbox_Index;
            //    Setting_Color = () => Settings.OperativeStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Arcane"):
            //    IconType = IconTypes.ArcaneStrongbox;
            //    Setting_Draw = () => Settings.ArcaneStrongbox_Draw;
            //    Setting_DrawText = () => Settings.ArcaneStrongbox_DrawText;
            //    Setting_Size = () => Settings.ArcaneStrongbox_Size;
            //    Setting_Index = () => Settings.ArcaneStrongbox_Index;
            //    Setting_Color = () => Settings.ArcaneStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes/Researcher"):
            //    IconType = IconTypes.ResearcherStrongbox;
            //    Setting_Draw = () => Settings.ResearcherStrongbox_Draw;
            //    Setting_DrawText = () => Settings.ResearcherStrongbox_DrawText;
            //    Setting_Size = () => Settings.ResearcherStrongbox_Size;
            //    Setting_Index = () => Settings.ResearcherStrongbox_Index;
            //    Setting_Color = () => Settings.ResearcherStrongbox_Tint;
            //    break;
            //case string path when path.StartsWith("Metadata/Chests/StrongBoxes"):
            //    IconType = IconTypes.UnknownStrongbox;
            //    Setting_Draw = () => Settings.UnknownStrongbox_Draw;
            //    Setting_DrawText = () => Settings.UnknownStrongbox_DrawText;
            //    Setting_Size = () => Settings.UnknownStrongbox_Size;
            //    Setting_Index = () => Settings.UnknownStrongbox_Index;
            //    Setting_Color = () => Settings.UnknownStrongbox_Tint;
            //    break;
            // Default
            default:
                if (entity.Rarity == MonsterRarity.White) {
                    icon.IconType = IconTypes.ChestWhite;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.NormalChest_Draw;
                        icon.DrawText = Settings.NormalChest_DrawText;
                        icon.Size = Settings.NormalChest_Size;
                        icon.Index = Settings.NormalChest_Index;
                        icon.Tint = Settings.NormalChest_Tint;
                    };
                }
                else if (entity.Rarity == MonsterRarity.Magic) {
                    icon.IconType = IconTypes.ChestMagic;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.MagicChest_Draw;
                        icon.DrawText = Settings.MagicChest_DrawText;
                        icon.Size = Settings.MagicChest_Size;
                        icon.Index = Settings.MagicChest_Index;
                        icon.Tint = Settings.MagicChest_Tint;
                    };
                }
                else if (entity.Rarity == MonsterRarity.Rare) {
                    icon.IconType = IconTypes.ChestRare;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.RareChest_Draw;
                        icon.DrawText = Settings.RareChest_DrawText;
                        icon.Size = Settings.RareChest_Size;
                        icon.Index = Settings.RareChest_Index;
                        icon.Tint = Settings.RareChest_Tint;
                    };
                }
                else if (entity.Rarity == MonsterRarity.Unique) {
                    icon.IconType = IconTypes.ChestUnique;
                    icon.UpdateSettingsAction = () => {
                        icon.Draw = Settings.UniqueChest_Draw;
                        icon.DrawText = Settings.UniqueChest_DrawText;
                        icon.Size = Settings.UniqueChest_Size;
                        icon.Index = Settings.UniqueChest_Index;
                        icon.Tint = Settings.UniqueChest_Tint;
                    };
                }
                else return null;
                break;
        }
        DebugChestIcon(icon);
        return icon;
    }
    private MapIcon CreateDeliriumIcon(Entity entity) {
        var icon = new MapIcon(entity);
        icon.IconRenderType = IconRenderTypes.Monster;
        icon.IconCategory = IconCategories.Delirium;
        icon.Text = icon.RenderName;
        icon.Show = () => entity.IsAlive;

        var pathstring = "Metadata/Monsters/LeagueAffliction/DoodadDaemons/DoodadDaemon";

        if (entity.Path.StartsWith(pathstring + "BloodBag", StringComparison.Ordinal)) {
            icon.IconType = IconTypes.BloodBag;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.BloodBag_Draw;
                icon.DrawText = Settings.BloodBag_DrawText;
                icon.Size = Settings.BloodBag_Size;
                icon.Index = Settings.BloodBag_Index;
                icon.Tint = Settings.BloodBag_Tint;
                icon.HiddenTint = Settings.BloodBag_HiddenTint;
            };
        }
        else if (entity.Path.StartsWith(pathstring + "EggFodder", StringComparison.Ordinal)) {
            icon.IconType = IconTypes.EggFodder;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.EggFodder_Draw;
                icon.DrawText = Settings.EggFodder_DrawText;
                icon.Size = Settings.EggFodder_Size;
                icon.Index = Settings.EggFodder_Index;
                icon.Tint = Settings.EggFodder_Tint;
                icon.HiddenTint = Settings.EggFodder_HiddenTint;
            };
        }
        else if (entity.Path.StartsWith(pathstring + "GlobSpawn", StringComparison.Ordinal)) {
            icon.IconType = IconTypes.GlobSpawn;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.GlobSpawn_Draw;
                icon.DrawText = Settings.GlobSpawn_DrawText;
                icon.Size = Settings.GlobSpawn_Size;
                icon.Index = Settings.GlobSpawn_Index;
                icon.Tint = Settings.GlobSpawn_Tint;
                icon.HiddenTint = Settings.GlobSpawn_HiddenTint;
            };
        }
        else return null;

        icon.Hidden = () => false;

        DebugNPCIcon(icon);
        return icon;
    }
    private MapIcon CreateNPCIcon(Entity entity) {
        if (entity.Path.Contains("Breach", StringComparison.OrdinalIgnoreCase) && 
           (entity.Rarity == MonsterRarity.White || entity.Rarity == MonsterRarity.Magic || entity.Rarity == MonsterRarity.Rare || entity.Rarity == MonsterRarity.Error)) {
            return null;
        }

        var icon = new MapIcon(entity);
        icon.IconRenderType = IconRenderTypes.Monster;
        icon.IconCategory = IconCategories.NPC;
        icon.Text = icon.RenderName;
        // Despawned monsters that don't die (Breach/Ultimatum ends) often retain HP but become untargetable
        icon.Show = () => entity.IsAlive && (!entity.HasComponent<Targetable>() || entity.IsTargetable);
        
        // NPC
        if (entity.Type == EntityType.Npc) {
            if (!entity.HasComponent<Render>()) return null;
            var component = entity.GetComponent<Render>();
            icon.IconType = IconTypes.NPC;
            icon.Text = component?.Name.Split(',')[0];
            icon.Show = () => entity.IsValid && (!entity.HasComponent<Targetable>() || entity.IsTargetable);
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.NPC_Draw;
                icon.DrawText = Settings.NPC_DrawText;
                icon.Size = Settings.NPC_Size;
                icon.Index = Settings.NPC_Index;
                icon.Tint = Settings.NPC_Tint;
                icon.HiddenTint = Settings.NPC_HiddenTint;
            };
        }
        //else if (entity.Path.StartsWith("Metadata/Monsters/LeagueDelirium/DoodadDaemons", StringComparison.Ordinal)) {
        //    if (entity.Path.Contains("ShardPack", StringComparison.OrdinalIgnoreCase)) {
        //        icon.Priority = IconPriority.Medium;
        //        icon.Hidden = () => false;
        //        icon.IconType = IconTypes.FracturingMirror;
        //        icon.UpdateSettingsAction = () => {
        //            icon.Draw = Settings.FracturingMirror_Draw;
        //            icon.DrawText = Settings.FracturingMirror_DrawText;
        //            icon.Size = Settings.FracturingMirror_Size;
        //            icon.Index = Settings.FracturingMirror_Index;
        //            icon.Tint = Settings.FracturingMirror_Tint;
        //            icon.HiddenTint = Settings.FracturingMirror_HiddenTint;
        //        };
        //    }
        //    else return null;
        //}
        //Volatile Core
        else if (entity.Path.Contains("Metadata/Monsters/LeagueAffliction/Volatile")) {
            icon.Priority = IconPriority.Medium;
            icon.IconType = IconTypes.VolatileCore;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.VolatileCore_Draw;
                icon.DrawText = Settings.VolatileCore_DrawText;
                icon.Size = Settings.VolatileCore_Size;
                icon.Index = Settings.VolatileCore_Index;
                icon.Tint = Settings.VolatileCore_Tint;
                icon.HiddenTint = Settings.VolatileCore_HiddenTint;
            };
            icon.Hidden = () => false;
        }
        // Minion
        else if (!entity.IsHostile) {
            icon.Priority = IconPriority.Low;
            icon.IconType = IconTypes.Minion;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.Minion_Draw;
                icon.DrawText = Settings.Minion_DrawText;
                icon.Size = Settings.Minion_Size;
                icon.Index = Settings.Minion_Index;
                icon.Tint = Settings.Minion_Tint;
                icon.HiddenTint = Settings.Minion_HiddenTint;
            };
        }
        // Spirit
        else if (icon.Rarity == MonsterRarity.Unique && entity.Path.Contains("Metadata/Monsters/Spirit/")) {
            icon.IconType = IconTypes.Spirit;
            icon.UpdateSettingsAction = () => {
                icon.Draw = Settings.Spirit_Draw;
                icon.DrawText = Settings.Spirit_DrawText;
                icon.Size = Settings.Spirit_Size;
                icon.Index = Settings.Spirit_Index;
                icon.Tint = Settings.Spirit_Tint;
                icon.HiddenTint = Settings.Spirit_HiddenTint;
            };
        }
        // Monster
        else {
            // legion monsters
            if (entity.Path.Contains("Legion")) {
                var statDictionary = entity.Stats;
                if (statDictionary == null) {
                    icon.Show = () => entity.GetComponent<Life>().HPPercentage > 0.02;
                }
                else if (statDictionary.Count == 0) {
                    statDictionary = entity.GetComponentFromMemory<Stats>()?.StatDictionary ?? statDictionary;
                    if (statDictionary.Count == 0) icon.Text = "Error";
                }
                //else if (statDictionary.TryGetValue(GameStat.MonsterMinimapIcon, out var indexMinimapIcon)) {
                //    var name = (MapIconsIndex)indexMinimapIcon;
                //    icon.Priority = IconPriority.Critical;
                //
                //    var frozenCheck = new TimeCache<bool>(() =>
                //    {
                //        var stats = entity.Stats;
                //        if (stats.Count == 0) return false;
                //        stats.TryGetValue(GameStat.FrozenInTime, out var frozenInTime);
                //        stats.TryGetValue(GameStat.MonsterHideMinimapIcon, out var monsterHideMinimapIcon);
                //        return frozenInTime == 1 && monsterHideMinimapIcon == 1 || frozenInTime == 0 && monsterHideMinimapIcon == 0;
                //    }, 75);
                //
                //    icon.Show = () => entity.IsAlive && frozenCheck.Value;
                //}
                else
                    icon.Show = () => !icon.Hidden() && entity.GetComponent<Life>()?.HPPercentage > 0.02;
            }
            // White Monster
            if (icon.Rarity == MonsterRarity.White) {
                icon.IconType = IconTypes.WhiteMonster;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.WhiteMonster_Draw;
                    icon.DrawText = Settings.WhiteMonster_DrawText;
                    icon.Size = Settings.WhiteMonster_Size;
                    icon.Index = Settings.WhiteMonster_Index;
                    icon.Tint = Settings.WhiteMonster_Tint;
                    icon.HiddenTint = Settings.WhiteMonster_HiddenTint;
                };
            }
            // Magic
            else if (icon.Rarity == MonsterRarity.Magic) {
                icon.IconType = IconTypes.MagicMonster;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.MagicMonster_Draw;
                    icon.DrawText = Settings.MagicMonster_DrawText;
                    icon.Size = Settings.MagicMonster_Size;
                    icon.Index = Settings.MagicMonster_Index;
                    icon.Tint = Settings.MagicMonster_Tint;
                    icon.HiddenTint = Settings.MagicMonster_HiddenTint;
                };
            }
            // Rare
            else if (icon.Rarity == MonsterRarity.Rare) {
                icon.IconType = IconTypes.RareMonster;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.RareMonster_Draw;
                    icon.DrawText = Settings.RareMonster_DrawText;
                    icon.Size = Settings.RareMonster_Size;
                    icon.Index = Settings.RareMonster_Index;
                    icon.Tint = Settings.RareMonster_Tint;
                    icon.HiddenTint = Settings.RareMonster_HiddenTint;
                };
            }
            // Unique
            else if (icon.Rarity == MonsterRarity.Unique) {
                icon.IconType = IconTypes.UniqueMonster;
                icon.UpdateSettingsAction = () => {
                    icon.Draw = Settings.UniqueMonster_Draw;
                    icon.DrawText = Settings.UniqueMonster_DrawText;
                    icon.Size = Settings.UniqueMonster_Size;
                    icon.Index = Settings.UniqueMonster_Index;
                    icon.Tint = Settings.UniqueMonster_Tint;
                    icon.HiddenTint = Settings.UniqueMonster_HiddenTint;
                };
            }
            //Converts on death
            if (entity.HasComponent<ObjectMagicProperties>()) {
                var objectMagicProperties = entity.GetComponent<ObjectMagicProperties>();
                var mods = objectMagicProperties?.Mods;
                if (mods != null) {
                    if (mods.Contains("MonsterConvertsOnDeath_")) icon.Show = () => entity.IsAlive && entity.IsHostile;
                }
            }
        }

        DebugNPCIcon(icon);
        return icon;
    }
    private MapIcon CreateIngameIcon(Entity entity) {
        var icon = new MapIcon(entity);
        icon.IconRenderType = IconRenderTypes.IngameIcon;
        icon.IconCategory = IconCategories.IngameIcon;
        icon.Text = icon.RenderName;

        var minimapIconComponent = entity.GetComponent<MinimapIcon>();
        if (minimapIconComponent == null || minimapIconComponent.IsHide) return null;

        var minimapIconName = minimapIconComponent.Name;
        if (string.IsNullOrEmpty(minimapIconName)) return null;

        var iconIndexByName = Extensions.IconIndexByName(minimapIconName);
        if (iconIndexByName == MapIconsIndex.MyPlayer) return null;

        icon.InGameTexture = new HudTexture("Icons.png") { UV = SpriteHelper.GetUV(iconIndexByName), Size = 16 };
        // revisit this later and recheck logic
        var isHidden = false;
        var transitionableFlag1 = 1;
        var shrineIsAvailable = true;
        var isOpened = false;
        var isTargetable = true;

        T Update<T>(ref T store, Func<T> update) { return entity.IsValid ? store = update() : store; }

        icon.Show = () => !Update(ref isHidden, () => entity.GetComponent<MinimapIcon>()?.IsHide ?? isHidden) &&
                           Update(ref transitionableFlag1, () => entity.GetComponent<Transitionable>()?.Flag1 ?? 1) == 1 &&
                           Update(ref shrineIsAvailable, () => entity.GetComponent<Shrine>()?.IsAvailable ?? shrineIsAvailable) &&
                          !Update(ref isOpened, () => entity.GetComponent<Chest>()?.IsOpened ?? isOpened) &&
                           Update(ref isTargetable, () => !entity.HasComponent<Targetable>() || entity.IsTargetable);

        var iconSizeMultiplier = RemoteMemoryObject.pTheGame.Files.MinimapIcons.EntriesList.ElementAtOrDefault((int)iconIndexByName)?.LargeMinimapSize ?? 1;
        icon.InGameTexture.Size = RemoteMemoryObject.pTheGame.IngameState.IngameUi.Map.LargeMapZoom * RemoteMemoryObject.pTheGame.IngameState.Camera.Height * iconSizeMultiplier / 64;

        // Shrine
        if (entity.HasComponent<Shrine>()) {
            icon.IconType = IconTypes.Shrine;
            icon.UpdateSettingsAction = () => {
                icon.DrawState = Settings.Shrine_State;
                icon.DrawText = Settings.Shrine_DrawText;
            };
        }
        // NPC
        else if (entity.Type == EntityType.Npc) {
            icon.IconType = IconTypes.IngameNPC;
            icon.UpdateSettingsAction = () => {
                icon.DrawState = Settings.IngameNPC_State;
                icon.DrawText = Settings.IngameNPC_DrawText;
            };
        }
        // QuestObject
        else if (minimapIconName == "QuestObject") {
            icon.IconType = IconTypes.QuestObject;
            icon.UpdateSettingsAction = () => {
                icon.DrawState = Settings.QuestObject_State;
                icon.DrawText = Settings.QuestObject_DrawText;
            };
        }
        else {
            switch (entity.Path) {
                case string path when path.StartsWith("Metadata/MiscellaneousObjects/Waypoint"):
                    icon.IconType = IconTypes.Waypoint;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.Waypoint_State;
                        icon.DrawText = Settings.Waypoint_DrawText;
                    };
                    break;
                case string path when path.StartsWith("Metadata/MiscellaneousObjects/Checkpoint"):
                    icon.IconType = IconTypes.Checkpoint;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.Checkpoint_State;
                        icon.DrawText = Settings.Checkpoint_DrawText;
                    };
                    break;
                case string path when path.StartsWith("Metadata/MiscellaneousObjects/AreaTransition"):
                    icon.IconType = IconTypes.AreaTransition;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.AreaTransition_State;
                        icon.DrawText = Settings.AreaTransition_DrawText;
                    };
                    break;
                case string path when path.StartsWith("Metadata/Terrain/Leagues/Ritual/RitualRuneInteractable"):
                    icon.IconType = IconTypes.Ritual;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.Ritual_State;
                        icon.DrawText = Settings.Ritual_DrawText;
                    };
                    break;
                case string path when path.StartsWith("Metadata/MiscellaneousObjects/Breach/BreachObject"):
                    icon.IconType = IconTypes.Breach;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.Breach_State;
                        icon.DrawText = Settings.Breach_DrawText;
                    };
                    break;
                default:
                    icon.IconType = IconTypes.IngameUncategorized;
                    icon.UpdateSettingsAction = () => {
                        icon.DrawState = Settings.IngameUncategorized_State;
                        icon.DrawText = Settings.IngameUncategorized_DrawText;
                    };
                    break;
            }
        }
        DebugIngameIcon(icon);
        return icon;
    }
    private MapIcon CreateIcon(Entity entity) {
        if (entity is null) return null;
        if (string.IsNullOrEmpty(entity.Path)) return null;
        if (!entity.IsValid) return null; // only adding the icon if its valid, to make sure components are available when creating icon... 
        // custom icon by path
        var customIconSettings = Settings.CustomIconSettingsList.FirstOrDefault(setting => entity.Path.StartsWith(setting.Path, StringComparison.Ordinal));
        if (customIconSettings != null) {
            var icon = new MapIcon(entity);
            icon.IconRenderType = IconRenderTypes.Monster;
            icon.IconType = IconTypes.CustomPath;
            icon.Text = entity.RenderName;
            icon.Show = () => true;
            icon.UpdateSettingsAction = () => {
                icon.Draw = customIconSettings.Setting_Draw;
                icon.DrawText = customIconSettings.Setting_DrawText;
                icon.Size = customIconSettings.Setting_Size;
                icon.Index = customIconSettings.Setting_Index;
                icon.Tint = customIconSettings.Setting_Tint;
                icon.HiddenTint = customIconSettings.Setting_HiddenTint;
                icon.Check_IsAlive = customIconSettings.Setting_IsAlive;
                icon.Check_IsOpened = customIconSettings.Setting_IsOpened;
            };
            DebugCustomIcon(icon);
            return icon;
        }
        // skip 
        if (SkippedEntityTypes.Any(type => type == entity.Type)) return null;
        if (SkippedEntityPaths.Any(path => entity.Path?.StartsWith(path) == true)) return null;
        if (UserSkippedEntityPaths.Any(path => entity.Path?.StartsWith(path) == true)) return null;
        if (entity.Type == EntityType.WorldItem) return null; // skip world items
        if (entity.Type == EntityType.MiscellaneousObjects) return null; // skip miscellaneous objects
        // Sanctum Mote
        if (entity.Type == EntityType.Terrain) {
            if (entity.Path.StartsWith("Metadata/Terrain/Leagues/Sanctum/Objects/SanctumMote")) {
                var icon = new MapIcon(entity);
                icon.IconRenderType = IconRenderTypes.Chest;
                icon.IconCategory = IconCategories.Misc;
                icon.IconType = IconTypes.SanctumMote;
                icon.UpdateSettingsAction = () =>
                {
                    icon.Draw = customIconSettings.Setting_Draw;
                    icon.DrawText = customIconSettings.Setting_DrawText;
                    icon.Size = customIconSettings.Setting_Size;
                    icon.Index = customIconSettings.Setting_Index;
                    icon.Tint = customIconSettings.Setting_Tint;
                };
                DebugMiscIcon(icon);
                return icon;
            }
            return null; // skip most of these entity types
        }
        // Ingame Icon
        if (entity.HasComponent<MinimapIcon>()) return CreateIngameIcon(entity);
        // Delirium Icon
        if (entity.Path.StartsWith("Metadata/Monsters/LeagueAffliction/DoodadDaemons", StringComparison.Ordinal)) {
            return CreateDeliriumIcon(entity);
        }
        // NPC
        if (entity.Type == EntityType.Monster || entity.Type == EntityType.Npc) {
            if (!entity.IsAlive) return null;
            return CreateNPCIcon(entity);
        }
        // Chest
        if (entity.Type == EntityType.Chest && !entity.IsOpened) return CreateChestIcon(entity);
        //Miscellaneous
        return CreateMiscIcon(entity);
    }

    private void DebugIcon(MapIcon icon) {
        if (!Settings.Debug) return;
        Log.Write($"--| IconType: {icon.IconType} | IconCategory: {icon.IconCategory} | IconRenderType: {icon.IconRenderType} | Entity.Type: {icon.Entity.Type} | RenderName: {icon.Entity.RenderName} | Path: {icon.Entity.Path}");
    }
    private void DebugIngameIcon(MapIcon icon) {
        if (Settings.DebugIngameIcon) DebugIcon(icon);
    }
    private void DebugNPCIcon(MapIcon icon) {
        if (Settings.DebugNPCIcon) DebugIcon(icon);
    }
    private void DebugMiscIcon(MapIcon icon) {
        if (Settings.DebugMiscIcon) DebugIcon(icon);
    }
    private void DebugCustomIcon(MapIcon icon) {
        if (Settings.DebugCustomIcon) DebugIcon(icon);
    }
    private void DebugChestIcon(MapIcon icon) {
        if (Settings.DebugChestIcon) DebugIcon(icon);
    }


    public void UpdateUserSkippedEntities() {
        // Split the input by lines and add to the list, ignoring lines starting with #
        UserSkippedEntityPaths = new List<string>();
        var lines = Settings.ignoredEntities.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines) {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#")) {
                UserSkippedEntityPaths.Add(line.Trim());
            }
        }
        RebuildIcons(); // Forced update on all icons
    }
}
