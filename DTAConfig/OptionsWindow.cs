﻿using ClientCore;
using ClientCore.CnCNet5;
using ClientGUI;
using DTAConfig.OptionPanels;
using Localization;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using ClientUpdater;

namespace DTAConfig
{
    public class OptionsWindow : XNAWindow
    {
        public OptionsWindow(WindowManager windowManager, GameCollection gameCollection) : base(windowManager)
        {
            this.gameCollection = gameCollection;
        }

        public event EventHandler OnForceUpdate;

        private XNAClientTabControl tabControl;

        private XNAOptionsPanel[] optionsPanels;
        private ComponentsPanel componentsPanel;

        private DisplayOptionsPanel displayOptionsPanel;

        private readonly GameCollection gameCollection;

        public override void Initialize()
        {
            Name = "OptionsWindow";
            ClientRectangle = new Rectangle(0, 0, 576, 475);
            BackgroundTexture = AssetLoader.LoadTextureUncached("optionsbg.png");

            tabControl = new XNAClientTabControl(WindowManager)
            {
                Name = "tabControl",
                ClientRectangle = new Rectangle(12, 12, 0, 23),
                FontIndex = 1,
                ClickSound = new EnhancedSoundEffect("button.wav")
            };
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            tabControl.AddTab("Display", 138);
            tabControl.AddTab("Audio", 138);
            tabControl.AddTab("Game", 138);
            tabControl.AddTab("Updater", 138);

<<<<<<< Updated upstream
            var btnCancel = new XNAClientButton(WindowManager);
            btnCancel.Name = "btnCancel";
            btnCancel.ClientRectangle = new Rectangle(Width - 104,
                Height - 35, UIDesignConstants.BUTTON_WIDTH_92, UIDesignConstants.BUTTON_HEIGHT);
            btnCancel.Text = "Cancel".L10N("UI:DTAConfig:ButtonCancel");
            btnCancel.LeftClick += BtnBack_LeftClick;

            var btnSave = new XNAClientButton(WindowManager);
            btnSave.Name = "btnSave";
            btnSave.ClientRectangle = new Rectangle(12, btnCancel.Y, UIDesignConstants.BUTTON_WIDTH_92, UIDesignConstants.BUTTON_HEIGHT);
            btnSave.Text = "Save".L10N("UI:DTAConfig:ButtonSave");
=======
            var btnCancel = new XNAClientButton(WindowManager)
            {
                Name = "btnCancel",
                ClientRectangle = new Rectangle(Width - 104,
                Height - 35, UIDesignConstants.BUTTON_WIDTH_92, UIDesignConstants.BUTTON_HEIGHT),
                Text = "Cancel"
            };
            btnCancel.LeftClick += BtnBack_LeftClick;

            var btnSave = new XNAClientButton(WindowManager)
            {
                Name = "btnSave",
                ClientRectangle = new Rectangle(12, btnCancel.Y, UIDesignConstants.BUTTON_WIDTH_92, UIDesignConstants.BUTTON_HEIGHT),
                Text = "Save"
            };
>>>>>>> Stashed changes
            btnSave.LeftClick += BtnSave_LeftClick;

            displayOptionsPanel = new DisplayOptionsPanel(WindowManager, UserINISettings.Instance);
            componentsPanel = new ComponentsPanel(WindowManager, UserINISettings.Instance);
            var updaterOptionsPanel = new UpdaterOptionsPanel(WindowManager, UserINISettings.Instance);
            updaterOptionsPanel.OnForceUpdate += (s, e) => { Disable(); OnForceUpdate?.Invoke(this, EventArgs.Empty); };

            optionsPanels = new XNAOptionsPanel[]
            {
                displayOptionsPanel,
                new AudioOptionsPanel(WindowManager, UserINISettings.Instance),
                new GameOptionsPanel(WindowManager, UserINISettings.Instance),
                //new CnCNetOptionsPanel(WindowManager, UserINISettings.Instance, gameCollection),
                updaterOptionsPanel
                //componentsPanel
            };

            if (ClientConfiguration.Instance.ModMode || Updater.UpdateMirrors == null || Updater.UpdateMirrors.Count < 1)
            {
                tabControl.MakeUnselectable(3);
            }

            //else
            //{
            //
            //}

            foreach (var panel in optionsPanels)
            {
                AddChild(panel);
                panel.Load();
                panel.Disable();
            }



            optionsPanels[0].Enable();

            AddChild(tabControl);
            AddChild(btnCancel);
            AddChild(btnSave);

            base.Initialize();

            CenterOnParent();
        }

        /// <summary>
        /// Parses extra options defined by the modder
        /// from an INI file. Called from XNAWindow.SetAttributesFromINI.
        /// </summary>
        /// <param name="iniFile">The INI file.</param>
        protected override void GetINIAttributes(IniFile iniFile)
        {
            base.GetINIAttributes(iniFile);

            foreach (var panel in optionsPanels)
                panel.ParseUserOptions(iniFile);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var panel in optionsPanels)
                panel.Disable();

            optionsPanels[tabControl.SelectedTab].Enable();
            optionsPanels[tabControl.SelectedTab].RefreshPanel();
        }

        private void BtnBack_LeftClick(object sender, EventArgs e)
        {
            if (Updater.IsComponentDownloadInProgress())
            {
                var msgBox = new XNAMessageBox(WindowManager, "Downloads in progress".L10N("UI:DTAConfig:DownloadingTitle"),
                    ("Optional component downloads are in progress. The downloads will be cancelled if you exit the Options menu." +
                    Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to continue?").L10N("UI:DTAConfig:DownloadingText"), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = ExitDownloadCancelConfirmation_YesClicked;

                return;
            }

            WindowManager.SoundPlayer.SetVolume(Convert.ToSingle(UserINISettings.Instance.ClientVolume));
            Disable();
        }

        private void ExitDownloadCancelConfirmation_YesClicked(XNAMessageBox messageBox)
        {
            componentsPanel.CancelAllDownloads();
            WindowManager.SoundPlayer.SetVolume(Convert.ToSingle(UserINISettings.Instance.ClientVolume));
            Disable();
        }

        private void BtnSave_LeftClick(object sender, EventArgs e)
        {
            if (Updater.IsComponentDownloadInProgress())
            {
                XNAMessageBox msgBox = new XNAMessageBox(WindowManager, "Downloads in progress".L10N("UI:DTAConfig:DownloadingTitle"),
                      ("Optional component downloads are in progress. The downloads will be cancelled if you exit the Options menu." +
                      Environment.NewLine + Environment.NewLine +
                      "Are you sure you want to continue?").L10N("UI:DTAConfig:DownloadingText"), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = SaveDownloadCancelConfirmation_YesClicked;

                return;
            }

            SaveSettings();
        }

        private void SaveDownloadCancelConfirmation_YesClicked(XNAMessageBox messageBox)
        {
            componentsPanel.CancelAllDownloads();

            SaveSettings();
        }

        private void SaveSettings()
        {
            if (RefreshOptionPanels())
                return;

            bool restartRequired = false;

            try
            {
                foreach (var panel in optionsPanels)
                    restartRequired = panel.Save() || restartRequired;

                UserINISettings.Instance.SaveSettings();
            }
            catch (Exception ex)
            {
                Logger.Log("Saving settings failed! Error message: " + ex.Message);
                XNAMessageBox.Show(WindowManager, "Saving Settings Failed".L10N("UI:DTAConfig:SaveSettingFailTitle"),
                    "Saving settings failed! Error message:".L10N("UI:DTAConfig:SaveSettingFailText") + " " + ex.Message);
            }

            Disable();

            if (restartRequired)
            {
                var msgBox = new XNAMessageBox(WindowManager, "Restart Required".L10N("UI:DTAConfig:RestartClientTitle"),
                    ("The client needs to be restarted for some of the changes to take effect." +
                    Environment.NewLine + Environment.NewLine +
                    "Do you want to restart now?").L10N("UI:DTAConfig:RestartClientText"), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = RestartMsgBox_YesClicked;
            }
        }

        private void RestartMsgBox_YesClicked(XNAMessageBox messageBox) => WindowManager.RestartGame();

        /// <summary>
        /// Refreshes the option panels to account for possible
        /// changes that could affect theirs functionality.
        /// Shows the popup to inform the user if needed.
        /// </summary>
        /// <returns>A bool that determines whether the 
        /// settings values were changed.</returns>
        private bool RefreshOptionPanels()
        {
            bool optionValuesChanged = false;

            foreach (var panel in optionsPanels)
                optionValuesChanged = panel.RefreshPanel() || optionValuesChanged;

            if (optionValuesChanged)
            {
                XNAMessageBox.Show(WindowManager, "Setting Value(s) Changed".L10N("UI:DTAConfig:SettingChangedTitle"),
                    ("One or more setting values are" + Environment.NewLine +
                    "no longer available and were changed." +
                    Environment.NewLine + Environment.NewLine +
                    "You may want to verify the new setting" + Environment.NewLine +
                    "values in client's options window.").L10N("UI:DTAConfig:SettingChangedText"));

                return true;
            }

            return false;
        }

        public void RefreshSettings()
        {
            foreach (var panel in optionsPanels)
                panel.Load();

            RefreshOptionPanels();

            foreach (var panel in optionsPanels)
                panel.Save();

            UserINISettings.Instance.SaveSettings();
        }

        public void Open()
        {
            foreach (var panel in optionsPanels)
                panel.Load();

            RefreshOptionPanels();

            componentsPanel.Open();

            Enable();
        }

        public void ToggleMainMenuOnlyOptions(bool enable)
        {
            foreach (var panel in optionsPanels)
            {
                panel.ToggleMainMenuOnlyOptions(enable);
            }
        }

        public void SwitchToCustomComponentsPanel()
        {
            foreach (var panel in optionsPanels)
                panel.Disable();

            tabControl.SelectedTab = 5;
        }

        public void InstallCustomComponent(int id) => componentsPanel.InstallComponent(id);

        public void PostInit()
        {
#if TS
            displayOptionsPanel.PostInit();
#endif
        }
    }
}
