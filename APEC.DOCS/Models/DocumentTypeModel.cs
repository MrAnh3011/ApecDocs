using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Common;

namespace APEC.DOCS.Models
{
    public class DocumentTypeModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public string Action { get; set; }
    }
}