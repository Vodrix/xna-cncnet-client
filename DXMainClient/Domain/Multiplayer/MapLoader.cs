using System.Collections.Generic;

namespace DTAClient.Domain.Multiplayer
{
    public class MapLoader
    {
        public const string MAP_FILE_EXTENSION = ".map";

        /// <summary>
        /// List of game modes.
        /// </summary>
        public List<GameMode> GameModes = new List<GameMode>();
    }
}
