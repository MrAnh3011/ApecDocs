using System.Collections.Generic;
using System.ServiceModel;

namespace Gate.Entities.Interface
{
    [ServiceContract()]
    [ServiceKnownType(typeof(ParamInfo))]
    [ServiceKnownType(typeof(List<ParamInfo>))]
    [ServiceKnownType(typeof(DataContainer))]
    public interface IPush
    {
        [OperationContract]
        DataContainer Execute(string storeId, List<ParamInfo> values);
    }
}
