using System.Collections.Generic;

namespace Arda.Common.ViewModels.Reports
{
    public class D3BubbleViewModel
    {
        public string name { get; set; }
        public List<D3BubbleChild0> children { get; set; }
    }

    public class D3BubbleChild0
    {
        public string name { get; set; }
        public int? size { get; set; }
        public List<D3BubbleChild1> children { get; set; }
    }

    public class D3BubbleChild1
    {
        public string name { get; set; }
        public int size { get; set; }
    }
}