using System.Collections.Generic;

namespace APEC.DOCS.Models
{
  public class MenuViewModel
    {
        public MenuViewModel()
        {
            Child = new List<MenuViewModel>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public string ChildGroup { get; set; }

        public IList<MenuViewModel> Child { get; set; }
    }
}