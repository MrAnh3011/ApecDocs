using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace APEC.DOCS.Models
{
    public class MenuModel
    {
        [Column(Name = "MENUUSERID")]
        public int Id { get; set; }

        [Column(Name = "NAME")]
        public string Name { get; set; }

        [Column(Name = "PARENTID")]
        public int ParentId { get; set; }

        [Column(Name = "IS_SELECTED")]
        public bool Disable { get; set; }
    }
}