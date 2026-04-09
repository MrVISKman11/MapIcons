
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

        // Delve Chests Generic
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
        public bool DelveFossil_Aberrant_Draw = true;
        public bool DelveFossil_Aetheric_Draw = true;
        public bool DelveFossil_Bloodstained_Draw = true;
        public bool DelveFossil_Bound_Draw = true;
        public bool DelveFossil_Corroded_Draw = true;
        public bool DelveFossil_Dense_Draw = true;
        public bool DelveFossil_Enchanted_Draw = true;
        public bool DelveFossil_Encrusted_Draw = true;
        public bool DelveFossil_Faceted_Draw = true;
        public bool DelveFossil_Fractured_Draw = true;
        public bool DelveFossil_Frigid_Draw = true;
        public bool DelveFossil_Gilded_Draw = true;
        public bool DelveFossil_Glyphic_Draw = true;
        public bool DelveFossil_Hollow_Draw = true;
        public bool DelveFossil_Jagged_Draw = true;
        public bool DelveFossil_Lucent_Draw = true;
        public bool DelveFossil_Metallic_Draw = true;
        public bool DelveFossil_Perfect_Draw = true;
        public bool DelveFossil_Prismatic_Draw = true;
        public bool DelveFossil_Pristine_Draw = true;
        public bool DelveFossil_Sanctified_Draw = true;
        public bool DelveFossil_Scorched_Draw = true;
        public bool DelveFossil_Serrated_Draw = true;
        public bool DelveFossil_Shuddering_Draw = true;
        public bool DelveFossil_Tangled_Draw = true;
        public bool DelveFossil_Generic_Draw = true;

        public bool DelveChestFossil_DrawText = false;
        public int DelveChestFossil_Size = 32;
        public int DelveChestFossil_Index = 241;
        public Vector4 DelveChestFossil_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Unified Route/Dynamite Settings (Opulence is kept separate)
        public bool DelveDynamite_Opulence_Draw = true;
        public bool DelveDynamite_Opulence_DrawText = false;
        public int DelveDynamite_Opulence_Size = 32;
        public int DelveDynamite_Opulence_Index = 241;
        public Vector4 DelveDynamite_Opulence_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Supplies
        public bool DelveSupplies_Flare_Draw = true;
        public bool DelveSupplies_LargeFlare_Draw = true;
        public bool DelveSupplies_Dynamite_Draw = true;
        
        public bool DelveSupplies_DrawText = false;
        public int DelveSupplies_Size = 32;
        public int DelveSupplies_Index = 241;
        public Vector4 DelveSupplies_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Unified Categories (Suffixes)
        // Group: Equipment
        public bool Delve_Armour_Draw = true;
        public bool Delve_Armaments_Draw = true;
        public bool Delve_Jewellery_Draw = true;
        public bool Delve_Amulets_Draw = true;
        public bool Delve_Rings_Draw = true;
        public bool Delve_Jewels_Draw = true;
        public bool Delve_Talismans_Draw = true;

        public bool DelveEquipment_DrawText = false;
        public int DelveEquipment_Size = 32;
        public int DelveEquipment_Index = 241;
        public Vector4 DelveEquipment_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Group: Gems
        public bool Delve_Gems_Draw = true;

        public bool DelveGem_DrawText = false;
        public int DelveGem_Size = 32;
        public int DelveGem_Index = 241;
        public Vector4 DelveGem_Tint = new(0.0f, 1.0f, 0.5f, 1.0f);

        // Group: Wealth/Currency
        public bool Delve_Loot_Draw = true;
        public bool Delve_Treasures_Draw = true;
        public bool Delve_Wealth_Draw = true;
        public bool Delve_Riches_Draw = true;
        public bool Delve_Fortune_Draw = true;

        public bool DelveWealth_DrawText = false;
        public int DelveWealth_Size = 32;
        public int DelveWealth_Index = 241;
        public Vector4 DelveWealth_Tint = new(1.0f, 0.8235294f, 0.0f, 1.0f);

        // Group: Map/Charts
        public bool Delve_Charts_Draw = true;

        public bool DelveMap_DrawText = false;
        public int DelveMap_Size = 32;
        public int DelveMap_Index = 241;
        public Vector4 DelveMap_Tint = new(1.0f, 1.0f, 1.0f, 1.0f);

        // Group: Containers (Coffer, Lockbox, Crate, etc)
        public bool Delve_Lockbox_Draw = true;
        public bool Delve_Coffer_Draw = true;
        public bool Delve_Crate_Draw = true;
        public bool Delve_Antiques_Draw = true;
        public bool Delve_Oddities_Draw = true;
        public bool Delve_Adornments_Draw = true;
        public bool Delve_Heirlooms_Draw = true;
        public bool Delve_Shadowstitch_Draw = true;
        public bool Delve_Offering_Draw = true;
        public bool Delve_Ore_Draw = true;

        public bool DelveContainers_DrawText = false;
        public int DelveContainers_Size = 32;
        public int DelveContainers_Index = 241;
        public Vector4 DelveContainers_Tint = new(0.5f, 0.0f, 1.0f, 1.0f);

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