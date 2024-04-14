using ClientCore;
using ClientGUI;
using DTAClient.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;

namespace DTAClient.DXGUI.Generic
{
    /// <summary>
    /// A battle (as in Battle.ini entry) list box that can
    /// draw difficulty rank icons on its items.
    /// </summary>
    public class BattleListBox : XNAListBox
    {
        public BattleListBox(WindowManager windowManager) : base(windowManager)
        {
        }

        private Texture2D[] rankTextures;

        public override void Initialize()
        {
            EnableScrollbar = true;

            base.Initialize();

            rankTextures = new Texture2D[]
            {
                AssetLoader.LoadTexture("rankEasy.png"),
                AssetLoader.LoadTexture("rankNormal.png"),
                AssetLoader.LoadTexture("rankHard.png"),
            };
        }

        private Texture2D DifficultyRankToTexture(int rank)
        {
            switch (rank)
            {
                case 3:
                    return rankTextures[2];
                case 2:
                    return rankTextures[1];
                case 1:
                    return rankTextures[0];
                case 0:
                default:
                    return null;
            }
        }

        private int MissionToDifficultyRank(string mission)
        {
            return ClientConfiguration.Instance.MissionNameToInteger(mission);
        }

        protected override void DrawListBoxItem(int index, int y)
        {
            base.DrawListBoxItem(index, y);

            var lbItem = Items[index];

            var mission = (Mission)lbItem.Tag;

            Texture2D rankTexture = DifficultyRankToTexture(MissionToDifficultyRank(mission.Scenario.ToString().Split('.')[0]));
            if (rankTexture != null)
            {
                DrawTexture(rankTexture,
                    new Rectangle(Width - rankTexture.Width - TextBorderDistance - ScrollBar.Width, y + (LineHeight - rankTexture.Height) / 2,
                    rankTexture.Width, rankTexture.Height), Color.White);
            }
        }
    }
}
