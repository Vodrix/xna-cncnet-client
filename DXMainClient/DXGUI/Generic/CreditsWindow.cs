using ClientGUI;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;

namespace DTAClient.DXGUI.Generic
{
    public class CreditsWindow : XNAWindow
    {
        public CreditsWindow(WindowManager windowManager) : base(windowManager) { }

        private XNAPanel panelCredits;

        public override void Initialize()
        {
            //TODO:
            //split the image into header images and credits text it loads from file?
            Name = "CreditsWindow";
            BackgroundTexture = AssetLoader.LoadTexture("scoreviewerbg.png");
            ClientRectangle = new Rectangle(0, 0, 700, 521);

            var btnReturnToMenu = new XNAClientButton(WindowManager);
            btnReturnToMenu.Name = nameof(btnReturnToMenu);
            btnReturnToMenu.ClientRectangle = new Rectangle(270, 486, 160, 23);
            btnReturnToMenu.Text = "Return to Main Menu";
            btnReturnToMenu.LeftClick += BtnReturnToMenu_LeftClick;

            panelCredits = new XNAPanel(WindowManager);
            panelCredits.Name = "panelCredits";
            panelCredits.BackgroundTexture = AssetLoader.LoadTexture("scoreviewerpanelbg.png");
            panelCredits.ClientRectangle = new Rectangle(10, 55, 680, 425);

            AddChild(btnReturnToMenu);

            base.Initialize();

            CenterOnParent();

        }

        private void BtnReturnToMenu_LeftClick(object sender, EventArgs e)
        {
            // To hide the control, just set Enabled=false
            // and MainMenuDarkeningPanel will deal with the rest
            Enabled = false;
        }

    }
}

