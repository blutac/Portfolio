/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.IO;
using System.ServiceModel;
 
namespace Dmog.DataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            // Setup networking paramaters //
            string dataAddress = "net.tcp://localhost:50000/DmogDataServer"; // server address
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.MaxReceivedMessageSize = System.Int32.MaxValue;
            tcpBinding.ReaderQuotas.MaxArrayLength = System.Int32.MaxValue;
            
            try
            {
                // Setup server //
                Console.Write("> Initiating Server Object: DmogDataServer... ");
                DataServerController server = new DataServerController();

                // Setup host (singleton) //
                Console.WriteLine("> Launching DmogDataServer host...");
                ServiceHost host = new ServiceHost(server);
                host.AddServiceEndpoint(typeof(IDataServerController), tcpBinding, dataAddress);

                // Launch host //
                Console.Write("> Opening host on address: " + dataAddress + " ... ");
                host.Open();
                Console.WriteLine("OK!");
                Console.WriteLine("> (Press Enter to terminate this host)");
                Console.ReadLine(); // pause

                // Close host //
                Console.Write("> Terminating host... ");
                host.Close();
                Console.WriteLine("OK!");

            } catch (DatabaseException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch(CommunicationException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (TimeoutException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (UriFormatException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (FileNotFoundException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (DirectoryNotFoundException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (InvalidOperationException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (IndexOutOfRangeException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            } catch (ArgumentException ex) {
                Console.WriteLine("FATAL ERROR!\n> " + ex.Message);
            }

            Console.WriteLine("> (Press Enter to close this program)");
            Console.ReadLine(); // pause
        }
    }
}
