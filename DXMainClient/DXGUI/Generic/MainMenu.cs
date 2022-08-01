using ClientCore;
using ClientGUI;
using DTAClient.Domain;
using DTAConfig;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Updater;
using Microsoft.Xna.Framework.Graphics;

namespace DTAClient.DXGUI.Generic
{
    /// <summary>
    /// The main menu of the client.
    /// </summary>
    class MainMenu : XNAWindow, ISwitchable
    {
        private const float MEDIA_PLAYER_VOLUME_FADE_STEP = 0.01f;
        private const float MEDIA_PLAYER_VOLUME_EXIT_FADE_STEP = 0.025f;
        private const double UPDATE_RE_CHECK_THRESHOLD = 30.0;

        /// <summary>
        /// Creates a new instance of the main menu.
        /// </summary>
        public MainMenu(WindowManager windowManager, OptionsWindow optionsWindow, DiscordHandler discordHandler) : base(windowManager)
        {
            this.optionsWindow = optionsWindow;
            this.discordHandler = discordHandler;
            isMediaPlayerAvailable = IsMediaPlayerAvailable();
        }
        private MainMenuDarkeningPanel innerPanel;

        //private XNALabel lblCnCNetPlayerCount;
        private XNALinkLabel lblUpdateStatus;
        private XNALinkLabel lblVersion;

        private readonly OptionsWindow optionsWindow;

        private readonly DiscordHandler discordHandler;

        private XNAMessageBox firstRunMessageBox;
        private XNAMessageBox forbiddenMessageBox;


        private XNALabel lblTime;
        private XNALabel lblEaster;
        public bool easterMode = false;

        private bool _updateInProgress;
        private bool UpdateInProgress
        {
            get { return _updateInProgress; }
            set
            {
                _updateInProgress = value;
                SetButtonHotkeys(!_updateInProgress);
            }
        }

        private bool customComponentDialogQueued = false;

        private DateTime lastUpdateCheckTime;

        private Song themeSong;

        private static readonly object locker = new object();

        private bool isMusicFading = false;

        private readonly bool isMediaPlayerAvailable;

        private XNAClientButton btnRestart;

        // Main Menu Buttons
        //private XNAClientButton btnCampaignSelector4;
        private XNAClientButton btnCampaignSelect;
        private XNAClientButton btnLoadGame;
        //private XNAClientButton btnSkirmish;
        //private XNAClientButton btnCnCNet;
        //private XNAClientButton btnLan;
        private XNAClientButton btnOptions;
        //private XNAClientButton btnMapEditor;
        private XNAClientButton btnStatistics;
        //private XNAClientButton btnCredits;
        //private XNAClientButton btnExtras;

        //private Video MainMenuVideo = AssetLoader.LoadVideo(ProgramConstants.GetResourcePath() + "MainMenu/MainMenuVideo.wmv");
        //private static VideoPlayer videoPlayer = new VideoPlayer();

        /// <summary>
        /// Initializes the main menu's controls.
        /// </summary>
        public override void Initialize()
        {
            GameProcessLogic.GameProcessExited += SharedUILogic_GameProcessExited;

            Name = nameof(MainMenu);
            BackgroundTexture = AssetLoader.LoadTexture("MainMenu/mainmenubg.png");
            ClientRectangle = new Rectangle(0, 0, BackgroundTexture.Width, BackgroundTexture.Height);

            WindowManager.CenterControlOnScreen(this);

            btnRestart = new XNAClientButton(WindowManager);
            btnRestart.Name = nameof(btnRestart);
            btnRestart.LeftClick += BtnRestart_LeftClick;

            btnCampaignSelect = new XNAClientButton(WindowManager);
            btnCampaignSelect.Name = nameof(btnCampaignSelect);
            btnCampaignSelect.IdleTexture = AssetLoader.LoadTexture("MainMenu/campaign.png");
            btnCampaignSelect.HoverTexture = AssetLoader.LoadTexture("MainMenu/campaign_c.png");
            btnCampaignSelect.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            btnCampaignSelect.LeftClick += BtnCampaignSelect_LeftClick;

            //btnCampaignSelector4 = new XNAClientButton(WindowManager);
            //btnCampaignSelector4.Name = nameof(btnCampaignSelector4);
            //btnCampaignSelector4.IdleTexture = AssetLoader.LoadTexture("MainMenu/campaign.png");
            //btnCampaignSelector4.HoverTexture = AssetLoader.LoadTexture("MainMenu/campaign_c.png");
            //btnCampaignSelector4.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnCampaignSelector4.LeftClick += BtnCampaignSelector4_LeftClick;

            btnLoadGame = new XNAClientButton(WindowManager);
            btnLoadGame.Name = nameof(btnLoadGame);
            btnLoadGame.IdleTexture = AssetLoader.LoadTexture("MainMenu/loadmission.png");
            btnLoadGame.HoverTexture = AssetLoader.LoadTexture("MainMenu/loadmission_c.png");
            btnLoadGame.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            btnLoadGame.LeftClick += BtnLoadGame_LeftClick;

            //btnSkirmish = new XNAClientButton(WindowManager);
            //btnSkirmish.Name = nameof(btnSkirmish);
            //btnSkirmish.IdleTexture = AssetLoader.LoadTexture("MainMenu/skirmish.png");
            //btnSkirmish.HoverTexture = AssetLoader.LoadTexture("MainMenu/skirmish_c.png");
            //btnSkirmish.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnSkirmish.LeftClick += BtnSkirmish_LeftClick;

            //btnCnCNet = new XNAClientButton(WindowManager);
            //btnCnCNet.Name = nameof(btnCnCNet);
            //btnCnCNet.IdleTexture = AssetLoader.LoadTexture("MainMenu/cncnet.png");
            //btnCnCNet.HoverTexture = AssetLoader.LoadTexture("MainMenu/cncnet_c.png");
            //btnCnCNet.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");

            //btnLan = new XNAClientButton(WindowManager);
            //btnLan.Name = nameof(btnLan);
            //btnLan.IdleTexture = AssetLoader.LoadTexture("MainMenu/lan.png");
            //btnLan.HoverTexture = AssetLoader.LoadTexture("MainMenu/lan_c.png");
            //btnLan.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnLan.LeftClick += BtnLan_LeftClick;

            btnOptions = new XNAClientButton(WindowManager);
            btnOptions.Name = nameof(btnOptions);
            btnOptions.IdleTexture = AssetLoader.LoadTexture("MainMenu/options.png");
            btnOptions.HoverTexture = AssetLoader.LoadTexture("MainMenu/options_c.png");
            btnOptions.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            btnOptions.LeftClick += BtnOptions_LeftClick;

            //btnMapEditor = new XNAClientButton(WindowManager);
            //btnMapEditor.Name = nameof(btnMapEditor);
            //btnMapEditor.IdleTexture = AssetLoader.LoadTexture("MainMenu/mapeditor.png");
            //btnMapEditor.HoverTexture = AssetLoader.LoadTexture("MainMenu/mapeditor_c.png");
            //btnMapEditor.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnMapEditor.LeftClick += BtnMapEditor_LeftClick;

            btnStatistics = new XNAClientButton(WindowManager);
            btnStatistics.Name = nameof(btnStatistics);
            btnStatistics.IdleTexture = AssetLoader.LoadTexture("MainMenu/statistics.png");
            btnStatistics.HoverTexture = AssetLoader.LoadTexture("MainMenu/statistics_c.png");
            btnStatistics.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            btnStatistics.LeftClick += BtnStatistics_LeftClick;

            //btnCredits = new XNAClientButton(WindowManager);
            //btnCredits.Name = nameof(btnCredits);
            //btnCredits.IdleTexture = AssetLoader.LoadTexture("MainMenu/credits.png");
            //btnCredits.HoverTexture = AssetLoader.LoadTexture("MainMenu/credits_c.png");
            //btnCredits.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnCredits.LeftClick += BtnCredits_LeftClick;

            //btnExtras = new XNAClientButton(WindowManager);
            //btnExtras.Name = nameof(btnExtras);
            //btnExtras.IdleTexture = AssetLoader.LoadTexture("MainMenu/extras.png");
            //btnExtras.HoverTexture = AssetLoader.LoadTexture("MainMenu/extras_c.png");
            //btnExtras.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            //btnExtras.LeftClick += BtnExtras_LeftClick;

            var btnExit = new XNAClientButton(WindowManager);
            btnExit.Name = nameof(btnExit);
            btnExit.IdleTexture = AssetLoader.LoadTexture("MainMenu/exitgame.png");
            btnExit.HoverTexture = AssetLoader.LoadTexture("MainMenu/exitgame_c.png");
            btnExit.HoverSoundEffect = new EnhancedSoundEffect("MainMenu/button.wav");
            btnExit.LeftClick += BtnExit_LeftClick;

            //XNALabel lblCnCNetStatus = new XNALabel(WindowManager);
            //lblCnCNetStatus.Name = nameof(lblCnCNetStatus);
            //lblCnCNetStatus.Text = "DTA players on CnCNet:";
            //lblCnCNetStatus.ClientRectangle = new Rectangle(12, 9, 0, 0);

            //lblCnCNetPlayerCount = new XNALabel(WindowManager);
            //lblCnCNetPlayerCount.Name = nameof(lblCnCNetPlayerCount);
            //lblCnCNetPlayerCount.Text = "-";

            lblVersion = new XNALinkLabel(WindowManager);
            lblVersion.Name = nameof(lblVersion);
            lblVersion.LeftClick += LblVersion_LeftClick;

            lblTime = new XNALabel(WindowManager);
            lblTime.Name = "lblTime";
            lblTime.FontIndex = 1;
            lblTime.Text = "99:99:99";
            lblTime.ClientRectangle = new Rectangle(Width -
                (int)Renderer.GetTextDimensions(lblTime.Text, lblTime.FontIndex).X - 58, 4,
                lblTime.Width, lblTime.Height);

            lblUpdateStatus = new XNALinkLabel(WindowManager);
            lblUpdateStatus.Name = nameof(lblUpdateStatus);
            lblUpdateStatus.LeftClick += LblUpdateStatus_LeftClick;
            lblUpdateStatus.ClientRectangle = new Rectangle(0, 0, 160, 20);

            AddChild(btnCampaignSelect);
            //AddChild(btnCampaignSelector4);
            AddChild(btnLoadGame);
            AddChild(lblTime);
            //AddChild(btnSkirmish);
            //AddChild(btnCnCNet);
            //AddChild(btnLan);
            AddChild(btnOptions);
            //AddChild(btnMapEditor);
            AddChild(btnStatistics);
            //AddChild(btnCredits);
            //AddChild(btnExtras);
            AddChild(btnExit);
            //AddChild(lblCnCNetStatus);
            //AddChild(lblCnCNetPlayerCount);

            AddChild(btnRestart);
                
            #region time
            string date = DateTime.Now.ToString("MMdd");
            if (date.Equals(ProgramConstants.EASTEREGG) & ClientConfiguration.Instance.EasterEggMode)     //no one will ever find this lol
            {
                lblEaster = new XNALabel(WindowManager);
                lblEaster.Name = nameof(lblEaster);
                lblEaster.Text = "Easter Egg Mode Enabled!";
                lblEaster.ClientRectangle = new Rectangle(5, 5, 1, 1);
                easterMode = true;
                AddChild(lblEaster);
            }
            #endregion
            if (!ClientConfiguration.Instance.ModMode)
            {
                // ModMode disables version tracking and the updater if it's enabled

                AddChild(lblVersion);
                AddChild(lblUpdateStatus);

                CUpdater.FileIdentifiersUpdated += CUpdater_FileIdentifiersUpdated;
                CUpdater.OnCustomComponentsOutdated += CUpdater_OnCustomComponentsOutdated;
            }

            base.Initialize(); // Read control attributes from INI

            innerPanel = new MainMenuDarkeningPanel(WindowManager, discordHandler)
            {
                ClientRectangle = new Rectangle(0, 0,
                Width,
                Height),
                DrawOrder = int.MaxValue,
                UpdateOrder = int.MaxValue
            };
            AddChild(innerPanel);
            innerPanel.Hide();

            lblVersion.Text = CUpdater.GameVersion;

            innerPanel.UpdateQueryWindow.UpdateDeclined += UpdateQueryWindow_UpdateDeclined;
            innerPanel.UpdateQueryWindow.UpdateAccepted += UpdateQueryWindow_UpdateAccepted;
            innerPanel.ManualUpdateQueryWindow.Closed += ManualUpdateQueryWindow_Closed;
            innerPanel.UpdateWindow.UpdateCompleted += UpdateWindow_UpdateCompleted;
            innerPanel.UpdateWindow.UpdateCancelled += UpdateWindow_UpdateCancelled;
            innerPanel.UpdateWindow.UpdateFailed += UpdateWindow_UpdateFailed;

            this.ClientRectangle = new Rectangle((WindowManager.RenderResolutionX - Width) / 2,
                (WindowManager.RenderResolutionY - Height) / 2,
                Width, Height);
            innerPanel.ClientRectangle = new Rectangle(0, 0,
                Math.Max(WindowManager.RenderResolutionX, Width),
                Math.Max(WindowManager.RenderResolutionY, Height));

            //cncnetPlayerCountCancellationSource = new CancellationTokenSource();

            WindowManager.GameClosing += WindowManager_GameClosing;

            optionsWindow.EnabledChanged += OptionsWindow_EnabledChanged;

            optionsWindow.OnForceUpdate += (s, e) => ForceUpdate();

            GameProcessLogic.GameProcessStarted += SharedUILogic_GameProcessStarted;
            GameProcessLogic.GameProcessStarting += SharedUILogic_GameProcessStarting;

            UserINISettings.Instance.SettingsSaved += SettingsSaved;

            CUpdater.Restart += CUpdater_Restart;

            SetButtonHotkeys(true);
            }

        private void SetButtonHotkeys(bool enableHotkeys)
        {
            if (!Initialized)
                return;

            if (UserINISettings.Instance.PlayerName == "marsu") //no one but me needs this and i can need it even when using non-debug builds so
                btnRestart.HotKey = Keys.F24;

            if (enableHotkeys)
            {
                //btnCampaignSelector4.HotKey = Keys.V;
                btnCampaignSelect.HotKey = Keys.C;
                btnLoadGame.HotKey = Keys.L;
                //btnSkirmish.HotKey = Keys.S;
                //btnCnCNet.HotKey = Keys.M;
                //btnLan.HotKey = Keys.N;
                btnOptions.HotKey = Keys.O;
                //btnMapEditor.HotKey = Keys.E;
                btnStatistics.HotKey = Keys.T;
                //btnCredits.HotKey = Keys.R;
                //btnExtras.HotKey = Keys.X;
            }
            else
            {
                //btnCampaignSelector4.HotKey = Keys.None;
                btnCampaignSelect.HotKey = Keys.None;
                btnLoadGame.HotKey = Keys.None;
                //btnSkirmish.HotKey = Keys.None;
                //btnCnCNet.HotKey = Keys.None;
                //btnLan.HotKey = Keys.None;
                btnOptions.HotKey = Keys.None;
                //btnMapEditor.HotKey = Keys.None;
                btnStatistics.HotKey = Keys.None;
                //btnCredits.HotKey = Keys.None;
                //btnExtras.HotKey = Keys.None;
            }
        }

        private void OptionsWindow_EnabledChanged(object sender, EventArgs e)
        {
            if (!optionsWindow.Enabled)
            {
                if (customComponentDialogQueued)
                    CUpdater_OnCustomComponentsOutdated();
            }
        }

        /// <summary>
        /// Refreshes settings. Called when the game process is starting.
        /// </summary>
        private void SharedUILogic_GameProcessStarting()
        {
            UserINISettings.Instance.ReloadSettings();

            try
            {
                optionsWindow.RefreshSettings();
            }
            catch (Exception ex)
            {
                Logger.Log("Refreshing settings failed! Exception message: " + ex.Message);
                // We don't want to show the dialog when starting a game
                //XNAMessageBox.Show(WindowManager, "Saving settings failed",
                //    "Saving settings failed! Error message: " + ex.Message);
            }
        }

        private void CUpdater_Restart(object sender, EventArgs e) =>
            WindowManager.AddCallback(new Action(ExitClient), null);

        /// <summary>
        /// Applies configuration changes (music playback and volume)
        /// when settings are saved.
        /// </summary>
        private void SettingsSaved(object sender, EventArgs e)
        {
            if (isMediaPlayerAvailable)
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    if (!UserINISettings.Instance.PlayMainMenuMusic)
                        isMusicFading = true;
                }
                {
                    PlayMusic();
                }
            }

            if (UserINISettings.Instance.DiscordIntegration)
                discordHandler?.Connect();
            else
                discordHandler?.Disconnect();
        }

        /// <summary>
        /// Checks files which are from the vanilla CnCNet
        /// install that means the user installed ontop of a
        /// CnCNet install which breaks it.
        /// </summary>
        private void CheckForYRCNCNET()
        {
            List<string> otherFiles = ClientConfiguration.Instance.CncnetFiles.ToList()
                .FindAll(f => !string.IsNullOrWhiteSpace(f) && File.Exists(ProgramConstants.GamePath + f));
            if (otherFiles.Count > 0)
                XNAMessageBox.Show(WindowManager, "CnCNet Install Detected!",
                    "You have installed CnCNet Yuri's Revenge in this folder," + Environment.NewLine +
                    "by installing FM in the same folder you have broken your" + Environment.NewLine +
                    "CnCNet install, you need to uninstall FM and reinstall" + Environment.NewLine +
                    "CnCNet for it to work again." + Environment.NewLine +
                    "Keep in mind you can copy over clean YR files to a new" + Environment.NewLine +
                    "folder and install FM there, and that wont break anything." + Environment.NewLine +
                    "See the Discord for more or for further help.");
        }

        /// <summary>
        /// Checks files which are required for the mod to function
        /// but not distributed with the mod (usually base game files
        /// for YR mods which can't be standalone).
        /// </summary>
        private void CheckRequiredFiles()
        {
            List<string> absentFiles = ClientConfiguration.Instance.RequiredFiles.ToList()
                .FindAll(f => !string.IsNullOrWhiteSpace(f) && !File.Exists(ProgramConstants.GamePath + f));

            if (absentFiles.Count > 0)
                XNAMessageBox.Show(WindowManager, "Missing Files",
#if ARES
                    "You are missing Yuri's Revenge files that are required" + Environment.NewLine +
                    "to play this mod! Yuri's Revenge mods are not standalone," + Environment.NewLine +
                    "so you need a copy of following Yuri's Revenge (v. 1.001)" + Environment.NewLine +
                    "files placed in the mod folder to play the mod:" +
#else
                    "The following required files are missing:" +
#endif
                    Environment.NewLine + Environment.NewLine +
                    string.Join(Environment.NewLine, absentFiles) +
                    Environment.NewLine + Environment.NewLine +
                    "You won't be able to play without those files.");
        }

        private void ForbiddenMessageBox_YesClicked(XNAMessageBox messageBox)
        {
            UserINISettings.Instance.SearchMixes.Value = false;
            UserINISettings.Instance.SaveSettings();
            innerPanel.Hide();
        }

        private void ForbiddenMessageBox_NoClicked(XNAMessageBox messageBox)
        {
            innerPanel.Hide();
        }

        private void CheckForbiddenFiles()
        {
            List<string> MixesList = new List<string>();
            for (int i = 1; i < 100; i += 1)
                 if (i > ClientConfiguration.Instance.MixNum)
                    MixesList.Add(string.Format("expandmd{0}.mix", i.ToString("00")));
            List<string> presentFiles = ClientConfiguration.Instance.ForbiddenFiles.ToList().FindAll(f => !string.IsNullOrWhiteSpace(f) && File.Exists(ProgramConstants.GamePath + f));
            List<string> finalList = presentFiles.Concat(MixesList).ToList().FindAll(f => !string.IsNullOrWhiteSpace(f) && File.Exists(ProgramConstants.GamePath + f));

            if (finalList.Count > 0)
            {
                forbiddenMessageBox = XNAMessageBox.ShowYesNoDialog(WindowManager, "Interfering Files Detected",
                            "The following interfering files are present:" +
                            Environment.NewLine + Environment.NewLine +
                            string.Join(Environment.NewLine, finalList) +
                            Environment.NewLine + Environment.NewLine +
                            "Flipped Missions might not work correctly without those files removed."
                            + Environment.NewLine + Environment.NewLine +
                            "Would you like to disable this prompt?");
                forbiddenMessageBox.YesClickedAction = ForbiddenMessageBox_YesClicked;
                forbiddenMessageBox.NoClickedAction = ForbiddenMessageBox_NoClicked;
            }
        }

        private void CheckIfWine()
        {
            UserINISettings.Instance.WineCheck.Value = true;
            UserINISettings.Instance.SaveSettings();

            XNAMessageBox.Show(WindowManager, "Wine Detected",
                        "The Client has detected that you're running on Wine." +
                        Environment.NewLine +
                        "Make sure ddraw.dll is set to run as native before builtin" + 
                        Environment.NewLine + 
                        "using winecfg or else the game might not show up properly.");
        }

        /// <summary>
        /// Checks whether the client is running for the first time.
        /// If it is, displays a dialog asking the user if they'd like
        /// to configure settings.
        /// </summary>
        private void CheckIfFirstRun()
        {
            UserINISettings.Instance.IsFirstRun.Value = false;
            UserINISettings.Instance.SaveSettings();

            firstRunMessageBox = XNAMessageBox.ShowYesNoDialog(WindowManager, "Initial Installation",
                string.Format("You have just installed {0}." + Environment.NewLine +
                "It's highly recommended that you configure your settings before playing." +
                Environment.NewLine + "Do you want to configure them now?", ClientConfiguration.Instance.LocalGame));
            firstRunMessageBox.YesClickedAction = FirstRunMessageBox_YesClicked;
            firstRunMessageBox.NoClickedAction = FirstRunMessageBox_NoClicked;
            optionsWindow.PostInit();
        }

        private void FirstRunMessageBox_NoClicked(XNAMessageBox messageBox)
        {
            if (customComponentDialogQueued)
                CUpdater_OnCustomComponentsOutdated();
        }

        private void FirstRunMessageBox_YesClicked(XNAMessageBox messageBox)// => optionsWindow.Open();
        {
            optionsWindow.Open();
        }

        private void SharedUILogic_GameProcessStarted() => MusicOff();

        private void WindowManager_GameClosing(object sender, EventArgs e) => Clean();

        /// <summary>
        /// Attemps to "clean" the client session in a nice way if the user closes the game.
        /// </summary>
        private void Clean()
        {
            CUpdater.FileIdentifiersUpdated -= CUpdater_FileIdentifiersUpdated;

            if (UpdateInProgress)
                CUpdater.TerminateUpdate = true;

        }

        /// <summary>
        /// Starts playing music, initiates an update check if automatic updates
        /// are enabled and checks whether the client is run for the first time.
        /// Called after all internal client UI logic has been initialized.
        /// </summary>
        public void PostInit()
        {
            themeSong = AssetLoader.LoadSong(ClientConfiguration.Instance.MainMenuMusicName);
            //MainMenuVideo = AssetLoader.LoadVideo(ProgramConstants.GetResourcePath() + "MainMenu/MainMenuVideo.wmv");
            //if (File.Exists(ProgramConstants.GetResourcePath() + "MainMenu/MainMenuVideo.wmv"))
            //    Logger.Log("wtf");

            PlayMusic();

            if (!ClientConfiguration.Instance.ModMode)
            {
                if (CUpdater.UPDATEMIRRORS.Count < 1)
                {
                    lblUpdateStatus.Text = "No update download mirrors available.";
                    lblUpdateStatus.DrawUnderline = false;
                }
                else if (UserINISettings.Instance.CheckForUpdates)
                    CheckForUpdates();
                else
                    lblUpdateStatus.Text = "Click to check for updates.";
            }

            CheckRequiredFiles();
            CheckForYRCNCNET();
            if (ClientConfiguration.Instance.GetOperatingSystemVersion() == OSVersion.WINE && !UserINISettings.Instance.WineCheck)
            {
                CheckIfWine();
            }
            if (UserINISettings.Instance.IsFirstRun)
            {
                CheckIfFirstRun();
            }
            if (UserINISettings.Instance.SearchMixes.Value)
            {
                CheckForbiddenFiles();
            }

        }

        #region Updating / versioning system

        private void UpdateWindow_UpdateFailed(object sender, UpdateFailureEventArgs e)
        {
            innerPanel.Hide();
            lblUpdateStatus.Text = "Updating failed! Click to retry.";
            lblUpdateStatus.DrawUnderline = true;
            lblUpdateStatus.Enabled = true;
            UpdateInProgress = false;

            innerPanel.Show(null); // Darkening
            XNAMessageBox msgBox = new XNAMessageBox(WindowManager, "Update failed",
                string.Format("An error occured while updating. Returned error was: {0}" +
                Environment.NewLine + Environment.NewLine +
                "If you are connected to the Internet and your firewall isn't blocking" + Environment.NewLine +
                "{1}, and the issue is reproducible, contact us at " + Environment.NewLine +
                "{2} for support.",
                e.Reason, CUpdater.CURRENT_LAUNCHER_NAME, MainClientConstants.SUPPORT_URL_SHORT), XNAMessageBoxButtons.OK)
            {
                OKClickedAction = MsgBox_OKClicked
            };
            msgBox.Show();
        }

        private void MsgBox_OKClicked(XNAMessageBox messageBox)
        {
            innerPanel.Hide();
        }

        private void UpdateWindow_UpdateCancelled(object sender, EventArgs e)
        {
            innerPanel.Hide();
            lblUpdateStatus.Text = "The update was cancelled. Click to retry.";
            lblUpdateStatus.DrawUnderline = true;
            lblUpdateStatus.Enabled = true;
            UpdateInProgress = false;
        }

        private void UpdateWindow_UpdateCompleted(object sender, EventArgs e)
        {
            innerPanel.Hide();
            lblUpdateStatus.Text = MainClientConstants.GAME_NAME_SHORT + " was succesfully updated to v." + CUpdater.GameVersion;
            lblVersion.Text = CUpdater.GameVersion;
            UpdateInProgress = false;
            lblUpdateStatus.Enabled = true;
            lblUpdateStatus.DrawUnderline = false;
        }

        private void LblUpdateStatus_LeftClick(object sender, EventArgs e)
        {
            Logger.Log(CUpdater.DTAVersionState.ToString());

            if (CUpdater.DTAVersionState == VersionState.OUTDATED ||
                CUpdater.DTAVersionState == VersionState.MISMATCHED ||
                CUpdater.DTAVersionState == VersionState.UNKNOWN ||
                CUpdater.DTAVersionState == VersionState.UPTODATE)
            {
                CheckForUpdates();
            }
        }

        private void LblVersion_LeftClick(object sender, EventArgs e)
        {
            Process.Start(ClientConfiguration.Instance.ChangelogURL);
        }

        private void ForceUpdate()
        {
            UpdateInProgress = true;
            innerPanel.Hide();
            innerPanel.UpdateWindow.ForceUpdate();
            innerPanel.Show(innerPanel.UpdateWindow);
            lblUpdateStatus.Text = "Force updating...";
        }

        /// <summary>
        /// Starts a check for updates.
        /// </summary>
        private void CheckForUpdates()
        {
            if (CUpdater.UPDATEMIRRORS.Count < 1)
                return;
            CUpdater.CheckForUpdates();
            lblUpdateStatus.Enabled = false;
            lblUpdateStatus.Text = "Checking for updates...";
            try
            {
                StatisticsSender.Instance.SendUpdate();
            }
            catch { }
            lastUpdateCheckTime = DateTime.Now;
        }

        private void CUpdater_FileIdentifiersUpdated()
        {
            WindowManager.AddCallback(new Action(HandleFileIdentifierUpdate), null);
        }

        /// <summary>
        /// Used for displaying the result of an update check in the UI.
        /// </summary>
        private void HandleFileIdentifierUpdate()
        {
            if (UpdateInProgress)
            {
                return;
            }

            if (CUpdater.DTAVersionState == VersionState.UPTODATE)
            {
                lblUpdateStatus.Text = MainClientConstants.GAME_NAME_SHORT + " is up to date.";
                lblUpdateStatus.Enabled = true;
                lblUpdateStatus.DrawUnderline = false;
            }
            else if (CUpdater.DTAVersionState == VersionState.OUTDATED && CUpdater.ManualUpdateRequired)
            {
                lblUpdateStatus.Text = "An update is available. Manual download & installation required.";
                lblUpdateStatus.Enabled = true;
                lblUpdateStatus.DrawUnderline = false;
                innerPanel.ManualUpdateQueryWindow.SetInfo(CUpdater.ServerGameVersion, CUpdater.ManualDownloadURL);
                if (!string.IsNullOrEmpty(CUpdater.ManualDownloadURL))
                    innerPanel.Show(innerPanel.ManualUpdateQueryWindow);
            }
            else if (CUpdater.DTAVersionState == VersionState.OUTDATED)
            {
                lblUpdateStatus.Text = "An update is available.";
                innerPanel.UpdateQueryWindow.SetInfo(CUpdater.ServerGameVersion, CUpdater.UpdateSizeInKb);
                innerPanel.Show(innerPanel.UpdateQueryWindow);
            }
            else if (CUpdater.DTAVersionState == VersionState.UNKNOWN)
            {
                lblUpdateStatus.Text = "Checking for updates failed! Click to retry.";
                lblUpdateStatus.Enabled = true;
                lblUpdateStatus.DrawUnderline = true;
            }
        }

        /// <summary>
        /// Asks the user if they'd like to update their custom components.
        /// Handles an event raised by the updater when it has detected
        /// that the custom components are out of date.
        /// </summary>
        private void CUpdater_OnCustomComponentsOutdated()
        {
            if (innerPanel.UpdateQueryWindow.Visible)
                return;

            if (UpdateInProgress)
                return;

            if ((firstRunMessageBox != null && firstRunMessageBox.Visible) || optionsWindow.Enabled)
            {
                // If the custom components are out of date on the first run
                // or the options window is already open, don't show the dialog
                customComponentDialogQueued = true;
                return;
            }

            customComponentDialogQueued = false;

            XNAMessageBox ccMsgBox = XNAMessageBox.ShowYesNoDialog(WindowManager,
                "Custom Component Updates Available",
                "Updates for custom components are available. Do you want to open" + Environment.NewLine +
                "the Options menu where you can update the custom components?");
            ccMsgBox.YesClickedAction = CCMsgBox_YesClicked;
        }

        private void CCMsgBox_YesClicked(XNAMessageBox messageBox)
        {
            optionsWindow.Open();
            optionsWindow.SwitchToCustomComponentsPanel();
        }

        /// <summary>
        /// Called when the user has declined an update.
        /// </summary>
        private void UpdateQueryWindow_UpdateDeclined(object sender, EventArgs e)
        {
            UpdateQueryWindow uqw = (UpdateQueryWindow)sender;
            innerPanel.Hide();
            lblUpdateStatus.Text = "An update is available, click to install.";
            lblUpdateStatus.Enabled = true;
            lblUpdateStatus.DrawUnderline = true;
        }

        /// <summary>
        /// Called when the user has accepted an update.
        /// </summary>
        private void UpdateQueryWindow_UpdateAccepted(object sender, EventArgs e)
        {
            innerPanel.Hide();
            innerPanel.UpdateWindow.SetData(CUpdater.ServerGameVersion);
            innerPanel.Show(innerPanel.UpdateWindow);
            lblUpdateStatus.Text = "Updating...";
            UpdateInProgress = true;
            CUpdater.StartAsyncUpdate();
        }
        private void ManualUpdateQueryWindow_Closed(object sender, EventArgs e)
        {
            innerPanel.Hide();
        }

        #endregion

        private void BtnOptions_LeftClick(object sender, EventArgs e) => optionsWindow.Open();

        private void BtnCampaignSelect_LeftClick(object sender, EventArgs e) =>
            innerPanel.Show(innerPanel.CampaignSelect);

        //private void BtnCampaignSelector4_LeftClick(object sender, EventArgs e) =>
        //    innerPanel.Show(innerPanel.CampaignSelector4);

        private void BtnLoadGame_LeftClick(object sender, EventArgs e) =>
            innerPanel.Show(innerPanel.GameLoadingWindow);

        private void BtnRestart_LeftClick(object sender, EventArgs e) => WindowManager.RestartGame();

        //private void BtnLan_LeftClick(object sender, EventArgs e)
        //{
        //
        //    if (UserINISettings.Instance.StopMusicOnMenu)
        //        MusicOff();
        //}
        //
        //private void BtnSkirmish_LeftClick(object sender, EventArgs e)
        //{
        //
        //    if (UserINISettings.Instance.StopMusicOnMenu)
        //        MusicOff();
        //}
        //
        //private void BtnMapEditor_LeftClick(object sender, EventArgs e) => LaunchMapEditor();

        private void BtnStatistics_LeftClick(object sender, EventArgs e) =>
            innerPanel.Show(innerPanel.CreditsWindow);

        //private void BtnCredits_LeftClick(object sender, EventArgs e) =>
        //    Process.Start(MainClientConstants.CREDITS_URL);
        //
        //private void BtnExtras_LeftClick(object sender, EventArgs e) =>
        //    innerPanel.Show(innerPanel.ExtrasWindow);

        private void BtnExit_LeftClick(object sender, EventArgs e)
        {
            WindowManager.HideWindow();
            FadeMusicExit();
        }

        private void SharedUILogic_GameProcessExited() =>
            AddCallback(new Action(HandleGameProcessExited), null);

        private void HandleGameProcessExited()
        {
            innerPanel.GameLoadingWindow.ListSaves();
            innerPanel.Hide();

            // If music is disabled on menus, check if the main menu is the top-most
            // window of the top bar and only play music if it is
            // LAN has the top bar disabled, so to detect the LAN game lobby
            // we'll check whether the top bar is enabled
        }

        public override void Update(GameTime gameTime)
        {
            if (isMusicFading)
                FadeMusic(gameTime);

            base.Update(gameTime);

            lblTime.Text = Renderer.GetSafeString(DateTime.Now.ToLongTimeString(), lblTime.FontIndex);

            //if (MainMenuVideo != null)
            //    videoPlayer.Play(MainMenuVideo);
        }

        public override void Draw(GameTime gameTime)
        {
            lock (locker)
            {
                base.Draw(gameTime);
            }
            //if (MainMenuVideo != null)
            //{
            //    spriteBatch.Begin();
            //    spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, MainMenuVideo.Width, MainMenuVideo.Height), Color.CornflowerBlue);
            //    spriteBatch.End();
            //}

        }

        /// <summary>
        /// Attempts to start playing the menu music.
        /// </summary>
        private void PlayMusic()
        {
            if (!isMediaPlayerAvailable)
                return; // SharpDX fails at music playback on Vista

            if (themeSong != null && UserINISettings.Instance.PlayMainMenuMusic)
            {
                isMusicFading = false;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = (float)UserINISettings.Instance.ClientVolume;

                try
                {
                    MediaPlayer.Play(themeSong);
                }
                catch (InvalidOperationException ex)
                {
                    Logger.Log("Playing main menu music failed! " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Lowers the volume of the menu music, or stops playing it if the
        /// volume is unaudibly low.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void FadeMusic(GameTime gameTime)
        {
            if (!isMediaPlayerAvailable || !isMusicFading || themeSong == null)
                return;

            // Fade during 1 second
            float step = SoundPlayer.Volume * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (MediaPlayer.Volume > step)
                MediaPlayer.Volume -= step;
            else
            {
                MediaPlayer.Stop();
                isMusicFading = false;
            }
        }

        /// <summary>
        /// Exits the client. Quickly fades the music if it's playing.
        /// </summary>
        private void FadeMusicExit()
        {
            if (!isMediaPlayerAvailable || themeSong == null)
            {
                ExitClient();
                return;
            }

            float step = MEDIA_PLAYER_VOLUME_EXIT_FADE_STEP * (float)UserINISettings.Instance.ClientVolume;

            if (MediaPlayer.Volume > step)
            {
                MediaPlayer.Volume -= step;
                AddCallback(new Action(FadeMusicExit), null);
            }
            else
            {
                MediaPlayer.Stop();
                ExitClient();
            }
        }

        private void ExitClient()
        {
            Logger.Log("Exiting.");
            WindowManager.CloseGame();
#if !XNA
            Thread.Sleep(1000);
            Environment.Exit(0);
#endif
        }

        public void SwitchOn()
        {
            if (UserINISettings.Instance.StopMusicOnMenu)
                PlayMusic();

            if (!ClientConfiguration.Instance.ModMode && UserINISettings.Instance.CheckForUpdates)
            {
                // Re-check for updates

                if ((DateTime.Now - lastUpdateCheckTime) > TimeSpan.FromSeconds(UPDATE_RE_CHECK_THRESHOLD))
                    CheckForUpdates();
            }
        }

        public void SwitchOff()
        {
            if (UserINISettings.Instance.StopMusicOnMenu)
                MusicOff();
        }

        private void MusicOff()
        {
            try
            {
                if (isMediaPlayerAvailable &&
                    MediaPlayer.State == MediaState.Playing)
                {
                    isMusicFading = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Turning music off failed! Message: " + ex.Message);
            }
        }

        /// <summary>
        /// Checks if media player is available currently.
        /// It is not available on Windows Vista or other systems without the appropriate media player components.
        /// </summary>
        /// <returns>True if media player is available, false otherwise.</returns>
        private bool IsMediaPlayerAvailable()
        {
            if (MainClientConstants.OSId == OSVersion.WINVISTA)
                return false;

            try
            {
                MediaState state = MediaPlayer.State;
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Error encountered when checking media player availability. Error message: " + e.Message);
                return false;
            }
        }

       //private void LaunchMapEditor()
       //{
       //    OSVersion osVersion = ClientConfiguration.Instance.GetOperatingSystemVersion();
       //    Process mapEditorProcess = new Process();
       //
       //    if (osVersion != OSVersion.UNIX)
       //    {
       //        mapEditorProcess.StartInfo.FileName = ProgramConstants.GamePath + ClientConfiguration.Instance.MapEditorExePath;
       //    }
       //    else
       //    {
       //        mapEditorProcess.StartInfo.FileName = ProgramConstants.GamePath + ClientConfiguration.Instance.UnixMapEditorExePath;
       //        mapEditorProcess.StartInfo.UseShellExecute = false;
       //    }
       //
       //    mapEditorProcess.Start();
       //}

        public string GetSwitchName() => "Main Menu";
    }
}