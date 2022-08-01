using ClientCore;
using ClientGUI;
using Localization;
using DTAConfig.Settings;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;

namespace DTAConfig.OptionPanels
{
    class GameOptionsPanel : XNAOptionsPanel
    {

<<<<<<< Updated upstream
#if TS
        private const string TEXT_BACKGROUND_COLOR_TRANSPARENT = "0";
        private const string TEXT_BACKGROUND_COLOR_BLACK = "12";
#endif
=======
>>>>>>> Stashed changes
        private const int MAX_SCROLL_RATE = 6;
        private const int MAX_SPEED = 6;

        public GameOptionsPanel(WindowManager windowManager, UserINISettings iniSettings)
            : base(windowManager, iniSettings)
        {
        }

        private XNALabel lblScrollRateValue;

        private XNATrackbar trbScrollRate;
        private XNAClientCheckBox chkTargetLines;
        private XNAClientCheckBox chkScrollCoasting;
        private XNAClientCheckBox chkTooltips;

        //private XNAClientCheckBox chkAlstar;

#if YR || DEBUG
        private XNAClientCheckBox chkShowHiddenObjects;
#elif TS
        private XNAClientCheckBox chkAltToUndeploy;
        private XNAClientCheckBox chkBlackChatBackground;
#endif

        //private XNAClientDropDown ddSpeedSelector;

        //private XNALabel lblSpeedSliderValue;
        //private XNATrackbar trbSpeedSlider;

        //private XNATextBox tbPlayerName;

        XNAClientCheckBox chkDiscordIntegration;

        private HotkeyConfigurationWindow hotkeyConfigWindow;

        public override void Initialize()
        {
            base.Initialize();

            Name = "GameOptionsPanel";

<<<<<<< Updated upstream
            var lblScrollRate = new XNALabel(WindowManager);
            lblScrollRate.Name = "lblScrollRate";
            lblScrollRate.ClientRectangle = new Rectangle(12,
                14, 0, 0);
            lblScrollRate.Text = "Scroll Rate:".L10N("UI:DTAConfig:ScrollRate");
=======
            var lblScrollRate = new XNALabel(WindowManager)
            {
                Name = "lblScrollRate",
                ClientRectangle = new Rectangle(12,
                14, 0, 0),
                Text = "Scroll Rate:"
            };
>>>>>>> Stashed changes

            lblScrollRateValue = new XNALabel(WindowManager)
            {
                Name = "lblScrollRateValue",
                FontIndex = 1,
                Text = "3"
            };
            lblScrollRateValue.ClientRectangle = new Rectangle(
                Width - lblScrollRateValue.Width - 12,
                lblScrollRate.Y, 0, 0);

            trbScrollRate = new XNATrackbar(WindowManager)
            {
                Name = "trbClientVolume",
                ClientRectangle = new Rectangle(
                lblScrollRate.Right + 32,
                lblScrollRate.Y - 2,
                lblScrollRateValue.X - lblScrollRate.Right - 47,
                22),
                BackgroundTexture = AssetLoader.CreateTexture(new Color(0, 0, 0, 128), 2, 2),
                MinValue = 0,
                MaxValue = MAX_SCROLL_RATE
            };
            trbScrollRate.ValueChanged += TrbScrollRate_ValueChanged;

<<<<<<< Updated upstream
            chkScrollCoasting = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "ScrollMethod", true, "0", "1");
            chkScrollCoasting.Name = "chkScrollCoasting";
            chkScrollCoasting.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                trbScrollRate.Bottom + 20, 0, 0);
            chkScrollCoasting.Text = "Scroll Coasting".L10N("UI:DTAConfig:ScrollCoasting");

            chkTargetLines = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "UnitActionLines");
            chkTargetLines.Name = "chkTargetLines";
            chkTargetLines.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkScrollCoasting.Bottom + 24, 0, 0);
            chkTargetLines.Text = "Target Lines".L10N("UI:DTAConfig:TargetLines");

            chkTooltips = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "ToolTips");
            chkTooltips.Name = "chkTooltips";
            chkTooltips.Text = "Tooltips".L10N("UI:DTAConfig:Tooltips");

            var lblPlayerName = new XNALabel(WindowManager);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Text = "Player Name*:".L10N("UI:DTAConfig:PlayerName");

#if YR
            chkShowHiddenObjects = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "ShowHidden");
            chkShowHiddenObjects.Name = "chkShowHiddenObjects";
            chkShowHiddenObjects.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkTargetLines.Bottom + 24, 0, 0);
            chkShowHiddenObjects.Text = "Show Hidden Objects".L10N("UI:DTAConfig:YRShowHidden");
=======
            //var lblSpeedSlider = new XNALabel(WindowManager);
            //lblSpeedSlider.Name = "lblSpeedSlider";
            //lblSpeedSlider.ClientRectangle = new Rectangle(12, lblScrollRate.Bottom + 20, 0, 0);
            //lblSpeedSlider.Text = "Game Speed:";
            //
            //lblSpeedSliderValue = new XNALabel(WindowManager);
            //lblSpeedSliderValue.Name = "lblSpeedSliderValue";
            //lblSpeedSliderValue.FontIndex = 1;
            //lblSpeedSliderValue.Text = "4";
            //lblSpeedSliderValue.ClientRectangle = new Rectangle(
            //    Width - lblSpeedSliderValue.Width - 12,
            //    lblSpeedSlider.Y, 0, 0);
            //
            //trbSpeedSlider = new XNATrackbar(WindowManager);
            //trbSpeedSlider.Name = "trbSpeedSlider";
            //trbSpeedSlider.ClientRectangle = new Rectangle(lblSpeedSlider.Right + 22,
            //    lblSpeedSlider.Y - 2,
            //    lblSpeedSliderValue.X - lblScrollRate.Right - 47,
            //    22);
            //trbSpeedSlider.BackgroundTexture = AssetLoader.CreateTexture(new Color(0, 0, 0, 128), 2, 2);
            //trbSpeedSlider.MinValue = 0;
            //trbSpeedSlider.MaxValue = MAX_SPEED;
            //trbSpeedSlider.ValueChanged += TrbSpeedSlider_ValueChanged;

            chkScrollCoasting = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkScrollCoasting",
                ClientRectangle = new Rectangle(
                lblScrollRate.X,
                trbScrollRate.Bottom + 20, 0, 0),
                Text = "Scroll Coasting"
            };

            chkTargetLines = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkTargetLines",
                ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkScrollCoasting.Bottom + 24, 0, 0),
                Text = "Target Lines"
            };

            chkTooltips = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkTooltips",
                Text = "Tooltips"
            };

            var lblPlayerName = new XNALabel(WindowManager)
            {
                Name = "lblPlayerName",
                Text = ""
            };

#if YR || DEBUG || ARES
            chkShowHiddenObjects = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkShowHiddenObjects",
                ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkTargetLines.Bottom + 24, 0, 0),
                Text = "Show Hidden Objects"
            };
>>>>>>> Stashed changes

            chkTooltips.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkShowHiddenObjects.Bottom + 24, 0, 0);

            lblPlayerName.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkTooltips.Bottom + 30, 0, 0);

            AddChild(chkShowHiddenObjects);
#else
            chkTooltips.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkTargetLines.Bottom + 24, 0, 0);
#endif

#if TS
            chkBlackChatBackground = new SettingCheckBox(WindowManager, false, UserINISettings.OPTIONS, "TextBackgroundColor", true, TEXT_BACKGROUND_COLOR_BLACK, TEXT_BACKGROUND_COLOR_TRANSPARENT);
            chkBlackChatBackground.Name = "chkBlackChatBackground";
            chkBlackChatBackground.ClientRectangle = new Rectangle(
                chkScrollCoasting.X,
                chkTooltips.Bottom + 24, 0, 0);
            chkBlackChatBackground.Text = "Use black background for in-game chat messages".L10N("UI:DTAConfig:TSUseBlackBackgroundChat");

            AddChild(chkBlackChatBackground);

            chkAltToUndeploy = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "MoveToUndeploy");
            chkAltToUndeploy.Name = "chkAltToUndeploy";
            chkAltToUndeploy.ClientRectangle = new Rectangle(
                chkScrollCoasting.X,
                chkBlackChatBackground.Bottom + 24, 0, 0);
            chkAltToUndeploy.Text = "Undeploy units by holding Alt key instead of a regular move command".L10N("UI:DTAConfig:TsUndeployAltKey");

            AddChild(chkAltToUndeploy);

            lblPlayerName.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkAltToUndeploy.Bottom + 30, 0, 0);
#endif

<<<<<<< Updated upstream
            tbPlayerName = new XNATextBox(WindowManager);
            tbPlayerName.Name = "tbPlayerName";
            tbPlayerName.MaximumTextLength = ClientConfiguration.Instance.MaxNameLength;
            tbPlayerName.ClientRectangle = new Rectangle(trbScrollRate.X,
                lblPlayerName.Y - 2, 200, 19);
            tbPlayerName.Text = ProgramConstants.PLAYERNAME;

            var lblNotice = new XNALabel(WindowManager);
            lblNotice.Name = "lblNotice";
            lblNotice.ClientRectangle = new Rectangle(lblPlayerName.X,
                lblPlayerName.Bottom + 30, 0, 0);
            lblNotice.Text = ("* If you are currently connected to CnCNet, you need to log out and reconnect" +
                Environment.NewLine + "for your new name to be applied.").L10N("UI:DTAConfig:ReconnectAfterRename");
=======



            //tbPlayerName = new XNATextBox(WindowManager);
            //tbPlayerName.Name = "tbPlayerName";
            //tbPlayerName.MaximumTextLength = ClientConfiguration.Instance.MaxNameLength;
            //tbPlayerName.ClientRectangle = new Rectangle(trbScrollRate.X,
            //    lblPlayerName.Y - 99999, 0, 0);
            //tbPlayerName.Text = ProgramConstants.PLAYERNAME;

            var lblNotice = new XNALabel(WindowManager)
            {
                Name = "lblNotice",
                ClientRectangle = new Rectangle(lblPlayerName.X,
                lblPlayerName.Bottom + 0, 0, 0),
                Text = ""
            };

            chkDiscordIntegration = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkDiscordIntegration",
                ClientRectangle = new Rectangle(lblNotice.X,
                lblNotice.Bottom - 6, 0, 0),
                Text = "Show detailed game info in Discord status"
            };

            if (String.IsNullOrEmpty(ClientConfiguration.Instance.DiscordAppId))
            {
                chkDiscordIntegration.AllowChecking = false;
                chkDiscordIntegration.Checked = false;
            }
            else
            {
                chkDiscordIntegration.AllowChecking = true;
            }

            AddChild(chkDiscordIntegration);
>>>>>>> Stashed changes

            hotkeyConfigWindow = new HotkeyConfigurationWindow(WindowManager);
            DarkeningPanel.AddAndInitializeWithControl(WindowManager, hotkeyConfigWindow);
            hotkeyConfigWindow.Disable();

<<<<<<< Updated upstream
            var btnConfigureHotkeys = new XNAClientButton(WindowManager);
            btnConfigureHotkeys.Name = "btnConfigureHotkeys";
            btnConfigureHotkeys.ClientRectangle = new Rectangle(lblPlayerName.X, lblNotice.Bottom + 36, UIDesignConstants.BUTTON_WIDTH_160, UIDesignConstants.BUTTON_HEIGHT);
            btnConfigureHotkeys.Text = "Configure Hotkeys".L10N("UI:DTAConfig:ConfigureHotkeys");
=======
            var btnConfigureHotkeys = new XNAClientButton(WindowManager)
            {
                Name = "btnConfigureHotkeys",
                ClientRectangle = new Rectangle(lblPlayerName.X, lblNotice.Bottom + 36, 160, 23),
                Text = "Configure Hotkeys"
            };
>>>>>>> Stashed changes
            btnConfigureHotkeys.LeftClick += BtnConfigureHotkeys_LeftClick;

            //ddSpeedSelector = new XNAClientDropDown(WindowManager);
            //ddSpeedSelector.Name = "ddSpeedSelector";
            //ddSpeedSelector.ClientRectangle = new Rectangle(trbScrollRate.X,
            //    lblPlayerName.Y - 2, 200, 19);
            //ddSpeedSelector.AddItem("Fastest");
            //ddSpeedSelector.AddItem("Faster");
            //ddSpeedSelector.AddItem("Fast");
            //ddSpeedSelector.AddItem("Medium");
            //ddSpeedSelector.AddItem("Slow");
            //ddSpeedSelector.AddItem("Slower");
            //ddSpeedSelector.AddItem("Slowest");
            //ddSpeedSelector.AllowDropDown = true;

            //AddChild(lblSpeedSlider);
            //AddChild(lblSpeedSliderValue);
            //AddChild(trbSpeedSlider);
            //AddChild(ddSpeedSelector);
            AddChild(chkTargetLines);
            AddChild(lblScrollRate);
            AddChild(lblScrollRateValue);
            AddChild(trbScrollRate);
            AddChild(chkScrollCoasting);
            AddChild(chkTooltips);
            AddChild(lblPlayerName);
            //AddChild(tbPlayerName);
            AddChild(lblNotice);
            AddChild(btnConfigureHotkeys);
        }

        private void BtnConfigureHotkeys_LeftClick(object sender, EventArgs e)
        {
            hotkeyConfigWindow.Enable();

        }

        private void TrbScrollRate_ValueChanged(object sender, EventArgs e)
        {
            lblScrollRateValue.Text = trbScrollRate.Value.ToString();
        }

        //private void TrbSpeedSlider_ValueChanged(object sender, EventArgs e)
        //{
        //    lblSpeedSliderValue.Text = trbSpeedSlider.Value.ToString();
        //}

        public override void Load()
        {
            base.Load();

            int scrollRate = ReverseScrollRate(IniSettings.ScrollRate);

            int speed = ReverseSpeed(IniSettings.GameSpeed);

            //if (speed >= trbSpeedSlider.MinValue && speed <= trbSpeedSlider.MaxValue)
            //{
            //    trbSpeedSlider.Value = speed;
            //    lblSpeedSliderValue.Text = speed.ToString();
            //}

            if (scrollRate >= trbScrollRate.MinValue && scrollRate <= trbScrollRate.MaxValue)
            {
                trbScrollRate.Value = scrollRate;
                lblScrollRateValue.Text = scrollRate.ToString();
            }

<<<<<<< Updated upstream
            tbPlayerName.Text = UserINISettings.Instance.PlayerName;
=======
            chkScrollCoasting.Checked = !Convert.ToBoolean(IniSettings.ScrollCoasting);
            chkTargetLines.Checked = IniSettings.TargetLines;
            chkTooltips.Checked = IniSettings.Tooltips;

            chkDiscordIntegration.Checked = !String.IsNullOrEmpty(ClientConfiguration.Instance.DiscordAppId)
                && IniSettings.DiscordIntegration;
#if YR || DEBUG
            chkShowHiddenObjects.Checked = IniSettings.ShowHiddenObjects;
#endif

#if TS
            chkAltToUndeploy.Checked = !IniSettings.MoveToUndeploy;
            chkBlackChatBackground.Checked = IniSettings.TextBackgroundColor == TEXT_BACKGROUND_COLOR_BLACK;
#endif
            //tbPlayerName.Text = UserINISettings.Instance.PlayerName;
>>>>>>> Stashed changes
        }

        public override bool Save()
        {
            bool restartRequired = base.Save();

            IniSettings.ScrollRate.Value = ReverseScrollRate(trbScrollRate.Value);

<<<<<<< Updated upstream
            string playerName = NameValidator.GetValidOfflineName(tbPlayerName.Text);

            if (playerName.Length > 0)
                IniSettings.PlayerName.Value = playerName;
=======
            //IniSettings.GameSpeed.Value = ReverseSpeed(trbSpeedSlider.Value);

            IniSettings.ScrollCoasting.Value = Convert.ToInt32(!chkScrollCoasting.Checked);
            IniSettings.TargetLines.Value = chkTargetLines.Checked;
            IniSettings.Tooltips.Value = chkTooltips.Checked;

            if (!String.IsNullOrEmpty(ClientConfiguration.Instance.DiscordAppId))
            {
                IniSettings.DiscordIntegration.Value = chkDiscordIntegration.Checked;
            }

#if YR || DEBUG
            IniSettings.ShowHiddenObjects.Value = chkShowHiddenObjects.Checked;
#endif

#if TS
            IniSettings.MoveToUndeploy.Value = !chkAltToUndeploy.Checked;
            if (chkBlackChatBackground.Checked)
                IniSettings.TextBackgroundColor.Value = TEXT_BACKGROUND_COLOR_BLACK;
            else
                IniSettings.TextBackgroundColor.Value = TEXT_BACKGROUND_COLOR_TRANSPARENT;
#endif

           // string playerName = NameValidator.GetValidOfflineName(tbPlayerName.Text);
           //
           // if (playerName.Length > 0)
           //     IniSettings.PlayerName.Value = playerName;
>>>>>>> Stashed changes

            return restartRequired;
        }

        private int ReverseScrollRate(int scrollRate)
        {
            return Math.Abs(scrollRate - MAX_SCROLL_RATE);
        }

        private int ReverseSpeed(int speed)
        {
            return Math.Abs(speed - MAX_SPEED);
        }
    }
}
