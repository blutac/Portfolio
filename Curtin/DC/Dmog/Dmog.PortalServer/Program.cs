/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.DataServer;

namespace Dmog.PortalServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Setup networking paramaters //
            string dataAddress = "net.tcp://localhost:50000/DmogDataServer";     // data server
            string portalAddress = "net.tcp://localhost:50001/DmogPortalServer"; // portal server
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;

            try
            {
                // Setup server //
                Console.Write("> Initiating Server Object: DmogPortalServer... ");
                PortalServerController server = new PortalServerController();

                // Connect to external servers //
                server.InitiateDataServerConnection(dataAddress);

                // Setup host (singleton) //
                Console.Write("> Initiating host... ");
                ServiceHost host = new ServiceHost(server);
                host.AddServiceEndpoint(typeof(IPortalServerController), tcpBinding, portalAddress);
                Console.WriteLine("OK!");

                // Launch host //
                Console.Write("> Opening host on address: " + portalAddress + " ... ");
                host.Open();
                Console.WriteLine("OK!");
                Console.WriteLine("> (Press Enter to terminate this host)");
                Console.ReadLine(); // pause

                // Close host //
                server.CloseDataServerConnection();
                Console.Write("> Terminating host... ");
                host.Close();
                Console.WriteLine("OK!");
                
            } catch (ServerConnectionException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (DataServerException ex) {
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
    }
}

