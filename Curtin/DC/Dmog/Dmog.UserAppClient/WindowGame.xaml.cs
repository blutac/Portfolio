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
    /// Interaction logic for WindowGame.xaml
    /// </summary>
    //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                  //UseSynchronizationContext = false)]
    public partial class WindowGame : Window
    {
        ///// FIELDS /////////////////////////////////////
        private App Parent;
        private Ability AbilitySelection; // Stores currently selected ability
        private string TargetSelection;   // Selected target
        private bool ConfirmClose;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public WindowGame(App app)
        {
            InitializeComponent();
            Parent = app;
            AbilitySelection = null;
            TargetSelection = null;
            LoadListViews();
        }
        //////////////////////////////////////////////////

        private PlayerMove GetPlayerMove()
        {
            PlayerMove move = null;
            if (AbilitySelection != null)
                move = new PlayerMove(AbilitySelection.Id, TargetSelection);

            return move;
        }
        
        #region "///// GUI EVENTS /////"
        private void winGame_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void lvAbilitySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                AbilitySelection = (Ability)e.AddedItems[0];
            }
        }

        private void lvPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TargetSelection = ((PlayerDetail)e.AddedItems[0]).Username;
            }
        }

        private void lvBoss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TargetSelection = ((BotDetail)e.AddedItems[0]).Username;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parent.PlayerList = Parent.GSConnection.QueryPlayerListing(Parent.Token);
                LoadListViews();

            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to game server, attempting to reconnect",
                                "Error Loading Player Listing!");
                Parent.GSConnection.Reconnect(Parent, Parent.Token);
            } catch (PortalServerException) {
                MessageBox.Show("The game server could not process your request at this time",
                                "Error Loading Player Listing!");
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Parent.GSConnection.SubmitPlayerMove(Parent.Token, GetPlayerMove());
            } catch (ServerConnectionException) {
                MessageBox.Show("Problem connecting to game server, attempting to reconnect",
                                "Error Submitting move!");
                Parent.GSConnection.Reconnect(Parent, Parent.Token);
            } catch (GameServerException) {
                MessageBox.Show("The game server could not process your request at this time",
                                "Error Submitting move!");
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
        
        private void winGame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
        public void CloseWindow(bool returnValue)
        {
            ConfirmClose = false;
            this.DialogResult = returnValue;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LoadListViews()
        {
            try
            {
                this.lvBoss.ItemsSource = null;
                this.lvEventLog.ItemsSource = null;
                this.lvPlayers.ItemsSource = null;
                this.lvAbilitySelection.ItemsSource = null;

                this.lvEventLog.ItemsSource = Parent.GameLog.GameEventList;
                this.lvAbilitySelection.ItemsSource = Parent.HeroSelection.Abilities;
                this.lvPlayers.ItemsSource = Parent.PlayerList.PlayerDetailList;
                this.lvBoss.ItemsSource = Parent.BotList.BotDetailList;
            } catch (NullReferenceException) { }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void TriggerRefresh()
        {
            btnRefresh_Click(null, null);
        }
    }
}
