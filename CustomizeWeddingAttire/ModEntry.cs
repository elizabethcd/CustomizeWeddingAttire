using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using HarmonyLib;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace CustomizeWeddingAttire
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        // Add a config
        private ModConfig Config;
        // Add a translator
        private ITranslationHelper I18n;

        // Config option strings defined here as constants for use elsewhere
        public const string tuxOption = "weddingAttire.tuxOption";
        public const string dressOption = "weddingAttire.dressOption";
        public const string noneOption = "weddingAttire.noneOption";
        public const string defaultOption = "weddingAttire.defaultOption";

        // TODO investigate Fashion Sense compatibility
        // TODO make sure translations work in GMCM

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            // Read in config file and create if needed
            this.Config = this.Helper.ReadConfig<ModConfig>();

            // Initialize the i18n helper
            this.I18n = this.Helper.Translation;

            // Initialize useful things from this class in WeddingPatcher
            WeddingPatcher.Initialize(this.Monitor, this.Config, this.I18n, this.ModManifest);

            // Apply the Harmony patches
            var harmony = new Harmony(this.ModManifest.UniqueID);
            WeddingPatcher.Apply(harmony);

            // Set up GMCM config when game is launched
            helper.Events.GameLoop.GameLaunched += SetUpConfig;
        }

        private void SetUpConfig(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: ModManifest,
                reset: () => Config = new ModConfig(),
                save: () => {
                    Helper.WriteConfig(Config);
                    // Refresh the modData recording wedding attire preferences for this player
                    Game1.player.modData[$"{this.ModManifest.UniqueID}/weddingAttirePref"] = this.Config.WeddingAttire;
                }
            );
            configMenu.AddParagraph(
                mod: ModManifest,
                text: () => Helper.Translation.Get("mod.description")
                );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("weddingAttire.title"),
                tooltip: () => this.Helper.Translation.Get("weddingAttire.description"),
                getValue: () => this.Config.WeddingAttire,
                setValue: value => this.Config.WeddingAttire = value,
                allowedValues: new string[] {
                    tuxOption,
                    dressOption,
                    noneOption,
                    defaultOption
                },
                formatAllowedValue: (str) => this.Helper.Translation.Get(str)
            );
        }
    }
}