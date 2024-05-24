using ClientCore;
using ClientCore.CnCNet5;
using ClientGUI;
using ClientUpdater;
using DTAClient.Domain;
using DTAConfig;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DTAClient.DXGUI.Generic
{
    public class LoadingScreen : XNAWindow
    {
        public LoadingScreen(WindowManager windowManager) : base(windowManager)
        {
        }

        private bool visibleSpriteCursor = false;

        private Task updaterInitTask = null;

        public override void Initialize()
        {
            ClientRectangle = new Rectangle(0, 0, 800, 600);
            Name = "LoadingScreen";

            BackgroundTexture = AssetLoader.LoadTexture("loadingscreen.png");
            base.Initialize();

            CenterOnParent();

            updaterInitTask = new Task(InitUpdater);
            updaterInitTask.Start();

            if (Cursor.Visible)
            {
                Cursor.Visible = false;
                visibleSpriteCursor = true;
            }
        }

        private void InitUpdater()
        {
            Updater.OnLocalFileVersionsChecked += LogGameClientVersion;
            Updater.CheckLocalFileVersions();
        }

        private void LogGameClientVersion()
        {
            Logger.Log($"Game Client Version: {ClientConfiguration.Instance.LocalGame} {Updater.GameVersion}");
            Updater.OnLocalFileVersionsChecked -= LogGameClientVersion;
        }

        private void Finish()
        {
            ProgramConstants.GAME_VERSION = Updater.GameVersion;

            DiscordHandler discordHandler = null;
            if (!string.IsNullOrEmpty(ClientConfiguration.Instance.DiscordAppId))
                discordHandler = new DiscordHandler(WindowManager);

            var gameCollection = new GameCollection();
            gameCollection.Initialize(GraphicsDevice);

            var optionsWindow = new OptionsWindow(WindowManager, gameCollection);

            var gipw = new GameInProgressWindow(WindowManager);

            var mainMenu = new MainMenu(WindowManager, optionsWindow, discordHandler);
            WindowManager.AddAndInitializeControl(mainMenu);

            DarkeningPanel.AddAndInitializeWithControl(WindowManager, optionsWindow);

            WindowManager.AddAndInitializeControl(gipw);
            optionsWindow.Disable();

            //loadTask = mainMenu.AnimateAsync();
            mainMenu.AnimateAsyncTest();

            mainMenu.PostInit();

            WindowManager.RemoveControl(this);

            Cursor.Visible = visibleSpriteCursor;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (updaterInitTask == null || updaterInitTask.Status == TaskStatus.RanToCompletion)
            {
                    Finish();
                //if (loadTask.Status == TaskStatus.RanToCompletion)
                //    TestStuff();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
