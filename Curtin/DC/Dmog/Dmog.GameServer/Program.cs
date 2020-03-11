/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Text.RegularExpressions;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.PortalServer;
using Dmog.DataServer;

namespace Dmog.GameServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen; // for dramatic effect

            // Setup networking paramaters //
            string serverName = GetServerName();
            string gameAddress = "net.tcp://localhost:" + GetPort() + "/" + serverName; // server address
            string dataAddress = "net.tcp://localhost:50000/DmogDataServer";     // data server
            string portalAddress = "net.tcp://localhost:50001/DmogPortalServer"; // portal server
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
                // Setup server //
                Console.Write("> Initiating Server Object: DmogGameServer... ");
                GameServerController server = new GameServerController();

                // Setup host (singleton) //
                Console.Write("> Launching host...");
                ServiceHost host = new ServiceHost(server);
                host.AddServiceEndpoint(typeof(IGameServerController), tcpBinding, gameAddress);
                Console.WriteLine("OK!");

                // Launch host //
                Console.Write("> Opening host on address: " + gameAddress + " ... ");
                host.Open();
                Console.WriteLine("OK!");

                // Connect to external servers //
                server.InitiateDataServerConnection(dataAddress);
                server.InitiatePortalServerConnection(portalAddress);
                server.RegisterServer(gameAddress, serverName); // Register with Portal Server
                server.InitialiseGame();
                server.StartNewGame();
                Console.WriteLine("> (Press Enter to terminate this host)");
                Console.ReadLine(); // pause

                // Close host //
                server.StopGame();
                server.UnregisterServer();
                server.ClosePortalServerConnection();
                server.CloseDataServerConnection();
                Console.Write("> Terminating host... ");
                host.Close();
                Console.WriteLine("OK!");

            } catch (ServerConnectionException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (DataServerException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (PortalServerException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch(CommunicationException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (TimeoutException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (UriFormatException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (InvalidOperationException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (ArgumentException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            }

            Console.WriteLine("> (Press Enter to close this program)");
            Console.ReadLine(); // pause
        }

        private static string GetServerName()
        {
            Console.Write("> Specify server name: ");
            string name = Console.ReadLine();
            if (Regex.IsMatch(name, "^\\w{1,100}$") == false)
            {
                name = "DmogGameServer";
                Console.WriteLine("> Invalid! Defaulting to server name: " + name);
            }
            return name;
        }

        private static string GetPort()
        {
            Console.Write("> Specify port: ");
            string port = Console.ReadLine();
            if (Regex.IsMatch(port, "^\\d{1,5}$") == false)
            {
                port = "50003";
                Console.WriteLine("> Invalid! Defaulting to port: " + port);
            }
            return port;
        }
    }
}

