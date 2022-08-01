using ClientGUI;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using System;

namespace DTAClient.DXGUI.Generic
{
    public class CampaignSelect : XNAWindow
    {
        public CampaignSelect(WindowManager windowManager) : base(windowManager)
        {

        }

        private XNAClientButton btnNewCampaignRA2;
        private XNAClientButton btnNewCampaignBonus;
        private XNAClientButton btnNewCampaignYR;
        private XNAClientButton btnCancel;

        public override void Initialize()
        {
            Name = nameof(CampaignSelect);
            ClientRectangle = new Rectangle(0, 0, 284, 190);
            BackgroundTexture = AssetLoader.LoadTexture("CampaignSelect.png");

            WindowManager.CenterControlOnScreen(this);

            btnNewCampaignRA2 = new XNAClientButton(WindowManager);
            btnNewCampaignRA2.Name = nameof(btnNewCampaignRA2);
            btnNewCampaignRA2.ClientRectangle = new Rectangle(76, 15, 133, 23);
            btnNewCampaignRA2.Text = "Flipped Campaign";
            btnNewCampaignRA2.LeftClick += new EventHandler(BtnNewCampaignRA2_LeftClick);

            btnNewCampaignBonus = new XNAClientButton(WindowManager);
            btnNewCampaignBonus.Name = nameof(btnNewCampaignBonus);
            btnNewCampaignBonus.ClientRectangle = new Rectangle(76, 30, 133, 23);
            btnNewCampaignBonus.Text = "Bonus Missions";
            btnNewCampaignBonus.LeftClick += new EventHandler(BtnNewCampaignBonus_LeftClick);

            btnNewCampaignYR = new XNAClientButton(WindowManager);
            btnNewCampaignYR.Name = nameof(btnNewCampaignYR);
            btnNewCampaignYR.ClientRectangle = new Rectangle(76, 45, 133, 23);
            btnNewCampaignYR.Text = "Campaign";
            btnNewCampaignYR.LeftClick += new EventHandler(BtnNewCampaignYR_LeftClick);

            btnCancel = new XNAClientButton(WindowManager);
            btnCancel.Name = nameof(btnCancel);
            btnCancel.ClientRectangle = new Rectangle(76, 90, 133, 23);
            btnCancel.Text = "Cancel";
            btnCancel.LeftClick += new EventHandler(BtnCancel_LeftClick);

            AddChild(btnNewCampaignRA2);
            AddChild(btnNewCampaignBonus);
            AddChild(btnNewCampaignYR);
            AddChild(btnCancel);
            base.Initialize();
            CenterOnParent();
        }

        private void BtnNewCampaignRA2_LeftClick(object sender, EventArgs e)
        {
            MainMenuDarkeningPanel parent = (MainMenuDarkeningPanel)Parent;
            parent.Show(parent.CampaignRA2);
        }

        private void BtnNewCampaignBonus_LeftClick(object sender, EventArgs e)
        {
            MainMenuDarkeningPanel parent = (MainMenuDarkeningPanel)Parent;
            parent.Show(parent.CampaignBonus);
        }

        private void BtnNewCampaignYR_LeftClick(object sender, EventArgs e)
        {
            MainMenuDarkeningPanel parent = (MainMenuDarkeningPanel)Parent;
            parent.Show(parent.CampaignYR);
        }
        private void BtnCancel_LeftClick(object sender, EventArgs e)
        {
            Enabled = false;
        }
    }
}
