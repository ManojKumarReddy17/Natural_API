using System;
using System.Collections.Generic;
using System.Text;

#nullable disable

namespace Natural_Core.Models
{
    public class ResultResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }
        public int StatusCode { get; set; }

    }
}
 