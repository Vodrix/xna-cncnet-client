﻿using Microsoft.Xna.Framework.Graphics;

namespace ClientCore.CnCNet5
{
    /// <summary>
    /// A class for games supported on CnCNet (DTA, TI, TS, RA1/2, etc.)
    /// </summary>
    public class CnCNetGame
    {
        /// <summary>
        /// The name of the game that is displayed on the user-interface.
        /// </summary>
        public string UIName { get; set; }

        /// <summary>
        /// The internal name (suffix) of the game.
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// The executable name of the game's client.
        /// </summary>
        public string ClientExecutableName { get; set; }

        public Texture2D Texture { get; set; }

        /// <summary>
        /// The location where to read the game's installation path from the registry.
        /// </summary>
        public string RegistryInstallPath { get; set; }
    }
}
