using ClientCore;
using ClientGUI;
using DTAConfig.Settings;
using Localization;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;

namespace DTAConfig.OptionPanels
{
    class GameOptionsPanel : XNAOptionsPanel
    {

#if TS
        private const string TEXT_BACKGROUND_COLOR_TRANSPARENT = "0";
        private const string TEXT_BACKGROUND_COLOR_BLACK = "12";
#endif
        private const int MAX_SCROLL_RATE = 6;

        public GameOptionsPanel(WindowManager windowManager, UserINISettings iniSettings)
            : base(windowManager, iniSettings)
        {
        }

        private XNALabel lblScrollRateValue;

        private XNATrackbar trbScrollRate;
        private XNAClientCheckBox chkTargetLines;
        private XNAClientCheckBox chkScrollCoasting;
        private XNAClientCheckBox chkTooltips;
#if YR
        private XNAClientCheckBox chkShowHiddenObjects;
#elif TS
        private XNAClientCheckBox chkAltToUndeploy;
        private XNAClientCheckBox chkBlackChatBackground;
#endif


        private HotkeyConfigurationWindow hotkeyConfigWindow;

        public override void Initialize()
        {
            base.Initialize();

            Name = "GameOptionsPanel";

            var lblScrollRate = new XNALabel(WindowManager);
            lblScrollRate.Name = "lblScrollRate";
            lblScrollRate.ClientRectangle = new Rectangle(12,
                14, 0, 0);
            lblScrollRate.Text = "Scroll Rate:".L10N("UI:DTAConfig:ScrollRate");

            lblScrollRateValue = new XNALabel(WindowManager);
            lblScrollRateValue.Name = "lblScrollRateValue";
            lblScrollRateValue.FontIndex = 1;
            lblScrollRateValue.Text = "3";
            lblScrollRateValue.ClientRectangle = new Rectangle(
                Width - lblScrollRateValue.Width - 12,
                lblScrollRate.Y, 0, 0);

            trbScrollRate = new XNATrackbar(WindowManager);
            trbScrollRate.Name = "trbClientVolume";
            trbScrollRate.ClientRectangle = new Rectangle(
                lblScrollRate.Right + 32,
                lblScrollRate.Y - 2,
                lblScrollRateValue.X - lblScrollRate.Right - 47,
                22);
            trbScrollRate.BackgroundTexture = AssetLoader.CreateTexture(new Color(0, 0, 0, 128), 2, 2);
            trbScrollRate.MinValue = 0;
            trbScrollRate.MaxValue = MAX_SCROLL_RATE;
            trbScrollRate.ValueChanged += TrbScrollRate_ValueChanged;

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

#if YR
            chkShowHiddenObjects = new SettingCheckBox(WindowManager, true, UserINISettings.OPTIONS, "ShowHidden");
            chkShowHiddenObjects.Name = "chkShowHiddenObjects";
            chkShowHiddenObjects.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkTargetLines.Bottom + 24, 0, 0);
            chkShowHiddenObjects.Text = "Show Hidden Objects".L10N("UI:DTAConfig:YRShowHidden");

            chkTooltips.ClientRectangle = new Rectangle(
                lblScrollRate.X,
                chkShowHiddenObjects.Bottom + 24, 0, 0);

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

            hotkeyConfigWindow = new HotkeyConfigurationWindow(WindowManager);
            DarkeningPanel.AddAndInitializeWithControl(WindowManager, hotkeyConfigWindow);
            hotkeyConfigWindow.Disable();

            var btnConfigureHotkeys = new XNAClientButton(WindowManager);
            btnConfigureHotkeys.Name = "btnConfigureHotkeys";
            btnConfigureHotkeys.ClientRectangle = new Rectangle(10, chkTooltips.Bottom + 36, UIDesignConstants.BUTTON_WIDTH_160, UIDesignConstants.BUTTON_HEIGHT);
            btnConfigureHotkeys.Text = "Configure Hotkeys".L10N("UI:DTAConfig:ConfigureHotkeys");
            btnConfigureHotkeys.LeftClick += BtnConfigureHotkeys_LeftClick;
  
            AddChild(lblScrollRate);
            AddChild(lblScrollRateValue);
            AddChild(trbScrollRate);
            AddChild(chkScrollCoasting);
            AddChild(chkTargetLines);
            AddChild(chkTooltips);
            AddChild(btnConfigureHotkeys);
        }

        private void BtnConfigureHotkeys_LeftClick(object sender, EventArgs e)
        {
            hotkeyConfigWindow.Enable();
            hotkeyConfigWindow.EnabledChanged += HotkeyConfigWindow_EnabledChanged;
        }

        private void HotkeyConfigWindow_EnabledChanged(object sender, EventArgs e)
        {
            hotkeyConfigWindow.EnabledChanged -= HotkeyConfigWindow_EnabledChanged;
        }

        private void TrbScrollRate_ValueChanged(object sender, EventArgs e)
        {
            lblScrollRateValue.Text = trbScrollRate.Value.ToString();
        }

        public override void Load()
        {
            base.Load();

            int scrollRate = ReverseScrollRate(IniSettings.ScrollRate);

            if (scrollRate >= trbScrollRate.MinValue && scrollRate <= trbScrollRate.MaxValue)
            {
                trbScrollRate.Value = scrollRate;
                lblScrollRateValue.Text = scrollRate.ToString();
            }
        }

        public override bool Save()
        {
            bool restartRequired = base.Save();

            IniSettings.ScrollRate.Value = ReverseScrollRate(trbScrollRate.Value);

            return restartRequired;
        }

        private int ReverseScrollRate(int scrollRate)
        {
            return Math.Abs(scrollRate - MAX_SCROLL_RATE);
        }
    }
}
