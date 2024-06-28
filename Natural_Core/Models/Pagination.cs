using System;
using System.Collections.Generic;
using System.Text;

namespace Natural_Core.Models
{
    public class Pagination<T>
    {
        public int TotalItems { set; get; }
        public int TotalPageCount { set; get; }
        public List<T> Items { get; set; }
    }
}
