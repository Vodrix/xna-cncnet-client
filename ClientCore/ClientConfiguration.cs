using Rampastring.Tools;
using System;
using System.IO;

namespace ClientCore
{
    public class ClientConfiguration
    {
        private const string GENERAL = "General";
        private const string SETTINGS = "Settings";

        private const string CLIENT_SETTINGS = "DTACnCNetClient.ini";
        private const string GAME_OPTIONS = "GameOptions.ini";
        private const string CLIENT_DEFS = "ClientDefinitions.ini";

        private static ClientConfiguration _instance;

        private readonly IniFile gameOptions_ini;
        private IniFile DTACnCNetClient_ini;
        private readonly IniFile clientDefinitionsIni;

        protected ClientConfiguration()
        {
            if (!File.Exists(ProgramConstants.GetBaseResourcePath() + CLIENT_DEFS))
                throw new FileNotFoundException("Couldn't find " + CLIENT_DEFS + ". Please verify that you're running the client from the correct directory.");

            clientDefinitionsIni = new IniFile(ProgramConstants.GetBaseResourcePath() + CLIENT_DEFS);

            DTACnCNetClient_ini = new IniFile(ProgramConstants.GetResourcePath() + CLIENT_SETTINGS);

            gameOptions_ini = new IniFile(ProgramConstants.GetBaseResourcePath() + GAME_OPTIONS);
        }

        /// <summary>
        /// Singleton Pattern. Returns the object of this class.
        /// </summary>
        /// <returns>The object of the ClientConfiguration class.</returns>
        public static ClientConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClientConfiguration();
                }
                return _instance;
            }
        }

        public void RefreshSettings()
        {
            DTACnCNetClient_ini = new IniFile(ProgramConstants.GetResourcePath() + CLIENT_SETTINGS);
        }

        #region Client settings

        public string MainMenuMusicName => DTACnCNetClient_ini.GetStringValue(GENERAL, "MainMenuTheme", "mainmenu");

        public float DefaultAlphaRate => DTACnCNetClient_ini.GetSingleValue(GENERAL, "AlphaRate", 0.005f);

        public float CheckBoxAlphaRate => DTACnCNetClient_ini.GetSingleValue(GENERAL, "CheckBoxAlphaRate", 0.05f);

        #region Color settings

        public string UILabelColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "UILabelColor", "0,0,0");

        public string UIHintTextColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "HintTextColor", "128,128,128");

        public string DisabledButtonColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "DisabledButtonColor", "108,108,108");

        public string AltUIColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "AltUIColor", "255,255,255");

        public string ButtonHoverColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "ButtonHoverColor", "255,192,192");

        public string AltUIBackgroundColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "AltUIBackgroundColor", "196,196,196");

        public string PanelBorderColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "PanelBorderColor", "255,255,255");

        public string ListBoxHeaderColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "ListBoxHeaderColor", "255,255,255");

        public string ListBoxFocusColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "ListBoxFocusColor", "64,64,168");

<<<<<<< Updated upstream
        public string HoverOnGameColor => DTACnCNetClient_ini.GetStringValue(GENERAL, "HoverOnGameColor", "32,32,84");

        public IniSection GetParserConstants() => DTACnCNetClient_ini.GetSection("ParserConstants");

=======
>>>>>>> Stashed changes
        #endregion

        #region Tool tip settings

        public int ToolTipFontIndex => DTACnCNetClient_ini.GetIntValue(GENERAL, "ToolTipFontIndex", 0);

        public int ToolTipOffsetX => DTACnCNetClient_ini.GetIntValue(GENERAL, "ToolTipOffsetX", 0);

        public int ToolTipOffsetY => DTACnCNetClient_ini.GetIntValue(GENERAL, "ToolTipOffsetY", 0);

        public int ToolTipMargin => DTACnCNetClient_ini.GetIntValue(GENERAL, "ToolTipMargin", 4);

        public float ToolTipDelay => DTACnCNetClient_ini.GetSingleValue(GENERAL, "ToolTipDelay", 0.67f);

        public float ToolTipAlphaRatePerSecond => DTACnCNetClient_ini.GetSingleValue(GENERAL, "ToolTipAlphaRate", 4.0f);

        #endregion

        #region Audio options

        #endregion

        #region Game options

        public string Sides => gameOptions_ini.GetStringValue(GENERAL, nameof(Sides), "GDI,Nod,Allies,Soviet");

        public string InternalSideIndices => gameOptions_ini.GetStringValue(GENERAL, nameof(InternalSideIndices), string.Empty);

        public string SpectatorInternalSideIndex => gameOptions_ini.GetStringValue(GENERAL, nameof(SpectatorInternalSideIndex), string.Empty);

        #endregion

        #region Client definitions

        public string DiscordAppId => clientDefinitionsIni.GetStringValue(SETTINGS, "DiscordAppId", string.Empty);

        public int LoadingScreenCount => clientDefinitionsIni.GetIntValue(SETTINGS, "LoadingScreenCount", 2);

        public int ThemeCount => clientDefinitionsIni.GetSectionKeys("Themes").Count;

        public string LocalGame => clientDefinitionsIni.GetStringValue(SETTINGS, "LocalGame", "DTA");

        public bool SidebarHack => clientDefinitionsIni.GetBooleanValue(SETTINGS, "SidebarHack", false);

        public int MinimumRenderWidth => clientDefinitionsIni.GetIntValue(SETTINGS, "MinimumRenderWidth", 1280);

        public int MinimumRenderHeight => clientDefinitionsIni.GetIntValue(SETTINGS, "MinimumRenderHeight", 768);

        public int MaximumRenderWidth => clientDefinitionsIni.GetIntValue(SETTINGS, "MaximumRenderWidth", 1280);

        public int MaximumRenderHeight => clientDefinitionsIni.GetIntValue(SETTINGS, "MaximumRenderHeight", 800);

        public string[] RecommendedResolutions => clientDefinitionsIni.GetStringValue(SETTINGS, "RecommendedResolutions", "1280x720,2560x1440,3840x2160").Split(',');

        public string WindowTitle => clientDefinitionsIni.GetStringValue(SETTINGS, "WindowTitle", string.Empty);

        public string InstallationPathRegKey => clientDefinitionsIni.GetStringValue(SETTINGS, "RegistryInstallPath", "TiberianSun");

        public string CnCNetLiveStatusIdentifier => clientDefinitionsIni.GetStringValue(SETTINGS, "CnCNetLiveStatusIdentifier", "cncnet5_ts");

        public string BattleFSFileName2 => clientDefinitionsIni.GetStringValue(SETTINGS, "BattleName2", "Battle2.ini");

        public string BattleFSFileName3 => clientDefinitionsIni.GetStringValue(SETTINGS, "BattleName3", "Battle3.ini");

        public string Rulesmd => clientDefinitionsIni.GetPathStringValue(SETTINGS, "RulesPath", "Normal.ini");

        public string InsaneLocation => clientDefinitionsIni.GetPathStringValue(SETTINGS, "InsanePath", "Insane.ini");

        public string AlliedBrutalLocation => clientDefinitionsIni.GetPathStringValue(SETTINGS, "AlliedBrutalPath", "Allied.ini");

        public string SovietBrutalLocation => clientDefinitionsIni.GetPathStringValue(SETTINGS, "SovietBrutalPath", "Soviet.ini");

        public string YuriBrutalLocation => clientDefinitionsIni.GetPathStringValue(SETTINGS, "YuriBrutalPath", "Yuri.ini");


        public bool ModMode => clientDefinitionsIni.GetBooleanValue(SETTINGS, "ModMode", false);

        public string LongGameName => clientDefinitionsIni.GetStringValue(SETTINGS, "LongGameName", "Tiberian Sun");

        public string ShortSupportURL => clientDefinitionsIni.GetStringValue(SETTINGS, "ShortSupportURL", "www.moddb.com/members/rampastring");

        public string ChangelogURL => clientDefinitionsIni.GetStringValue(SETTINGS, "ChangelogURL", "http://www.moddb.com/mods/the-dawn-of-the-tiberium-age/tutorials/change-log");

        public string CreditsURL => clientDefinitionsIni.GetStringValue(SETTINGS, "CreditsURL", "http://www.moddb.com/mods/the-dawn-of-the-tiberium-age/tutorials/credits#Rampastring");

        public int MapCellSizeX => clientDefinitionsIni.GetIntValue(SETTINGS, "MapCellSizeX", 48);

        public int MapCellSizeY => clientDefinitionsIni.GetIntValue(SETTINGS, "MapCellSizeY", 24);

        public string[] GetThemeInfoFromIndex(int themeIndex) => clientDefinitionsIni.GetStringValue("Themes", themeIndex.ToString(), ",").Split(',');

        /// <summary>
        /// Returns the directory path for a theme, or null if the specified
        /// theme name doesn't exist.
        /// </summary>
        /// <param name="themeName">The name of the theme.</param>
        /// <returns>The path to the theme's directory.</returns>
        public string GetThemePath(string themeName)
        {
            var themeSection = clientDefinitionsIni.GetSection("Themes");
            foreach (var key in themeSection.Keys)
            {
                string[] parts = key.Value.Split(',');
                if (parts[0] == themeName)
                    return parts[1];
            }

            return null;
        }

        public string SettingsIniName => clientDefinitionsIni.GetStringValue(SETTINGS, "SettingsFile", "Settings.ini");

        public string TranslationIniName => clientDefinitionsIni.GetStringValue(SETTINGS, "TranslationFile", "Resources/Translation.ini");

        public bool GenerateTranslationStub => clientDefinitionsIni.GetBooleanValue(SETTINGS, "GenerateTranslationStub", false);

        public string ExtraExeCommandLineParameters => clientDefinitionsIni.GetStringValue(SETTINGS, "ExtraCommandLineParams", string.Empty);

        public string MPMapsIniPath => clientDefinitionsIni.GetStringValue(SETTINGS, "MPMapsPath", "");

        public string KeyboardINI => clientDefinitionsIni.GetStringValue(SETTINGS, "KeyboardINI", "Keyboard.ini");

        public int MinimumIngameWidth => clientDefinitionsIni.GetIntValue(SETTINGS, "MinimumIngameWidth", 640);

        public int MinimumIngameHeight => clientDefinitionsIni.GetIntValue(SETTINGS, "MinimumIngameHeight", 480);

        public int MaximumIngameWidth => clientDefinitionsIni.GetIntValue(SETTINGS, "MaximumIngameWidth", int.MaxValue);

        public int MaximumIngameHeight => clientDefinitionsIni.GetIntValue(SETTINGS, "MaximumIngameHeight", int.MaxValue);

        public bool CopyMissionsToSpawnmapINI => clientDefinitionsIni.GetBooleanValue(SETTINGS, "CopyMissionsToSpawnmapINI", false);

        public string GetGameExecutableName()
        {
            string[] exeNames = clientDefinitionsIni.GetStringValue(SETTINGS, "GameExecutableNames", "Game.exe").Split(',');

            return exeNames[0];
        }

        public string GameLauncherExecutableName => clientDefinitionsIni.GetStringValue(SETTINGS, "GameLauncherExecutableName", string.Empty);

        public bool CreateSavedGamesDirectory => clientDefinitionsIni.GetBooleanValue(SETTINGS, "CreateSavedGamesDirectory", false);

        /// <summary>
        /// The name of the executable in the main game directory that selects 
        /// the correct main client executable.
        /// For example, DTA.exe in case of DTA.
        /// </summary>
        public string LauncherExe => clientDefinitionsIni.GetStringValue(SETTINGS, "LauncherExe", string.Empty);

        /// <summary>
        /// Returns the name of the game executable file that is used on
        /// Linux and macOS.
        /// </summary>
        public string UnixGameExecutableName => clientDefinitionsIni.GetStringValue(SETTINGS, "UnixGameExecutableName", "wine-dta.sh");

        /// <summary>
        /// List of files that are not distributed but required to play.
        /// </summary>
        public string[] RequiredFiles => clientDefinitionsIni.GetStringValue(SETTINGS, "RequiredFiles", String.Empty).Split(',');

        /// <summary>
        /// List of files that can interfere with the mod functioning.
        /// </summary>
        public string[] ForbiddenFiles => clientDefinitionsIni.GetStringValue(SETTINGS, "ForbiddenFiles", String.Empty).Split(',');

        /// <summary>
        /// If checking for non-normal mixes is allowed.
        /// </summary>
        //public bool ForbiddenMixes => clientDefinitionsIni.GetBooleanValue(SETTINGS, "SearchMixes", true);

        /// <summary>
        /// What number to use to count above allowed expandmd mixes.
        /// </summary>
        public int MixNum => clientDefinitionsIni.GetIntValue(SETTINGS, "MixNum", 3);

        /// <summary>
        /// List of CnCNet files used to warn the player that their CnCNet install is broken
        /// </summary>
        public string[] CncnetFiles => clientDefinitionsIni.GetStringValue(SETTINGS, "CncnetFiles", String.Empty).Split(',');

        public bool EasterEggMode => clientDefinitionsIni.GetBooleanValue(SETTINGS, "EasterEggMode", false);

        /// <summary>
        /// in-testing thing to show missionimage when hovering over it, probably stupid
        /// </summary>
        public bool ShowMissionImage => clientDefinitionsIni.GetBooleanValue(SETTINGS, "MissionImageHover", false);

        public bool UseNameForMissionImage => clientDefinitionsIni.GetBooleanValue(SETTINGS, "UseNameForMissionImage", false);

        #endregion

        public OSVersion GetOperatingSystemVersion()
        {
            try
            {
                if (NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32"), "wine_get_unix_file_name") != (IntPtr)0)
                    return OSVersion.WINE;
            }
            catch { }

            Version osVersion = Environment.OSVersion.Version;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (osVersion.Major < 5)
                    return OSVersion.UNKNOWN;

                if (osVersion.Major == 5)
                    return OSVersion.WINXP;

                if (osVersion.Minor > 1)
                    return OSVersion.WIN810;
                else if (osVersion.Minor == 0)
                    return OSVersion.WINVISTA;

                return OSVersion.WIN7;
            }

            int p = (int)Environment.OSVersion.Platform;

            // http://mono.wikia.com/wiki/Detecting_the_execution_platform
            if (p == 4 || p == 6 || p == 128)
                return OSVersion.UNIX;

            return OSVersion.UNKNOWN;
        }
    }

    /// <summary>
    /// An exception that is thrown when a client configuration file contains invalid or
    /// unexpected settings / data or required settings / data are missing.
    /// </summary>
    public class ClientConfigurationException : Exception
    {
        public ClientConfigurationException(string message) : base(message)
        {
        }
    }
}
#endregion