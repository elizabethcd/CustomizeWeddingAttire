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

            // Initialize the error logger in FurniturePatcher
            WeddingPatcher.Initialize(this.Monitor, this.Config, this.I18n);

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
                    // TODO add Game1.player.modData[$"{this.ModManifest.UniqueID}/weddingAttirePref"] = this.Config.weddingAttire;
                }
            );
            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("weddingAttire.title"),
                tooltip: () => this.Helper.Translation.Get("weddingAttire.description"),
                getValue: () => this.Config.WeddingAttire,
                setValue: value => this.Config.WeddingAttire = value,
                allowedValues: new string[] {
                    "weddingAttire.tuxOption",
                    "weddingAttire.dressOption",
                    "weddingAttire.noneOption"
                },
                formatAllowedValue: (str) => this.Helper.Translation.Get(str)
            );
        }
    }
}