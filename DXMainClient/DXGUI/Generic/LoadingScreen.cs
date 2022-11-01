using ClientCore;
using ClientCore.CnCNet5;
using ClientGUI;
using DTAClient.Domain;

using DTAConfig;
using Microsoft.Xna.Framework;
using Rampastring.XNAUI;
using System.Threading.Tasks;
using Rampastring.Tools;
<<<<<<< HEAD
using ClientUpdater;
=======
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b

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

            bool initUpdater = !ClientConfiguration.Instance.ModMode;

            if (initUpdater)
            {
                updaterInitTask = new Task(InitUpdater);
                updaterInitTask.Start();
            }

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
<<<<<<< HEAD

        private void Finish()
        {
            ProgramConstants.GAME_VERSION = ClientConfiguration.Instance.ModMode ?
=======

            Finish();
        }

        private void Finish()
        {
            ProgramConstants.GAME_VERSION = ClientConfiguration.Instance.ModMode ? 
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b
                "N/A" : Updater.GameVersion;

            DiscordHandler discordHandler = null;
            if (!string.IsNullOrEmpty(ClientConfiguration.Instance.DiscordAppId))
                discordHandler = new DiscordHandler(WindowManager);

<<<<<<< HEAD
            var gameCollection = new GameCollection();
            gameCollection.Initialize(GraphicsDevice);

=======
            ClientGUICreator.Instance.AddControl(typeof(GameLobbyCheckBox));
            ClientGUICreator.Instance.AddControl(typeof(GameLobbyDropDown));
            ClientGUICreator.Instance.AddControl(typeof(MapPreviewBox));
            ClientGUICreator.Instance.AddControl(typeof(GameLaunchButton));
            ClientGUICreator.Instance.AddControl(typeof(ChatListBox));
            ClientGUICreator.Instance.AddControl(typeof(XNAChatTextBox));
            ClientGUICreator.Instance.AddControl(typeof(PlayerExtraOptionsPanel));
            var gameCollection = new GameCollection();
            gameCollection.Initialize(GraphicsDevice);


            var cncnetUserData = new CnCNetUserData(WindowManager);
            var cncnetManager = new CnCNetManager(WindowManager, gameCollection, cncnetUserData);
            var tunnelHandler = new TunnelHandler(WindowManager, cncnetManager);
            var privateMessageHandler = new PrivateMessageHandler(cncnetManager, cncnetUserData);
            
            var topBar = new TopBar(WindowManager, cncnetManager, privateMessageHandler);

            var optionsWindow = new OptionsWindow(WindowManager, gameCollection, topBar);

            var pmWindow = new PrivateMessagingWindow(WindowManager,
                cncnetManager, gameCollection, cncnetUserData, privateMessageHandler);
            privateMessagingPanel = new PrivateMessagingPanel(WindowManager);

            var cncnetGameLobby = new CnCNetGameLobby(WindowManager,
                "MultiplayerGameLobby", topBar, cncnetManager, tunnelHandler, gameCollection, cncnetUserData, mapLoader, discordHandler, pmWindow);
            var cncnetGameLoadingLobby = new CnCNetGameLoadingLobby(WindowManager, 
                topBar, cncnetManager, tunnelHandler, mapLoader.GameModes, gameCollection, discordHandler);
            var cncnetLobby = new CnCNetLobby(WindowManager, cncnetManager, 
                cncnetGameLobby, cncnetGameLoadingLobby, topBar, pmWindow, tunnelHandler,
                gameCollection, cncnetUserData, optionsWindow);
            var gipw = new GameInProgressWindow(WindowManager);

            var skirmishLobby = new SkirmishLobby(WindowManager, topBar, mapLoader, discordHandler);

            topBar.SetSecondarySwitch(cncnetLobby);

            var mainMenu = new MainMenu(WindowManager, skirmishLobby, lanLobby,
                topBar, optionsWindow, cncnetLobby, cncnetManager, discordHandler);
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b
            var optionsWindow = new OptionsWindow(WindowManager, gameCollection);

            var gipw = new GameInProgressWindow(WindowManager);

<<<<<<< HEAD
=======
            var mainMenu = new MainMenu(WindowManager, optionsWindow, discordHandler);
            WindowManager.AddAndInitializeControl(mainMenu);
>>>>>>> e76474081c28fa7e61dbab5dff28b8aba5d63d1b

            var mainMenu = new MainMenu(WindowManager, optionsWindow, discordHandler);
            WindowManager.AddAndInitializeControl(mainMenu);

            DarkeningPanel.AddAndInitializeWithControl(WindowManager, optionsWindow);

            WindowManager.AddAndInitializeControl(gipw);
            optionsWindow.Disable();

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
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
