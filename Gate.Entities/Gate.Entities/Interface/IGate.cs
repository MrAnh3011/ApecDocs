using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gate.Entities.Interface
{
    [ServiceContract()]
    [ServiceKnownType(typeof(ParamInfo))]
    [ServiceKnownType(typeof(List<ParamInfo>))]
    [ServiceKnownType(typeof(ApplicationUser))]
    [ServiceKnownType(typeof(UserProfile))]
    [ServiceKnownType(typeof(CusAccount))]
    [ServiceKnownType(typeof(List<CusAccount>))]
    public interface IGate
    {
        [OperationContract]
        IdentityUser Login(string username, string password, string langId, string clientIp);
        [OperationContract]
        UserProfile GetUserProfile(string sessionId);
        [OperationContract]
        DataContainer ExecuteModule(string sessionId, string moduleId, string submodule, List<ParamInfo> values);
        [OperationContract]
        DataContainer Execute2Data(string moduleId, string submodule, List<ParamInfo> values);
        [OperationContract]
        DataContainer ModuleFieldInfo(string sessionId, string moduleId, string submodule, string langId);
    }
}
