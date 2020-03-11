/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.GameServer
{
    [ServiceContract(CallbackContract = typeof(IGameServerController_Callback))]
    public interface IGameServerController
    {
        [OperationContract(IsOneWay = true)]
        void Reconnect(string userToken);

        [OperationContract] bool JoinGameServer(string userPuid, string userToken, out ECode ec);
        [OperationContract] void LeaveGameServer(string userPuid, string userToken, out ECode ec);
        [OperationContract] void SubmitPlayerHero(string userToken, int heroId, out ECode ec);
        [OperationContract] void SubmitPlayerMove(string userToken, PlayerMove move, out ECode ec);
        [OperationContract] bool QueryGameReadyState(string userToken, out ECode ec);
        [OperationContract] PlayerListing QueryPlayerListing(string userToken, out ECode ec);
    }

    [ServiceContract]
    public interface IGameServerController_Callback
    {
        [OperationContract(IsOneWay = true)]
        void GameServer_OnPlayerChange(PlayerDetail playerDetail);
        
        [OperationContract(IsOneWay = true)]
        void GameServer_OnPlayerLeave(string userPuid);
        
        [OperationContract(IsOneWay = true)]
        void GameServer_OnGameReady(bool ready);
        
        [OperationContract(IsOneWay = true)]
        void GameServer_OnGameEnd(bool win);
        
        [OperationContract(IsOneWay = true)]
        void GameServer_OnRoundEnd(BotListing botListing, PlayerListing playerListing, GameEventLog gameLog);
        
        [OperationContract] bool GameServer_OnPing();
    }
}
