/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace Dmog.ServerSupport
{
    /// <summary>
    /// Represents the client side connection to a server.
    /// Stores channel data and provides functionality to initiate a connection.
    /// </summary>
    /// <typeparam name="TChannel">The interface of the server that this object is to connect to</typeparam>
    /// <typeparam name="TCallbackChannel">The callback interface of the server that this object is to connect to</typeparam>
    public abstract class ServerConnection<TChannel, TCallbackChannel>
    {
        ///// FIELDS /////////////////////////////////////
        public string Address { get; set; }         // Server address
        protected DuplexChannelFactory<TChannel> Channel;
        protected TChannel Server;                  // Server channel object
        private TCallbackChannel CallbackObject;    // Local callback object
        //////////////////////////////////////////////////

        ///// STRUCTORS //////////////////////////////////
        public ServerConnection() {}
        //////////////////////////////////////////////////

        /// <summary>
        /// Initialises the connection to a server.
        /// </summary>
        /// <param name="callbackObject">The callback object that the client listens for server callbacks on</param>
        /// <param name="address">The server address to connect to</param>
        public void InitiateConnection(TCallbackChannel callbackObject, string address)
        {
            // Setup networking //
            Address = address;
            CallbackObject = callbackObject;
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            // Setup channel //
            try
            {
                Channel = new DuplexChannelFactory<TChannel>(CallbackObject, tcpBinding, Address);
                Server = Channel.CreateChannel();
            } catch (ArgumentNullException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, Address, "Address is null", ex);
            }
        }
        
        /// <summary>
        /// Reinitiates the connection to the currently connected server.
        /// </summary>
        public void Reconnect()
        {
            InitiateConnection(CallbackObject, Address);
        }

        /// <summary>
        /// Reinitiates the connection to the currently connected server.
        /// </summary>
        /// <param name="callbackObject">The new callback object to use</param>
        public void Reconnect(TCallbackChannel callbackObject)
        {
            InitiateConnection(callbackObject, Address);
        }

        /// <summary>
        /// Closes the connection to the currently connected server.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (Channel != null) Channel.Close();
            } catch (ObjectDisposedException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, Address, "Problem closing connection", ex);
            } catch (InvalidOperationException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, Address, "Problem closing connection", ex);
            } catch (CommunicationObjectFaultedException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, Address, "Problem closing connection", ex);
            } catch (TimeoutException ex) {
                throw new ServerConnectionException(ECode.ConnectionError, Address, "Problem closing connection", ex);
            }
        }
    }
}
