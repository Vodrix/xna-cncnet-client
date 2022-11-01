using ClientCore.Settings;
using Rampastring.Tools;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ClientCore.Enums;

namespace ClientCore
{
    public class UserINISettings
    {
        private static UserINISettings _instance;

<<<<<<< HEAD
        public const string VIDEO = "Video";
        public const string OPTIONS = "Options";
        public const string AUDIO = "Audio";
        public const string CUSTOM_SETTINGS = "CustomSettings";
        public const string COMPATIBILITY = "Compatibility";
=======
        private const string VIDEO = "Video";
        private const string OPTIONS = "Options";
        private const string AUDIO = "Audio";
        private const string CUSTOM_SETTINGS = "CustomSettings";
        private const string COMPATIBILITY = "Compatibility";
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b

        public static UserINISettings Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("UserINISettings not initialized!");

                return _instance;
            }
        }

        public static void Initialize(string iniFileName)
        {
            if (_instance != null)
                throw new InvalidOperationException("UserINISettings has already been initialized!");

            var iniFile = new IniFile(ProgramConstants.GamePath + iniFileName);

            _instance = new UserINISettings(iniFile);
        }

        protected UserINISettings(IniFile iniFile)
        {
            SettingsIni = iniFile;

#if YR || ARES
            const string WINDOWED_MODE_KEY = "Video.Windowed";
            BackBufferInVRAM = new BoolSetting(iniFile, VIDEO, "VideoBackBuffer", false);
#else
            const string WINDOWED_MODE_KEY = "Video.Windowed";
            BackBufferInVRAM = new BoolSetting(iniFile, VIDEO, "UseGraphicsPatch", true);
#endif

            IngameScreenWidth = new IntSetting(iniFile, VIDEO, "ScreenWidth", 1024);
            IngameScreenHeight = new IntSetting(iniFile, VIDEO, "ScreenHeight", 768);
            ClientTheme = new StringSetting(iniFile, OPTIONS, "Theme", string.Empty);
            DetailLevel = new IntSetting(iniFile, OPTIONS, "DetailLevel", 2);
            Renderer = new StringSetting(iniFile, COMPATIBILITY, "Renderer", string.Empty);
            WindowedMode = new BoolSetting(iniFile, VIDEO, WINDOWED_MODE_KEY, false);
            BorderlessWindowedMode = new BoolSetting(iniFile, VIDEO, "NoWindowFrame", false);

            ClientResolutionX = new IntSetting(iniFile, VIDEO, "ClientResolutionX", Screen.PrimaryScreen.Bounds.Width);
            ClientResolutionY = new IntSetting(iniFile, VIDEO, "ClientResolutionY", Screen.PrimaryScreen.Bounds.Height);
            BorderlessWindowedClient = new BoolSetting(iniFile, VIDEO, "BorderlessWindowedClient", true);
            ClientFPS = new IntSetting(iniFile, VIDEO, "ClientFPS", 60);

            ScoreVolume = new DoubleSetting(iniFile, AUDIO, "ScoreVolume", 0.7);
            SoundVolume = new DoubleSetting(iniFile, AUDIO, "SoundVolume", 0.7);
            VoiceVolume = new DoubleSetting(iniFile, AUDIO, "VoiceVolume", 0.7);
            IsScoreShuffle = new BoolSetting(iniFile, AUDIO, "IsScoreShuffle", true);
            ClientVolume = new DoubleSetting(iniFile, AUDIO, "ClientVolume", 0.3);
            PlayMainMenuMusic = new BoolSetting(iniFile, AUDIO, "PlayMainMenuMusic", true);
            StopMusicOnMenu = new BoolSetting(iniFile, AUDIO, "StopMusicOnMenu", true);

            ScrollRate = new IntSetting(iniFile, OPTIONS, "ScrollRate", 3);
            DragDistance = new IntSetting(iniFile, OPTIONS, "DragDistance", 4);
            DoubleTapInterval = new IntSetting(iniFile, OPTIONS, "DoubleTapInterval", 30);
            Win8CompatMode = new StringSetting(iniFile, OPTIONS, "Win8Compat", "No");

            WritePathToRegistry = new BoolSetting(iniFile, OPTIONS, "WriteInstallationPathToRegistry", false);
            DiscordIntegration = new BoolSetting(iniFile, OPTIONS, "DiscordIntegration", true);
            PlayerName = new StringSetting(iniFile, OPTIONS, "Handle", string.Empty);
            CheckForUpdates = new BoolSetting(iniFile, OPTIONS, "CheckforUpdates", true);

            IsFirstRun = new BoolSetting(iniFile, OPTIONS, "IsFirstRun", true);
            SearchMixes = new BoolSetting(iniFile, OPTIONS, "SearchMixes", true);
            WineCheck = new BoolSetting(iniFile, OPTIONS, "WineChecked", false);
            Difficulty = new IntSetting(iniFile, OPTIONS, "Difficulty", 1);
            ScrollDelay = new IntSetting(iniFile, OPTIONS, "ScrollDelay", 4);
            GameSpeed = new IntSetting(iniFile, OPTIONS, "GameSpeed", 1);
            ForceLowestDetailLevel = new BoolSetting(iniFile, VIDEO, "ForceLowestDetailLevel", false);
            MinimizeWindowsOnGameStart = new BoolSetting(iniFile, OPTIONS, "MinimizeWindowsOnGameStart", true);
            AutoRemoveUnderscoresFromName = new BoolSetting(iniFile, OPTIONS, "AutoRemoveUnderscoresFromName", true);
        }


        /*********************/
        /* GAME LIST FILTERS */
        /*********************/

        public IniFile SettingsIni { get; private set; }

        public event EventHandler SettingsSaved;

        /*********/
        /* VIDEO */
        /*********/

        public IntSetting IngameScreenWidth { get; private set; }
        public IntSetting IngameScreenHeight { get; private set; }
        public StringSetting ClientTheme { get; private set; }
        public IntSetting DetailLevel { get; private set; }
        public StringSetting Renderer { get; private set; }
        public BoolSetting WindowedMode { get; private set; }
        public BoolSetting BorderlessWindowedMode { get; private set; }
        public BoolSetting BackBufferInVRAM { get; private set; }
        public IntSetting ClientResolutionX { get; private set; }
        public IntSetting ClientResolutionY { get; private set; }
        public BoolSetting BorderlessWindowedClient { get; private set; }
        public IntSetting ClientFPS { get; private set; }

        /*********/
        /* AUDIO */
        /*********/

        public DoubleSetting ScoreVolume { get; private set; }
        public DoubleSetting SoundVolume { get; private set; }
        public DoubleSetting VoiceVolume { get; private set; }
        public BoolSetting IsScoreShuffle { get; private set; }
        public DoubleSetting ClientVolume { get; private set; }
        public BoolSetting PlayMainMenuMusic { get; private set; }
        public BoolSetting StopMusicOnMenu { get; private set; }

        /********/
        /* GAME */
        /********/

        public IntSetting ScrollRate { get; private set; }
        public IntSetting DragDistance { get; private set; }
        public IntSetting DoubleTapInterval { get; private set; }
        public StringSetting Win8CompatMode { get; private set; }

        /************************/
        /* MULTIPLAYER (CnCNet) */
        /************************/

        public StringSetting PlayerName { get; private set; }
        public BoolSetting WritePathToRegistry { get; private set; }
        public BoolSetting DiscordIntegration { get; private set; }


        /********/
        /* MISC */
        /********/

        public BoolSetting CheckForUpdates { get; private set; }
        public BoolSetting IsFirstRun { get; private set; }
        public BoolSetting SearchMixes { get; private set; }
        public BoolSetting WineCheck { get; private set; }
        public IntSetting Difficulty { get; private set; }

        public IntSetting GameSpeed { get; private set; }

        public IntSetting ScrollDelay { get; private set; }

        public BoolSetting ForceLowestDetailLevel { get; private set; }

        public BoolSetting MinimizeWindowsOnGameStart { get; private set; }

        public BoolSetting AutoRemoveUnderscoresFromName { get; private set; }
<<<<<<< HEAD

        public void SetValue(string section, string key, string value)
       => SettingsIni.SetStringValue(section, key, value);

        public void SetValue(string section, string key, bool value)
            => SettingsIni.SetBooleanValue(section, key, value);

        public void SetValue(string section, string key, int value)
            => SettingsIni.SetIntValue(section, key, value);

        public string GetValue(string section, string key, string defaultValue)
            => SettingsIni.GetStringValue(section, key, defaultValue);

        public bool GetValue(string section, string key, bool defaultValue)
            => SettingsIni.GetBooleanValue(section, key, defaultValue);

        public int GetValue(string section, string key, int defaultValue)
            => SettingsIni.GetIntValue(section, key, defaultValue);

=======

>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b
        public void ReloadSettings()
        {
            SettingsIni.Reload();
        }

        public void ApplyDefaults()
        {
            ForceLowestDetailLevel.SetDefaultIfNonexistent();
            DoubleTapInterval.SetDefaultIfNonexistent();
            ScrollDelay.SetDefaultIfNonexistent();
        }

        public void SaveSettings()
        {
            Logger.Log("Writing settings INI.");

            ApplyDefaults();
            // CleanUpLegacySettings();

            SettingsIni.WriteIniFile();

            SettingsSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
