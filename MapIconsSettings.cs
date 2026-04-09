
using ExileCore;
using ExileCore.Shared.Nodes;
using ExileCore.Shared.Interfaces;
using System.Numerics;

namespace MapIcons;

public class CustomIconSettings
{
    public string Path;
    public bool Setting_Draw;
    public bool Setting_DrawText;
    public int Setting_Size;
    public int Setting_Index;
    public Vector4 Setting_Tint;
    public Vector4 Setting_HiddenTint;

    public bool Setting_IsAlive;
    public bool Setting_IsOpened;
}
public sealed class MapIconsSettings : ISettings {
    public ToggleNode Enable { get; set; } = new(true);

    public int IconListUpdatePeriod = 100; // milliseconds
    public int RunEveryXTicks = 5;
    public bool RealtimeIconSettings = false;

    public bool DrawOnMinimap = true;
    public bool DrawCachedEntities = true;
    public bool DrawOverLargePanels = false;
    public bool DrawOverFullscreenPanels = false;

    public int IgnoredEntitiesHeight = 100;
    public bool DrawSetingsOpen = true;
    public bool IgnoredEntitesOpen = false;
    public bool CustomIconsOpen = true;
    public bool DeliriumIconsOpen = true;
    public bool NPCIconsOpen = true;
    public bool MiscIconsOpen = true;
    public bool ChestIconsOpen = true;
    public bool DelveChestIconsOpen = false;
    public bool StrongboxIconsOpen = false;
    public bool RangeIconsOpen = true;

    //debug
    public bool Debug = false;
    public bool DebugMiscIcon = false;
    public bool DebugIngameIcon = false;
    public bool DebugChestIcon = false;
    public bool DebugNPCIcon = false;
    public bool DebugCustomIcon = false;

    //ingame icons
    public int AreaTransition_State = 1;
    public bool AreaTransition_DrawText = false;
    public int Breach_State = 2;
    public bool Breach_DrawText = false;
    public int Checkpoint_State = 1;
    public bool Checkpoint_DrawText = false;
    public int QuestObject_State = 1;
    public bool QuestObject_DrawText = false;
    public int IngameNPC_State = 1;
    public bool IngameNPC_DrawText = false;
    public int Ritual_State = 2;
    public bool Ritual_DrawText = false;
    public int Shrine_State = 2;
    public bool Shrine_DrawText = true;
    public int Waypoint_State = 1;
    public bool Waypoint_DrawText = false;
    public int IngameUncategorized_State = 1;
    public bool IngameUncategorized_DrawText = false;

    #region--| Custom Icons |-----------------------------------------------------------------------
               
        public List<CustomIconSettings> CustomIconSettingsList { get; set; } = new List<CustomIconSettings> { };

        public void InitCustomIconSettings() {
            if (true) return;
            if (CustomIconSettingsList.Count > 0) return;
            CustomIconSettingsList = new List<CustomIconSettings>{
                new CustomIconSettings {
                    Path = "custom/path/starts/with",
                    Setting_Draw = true,
                    Setting_DrawText = true,
                    Setting_Size = 32,
                    Setting_Index = 1,
                    Setting_Tint = new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                    Setting_HiddenTint = new Vector4(1.0f, 0.6862745f, 0.6862745f, 1.0f),
                    Setting_IsAlive = false,
                    Setting_IsOpened = false,
                },
            };
        }
        public void NewCustomIconSettings() {
        var defaultSetting = new CustomIconSettings {
            Path = "Metadata/CustomPath/",
            Setting_Draw = true,
            Setting_DrawText = false,
            Setting_Size = 32,
            Setting_Index = 0,
            Setting_Tint = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
            Setting_HiddenTint = new Vector4(0.5f, 0.5f, 0.5f, 1.0f),
            Setting_IsAlive = false,
            Setting_IsOpened = false,
        };
            CustomIconSettingsList.Add(defaultSetting);
        }
        public void RemoveCustomIconSettings(int index) {
            if (index >= 0 && index < CustomIconSettingsList.Count) {
                CustomIconSettingsList.RemoveAt(index);
            }
        }

    #endregion--------------------------------------------------------------------------------------

    #region --| Npc Settings |----------------------------------------------------------------------

        public bool WhiteMonster_Draw = true;
        public bool WhiteMonster_DrawText = false;
        public int WhiteMonster_Size = 32;
        public int WhiteMonster_Index = 0;
        public Vector4 WhiteMonster_Tint = new(1.0f, 0.0f, 0.0f, 1.0f);
        public Vector4 WhiteMonster_HiddenTint = new(1.0f, 0.7254902f, 0.7254902f, 1.0f);

        public bool MagicMonster_Draw = true;
        public bool MagicMonster_DrawText = false;
        public int MagicMonster_Size = 32;
        public int MagicMonster_Index = 1;
        public Vector4 MagicMonster_Tint = new(0.0f, 0.57254905f, 1.0f, 1.0f);
        public Vector4 MagicMonster_HiddenTint = new(0.7254902f, 0.88050747f, 1.0f, 1.0f);

        public bool RareMonster_Draw = true;
        public bool RareMonster_DrawText = false;
        public int RareMonster_Size = 32;
        public int RareMonster_Index = 2;
        public Vector4 RareMonster_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);
        public Vector4 RareMonster_HiddenTint = new(1.0f, 0.9515571f, 0.7254902f, 1.0f);

        public bool UniqueMonster_Draw = true;
        public bool UniqueMonster_DrawText = false;
        public int UniqueMonster_Size = 32;
        public int UniqueMonster_Index = 3;
        public Vector4 UniqueMonster_Tint = new(1.0f, 0.44155842f, 0.0f, 1.0f);
        public Vector4 UniqueMonster_HiddenTint = new(1.0f, 0.82652825f, 0.6862745f, 1.0f);        
        
        public bool Spirit_Draw = true;
        public bool Spirit_DrawText = false;
        public int Spirit_Size = 32;
        public int Spirit_Index = 3;
        public Vector4 Spirit_Tint = new(0.8311689f, 1.0f, 0.0f, 1.0f);
        public Vector4 Spirit_HiddenTint = new(0.9474408f, 1.0f, 0.6883117f, 1.0f);

        public bool FracturingMirror_Draw = true;
        public bool FracturingMirror_DrawText = false;
        public int FracturingMirror_Size = 32;
        public int FracturingMirror_Index = 129;
        public Vector4 FracturingMirror_Tint = new Vector4(0.0f, 0.9411764f, 1.0f, 1.0f);
        public Vector4 FracturingMirror_HiddenTint = new Vector4(0.0f, 0.9411765f, 1.0f, 1.0f);

        public bool Minion_Draw = true;
        public bool Minion_DrawText = false;
        public int Minion_Size = 32;
        public int Minion_Index = 0;
        public Vector4 Minion_Tint = new(0.0f, 1.0f, 0.0f, 1.0f);
        public Vector4 Minion_HiddenTint = new(0.6862745f, 1.0f, 0.6862745f, 1.0f);

        public bool NPC_Draw = true;
        public bool NPC_DrawText = false;
        public int NPC_Size = 32;
        public int NPC_Index = 161;
        public Vector4 NPC_Tint = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
        public Vector4 NPC_HiddenTint = new Vector4(0.6862745f, 1.0f, 0.6862745f, 1.0f);

        public bool VolatileCore_Draw = true;
        public bool VolatileCore_DrawText = false;
        public int VolatileCore_Size = 32;
        public int VolatileCore_Index = 50;
        public Vector4 VolatileCore_Tint = new Vector4(1.0f, 0.0f, 0.73362494f, 1.0f);
        public Vector4 VolatileCore_HiddenTint = new Vector4(1.0f, 0.6862745f, 0.9493108f, 1.0f);

    #endregion


    //Delirium Icons
    public bool BloodBag_Draw = true;
    public bool BloodBag_DrawText = false;
    public int BloodBag_Size = 32;
    public int BloodBag_Index = 161;
    public Vector4 BloodBag_Tint = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
    public Vector4 BloodBag_HiddenTint = new Vector4(1.0f, 0.6862745f, 0.6862745f, 1.0f);

    public bool EggFodder_Draw = true;
    public bool EggFodder_DrawText = false;
    public int EggFodder_Size = 32;
    public int EggFodder_Index = 161;
    public Vector4 EggFodder_Tint = new Vector4(0.0f, 0.9411765f, 1.0f, 1.0f);
    public Vector4 EggFodder_HiddenTint = new Vector4(1.0f, 0.6862745f, 0.6862745f, 1.0f);

    public bool GlobSpawn_Draw = true;
    public bool GlobSpawn_DrawText = false;
    public int GlobSpawn_Size = 32;
    public int GlobSpawn_Index = 161;
    public Vector4 GlobSpawn_Tint = new Vector4(0.0f, 0.9411765f, 1.0f, 1.0f);
    public Vector4 GlobSpawn_HiddenTint = new Vector4(1.0f, 0.6862745f, 0.6862745f, 1.0f);

    // misc icons
    public bool LocalPlayer_Draw = false;
    public bool LocalPlayer_DrawText = false;
    public int LocalPlayer_Size = 32;
    public int LocalPlayer_Index = 17;
    public Vector4 LocalPlayer_Tint = new(0.0f, 1.0f, 0.0f, 1.0f);
    public Vector4 LocalPlayer_HiddenTint = new(0.6862745f, 1.0f, 0.6862745f, 1.0f);

    public bool Player_Draw = true;
    public int Player_Size = 32;
    public int Player_Index = 17;
    public Vector4 Player_Tint = new(0.0f, 1.0f, 0.0f, 1.0f);
    public Vector4 Player_HiddenTint = new( 0.6862745f, 1.0f, 0.6862745f, 1.0f );
    public bool Player_DrawText = false;

    public bool SanctumMote_Draw = true;
    public bool SanctumMote_DrawText = false;
    public int SanctumMote_Size = 32;
    public int SanctumMote_Index = 0;
    public Vector4 SanctumMote_Tint = new(0.49783552f, 0.95434874f, 1.0f, 1.0f);
    public Vector4 SanctumMote_HiddenTint = new(0.83116883f, 0.98417217f, 1.0f, 1.0f);

    #region --| Chest Settings |--------------------------------------------------------------------

    public bool NormalChest_Draw = false;
        public bool NormalChest_DrawText = false;
        public int NormalChest_Size = 32;
        public int NormalChest_Index = 240;
        public Vector4 NormalChest_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        public bool MagicChest_Draw = false;
        public bool MagicChest_DrawText = false;
        public int MagicChest_Size = 32;
        public int MagicChest_Index = 241;
        public Vector4 MagicChest_Tint = new(0.0f, 0.57254905f, 1.0f, 1.0f);

        public bool RareChest_Draw = true;
        public bool RareChest_DrawText = false;
        public int RareChest_Size = 32;
        public int RareChest_Index = 241;
        public Vector4 RareChest_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        public bool UniqueChest_Draw = true;
        public bool UniqueChest_DrawText = false;
        public int UniqueChest_Size = 32;
        public int UniqueChest_Index = 241;
        public Vector4 UniqueChest_Tint = new(1.0f, 0.82652825f, 0.6862745f, 1.0f);

        //expedition chests
        public bool ExpeditionNormalChest_Draw = true;
        public bool ExpeditionNormalChest_DrawText = false;
        public int ExpeditionNormalChest_Size = 32;
        public int ExpeditionNormalChest_Index = 241;
        public Vector4 ExpeditionNormalChest_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        public bool ExpeditionMagicChest_Draw = true;
        public bool ExpeditionMagicChest_DrawText = false;
        public int ExpeditionMagicChest_Size = 32;
        public int ExpeditionMagicChest_Index = 241;
        public Vector4 ExpeditionMagicChest_Tint = new(0.0f, 0.57254905f, 1.0f, 1.0f);

        public bool ExpeditionRareChest_Draw = true;
        public bool ExpeditionRareChest_DrawText = false;
        public int ExpeditionRareChest_Size = 32;
        public int ExpeditionRareChest_Index = 241;
        public Vector4 ExpeditionRareChest_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // breach chests
        public bool BreachChest_Draw = true;
        public bool BreachChest_DrawText = false;
        public int BreachChest_Size = 32;
        public int BreachChest_Index = 129;
        public Vector4 BreachChest_Tint = new(0.67532444f, 0.0f, 1.0f, 1.0f);

        public bool BreachChestBoss_Draw = true;
        public bool BreachChestBoss_DrawText = false;
        public int BreachChestBoss_Size = 32;
        public int BreachChestBoss_Index = 130;
        public Vector4 BreachChestBoss_Tint = new(0.96862745f, 0.0f, 0.93088883f, 1.0f);

        // sanctum chests
        public bool SanctumChest_Draw = true;
        public bool SanctumChest_DrawText = false;
        public int SanctumChest_Size = 32;
        public int SanctumChest_Index = 241;
        public Vector4 SanctumChest_Tint = new(1.0f, 0.0f, 0.0f, 1.0f);

        // delve chests
        public bool DelveChestGeneric_Draw = true;
        public bool DelveChestGeneric_DrawText = false;
        public int DelveChestGeneric_Size = 32;
        public int DelveChestGeneric_Index = 241;
        public Vector4 DelveChestGeneric_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Azurite
        public bool DelveAzurite_Vein1_Draw = true; // Flawed Azurite Vein
        public bool DelveAzurite_Vein2_Draw = true; // Azurite Vein
        public bool DelveAzurite_Vein3_Draw = true; // Rich Azurite Vein
        public bool DelveAzurite_Vein4_Draw = true; // Pure Azurite Vein
        public bool DelveAzurite_Chest1_Draw = true; // Flawed Azurite Chest
        public bool DelveAzurite_Chest2_Draw = true; // Azurite Chest
        public bool DelveAzurite_Chest3_Draw = true; // Rich Azurite Chest
        public bool DelveAzurite_Chest4_Draw = true; // Pure Azurite Chest
        
        public bool DelveAzurite_DrawText = false;
        public int DelveAzurite_Size = 32;
        public int DelveAzurite_Index = 241;
        public Vector4 DelveAzurite_Tint = new(0.0f, 0.75f, 1.0f, 1.0f);

        // Resonators
        public bool DelveResonator_Stash_Draw = true;
        public bool DelveResonator_Cache_Draw = true;
        public bool DelveResonator_Trove_Draw = true;
        
        public bool DelveResonator_DrawText = false;
        public int DelveResonator_Size = 32;
        public int DelveResonator_Index = 241;
        public Vector4 DelveResonator_Tint = new(1.0f, 0.5f, 0.0f, 1.0f);

        public bool DelveResonator_Trove_DrawText = false;
        public int DelveResonator_Trove_Size = 32;
        public int DelveResonator_Trove_Index = 241;
        public Vector4 DelveResonator_Trove_Tint = new(1.0f, 0.5f, 0.0f, 1.0f);

        // Fossils
        public bool DelveChestFossil_Draw = true;
        public bool DelveChestFossil_DrawText = false;
        public int DelveChestFossil_Size = 32;
        public int DelveChestFossil_Index = 241;
        public Vector4 DelveChestFossil_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Main Path
        public bool DelvePath_Loot_Draw = true;
        public bool DelvePath_Armour_Draw = true;
        public bool DelvePath_Armaments_Draw = true;
        public bool DelvePath_Jewellery_Draw = true;
        public bool DelvePath_Wealth_Draw = true;
        
        // Side Paths
        public bool DelveOffPath_Loot_Draw = true;
        public bool DelveOffPath_Armour_Draw = true;
        public bool DelveOffPath_Armaments_Draw = true;
        public bool DelveOffPath_Jewellery_Draw = true;
        public bool DelveOffPath_Wealth_Draw = true;

        // Behind Fractured Walls
        public bool DelveDynamite_Loot_Draw = true;
        public bool DelveDynamite_Armour_Draw = true;
        public bool DelveDynamite_Armaments_Draw = true;
        public bool DelveDynamite_Jewellery_Draw = true;
        public bool DelveDynamite_Wealth_Draw = true;
        public bool DelveDynamite_Riches_Draw = true;
        public bool DelveDynamite_Opulence_Draw = true;

        public bool DelveDynamite_Opulence_DrawText = false;
        public int DelveDynamite_Opulence_Size = 32;
        public int DelveDynamite_Opulence_Index = 241;
        public Vector4 DelveDynamite_Opulence_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Unified path/offpath/dynamite settings
        public bool DelveRoute_DrawText = false;
        public int DelveRoute_Size = 32;
        public int DelveRoute_Index = 241;
        public Vector4 DelveRoute_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Supplies
        public bool DelveSupplies_Flare_Draw = true;
        public bool DelveSupplies_LargeFlare_Draw = true;
        public bool DelveSupplies_Dynamite_Draw = true;
        
        public bool DelveSupplies_DrawText = false;
        public int DelveSupplies_Size = 32;
        public int DelveSupplies_Index = 241;
        public Vector4 DelveSupplies_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Generic Chests
        public bool DelveGeneric_Sealed_Draw = true; // Sealed Lockbox
        public bool DelveGeneric_HiddenTreasures_Draw = true; // Hidden Treasures
        public bool DelveGeneric_Murmuring_Draw = true; // Murmuring Lockbox
        public bool DelveGeneric_Bloodstained_Draw = true; // Bloodstained Lockbox
        public bool DelveGeneric_Opulent_Draw = true; // Opulent Lockbox
        public bool DelveGeneric_Encrusted_Draw = true; // Encrusted Lockbox
        public bool DelveGeneric_Light_Draw = true; // Light Lockbox
        public bool DelveGeneric_Enchanted_Draw = true; // Enchanted Lockbox
        public bool DelveGeneric_Eldritch_Draw = true; // Eldritch Lockbox
        public bool DelveGeneric_Astral_Draw = true; // Astral Lockbox
        public bool DelveGeneric_Clean_Draw = true; // Clean Lockbox
        public bool DelveGeneric_Offering_Draw = true; // Offering to the Goddess
        public bool DelveGeneric_Treasured_Draw = true; // Treasured Lockbox
        public bool DelveGeneric_HiddenHeirlooms_Draw = true; // Hidden Heirlooms
        public bool DelveGeneric_Shadowstitch_Draw = true; // Shadowstitch

        // Armour Nodes
        public bool DelveArmour_Abandoned_Draw = true; // Abandoned Armour
        public bool DelveArmour_Twisted_Draw = true; // Twisted Armour
        public bool DelveArmour_Traveller_Draw = true; // Traveller's Armour
        public bool DelveArmour_Vital_Draw = true; // Vital Armour
        public bool DelveArmour_Pocked_Draw = true; // Pocked Armour
        public bool DelveArmour_Resistant_Draw = true; // Resistant Armour
        public bool DelveArmour_Unusual_Draw = true; // Unusual Armour
        public bool DelveArmour_Superior_Draw = true; // Superior Armour
        public bool DelveArmour_Astral_Draw = true; // Astral Armour
        public bool DelveArmour_Eldritch_Draw = true; // Eldritch Armour
        public bool DelveArmour_Triumphant_Draw = true; // Triumphant Armour
        public bool DelveArmour_Intricate_Draw = true; // Intricate Armour
        public bool DelveArmour_Flexible_Draw = true; // Flexible Armour
        public bool DelveArmour_Exceptional_Draw = true; // Exceptional Armour
        public bool DelveArmour_Shattered_Draw = true; // Shattered Armour
        public bool DelveArmour_Amalgamated_Draw = true; // Amalgamated Armour
        
        public bool DelveArmour_DrawText = false;
        public int DelveArmour_Size = 32;
        public int DelveArmour_Index = 241;
        public Vector4 DelveArmour_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Weapon Nodes
        public bool DelveWeapon_Abandoned_Draw = true; // Abandoned Armaments
        public bool DelveWeapon_Twisted_Draw = true; // Twisted Armaments
        public bool DelveWeapon_Heavy_Draw = true; // Heavy Armaments
        public bool DelveWeapon_Runed_Draw = true; // Runed Armaments
        public bool DelveWeapon_Light_Draw = true; // Light Armaments
        public bool DelveWeapon_Superior_Draw = true; // Superior Armaments
        public bool DelveWeapon_Astral_Draw = true; // Astral Armaments
        public bool DelveWeapon_Eldritch_Draw = true; // Eldritch Armaments
        public bool DelveWeapon_Triumphant_Draw = true; // Triumphant Armaments
        public bool DelveWeapon_Honed_Draw = true; // Honed Armaments
        public bool DelveWeapon_Hexed_Draw = true; // Hexed Armaments
        public bool DelveWeapon_Exceptional_Draw = true; // Exceptional Armaments
        public bool DelveWeapon_Masterwork_Draw = true; // Masterwork Armaments
        public bool DelveWeapon_Shattered_Draw = true; // Shattered Armaments
        public bool DelveWeapon_Amalgamated_Draw = true; // Amalgamated Armaments

        public bool DelveWeapon_DrawText = false;
        public int DelveWeapon_Size = 32;
        public int DelveWeapon_Index = 241;
        public Vector4 DelveWeapon_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Jewellery Nodes
        public bool DelveJewelry_AbandonedJewellery_Draw = true;
        public bool DelveJewelry_AbandonedAmulets_Draw = true;
        public bool DelveJewelry_AbandonedRings_Draw = true;
        public bool DelveJewelry_AbandonedJewels_Draw = true;
        public bool DelveJewelry_TwistedJewellery_Draw = true;
        public bool DelveJewelry_Flexible_Draw = true;
        public bool DelveJewelry_Light_Draw = true;
        public bool DelveJewelry_Astral_Draw = true;
        public bool DelveJewelry_Eldritch_Draw = true;
        public bool DelveJewelry_Triumphant_Draw = true;
        public bool DelveJewelry_Unusual_Draw = true;
        public bool DelveJewelry_Ritualistic_Draw = true;
        public bool DelveJewelry_Superior_Draw = true;
        public bool DelveJewelry_Shattered_Draw = true;
        public bool DelveJewelry_Amalgamated_Draw = true;
        public bool DelveJewelry_TwistedJewels_Draw = true;
        public bool DelveJewelry_Compacted_Draw = true;

        public bool DelveJewelry_DrawText = false;
        public int DelveJewelry_Size = 32;
        public int DelveJewelry_Index = 241;
        public Vector4 DelveJewelry_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Gem Nodes
        public bool DelveGem_Abandoned_Draw = true;
        public bool DelveGem_Gemcutters_Draw = true;
        public bool DelveGem_Superior_Draw = true;
        public bool DelveGem_Ancient_Draw = true;
        public bool DelveGem_Inscribed_Draw = true;
        public bool DelveGem_Exceptional_Draw = true;
        
        public bool DelveGem_DrawText = false;
        public int DelveGem_Size = 32;
        public int DelveGem_Index = 241;
        public Vector4 DelveGem_Tint = new(0.0f, 1.0f, 0.5f, 1.0f);

        // Currency Nodes
        public bool DelveCurrency_AbandonedRiches_Draw = true;
        public bool DelveCurrency_Pocked_Draw = true;
        public bool DelveCurrency_Bloodstained_Draw = true;
        public bool DelveCurrency_Light_Draw = true;
        public bool DelveCurrency_Glimmering_Draw = true;
        public bool DelveCurrency_Packed_Draw = true;
        public bool DelveCurrency_Cartographers_Draw = true;
        public bool DelveCurrency_Shattered_Draw = true;
        public bool DelveCurrency_AbandonedFortune_Draw = true;
        
        public bool DelveCurrency_DrawText = false;
        public int DelveCurrency_Size = 32;
        public int DelveCurrency_Index = 241;
        public Vector4 DelveCurrency_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Map Nodes
        public bool DelveMap_Abandoned_Draw = true;
        public bool DelveMap_Cartographers_Draw = true;
        public bool DelveMap_Bloodstained_Draw = true;
        public bool DelveMap_Strange_Draw = true;
        public bool DelveMap_Explorers_Draw = true;
        public bool DelveMap_Beautiful_Draw = true;
        public bool DelveMap_Puzzling_Draw = true;
        public bool DelveMap_Maddening_Draw = true;
        public bool DelveMap_Remembered_Draw = true;
        
        public bool DelveMap_DrawText = false;
        public int DelveMap_Size = 32;
        public int DelveMap_Index = 241;
        public Vector4 DelveMap_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Special Nodes
        public bool DelveSpecial_GrimArmour_Draw = true;
        public bool DelveSpecial_GrimJewellery_Draw = true;
        public bool DelveSpecial_GrimLoot_Draw = true;
        public bool DelveSpecial_GrimTreasures_Draw = true;
        public bool DelveSpecial_HeavyArmour_Draw = true;
        public bool DelveSpecial_HeavyJewellery_Draw = true;
        public bool DelveSpecial_HeavyLoot_Draw = true;
        public bool DelveSpecial_HeavyTreasures_Draw = true;
        public bool DelveSpecial_SmoulderingArmour_Draw = true;
        public bool DelveSpecial_SmoulderingArmaments_Draw = true;
        public bool DelveSpecial_SmoulderingJewellery_Draw = true;
        public bool DelveSpecial_SmoulderingLoot_Draw = true;
        public bool DelveSpecial_SmoulderingTreasures_Draw = true;
        public bool DelveSpecial_FrostyArmour_Draw = true;
        public bool DelveSpecial_FrostyArmaments_Draw = true;
        public bool DelveSpecial_FrostyJewellery_Draw = true;
        public bool DelveSpecial_FrostyLoot_Draw = true;
        public bool DelveSpecial_FrostyTreasures_Draw = true;
        public bool DelveSpecial_ChargedArmour_Draw = true;
        public bool DelveSpecial_ChargedArmaments_Draw = true;
        public bool DelveSpecial_ChargedJewellery_Draw = true;
        public bool DelveSpecial_ChargedLoot_Draw = true;
        public bool DelveSpecial_ChargedTreasures_Draw = true;
        public bool DelveSpecial_MalignantArmour_Draw = true;
        public bool DelveSpecial_MalignantArmaments_Draw = true;
        public bool DelveSpecial_MalignantJewellery_Draw = true;
        public bool DelveSpecial_MalignantLoot_Draw = true;
        public bool DelveSpecial_MalignantTreasures_Draw = true;
        public bool DelveSpecial_FeralTalismans_Draw = true;
        public bool DelveSpecial_FeralArmour_Draw = true;
        public bool DelveSpecial_FeralJewellery_Draw = true;
        public bool DelveSpecial_WhisperingCoffer_Draw = true;
        public bool DelveSpecial_InspiringArmour_Draw = true;
        public bool DelveSpecial_InspiringJewellery_Draw = true;
        public bool DelveSpecial_InspiringLoot_Draw = true;
        public bool DelveSpecial_InspiringTreasures_Draw = true;
        public bool DelveSpecial_GazingJewellery_Draw = true;
        public bool DelveSpecial_PrehistoricRiches_Draw = true;
        
        public bool DelveSpecial_DrawText = false;
        public int DelveSpecial_Size = 32;
        public int DelveSpecial_Index = 241;
        public Vector4 DelveSpecial_Tint = new(1.0f, 0.0f, 1.0f, 1.0f);

        // City Biomes
        public bool DelveCity_BloodstainedLoot_Draw = true;
        public bool DelveCity_HandcraftedAntiques_Draw = true;
        public bool DelveCity_MythicalCrate_Draw = true;
        public bool DelveCity_DisturbingLoot_Draw = true;
        public bool DelveCity_RattlingCoffer_Draw = true;
        public bool DelveCity_AbyssalJewellery_Draw = true;
        public bool DelveCity_AbyssalTreasures_Draw = true;
        public bool DelveCity_PrehistoricOre_Draw = true;
        public bool DelveCity_PrehistoricFossils_Draw = true;
        public bool DelveCity_PrehistoricResonators_Draw = true;
        public bool DelveCity_PrehistoricRings_Draw = true;
        public bool DelveCity_PrehistoricJewels_Draw = true;
        public bool DelveCity_ConcealedOpulence_Draw = true;
        public bool DelveCity_ConcealedOddities_Draw = true;
        public bool DelveCity_ConcealedAdornments_Draw = true;
        
        public bool DelveCity_DrawText = false;
        public int DelveCity_Size = 32;
        public int DelveCity_Index = 241;
        public Vector4 DelveCity_Tint = new(0.5f, 0.0f, 1.0f, 1.0f);

    #endregion

    #region --| Strongbox Settings |--------------------------------------------------------------

        public bool UnknownStrongbox_Draw = true;
        public bool UnknownStrongbox_DrawText = false;   
        public int UnknownStrongbox_Size = 32;
        public int UnknownStrongbox_Index = 241;
        public Vector4 UnknownStrongbox_Tint = new(1.0f, 0.0f, 0.80519485f, 1.0f);

        public bool ArcanistStrongbox_Draw = true;
        public bool ArcanistStrongbox_DrawText = false;
        public int ArcanistStrongbox_Size = 32;
        public int ArcanistStrongbox_Index = 241;
        public Vector4 ArcanistStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool ArmourerStrongbox_Draw = true;
        public bool ArmourerStrongbox_DrawText = false;
        public int ArmourerStrongbox_Size = 32;
        public int ArmourerStrongbox_Index = 241;
        public Vector4 ArmourerStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool BlacksmithStrongbox_Draw = true;
        public bool BlacksmithStrongbox_DrawText = false;
        public int BlacksmithStrongbox_Size = 32;
        public int BlacksmithStrongbox_Index = 241;
        public Vector4 BlacksmithStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool ArtisanStrongbox_Draw = true;
        public bool ArtisanStrongbox_DrawText = false;
        public int ArtisanStrongbox_Size = 32;
        public int ArtisanStrongbox_Index = 241;
        public Vector4 ArtisanStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool CartographerStrongbox_Draw = true;
        public bool CartographerStrongbox_DrawText = false;
        public int CartographerStrongbox_Size = 32;
        public int CartographerStrongbox_Index = 241;
        public Vector4 CartographerStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool ChemistStrongbox_Draw = true;
        public bool ChemistStrongbox_DrawText = false;
        public int ChemistStrongbox_Size = 32;
        public int ChemistStrongbox_Index = 241;
        public Vector4 ChemistStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool GemcutterStrongbox_Draw = true; 
        public bool GemcutterStrongbox_DrawText = false;
        public int GemcutterStrongbox_Size = 32;
        public int GemcutterStrongbox_Index = 241;
        public Vector4 GemcutterStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool JewellerStrongbox_Draw = true;
        public bool JewellerStrongbox_DrawText = false;
        public int JewellerStrongbox_Size = 32;
        public int JewellerStrongbox_Index = 241;
        public Vector4 JewellerStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool LargeStrongbox_Draw = true;
        public bool LargeStrongbox_DrawText = false;
        public int LargeStrongbox_Size = 32;
        public int LargeStrongbox_Index = 241;
        public Vector4 LargeStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool OrnateStrongbox_Draw = true;
        public bool OrnateStrongbox_DrawText = false;
        public int OrnateStrongbox_Size = 32;
        public int OrnateStrongbox_Index = 241;
        public Vector4 OrnateStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool Strongbox_Draw = true;
        public bool Strongbox_DrawText = false;
        public int Strongbox_Size = 32;
        public int Strongbox_Index = 241;
        public Vector4 Strongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool DivinerStrongbox_Draw = true;
        public bool DivinerStrongbox_DrawText = false;
        public int DivinerStrongbox_Size = 32;
        public int DivinerStrongbox_Index = 241;
        public Vector4 DivinerStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool OperativeStrongbox_Draw = true;
        public bool OperativeStrongbox_DrawText = false;
        public int OperativeStrongbox_Size = 32;
        public int OperativeStrongbox_Index = 241;
        public Vector4 OperativeStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool ArcaneStrongbox_Draw = true;
        public bool ArcaneStrongbox_DrawText = false;
        public int ArcaneStrongbox_Size = 32;
        public int ArcaneStrongbox_Index = 241;
        public Vector4 ArcaneStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

        public bool ResearcherStrongbox_Draw = true;
        public bool ResearcherStrongbox_DrawText = false;
        public int ResearcherStrongbox_Size = 32;
        public int ResearcherStrongbox_Index = 241;
        public Vector4 ResearcherStrongbox_Tint = new(1.0f, 0.4705882f, 0.0f, 1.0f);

    #endregion
    
    //ignored entities
    public string ignoredEntities =
    @"# Random Ignores
Metadata/Monsters/InvisibleFire/InvisibleSandstorm_
Metadata/Monsters/InvisibleFire/InvisibleFrostnado
Metadata/Monsters/InvisibleFire/InvisibleFireAfflictionDemonColdDegen
Metadata/Monsters/InvisibleFire/InvisibleFireAfflictionDemonColdDegenUnique
Metadata/Monsters/InvisibleFire/InvisibleFireAfflictionCorpseDegen
Metadata/Monsters/InvisibleFire/InvisibleFireEyrieHurricane
Metadata/Monsters/InvisibleFire/InvisibleIonCannonFrost
Metadata/Monsters/InvisibleFire/AfflictionBossFinalDeathZone
Metadata/Monsters/InvisibleFire/InvisibleFireDoedreSewers
Metadata/Monsters/InvisibleFire/InvisibleFireDelveFlameTornadoSpiked
Metadata/Monsters/InvisibleFire/InvisibleHolyCannon
Metadata/Monsters/InvisibleCurse/InvisibleFrostbiteStationary
Metadata/Monsters/InvisibleCurse/InvisibleConductivityStationary
Metadata/Monsters/InvisibleCurse/InvisibleEnfeeble
Metadata/Monsters/InvisibleAura/InvisibleWrathStationary

# Metadata/Monsters/Labyrinth/GoddessOfJustice
# Metadata/Monsters/Labyrinth/GoddessOfJusticeMapBoss
Metadata/Monsters/Frog/FrogGod/SilverOrb
Metadata/Monsters/Frog/FrogGod/SilverPool
Metadata/Monsters/LunarisSolaris/SolarisCelestialFormAmbushUniqueMap
Metadata/Monsters/Invisible/MaligaroSoulInvisibleBladeVortex
Metadata/Monsters/Daemon
Metadata/Monsters/Daemon/MaligaroBladeVortexDaemon
Metadata/Monsters/Daemon/SilverPoolChillDaemon
Metadata/Monsters/AvariusCasticus/AvariusCasticusStatue
Metadata/Monsters/Maligaro/MaligaroDesecrate

# Ritual
Metadata/Monsters/LeagueRitual/FireMeteorDaemon
Metadata/Monsters/LeagueRitual/GenericSpeedDaemon
Metadata/Monsters/LeagueRitual/ColdRotatingBeamDaemon
Metadata/Monsters/LeagueRitual/ColdRotatingBeamDaemonUber
Metadata/Monsters/LeagueRitual/GenericEnergyShieldDaemon
Metadata/Monsters/LeagueRitual/GenericMassiveDaemon
Metadata/Monsters/LeagueRitual/ChaosGreenVinesDaemon_
Metadata/Monsters/LeagueRitual/ChaosSoulrendPortalDaemon
Metadata/Monsters/LeagueRitual/VaalAtziriDaemon
Metadata/Monsters/LeagueRitual/LightningPylonDaemon";

}