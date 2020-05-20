using System;
using Common;

namespace AppEntities
{
    public class Document
    {
        [Column(Name = "DOCUMENTID")] public int DocumentId { get; set; }

        [Column(Name = "DOCUMENTTYPEID")] public int DocumentTypeId { get; set; }

        [Column(Name = "DOCUMENTCODE")] public string DocumentCode { get; set; }

        [Column(Name = "DOCUMENTNO")] public string DocumentNo { get; set; }

        [Column(Name = "URL")] public string DocumentUrl { get; set; }

        [Column(Name = "NAME")] public string DocumentName { get; set; }

        [Column(Name = "DISPLAYNAME")] public string DisplayName { get; set; }

        [Column(Name = "ORGPUBLISH")] public string OrgPublish { get; set; }

        [Column(Name = "DOCTYPENAME")] public string DocTypeName { get; set; }

        [Column(Name = "ACTIVEDATE")] public DateTime ActiveDate { get; set; }

        [Column(Name = "EXPIREDATE")] public DateTime ExpireDate { get; set; }


        [Column(Name = "BRIEFDESCRIPTION")] public string BriefDescription { get; set; }

        [Column(Name = "STATUS")] public int Status { get; set; }

        [Column(Name = "ISACTIVE")] public int IsActive { get; set; }

        [Column(Name = "EXT")] public string Ext { get; set; }

        [Column(Name = "EXPIRELINK")] public string ExpireLink { get; set; }
    }
}