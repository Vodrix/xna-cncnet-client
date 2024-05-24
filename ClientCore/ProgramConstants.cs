using System.Windows.Forms;

namespace ClientCore
{
    /// <summary>
    /// Contains various static variables and constants that the client uses for operation.
    /// </summary>
    public static class ProgramConstants
    {
#if DEBUG
        public static readonly string GamePath = Application.StartupPath.Replace('\\', '/') + "/";
#else
        public static readonly string GamePath = Directory.GetParent(Application.StartupPath.TrimEnd(new char[] { '\\' })).FullName.Replace('\\', '/') + "/";
#endif

        public static string ClientUserFilesPath => GamePath + "Client/";

        public const string QRES_EXECUTABLE = "qres.dat";

        public const string SPAWNMAP_INI = "spawnmap.ini";
        public const string SPAWNER_SETTINGS = "spawn.ini";
        public const string SAVED_GAME_SPAWN_INI = "Saved Games/spawnSG.ini";

        public static string GAME_VERSION = "Undefined";

        public const int GAME_ID_MAX_LENGTH = 4;

        public static string BASE_RESOURCE_PATH = "Resources/";
        public static string RESOURCES_DIR = BASE_RESOURCE_PATH;    //"Resources/" + themePath

        public static bool IsInGame { get; set; }

        public static string GetResourcePath()
        {
            return GamePath + RESOURCES_DIR;
        }

        public static string GetBaseResourcePath()
        {
            return GamePath + BASE_RESOURCE_PATH;
        }
    }
}
