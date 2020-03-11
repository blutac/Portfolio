/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dmog.ServerSupport;
using Dmog.GameObjects;
using Dmog.PortalServer;
using Dmog.GameServer;

using System.ServiceModel;
using System.Runtime.CompilerServices;


namespace Dmog.UserAppClient
{
    /// <summary>
    /// Interaction logic for WindowGameLobby.xaml
    /// </summary>
    //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                  //UseSynchronizationContext = false)]
    public partial class WindowGameLobby : Window
    {
        ///// FIELDS /////////////////////////////////////
        private App Parent;
        private bool ConfirmClose;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public WindowGameLobby(App app)
        {
            InitializeComponent();
            Parent = app;
            ConfirmClose = true;

            this.lvHeroSelection.ItemsSource = Parent.HeroList;
            this.lvPlayers.ItemsSource = Parent.PlayerList.PlayerDetailList.ToArray();
        }
        //////////////////////////////////////////////////

        #region "///// GUI EVENTS /////"
        private void winGameLobby_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateReadyButton();
        }

        private void lvHeroSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Parent.HeroSelection = (Hero)e.AddedItems[0];
                UpdateReadyButton();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parent.GSConnection.QueryGameReadyState(Parent.Token);
                UpdateReadyButton();
                Parent.PlayerList = Parent.GSConnection.QueryPlayerListing(Parent.Token);
                this.lvPlayers.ItemsSource = Parent.PlayerList.PlayerDetailList.ToArray();

            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to game server, attempting to reconnect",
                                "Error Loading Player Listing!");
                Parent.GSConnection.Reconnect(Parent, Parent.Token);
            } catch (PortalServerException) {
                MessageBox.Show("The game server could not process your request at this time",
                                "Error Loading Player Listing!");
            }
        }

        private void btnReady_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parent.GSConnection.SubmitPlayerHero(Parent.Token, Parent.HeroSelection.Id);
                CloseWindow(true);
            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to game server, attempting to reconnect",
                                "Error Submitting hero selection!");
                Parent.GSConnection.Reconnect(Parent, Parent.Token);
            } catch (PortalServerException) {
                MessageBox.Show("The game server could not process your request at this time",
                                "Error Submitting hero selection!");
            }
        }

        private void btnLeaveGame_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult retry = MessageBoxResult.No;

            do
            {
                try
                {
                    Parent.GSConnection.LeaveGameServer(Parent.Username, Parent.Token);
                    retry = MessageBoxResult.No;

                } catch (ServerConnectionException) {
                    retry = MessageBox.Show("Problem connecting to game server!\nAttempt to reconnect and retry?",
                                            "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (retry == MessageBoxResult.Yes)
                        Parent.GSConnection.Reconnect(Parent, Parent.Token);

                } catch (GameServerException) {
                    retry = MessageBox.Show("The game server rejected your request!\nRetry?",
                                            "Error!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                }
            } while (retry == MessageBoxResult.Yes);

            Parent.GSConnection.Address = null;
            CloseWindow(false);
        }

        private void winGameLobby_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ConfirmClose)
            {
                MessageBoxResult close = MessageBox.Show("Are you sure you wish to leave the game?", "Leave Game?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (close == MessageBoxResult.No)
                    e.Cancel = true;
                else
                    btnLeaveGame_Click(null, null);
            }
        }
        #endregion

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void CloseWindow(bool returnValue)
        {
            ConfirmClose = false;
            this.DialogResult = returnValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateReadyButton()
        {
            this.btnReady.IsEnabled = (Parent.HeroSelection != null && Parent.GameServerReady == true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void TriggerRefresh()
        {
            btnRefresh_Click(null, null);
        }
    }
}
