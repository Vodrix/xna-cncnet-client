using ClientCore;
using ClientGUI;
using DTAClient.Domain;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using System.Collections.Generic;
using System.IO;

namespace DTAClient.DXGUI.Generic
{
    public class MissionSelect : XNAWindow
    {
        private const int DEFAULT_WIDTH = 650;
        private const int DEFAULT_HEIGHT = 600;

        private static readonly string[] DifficultyNames = new string[] { "Easy", "Medium", "Hard" };

        private static readonly string[] DifficultyIniPaths = new string[]
        {
            "INI/Easy.ini",
            "INI/Medium.ini",
            "INI/Hard.ini"
        };

        private int campaignValue;

        public MissionSelect(int campaign, WindowManager windowManager, DiscordHandler discordHandler) : base(windowManager)
        {
            campaignValue = campaign;
            this.discordHandler = discordHandler;
        }

        private readonly DiscordHandler discordHandler;

        private readonly List<Mission> Missions = new List<Mission>();
        private BattleListBox lbCampaignList;
        private XNATextBlock tbMissionImage;
        private XNAClientButton btnLaunch;
        private XNATextBlock tbMissionDescription;
        private XNATrackbar trbDifficultySelector;

        private XNACheckBox chkAlstar;
        private XNAClientDropDown ddSpeedSelector;

        public override void Initialize()
        {
            BackgroundTexture = AssetLoader.LoadTexture("missionselectorbg.png");
            ClientRectangle = new Rectangle(0, 0, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            BorderColor = UISettings.ActiveSettings.PanelBorderColor;

            switch (campaignValue)
            {
                case 0:
                    {
                        Name = "CampaignRA2";
                        break;
                    }
                case 1:
                    {
                        Name = "CampaignBonus";
                        break;
                    }
                case 2:
                    {
                        Name = "CampaignYR";
                        break;
                    }
            }

            var lblSelectCampaign = new XNALabel(WindowManager)
            {
                Name = "lblSelectCampaign",
                FontIndex = 1,
                ClientRectangle = new Rectangle(12, 12, 0, 0),
                Text = "MISSIONS:"
            };

            lbCampaignList = new BattleListBox(WindowManager)
            {
                Name = "lbCampaignList",
                BackgroundTexture = AssetLoader.CreateTexture(new Color(0, 0, 0, 128), 2, 2),
                PanelBackgroundDrawMode = PanelBackgroundImageDrawMode.STRETCHED,
                ClientRectangle = new Rectangle(12,
                lblSelectCampaign.Bottom + 6, 300, 516)
            };
            lbCampaignList.SelectedIndexChanged += LbCampaignList_SelectedIndexChanged;

            var lblMissionDescriptionHeader = new XNALabel(WindowManager)
            {
                Name = "lblMissionDescriptionHeader",
                FontIndex = 1,
                ClientRectangle = new Rectangle(
                lbCampaignList.Right + 12,
                lblSelectCampaign.Y, 0, 0),
                Text = "MISSION DESCRIPTION:"
            };

            tbMissionDescription = new XNATextBlock(WindowManager)
            {
                Name = "tbMissionDescription",
                ClientRectangle = new Rectangle(
                lblMissionDescriptionHeader.X,
                lblMissionDescriptionHeader.Bottom + 6,
                Width - 24 - lbCampaignList.Right, 430),
                PanelBackgroundDrawMode = PanelBackgroundImageDrawMode.STRETCHED,
                Alpha = 1.0f
            };

            tbMissionDescription.BackgroundTexture = AssetLoader.CreateTexture(AssetLoader.GetColorFromString(ClientConfiguration.Instance.AltUIBackgroundColor),
                tbMissionDescription.Width, tbMissionDescription.Height);

            var lblDifficultyLevel = new XNALabel(WindowManager)
            {
                Name = "lblDifficultyLevel",
                Text = "DIFFICULTY LEVEL",
                FontIndex = 1
            };
            Vector2 textSize = Renderer.GetTextDimensions(lblDifficultyLevel.Text, lblDifficultyLevel.FontIndex);
            lblDifficultyLevel.ClientRectangle = new Rectangle(
                tbMissionDescription.X + (tbMissionDescription.Width - (int)textSize.X) / 2,
                tbMissionDescription.Bottom + 12, (int)textSize.X, (int)textSize.Y);

            tbMissionImage = new XNATextBlock(WindowManager)
            {
                Name = "tbMissionImage",
                ClientRectangle = new Rectangle(0, 0, Width, Height),
                PanelBackgroundDrawMode = PanelBackgroundImageDrawMode.STRETCHED,
                Alpha = 1.0f,
                BackgroundTexture = AssetLoader.CreateTexture(AssetLoader.GetColorFromString(ClientConfiguration.Instance.AltUIBackgroundColor),
                tbMissionDescription.Width, tbMissionDescription.Height)
            };

            trbDifficultySelector = new XNATrackbar(WindowManager)
            {
                Name = "trbDifficultySelector",
                ClientRectangle = new Rectangle(
                tbMissionDescription.X, lblDifficultyLevel.Bottom + 6,
                tbMissionDescription.Width, 30),
                MinValue = 0,
                MaxValue = 2,
                BackgroundTexture = AssetLoader.CreateTexture(
                new Color(0, 0, 0, 128), 2, 2),
                ButtonTexture = AssetLoader.LoadTextureUncached(
                "trackbarButton_difficulty.png")
            };
            trbDifficultySelector.ValueChanged += TrbDifficultySelector_ValueChanged;

            var lblEasy = new XNALabel(WindowManager)
            {
                Name = "lblEasy",
                FontIndex = 1,
                Text = "EASY",
                ClientRectangle = new Rectangle(trbDifficultySelector.X,
                trbDifficultySelector.Bottom + 6, 1, 1)
            };

            var lblNormal = new XNALabel(WindowManager)
            {
                Name = "lblNormal",
                FontIndex = 1,
                Text = "NORMAL"
            };
            textSize = Renderer.GetTextDimensions(lblNormal.Text, lblNormal.FontIndex);
            lblNormal.ClientRectangle = new Rectangle(
                tbMissionDescription.X + (tbMissionDescription.Width - (int)textSize.X) / 2,
                lblEasy.Y, (int)textSize.X, (int)textSize.Y);

            var lblHard = new XNALabel(WindowManager)
            {
                Name = "lblHard",
                FontIndex = 1,
                Text = "HARD"
            };
            lblHard.ClientRectangle = new Rectangle(
                tbMissionDescription.Right - lblHard.Width,
                lblEasy.Y, 1, 1);

            btnLaunch = new XNAClientButton(WindowManager)
            {
                Name = "btnLaunch",
                ClientRectangle = new Rectangle(12, Height - 35, 133, 23),
                Text = "Launch",
                AllowClick = false
            };
            btnLaunch.LeftClick += BtnLaunch_LeftClick;

            var btnCancel = new XNAClientButton(WindowManager)
            {
                Name = "btnCancel",
                ClientRectangle = new Rectangle(Width - 145, btnLaunch.Y, 133, 23),
                Text = "Cancel"
            };
            btnCancel.LeftClick += BtnCancel_LeftClick;

            chkAlstar = new XNAClientCheckBox(WindowManager)
            {
                Name = "chkAlstar",
                ClientRectangle = new Rectangle(
                btnCancel.X,
                btnCancel.Bottom + 72, 0, 0),
                Text = "Alstar Difficulty Button"
            };

            ddSpeedSelector = new XNAClientDropDown(WindowManager);
            ddSpeedSelector.Name = "ddSpeedSelector";
            ddSpeedSelector.ClientRectangle = new Rectangle(lblHard.X + 63, lblHard.Y + 15, 200, 19);
            ddSpeedSelector.AddItem("Fastest (Uncapped)");
            ddSpeedSelector.AddItem("Faster");
            ddSpeedSelector.AddItem("Fast");
            ddSpeedSelector.AddItem("Medium");
            ddSpeedSelector.AddItem("Slow");
            ddSpeedSelector.AddItem("Slower");
            ddSpeedSelector.AddItem("Slowest");
            ddSpeedSelector.AllowDropDown = true;
            ddSpeedSelector.SelectedIndex = UserINISettings.Instance.GameSpeed.Value;
            ddSpeedSelector.SelectedIndexChanged += DdSpeedSelector_SelectedIndexChanged;

            var lblSpeed = new XNALabel(WindowManager)
            {
                Name = "lblSpeed",
                FontIndex = 1,
                Text = "SPEED",
                ClientRectangle = new Rectangle(ddSpeedSelector.X,
                ddSpeedSelector.Y - ddSpeedSelector.Height + 3, 1, 1)
            };

            AddChild(tbMissionImage);
            AddChild(chkAlstar);
            AddChild(ddSpeedSelector);
            AddChild(lblSelectCampaign);
            AddChild(lblMissionDescriptionHeader);
            AddChild(lbCampaignList);
            AddChild(tbMissionDescription);
            AddChild(lblDifficultyLevel);
            AddChild(btnLaunch);
            AddChild(btnCancel);
            AddChild(trbDifficultySelector);
            AddChild(lblEasy);
            AddChild(lblNormal);
            AddChild(lblHard);
            AddChild(lblSpeed);

            // Set control attributes from INI file
            base.Initialize();

            // Center on screen
            CenterOnParent();
            trbDifficultySelector.Value = UserINISettings.Instance.Difficulty;

            switch (campaignValue)  //the more versatile of doing this would be to read everything from a unique ini
            {
                case 0:
                    {
                        ParseBattleIni("INI/" + ClientConfiguration.Instance.BattleFSFileName);
                        break;
                    }
                case 1:
                    {
                        ParseBattleIni("INI/" + ClientConfiguration.Instance.BattleFSFileName2);
                        break;
                    }
                case 2:
                    {
                        ParseBattleIni("INI/" + ClientConfiguration.Instance.BattleFSFileName3);
                        break;
                    }
            }
        }

        private void DdSpeedSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserINISettings.Instance.GameSpeed.Value = ddSpeedSelector.SelectedIndex;
            UserINISettings.Instance.DefaultGameSpeed.Value = Math.Abs(ddSpeedSelector.SelectedIndex - ddSpeedSelector.Items.Count) - 1;
            UserINISettings.Instance.SaveSettings();
        }

        private void TrbDifficultySelector_ValueChanged(object sender, EventArgs e)
        {
            UserINISettings.Instance.Difficulty.Value = trbDifficultySelector.Value;
            UserINISettings.Instance.SaveSettings();
        }

        private void LbCampaignList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbCampaignList.SelectedIndex == -1)
            {
                tbMissionDescription.Text = string.Empty;
                btnLaunch.AllowClick = false;
                return;
            }

            Mission mission = Missions[lbCampaignList.SelectedIndex];

            if (string.IsNullOrEmpty(mission.Scenario))
            {
                tbMissionDescription.Text = string.Empty;
                btnLaunch.AllowClick = false;
                return;
            }

            tbMissionDescription.Text = mission.GUIDescription;

            if (!mission.Enabled)
            {
                btnLaunch.AllowClick = false;
                return;
            }

            btnLaunch.AllowClick = true;
            btnLaunch.IdleTexture = AssetLoader.LoadTexture("LaunchIdle.png");

            string MissionImage = string.Format("{0}.png", mission.Scenario.Substring(0, mission.Scenario.Length - 4));
            //example filename "ALLIES07.png" meaning that it's whatever map is called that in the battle.ini. (allied 7 ra2)

            if (File.Exists(ProgramConstants.GetBaseResourcePath() + MissionImage))
                tbMissionImage.BackgroundTexture = AssetLoader.LoadTexture(MissionImage);
            else if (File.Exists(ProgramConstants.GetResourcePath() + MissionImage))
                tbMissionImage.BackgroundTexture = AssetLoader.LoadTexture(MissionImage);
            else //this could probably be removed?
                tbMissionImage.BackgroundTexture = AssetLoader.CreateTexture(AssetLoader.GetColorFromString(ClientConfiguration.Instance.AltUIBackgroundColor),
                    tbMissionDescription.Width, tbMissionDescription.Height);
        }

        private void BtnCancel_LeftClick(object sender, EventArgs e)
        {
            MainMenuDarkeningPanel parent = (MainMenuDarkeningPanel)Parent;
            parent.Show(parent.CampaignSelect);
        }

        private void BtnLaunch_LeftClick(object sender, EventArgs e)
        {
            var mission = lbCampaignList.SelectedItem.Tag as Mission;

            LaunchMission(mission);
        }

        /// <summary>
        /// Starts a singleplayer mission.
        /// </summary>
        /// <param name="scenario">The internal name of the scenario.</param>
        /// <param name="requiresAddon">True if the mission is for Firestorm / Enhanced Mode.</param>
        private void LaunchMission(Mission mission)
        {
            bool copyMapsToSpawnmapINI = ClientConfiguration.Instance.CopyMissionsToSpawnmapINI;

            IniFile spawnIni = new IniFile(ProgramConstants.GamePath + ProgramConstants.SPAWNER_SETTINGS);

            UserINISettings.Instance.Difficulty.Value = trbDifficultySelector.Value;

            IniFile difficultyIni = new IniFile(ProgramConstants.GamePath + DifficultyIniPaths[trbDifficultySelector.Value]);
            string difficultyName = DifficultyNames[trbDifficultySelector.Value];

            IniFile NormalINI = new IniFile("INI/" + ClientConfiguration.Instance.RulesPath);

            if (File.Exists(ProgramConstants.GamePath + DifficultyIniPaths[trbDifficultySelector.Value]))
                IniFile.ConsolidateIniFiles(NormalINI, difficultyIni);

            if (chkAlstar.Checked)
            {
                IniFile InsaneINI;
                InsaneINI = mission.IconPath.Equals("Allies") ? new IniFile("INI/" + ClientConfiguration.Instance.AlliedBrutalLocation) : mission.IconPath.Equals("Soviets") ? new IniFile("INI/" + ClientConfiguration.Instance.SovietBrutalLocation) : new IniFile("INI/" + ClientConfiguration.Instance.YuriBrutalLocation);
                IniFile.ConsolidateIniFiles(NormalINI, InsaneINI);
            }

            NormalINI.WriteIniFile(ProgramConstants.GamePath + "rulesfm.ini");
            Logger.Log("About to write spawn.ini.");
            StreamWriter swriter = new StreamWriter(ProgramConstants.GamePath + "spawn.ini");
            swriter.WriteLine("; Generated by DTA Client");
            swriter.WriteLine("[Settings]");
            if (copyMapsToSpawnmapINI)
                swriter.WriteLine("Scenario=spawnmap.ini");
            else
                swriter.WriteLine("Scenario=" + mission.Scenario);
            swriter.WriteLine("GameSpeed=" + UserINISettings.Instance.GameSpeed);
            swriter.WriteLine("IsSinglePlayer=Yes");
            swriter.WriteLine("Side=" + 0); //it's zeroes all the way down
            swriter.WriteLine("BuildOffAlly=" + mission.BuildOffAlly);
            swriter.WriteLine("DifficultyModeHuman=" + (mission.PlayerAlwaysOnNormalDifficulty ? "1" : trbDifficultySelector.Value.ToString()));
            swriter.WriteLine("DifficultyModeComputer=" + GetComputerDifficulty());
            swriter.WriteLine();
            swriter.Close();
            if (copyMapsToSpawnmapINI)
            {
                IniFile mapIni2 = new IniFile(ProgramConstants.GamePath + mission.Scenario);
                IniFile.ConsolidateIniFiles(mapIni2, difficultyIni);
                mapIni2.WriteIniFile(ProgramConstants.GamePath + "spawnmap.ini");
            }

            UserINISettings.Instance.Difficulty.Value = trbDifficultySelector.Value;
            UserINISettings.Instance.SaveSettings();

            ((MainMenuDarkeningPanel)Parent).Hide();    //after game exit, go back to main menu

            discordHandler?.UpdatePresence(mission.GUIName, difficultyName, mission.IconPath, true);
            GameProcessLogic.GameProcessExited += GameProcessExited_Callback;

            GameProcessLogic.StartGameProcess();
        }

        private int GetComputerDifficulty() =>
        Math.Abs(trbDifficultySelector.Value - 2);


        private void GameProcessExited_Callback()
        {
            WindowManager.AddCallback(new Action(GameProcessExited), null);
        }

        protected virtual void GameProcessExited()
        {
            GameProcessLogic.GameProcessExited -= GameProcessExited_Callback;
            Logger.Log("GameProcessExited: Updating Discord Presence.");
            discordHandler?.UpdatePresence();
            ClientConfiguration.Instance.RefreshGlobals();
        }

        /// <summary>
        /// Parses a Battle(E).ini file. Returns true if succesful (file found), otherwise false.
        /// </summary>
        /// <param name="path">The path of the file, relative to the game directory.</param>
        /// <returns>True if succesful, otherwise false.</returns>
        private bool ParseBattleIni(string path)
        {
            Logger.Log("Attempting to parse " + path + " to populate mission list.");

            string battleIniPath = ProgramConstants.GamePath + path;
            if (!File.Exists(battleIniPath))
            {
                Logger.Log("File " + path + " not found. Ignoring.");
                return false;
            }

            IniFile battleIni = new IniFile(battleIniPath);

            List<string> battleKeys = battleIni.GetSectionKeys("Battles");

            if (battleKeys == null)
                return false; // File exists but [Battles] doesn't

            foreach (string battleEntry in battleKeys)
            {
                string battleSection = battleIni.GetStringValue("Battles", battleEntry, "NOT FOUND");

                if (!battleIni.SectionExists(battleSection))
                    continue;

                var mission = new Mission(battleIni, battleSection);

                Missions.Add(mission);

                XNAListBoxItem item = new XNAListBoxItem
                {
                    Text = mission.GUIName,
                    Tag = mission
                };
                if (!mission.Enabled)
                {
                    item.TextColor = UISettings.ActiveSettings.DisabledItemColor;
                }
                else if (string.IsNullOrEmpty(mission.Scenario))
                {
                    item.TextColor = AssetLoader.GetColorFromString(
                        ClientConfiguration.Instance.ListBoxHeaderColor);
                    item.IsHeader = true;
                    item.Selectable = false;
                }
                else
                {
                    item.TextColor = lbCampaignList.DefaultItemColor;
                }

                if (!string.IsNullOrEmpty(mission.IconPath))
                    item.Texture = AssetLoader.LoadTexture(mission.IconPath + "icon.png");

                lbCampaignList.AddItem(item);
            }

            Logger.Log("Finished parsing " + path + ".");
            return true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }

}
