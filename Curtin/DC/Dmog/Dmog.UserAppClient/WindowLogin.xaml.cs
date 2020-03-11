/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Windows;
using Dmog.ServerSupport;
using Dmog.PortalServer;

using System.ServiceModel;
using System.Runtime.CompilerServices;

namespace Dmog.UserAppClient
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
                  //UseSynchronizationContext = false)]
    public partial class WindowLogin : Window
    {
        ///// FIELDS /////////////////////////////////////
        private App Parent;
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public WindowLogin(App app)
        {
            InitializeComponent();
            Parent = app;
            tbUsername.Focus();
        }
        //////////////////////////////////////////////////

        #region "///// GUI EVENTS /////"
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text;
            string password = pbPassword.Password;
            Parent.Token = null;
            
            if (username != "" && password != "")
            {
                try
                {
                    Parent.Token = Parent.UPSConnection.Login("", username, password);

                    if (Parent.Token != null)
                    {
                        Parent.Username = username;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Login failed: Invalid credentials!");

                } catch (ServerConnectionException) {
                    MessageBox.Show("Problem connecting to portal server, please try again later");
                    Parent.UPSConnection.Reconnect();
                } catch (PortalServerException) {
                    MessageBox.Show("The portal server is currently under maintenance, please try again later");
                }
            }
            else
                MessageBox.Show("Login failed: Username/password fields are empty!");
        }

        private void winLogin_Closed(object sender, EventArgs e){}
        
        private void tbUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                pbPassword.Focus();
        }

        private void pbPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                btnLogin_Click(null, null);
        }
        #endregion
    }
}
