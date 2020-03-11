/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Windows;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;
using Dmog.PortalServer;
using Dmog.GameServer;
using System.Runtime.CompilerServices;

namespace Dmog.UserAppClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                      UseSynchronizationContext = false)]
    public partial class App : Application,
        IUserPortalServerController_Callback, IGameServerController_Callback
    {
        ///// FIELDS /////////////////////////////////////
        internal string PortalAddress = "net.tcp://localhost:50001/DmogPortalServer"; // portal server

        // Windows //
        private WindowLogin WinLogin;
        private WindowPortal WinPortal;
        private WindowGameLobby WinLobby;
        private WindowGame WinGame;

        // User State //
        internal string Username;
        internal string Token;              // User session token (issued by portal)
        internal List<string> FriendList;   // User friend list
        internal List<Hero> HeroList;       // Catched Hero list for viewing hero selection
        internal Hero HeroSelection;        // The selected hero from HeroList (TODO: change this to be an index of the HeroList)
        internal PlayerListing PlayerList;  // Game server player list to store & view updates from game
        internal BotListing BotList;        // Game server Bot player list to store & view updates from game
        internal GameEventLog GameLog;      // Game server event log to store & view updates from game
        internal bool GameServerReady;      // The ready state of the connected game server

        // Server connections //
        internal UserPortalServerConnection UPSConnection;
        internal GameServerConnection GSConnection;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public App()
        {
            // Allows WPF windows to be closed without triggering a shutdown
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Token = null;
            FriendList = new List<string>();
            HeroList = new List<Hero>();
            PlayerList = new PlayerListing();
            GameLog = new GameEventLog();

            UPSConnection = new UserPortalServerConnection();
            GSConnection = new GameServerConnection();

            HeroSelection = null;
            GameServerReady = false;
        }
        //////////////////////////////////////////////////

        private void appUserAppClient_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                UPSConnection.InitiateConnection(this, PortalAddress);      // Start connection with portal server
                // LOGIN WINDOW //
                ShowPortalLogin();

                while (IsLoggedIn())
                {
                    // PORTAL WINDOW //
                    ShowPortal(); // sets the game server address

                    if (GSConnection.Address != null)                       // If a game server was selected
                    {
                        bool inGame = JoinGameServer(GSConnection.Address); // Start connection with game server

                        while (IsLoggedIn() && inGame)
                        {
                            // LOBBY WINDOW //
                            inGame = ShowLobby();

                            if (IsLoggedIn() && inGame && IsGameReady())
                            {
                                // GAME WINDOW //
                                inGame = ShowGame();
                            }
                        }

                        if (inGame == false)
                        {
                            try
                            {
                                GSConnection.CloseConnection();             // Close connection with game server
                            } catch (ServerConnectionException) {}
                        }
                    }

                    // Prompt for login after logout
                    if (IsLoggedIn() == false)
                        ShowPortalLogin();
                }

                // Close connection with portal server
                UPSConnection.CloseConnection();

            } catch (ServerConnectionException) {
                MessageBox.Show("Fatal Error!\nProblem connecting to server.\nPlease try again later",
                                "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (PortalServerException) {
                MessageBox.Show("Fatal Error!\nThe portal server rejected your request.\nPlease try again later",
                                "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (GameServerException) {
                MessageBox.Show("Fatal Error!\nThe game server rejected your request.\nPlease try again later",
                                "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            } catch (CommunicationException) { // Just in case
                MessageBox.Show("Fatal Error!\n something happened, but it's ok cos it was caught.\nPlease try again later",
                                "Fatal Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // not actually needed //
            System.Windows.Forms.Application.Exit();
        }
        
        public bool IsLoggedIn()
        {
            return (Token != null);
        }

        public bool IsGameReady()
        {
            return GameServerReady;
        }

        public bool JoinGameServer(string address)
        {
            MessageBoxResult retry = MessageBoxResult.No;
            bool success = false;

            do
            {
                try
                {
                    GSConnection.InitiateConnection(this, address);
                    success = GSConnection.JoinGameServer(Username, Token);
                    PlayerList = GSConnection.QueryPlayerListing(Token);
                    retry = MessageBoxResult.No;

                } catch (ServerConnectionException) {
                    retry = MessageBox.Show("Problem connecting to game server.\nTry again?",
                            "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                } catch (GameServerException ex) {

                    string reason = "";
                    switch (ex.ErrorCode)
                    {
                        case ECode.ServerFull:
                            reason = "The server is full!";
                            break;
                        default:
                            reason = "";
                            break;
                    }
                    retry = MessageBox.Show("The game server rejected your request.\n" + reason +
                            "\nTry again?", "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                }
            } while (success == false && retry == MessageBoxResult.Yes);
            
            return success;
        }


        #region "///// GUI WINDOWS /////"
        private bool ShowPortalLogin()
        {
            WinLogin = new WindowLogin(this);
            bool bo = (bool)WinLogin.ShowDialog();
            WinLogin = null;
            return bo;
        }

        private bool ShowPortal()
        {
            WinPortal = new WindowPortal(this);
            bool bo = (bool)WinPortal.ShowDialog();
            WinPortal = null;
            return bo;
        }

        private bool ShowLobby()
        {
            WinLobby = new WindowGameLobby(this);
            bool bo = (bool)WinLobby.ShowDialog();
            WinLobby = null;
            return bo;
        }

        private bool ShowGame()
        {
            WinGame = new WindowGame(this);
            bool bo = (bool)WinGame.ShowDialog();
            WinGame = null;
            return bo;
        }
        #endregion


        #region "///// GAME CALLBACKS /////"
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GameServer_OnPlayerChange(PlayerDetail playerDetail)
        {
            if (playerDetail != null)
                PlayerList.PlayerDetailList.Add(playerDetail);
            
            if (WinLobby != null) WinLobby.TriggerRefresh();
            if (WinGame != null) WinGame.TriggerRefresh();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GameServer_OnPlayerLeave(string userPuid)
        {
            foreach (PlayerDetail pd in PlayerList.PlayerDetailList)
            {
                if (pd.Username == userPuid)
                    PlayerList.PlayerDetailList.Remove(pd);
            }
            
            if (WinLobby != null) WinLobby.TriggerRefresh();
            if (WinGame != null) WinGame.TriggerRefresh();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GameServer_OnGameReady(bool ready)
        {
            GameServerReady = ready;

            if (WinLobby != null) WinLobby.TriggerRefresh();
            if (WinGame != null) WinGame.TriggerRefresh();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GameServer_OnGameEnd(bool win)
        {
            if (win)
                MessageBox.Show("Yo, We won guys! :D", "Win", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("You lost the game! D:", "Lost", MessageBoxButton.OK, MessageBoxImage.Information);

            if (WinGame != null) WinGame.CloseWindow(true);

            GameLog.GameEventList.Clear();
            BotList.BotDetailList.Clear();
            PlayerList.PlayerDetailList.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GameServer_OnRoundEnd(BotListing botListing, PlayerListing playerListing, GameEventLog gameLog)
        {
            BotList = botListing;
            PlayerList = playerListing;
            if (gameLog != null)
            {
                gameLog.GameEventList.AddRange(GameLog.GameEventList);
                GameLog.GameEventList = gameLog.GameEventList;
            }
        }
        
        bool IGameServerController_Callback.GameServer_OnPing() { return true; }
        #endregion


        #region "///// PORTAL CALLBACKS /////"
        bool IUserPortalServerController_Callback.UserPortalServer_OnPing() { return true; }
        #endregion
    }
}
