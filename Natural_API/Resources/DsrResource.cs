﻿#nullable disable
using Natural_Core.Models;

namespace Natural_API.Resources
{
    public class DSRResource
    {
        public string Id { get; set; }
        public string Executive { get; set; }
        public string Distributor { get; set; }
        public string Retailor { get; set; }
        public string OrderBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public double TotalAmount { get; set; }




    }
}
