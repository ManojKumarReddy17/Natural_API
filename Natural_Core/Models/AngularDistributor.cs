using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
#nullable disable

namespace Natural_Core.Models
{
    public class AngularDistributor
    {
        public string Id { get; set; }
        [JsonPropertyName("FirstName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }

        public string UserName { get;set; }




        public int Statuscode { get; set; }
        public string Message { get; set; }





        public string Executives { get; set; }


        public string ExeId { get; set; }


        public string PresignedUrl { get; set; }
      










    }

}
