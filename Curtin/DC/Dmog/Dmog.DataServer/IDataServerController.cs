/*/////////////////////////////////////////////////
 * AUTHOR: Jason Gilbert (XXXXXXXX)
 * UNIT: Distributed Computing (COMP5002)
 * SUBMISSION: Assignment
 * SUBMISSION DATE: 27/05/2018
/*/////////////////////////////////////////////////
using System.ServiceModel;
using Dmog.ServerSupport;
using Dmog.GameObjects;

namespace Dmog.DataServer
{
    [ServiceContract(CallbackContract = typeof(IDataServerController_Callback))]
    public interface IDataServerController
    {
        [OperationContract] int GetUserCount(out ECode ec);
        [OperationContract] int GetHeroCount(out ECode ec);
        [OperationContract] int GetBossCount(out ECode ec);

        [OperationContract] int AuthenticateCredentials(string username, string password, out ECode ec);
        [OperationContract] int GetUserId(string username, out ECode ec);

        [OperationContract] User GetUser(int id, out ECode ec);
        [OperationContract] Hero GetHero(int id, out ECode ec);
        [OperationContract] Hero[] GetHeroTable(out ECode ec);
        [OperationContract] Boss GetBoss(int id, out ECode ec);
        [OperationContract] Boss[] GetBossTable(out ECode ec);
    }
    [ServiceContract]
    public interface IDataServerController_Callback
    {
        [OperationContract] bool DataServer_OnPing();
    }
}
