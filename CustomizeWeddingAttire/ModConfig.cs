using System;
namespace CustomizeWeddingAttire
{
    public class ModConfig
    {
        // Add an config for what kind of attire to use during the wedding scene
        // Options are: None, Tux, Dress, Default
        public string WeddingAttire { get; set; }
        public string OtherPlayersWeddingAttire { get; set; }

        public ModConfig()
        {
            // By default, make no changes
            this.WeddingAttire = "weddingAttire.noneOption";
            this.OtherPlayersWeddingAttire = "weddingAttire.noneOption";
        }
    }
}