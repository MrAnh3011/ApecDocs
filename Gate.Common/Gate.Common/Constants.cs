namespace Common
{
    public static class Constants
    {
        public static int SYSTEM_NOT_ERROR = 0;
        public static int SYSTEM_ERR_CODE = 1000;


        //Store

        public static string SP_AUTHEN = "PKG_AUTHEN.SP_LOGIN";
        public static string SP_USERBYID = "PKG_AUTHEN.SP_USER_BY_ID";
        public static string SP_USERBYNAME = "PKG_AUTHEN.SP_USER_BY_NAME";
        public static string SP_USERBYSESSIONID = "PKG_AUTHEN.SP_USER_BY_SESSIONID";
        public static string SP_GETUSERPROFILE = "PKG_AUTHEN.SP_GET_USERPROFILE";
        public static string SP_GETACCOUNTS = "PKG_AUTHEN.SP_GET_ACCOUNTS";

        public static string UPDATE_SESSION_INFO = "PKG_AUTHEN.UPDATE_SESSION_INFO";
        public static string SP_GENERATE_OTP = "PKG_AUTHEN.SP_GENERATE_OTP";
        public static string SP_VERTIFY_OTP = "PKG_AUTHEN.SP_VERTIFY_OTP";
    }
}
