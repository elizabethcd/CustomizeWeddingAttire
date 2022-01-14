using System;
using Microsoft.Xna.Framework;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace CustomizeWeddingAttire
{
    // Applies Harmony patches to Utility.cs to add a conversation topic for weddings.
    public class WeddingPatcher
    {
        private static IMonitor Monitor;
        private static ModConfig Config;
        private static ITranslationHelper I18n;

        // call this method from your Entry class
        public static void Initialize(IMonitor monitor, ModConfig config, ITranslationHelper translator)
        {
            Monitor = monitor;
            Config = config;
            I18n = translator;
        }

        // Method to apply harmony patch
        public static void Apply(Harmony harmony)
        {
            try
            {
                harmony.Patch(
                    original: AccessTools.Method(typeof(Event), "addSpecificTemporarySprite"),
                    prefix: new HarmonyMethod(typeof(WeddingPatcher), nameof(WeddingPatcher.Event_addSpecificTemporarySprite_Prefix))
                );
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to add prefix to wedding sprite function with exception: {ex}", LogLevel.Error);
            }
        }

        // Method that is used to prefix
        private static bool Event_addSpecificTemporarySprite_Prefix(string key, GameLocation location, Event __instance, ref int ___oldShirt, ref Color ___oldPants)
        {
            // TODO add try/catch statements everywhere

            // If this is not a temporary sprite for a wedding, skip this prefix entirely
            if (key != "wedding")
            {
                return true;
            }

            // Put the player in a tux if desired
            if (Config.WeddingAttire == "weddingAttire.tuxOption")
            {
                ___oldShirt = __instance.farmer.shirt;
                __instance.farmer.changeShirt(10);
                ___oldPants = __instance.farmer.pantsColor;
                __instance.farmer.changePantStyle(0);
                __instance.farmer.changePants(new Color(49, 49, 49));
            }
            // Put the player in a dress if desired (TBD, need to find indices)

            // TODO fix this area to do the right thing based on other player's preferences
            // Defaulting to no change if they don't have this mod
            foreach (Farmer farmerActor in __instance.farmerActors)
            {
                // TODO check their preferences using
                // Check for no change in clothing:
                // farmerActor.modData.tryGetData($"{this.ModManifest.UniqueID}/weddingAttirePref", "weddingAttire.noneOption");
                // Check for tux behavior
                // farmerActor.modData.tryGetData($"{this.ModManifest.UniqueID}/weddingAttirePref", "weddingAttire.tuxOption");
                farmerActor.changeShirt(10);
                farmerActor.changePants(new Color(49, 49, 49));
                farmerActor.changePantStyle(0);
                // Check for dress behavior
                // farmerActor.modData.tryGetData($"{this.ModManifest.UniqueID}/weddingAttirePref", "weddingAttire.dressOption");
            }

            // Do the sprite adding that needs to be done if the function is skipped
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(540, 1196, 98, 54), 99999f, 1, 99999, new Vector2(25f, 60f) * 64f + new Vector2(0f, -64f), flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(540, 1250, 98, 25), 99999f, 1, 99999, new Vector2(25f, 60f) * 64f + new Vector2(0f, 54f) * 4f + new Vector2(0f, -64f), flicker: false, flipped: false, 0f, 0f, Color.White, 4f, 0f, 0f, 0f));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 62f) * 64f, flicker: false, flipped: false, 0f, 0f, Color.White, 4f, 0f, 0f, 0f));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 62f) * 64f, flicker: false, flipped: false, 0f, 0f, Color.White, 4f, 0f, 0f, 0f));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(24f, 69f) * 64f, flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f));
            location.TemporarySprites.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Microsoft.Xna.Framework.Rectangle(527, 1249, 12, 25), 99999f, 1, 99999, new Vector2(32f, 69f) * 64f, flicker: false, flipped: false, 1f, 0f, Color.White, 4f, 0f, 0f, 0f));
            return false;
        }
    }

}