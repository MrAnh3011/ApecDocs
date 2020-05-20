using System;
using System.Linq;
using System.ServiceModel;

namespace DataAccess
{
    public static class ERR_SQL
    {
        public const int ERR_SQL_BASE = 200;
        public const int ERR_SQL_OPEN_CONNECTION_FAIL = ERR_SQL_BASE + 1;
        public const int ERR_SQL_EXECUTE_COMMAND_FAIL = ERR_SQL_BASE + 2;
        public const int ERR_SQL_DISCOVERY_PARAMS_FAIL = ERR_SQL_BASE + 3;
        public const int ERR_SQL_ASSIGN_PARAMS_FAIL = ERR_SQL_BASE + 4;
    }

    public static class ERR_GATEWAY
    {
        public const int ERR_GATEWAY_BASE = 900;
        public const int ERR_MESSAGE_NOT_SUPPORT_MULTI_TARGET = ERR_GATEWAY_BASE + 2;
        public const int ERR_GATEWAY_ERROR_CONNECT_TO_HNXBR = ERR_GATEWAY_BASE + 3;
        public const int REQUEST_FILE_SYNTAX_ERROR = 1703;
        public const int INVALID_FILE_NAME = 354;
    }

    public static class ErrorUtils
    {
        public static FaultException CreateError(int errorCode)
        {
            var ex = new FaultException("", new FaultCode(errorCode.ToString()));
            return ex;
        }

        public static FaultException CreateUnkownError(Exception innerEx)
        {
            var ex = new FaultException(innerEx.Message, new FaultCode("101"));
            return ex;
        }
        public static Exception CreateErrorWithSubMessage(int errorCode, string errorData)
        {
            return new FaultException(errorData, new FaultCode(errorCode.ToString()));
        }
    }
}
