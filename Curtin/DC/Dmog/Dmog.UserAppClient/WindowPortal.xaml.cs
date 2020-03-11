/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dmog.ServerSupport;
using Dmog.GameObjects;
using Dmog.PortalServer;

using System.ServiceModel;
using System.Runtime.CompilerServices;

namespace Dmog.UserAppClient
{
    /// <summary>
    /// Interaction logic for WindowPortal.xaml
    /// </summary>
    //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
    //UseSynchronizationContext = false)]
    public partial class WindowPortal : Window
    {
        ///// FIELDS /////////////////////////////////////
        private App Parent;
        private GameDetail GSSelection; // The selected Game server in list view
        private GameListing GSListing;  // The Stores the Game server list used by list view
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public WindowPortal(App app)
        {
            InitializeComponent();
            Parent = app;
            GSSelection = null;
            GSListing = null;

            this.btnJoinServer.IsEnabled = false;
        }
        //////////////////////////////////////////////////

        #region "///// GUI EVENTS /////"
        private void winPortal_Loaded(object sender, RoutedEventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void winPortal_Closing(object sender, System.ComponentModel.CancelEventArgs e) {}

        private void lvServerListing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                GSSelection = (GameDetail)e.AddedItems[0];
                UpdateServerDetailsView(GSSelection);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.btnJoinServer.IsEnabled = false;

            LoadFriendList();
            LoadHeroList();
            LoadGameServerListing();
        }

        private void btnJoinServer_Click(object sender, RoutedEventArgs e)
        {
            Parent.GSConnection.Address = GSSelection.ServerAddress;
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult retry = MessageBoxResult.No;

            do
            {
                try
                {
                    Parent.UPSConnection.Logout(Parent.Token);
                    retry = MessageBoxResult.No;
                } catch (ServerConnectionException) {
                    retry = MessageBox.Show("Problem connecting to portal server!\nAttempt to reconnect and retry?",
                                            "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (retry == MessageBoxResult.Yes)
                        Parent.UPSConnection.Reconnect();

                } catch (PortalServerException) {
                    retry = MessageBox.Show("The portal server could not process your request at this time!\nRetry?",
                                            "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                }
            } while (retry == MessageBoxResult.Yes);

            Parent.Token = null;
            this.Close();
        }

        private void UpdateServerDetailsView(GameDetail gsd)
        {
            if (gsd != null)
            {
                bool serverStatus = TestGameServerStatus(gsd.ServerName);
                string[] friendslist = GetFriendsFromPlayerList(gsd.PlayerList).ToArray();

                this.lbDetail_ServerName.Content = gsd.ServerName;
                this.lbDetail_ServerStatus.Content = serverStatus.ToString();
                this.lbDetail_JoinedFriends.Content = friendslist.Length;
                this.ltbDetail_ListedFriends.ItemsSource = friendslist;
                this.btnJoinServer.IsEnabled = serverStatus;
            }
        }
        #endregion
        
        /// <summary>
        /// Gets friends list from portal
        /// </summary>
        private void LoadFriendList()
        {
            try
            {
                string[] result = Parent.UPSConnection.QueryFriends(Parent.Token);
                if (result != null)
                {
                    Parent.FriendList.Clear();
                    Parent.FriendList.AddRange(result);
                }
            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to portal server, attempting to reconnect",
                                "Error Loading Friends List!");
                Parent.UPSConnection.Reconnect();
            } catch (PortalServerException) {
                MessageBox.Show("The portal server could not process your request at this time",
                                "Error Loading Friends List!");
            }
        }

        /// <summary>
        /// Gets hero list from portal
        /// </summary>
        private void LoadHeroList()
        {
            try
            {
                Hero[] result = Parent.UPSConnection.GetHeroTable(Parent.Token);
                if (result != null)
                {
                    Parent.HeroList.Clear();
                    Parent.HeroList.AddRange(result);
                }
            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to portal server, attempting to reconnect",
                                "Error Loading Hero List!");
                Parent.UPSConnection.Reconnect();
            } catch (PortalServerException) {
                MessageBox.Show("The portal server could not process your request at this time",
                                "Error Loading Hero List!");
            }
        }

        /// <summary>
        /// Gets game server list from portal
        /// </summary>
        private void LoadGameServerListing()
        {
            try
            {
                GSListing = Parent.UPSConnection.QueryGameServers(Parent.Token);
                this.lvServerListing.ItemsSource = GSListing.GameServerDetailList;
            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to portal server, attempting to reconnect",
                                "Error Loading Game Server List!");
                Parent.UPSConnection.Reconnect();
            } catch (PortalServerException) {
                MessageBox.Show("The portal server could not process your request at this time",
                                "Error Loading Game Server List!");
            }
        }
        
        /// <summary>
        /// Searches Game server player list for friends and returns result
        /// </summary>
        private List<string> GetFriendsFromPlayerList(List<string> playerList)
        {
            List<string> result = new List<string>();
            if (Parent.FriendList != null && playerList != null)
            {
                foreach (string username in playerList)
                {
                    if (Parent.FriendList.Contains(username))
                        result.Add(username);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Pings game server to test avaliability
        /// </summary>
        private bool TestGameServerStatus(string gameServerName)
        {
            return (Parent.UPSConnection.PingGameServer(gameServerName));
        }
    }
}
