using ClientGUI;
using DTAClient.Domain;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;

namespace DTAClient.DXGUI.Generic
{
    /// <summary>
    /// TODO Replace this class with DarkeningPanels.
    /// Handles transitions between the main menu and its sub-menus.
    /// </summary>
    public class MainMenuDarkeningPanel : XNAPanel
    {
        public MainMenuDarkeningPanel(WindowManager windowManager, DiscordHandler discordHandler) : base(windowManager)
        {
            this.discordHandler = discordHandler;
            DrawBorders = false;
            DrawMode = ControlDrawMode.UNIQUE_RENDER_TARGET;
        }

        private DiscordHandler discordHandler;

        public MissionSelect CampaignRA2;
        public MissionSelect CampaignBonus;
        public MissionSelect CampaignYR;
        public CampaignSelect CampaignSelect;
        public GameLoadingWindow GameLoadingWindow;
        public CreditsWindow CreditsWindow;
        public UpdateQueryWindow UpdateQueryWindow;
        public ManualUpdateQueryWindow ManualUpdateQueryWindow;
        public UpdateWindow UpdateWindow;

        public override void Initialize()
        {
            base.Initialize();

            Name = "DarkeningPanel";
            BorderColor = UISettings.ActiveSettings.PanelBorderColor;
            BackgroundTexture = AssetLoader.CreateTexture(new Color(0, 0, 0, 128), 1, 1);
            PanelBackgroundDrawMode = PanelBackgroundImageDrawMode.STRETCHED;
            Alpha = 1.0f;

            CampaignSelect = new CampaignSelect(WindowManager);
            AddChild(CampaignSelect);

            CampaignRA2 = new MissionSelect(0, WindowManager, discordHandler);
            AddChild(CampaignRA2);

            CampaignBonus = new MissionSelect(1, WindowManager, discordHandler);
            AddChild(CampaignBonus);

            CampaignYR = new MissionSelect(2, WindowManager, discordHandler);
            AddChild(CampaignYR);

            GameLoadingWindow = new GameLoadingWindow(WindowManager, discordHandler);
            AddChild(GameLoadingWindow);

            CreditsWindow = new CreditsWindow(WindowManager);
            AddChild(CreditsWindow);

            UpdateQueryWindow = new UpdateQueryWindow(WindowManager);
            AddChild(UpdateQueryWindow);

            ManualUpdateQueryWindow = new ManualUpdateQueryWindow(WindowManager);
            AddChild(ManualUpdateQueryWindow);

            UpdateWindow = new UpdateWindow(WindowManager);
            AddChild(UpdateWindow);

            foreach (XNAControl child in Children)
            {
                child.Visible = false;
                child.Enabled = false;
                child.EnabledChanged += Child_EnabledChanged;
            }
        }

        private void Child_EnabledChanged(object sender, EventArgs e)
        {
            XNAWindow child = (XNAWindow)sender;
            if (!child.Enabled)
                Hide();
        }

        public void Show(XNAControl control)
        {
            foreach (XNAControl child in Children)
            {
                child.Enabled = false;
                child.Visible = false;
            }

            Enabled = true;
            Visible = true;

            AlphaRate = DarkeningPanel.ALPHA_RATE;

            if (control != null)
            {
                control.Enabled = true;
                control.Visible = true;
                control.IgnoreInputOnFrame = true;
            }
        }

        public void Hide()
        {
            AlphaRate = -DarkeningPanel.ALPHA_RATE;

            foreach (XNAControl child in Children)
            {
                child.Enabled = false;
                child.Visible = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Alpha <= 0f)
            {
                Enabled = false;
                Visible = false;

                foreach (XNAControl child in Children)
                {
                    child.Visible = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DrawTexture(BackgroundTexture, Point.Zero, Color.White);
            base.Draw(gameTime);
        }
    }
}
