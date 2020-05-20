using Common;

namespace AppEntities
{
    public class ModuleFieldInfo
    {
        [Column(Name = "ALIGN")]
        public string alignment { get; set; }

        [Column(Name = "ALLOWEDIT")]
        public string IsEdit { get; set; }
        public bool allowEditing {
            get { return IsEdit == "Y"; }
        }

        [Column(Name = "ALLOWFILTER")]
        public string IsFilter { get; set; }
        public bool allowFiltering
        {
            get { return IsFilter == "Y"; }
        }

        [Column(Name = "ALLOWGROUP")]
        public string IsGroup { get; set; }
        public bool allowGrouping
        {
            get { return IsGroup == "Y"; }
        }

        [Column(Name = "ALLOWSORT")]
        public string IsSort { get; set; }
        public bool allowSorting
        {
            get { return IsGroup == "Y"; }
        }

        [Column(Name = "CAPTION")]
        public string caption { get; set; }
        [Column(Name = "FIELD")]
        public string dataField { get; set; }
        [Column(Name = "DATATYPE")]
        public string dataType { get; set; }
        [Column(Name = "FLDFORMAT")]
        public string format { get; set; }
        
        [Column(Name = "VISIBLE")]
        public string IsShow { get; set; }
        public bool visible {
            get { return IsShow == "Y"; }
        }
    }
}

