using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using APEC.DOCS.Models;
using Newtonsoft.Json;

namespace APEC.DOCS.Common
{
    public static class HtmlExtension
    {

        public static string GetAction(this HtmlHelper helper)
        {
            return helper.ViewContext.RouteData.Values["action"].ToString();
        }

        public static string GetController(this HtmlHelper helper)
        {
            return helper.ViewContext.RouteData.Values["controller"].ToString();
        }
        public static MvcHtmlString Menu(this HtmlHelper helper)
        {
            var strAction = GetAction(helper);
            var strController = GetController(helper);
            var _filePath = helper.ViewContext.HttpContext.Server.MapPath("~/App_Data/menu.json");
            var _menuContent = string.Empty;

            if (File.Exists(_filePath))
            {
                _menuContent = File.ReadAllText(_filePath);
            }
            else
            {
                throw new Exception("Không có file cấu hình menu. Kiểm tra lại App_Data/menu.json");
            }

            var _data = JsonConvert.DeserializeObject<IList<MenuViewModel>>(_menuContent);

            if (_data.Count > 0)
            {
                var _sb = new StringBuilder();
                //_sb.Append("<ul class='nav navbar-nav'>");
                foreach (var item in _data)
                {
                    GenerateMenuItem(item, _sb, "", strAction, strController);
                }
                //_sb.Append("</ul>");

                return new MvcHtmlString(_sb.ToString());
            }

            return new MvcHtmlString("");
        }
        private static void GenerateMenuItem(MenuViewModel item, StringBuilder sb, string itemClass, string action, string controller)
        {
            var lSplit = item.Url.Split('/');

            var itemController = lSplit[1] != string.Empty ? lSplit[1] : "Home";
            var itemAction = lSplit.Count() > 2 && lSplit[2] != string.Empty ? lSplit[2] : "Index";


//            if (itemController == "Home")
//            {
                if (item.Child.Count > 0)
                {
                    //sb.AppendFormat("<li class='{0}'>", itemClass);
                    sb.AppendFormat("<li {2}><a href='javascript:;'>{1}<span class='title'>{0}</span><span class='arrow'></span></a>", item.Name, !string.IsNullOrWhiteSpace(item.Icon) ? string.Format("<i class='{0}'></i>", item.Icon) : "", item.ChildGroup.Contains(controller) ? "class='active open'" : "");
                    sb.Append("<ul class='sub-menu'>");
                    foreach (var child in item.Child)
                    {
                        GenerateMenuItem(child, sb, string.Empty, action, controller);
                    }
                    sb.Append("</ul>");
                    sb.AppendFormat("</li>");
                }
                else
                {
                    sb.AppendFormat("<li class='{3}' data-container='body' data-placement='right' data-html='true' data-original-title={1}><a href='{0}'>{2} <span class='title'>{1}</span></a></li>", item.Url, item.Name, !string.IsNullOrWhiteSpace(item.Icon) ? string.Format("<i class='{0}'></i>", item.Icon) : "", itemController.ToLower() == controller.ToLower() ? "active" : "");
                }
//            }

        }
    }
}