using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Common;
using Resources;

namespace APEC.DOCS.Models
{
    public class LoginModel
    {
        [Column(Name = "USERNAME")]
        public string Username { set; get; }

        [Column(Name = "SESSION_KEY")]
        public string SessionKey { set; get; }

        public string AllowDevelop { set; get; }

        public string AllowViewAllData { set; get; }

        public string DisplayName { set; get; }
    }
}