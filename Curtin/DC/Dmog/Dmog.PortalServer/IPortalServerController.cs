/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.PortalServer
{
    [ServiceContract(CallbackContract = typeof(IPortalServerController_Callback))]
    public interface IPortalServerController : IGamePortalServerController, IUserPortalServerController
    {

    }
    [ServiceContract]
    public interface IPortalServerController_Callback : IGamePortalServerController_Callback, IUserPortalServerController_Callback
    {
        
    }


    [ServiceContract(CallbackContract = typeof(IGamePortalServerController_Callback))]
    public interface IGamePortalServerController
    {
        [OperationContract] string RegisterGameServer(string address, string serverName, out ECode ec);
        [OperationContract] void UnregisterGameServer(string token, out ECode ec);

        [OperationContract(IsOneWay = true)]
        void AddUserToPlayerList(string token, string userPuid);

        [OperationContract(IsOneWay = true)]
        void RemoveUserFromPlayerList(string token, string userPuid);

        [OperationContract] bool IsUserTokenVaild(string userToken);
        [OperationContract] Boss[] GetBossTable(string token, out ECode ec);
    }
    [ServiceContract]
    public interface IGamePortalServerController_Callback
    {
        [OperationContract] bool GamePortalServer_OnPing();
    }


    [ServiceContract(CallbackContract = typeof(IUserPortalServerController_Callback))]
    public interface IUserPortalServerController
    {
        [OperationContract] string Login(string address, string username, string password, out ECode ec);
        [OperationContract] void Logout(string token, out ECode ec);
        
        [OperationContract] bool PingGameServer(string serverName);

        [OperationContract] GameListing QueryGameServers(string token, out ECode ec);
        [OperationContract] string[] QueryFriends(string token, out ECode ec);
        [OperationContract] string[] QueryFriendsOnline(string token, out ECode ec);
        [OperationContract] Hero[] GetHeroTable(string token, out ECode ec);
    }
    [ServiceContract]
    public interface IUserPortalServerController_Callback
    {
        [OperationContract] bool UserPortalServer_OnPing();
    }
}
