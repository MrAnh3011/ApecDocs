using Common;

namespace AppEntities
{
    public class ModuleInfo
    {
        [Column(Name = "MODID")]
        public string ModuleId { get; set; }
        [Column(Name = "SUBMOD")]
        public string SubModule { get; set; }
        [Column(Name = "EXEC_HANDLER")]
        public string ExecuteStore { get; set; }
    }
}
