
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using AppEntities;
using APEC.DOCS.Common;
using APEC.DOCS.Models;
using DataAccess;

namespace APEC.DOCS.Controllers
{
    public class BaseController : Controller
    {
        public string ConnectionString = ConfigurationManager.AppSettings.Get("OracleConnection");

//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var sessionUser = (LoginModel) Session[CommonConstants.UserSession];
//            if (sessionUser == null)
//            {
//                var sessionKey = filterContext.RouteData.Values["ssk"];
//
//                if (sessionKey == null || sessionKey == "")
//                {
//                    filterContext.Result = new RedirectToRouteResult(new
//                        RouteValueDictionary(new {controller = "Login", action = "Index"}));
//                }
//                else
//                {
//                    LoginModel loginModel = new LoginModel();
//                    string sql = "select s.session_key, s.username from sessions s where s.session_key = :0";
//                    List<LoginModel> lstUser =
//                        OracleHelper.ExecuteCommandText<LoginModel>(ConnectionString, sql, sessionKey);
//                    if (lstUser.Count > 0)
//                    {
//                        loginModel.SessionKey = lstUser[0].SessionKey;
//                        loginModel.Username = lstUser[0].Username;
//                        Session.Add(CommonConstants.UserSession, loginModel);
//                    }
//                    else
//                    {
//                        filterContext.Result = new RedirectToRouteResult(new
//                            RouteValueDictionary(new {controller = "Login", action = "Index"}));
//                    }
//                }
//            }
//
//            base.OnActionExecuting(filterContext);
//        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var sessionUser = (LoginModel)Session[CommonConstants.UserSession];
            var sessionKey = filterContext.RouteData.Values["ssk"];
            if (sessionUser == null)
            {

                if (sessionKey == null || sessionKey == "")
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Login", action = "Index" }));
                }
                else
                {
                    LoginModel loginModel = new LoginModel();
                    string sql = "select s.session_key, s.username from sessions s where s.session_key = :0";
                    List<LoginModel> lstUser =
                        OracleHelper.ExecuteCommandText<LoginModel>(ConnectionString, sql, sessionKey);
                    if (lstUser.Count > 0)
                    {
                        loginModel.SessionKey = lstUser[0].SessionKey;
                        loginModel.Username = lstUser[0].Username;
                        Session.Add(CommonConstants.UserSession, loginModel);
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Login", action = "Index" }));
                    }
                }
            }
            else
            {
                var ssk = sessionKey != null ? sessionKey : sessionUser.SessionKey;
                if (sessionKey != null && sessionUser.SessionKey != sessionKey)
                {
                    Session[CommonConstants.UserSession] = null;
                    LoginModel loginModel = new LoginModel();
                    string sql = "select s.session_key, s.username from sessions s where s.session_key = :0";
                    List<LoginModel> lstUser =
                        OracleHelper.ExecuteCommandText<LoginModel>(ConnectionString, sql, sessionKey);
                    if (lstUser.Count > 0)
                    {
                        loginModel.SessionKey = lstUser[0].SessionKey;
                        loginModel.Username = lstUser[0].Username;
                        Session.Add(CommonConstants.UserSession, loginModel);
                    }
                }
                string sqlGetRoleType =
                    "select\r\n        ur.ROLETYPE_LIST || ',' || (select dr.ROLETYPE_LIST from apec_docs_department_role dr where dr.DEPARTMENT_ID = sd.dep_id) ROLETYPE_LIST\r\n        from sessions s\r\n       left join users u on s.user_id = u.user_id\r\n       left join apec_docs_user_role ur on u.staff_id = ur.staff_id\r\n       left join apec_staff_deparment sd on u.staff_id = sd.staff_id\r\n        where s.session_key = :0 and sd.dep_id is not null";
                List<RoleType> lstRoleType =
                    OracleHelper.ExecuteCommandText<RoleType>(ConnectionString, sqlGetRoleType, ssk);
                var strAccessAction = lstRoleType.Count > 0 ? lstRoleType[0].RoleTypeString : "";
                var strAction = filterContext.HttpContext.Request.Url.ToString();
                var strActionNumber = "";
                if (strAction.Contains("Create") || strAction.Contains("create"))
                {
                    strActionNumber = "1";
                }

                if (strAction.Contains("Edit") || strAction.Contains("Edit"))
                {
                    strActionNumber = "2";
                }

                if (strAction.Contains("DownloadDoc") || strAction.Contains( "DownloadDocZip") || strAction.Contains("downloadDoc") || strAction.Contains("downloadDocZip") || strAction.Contains("UploadedFiles"))
                {
                    strActionNumber = "5";
                }

                if (!strAccessAction.Contains(strActionNumber))
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Home", action = "CheckPermission" }));
                }
            }
            base.OnAuthorization(filterContext);
        }
    }
}