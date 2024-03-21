﻿using System;
namespace CustomizeWeddingAttire
{
    public class ModConfig
    {
        // Add an config for what kind of attire to use during the wedding scene
        // Options are: None, Tux, Dress, Default, Custom
        public string WeddingAttire { get; set; }
        public string CustomShirt { get; set; }
        public string CustomPants { get; set; }
        public int PantsR { get; set; }
        public int PantsG { get; set; }
        public int PantsB { get; set; }

        public ModConfig()
        {
            // By default, make no changes
            this.WeddingAttire = ModEntry.noneOption;
            this.CustomShirt = "1087";
            this.CustomPants = "8";
            this.PantsR = 100;
            this.PantsG = 50;
            this.PantsB = 255;
        }
    }
}