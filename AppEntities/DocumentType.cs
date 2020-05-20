using System;
using Common;

namespace AppEntities
{
    public class DocumentType
    {
        [Column(Name = "MENUUSERID")] public int MenuUserId { get; set; }

        [Column(Name = "NAME")] public string Name { get; set; }

        [Column(Name = "PARENTID")] public int ParentId { get; set; }

        [Column(Name = "DISPLAYTREE")] public string DisplayTree { get; set; }
    }
}