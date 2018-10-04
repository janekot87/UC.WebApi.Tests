
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    public class DealerDataModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Range(0, 15, ErrorMessage = "Invalid Results number")]
            public int Results { get; set; }

            [Required]
            public Items Items { get; set; }
        }

        public class Items
        {
            [Required]
            public string User_name { get; set; }

            [Required]
            public string First_name { get; set; }

            [Required]
            public string Last_name { get; set; }

            [Required]
            public string Appointment { get; set; }

            public object File_id { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public string Phone { get; set; }

            public object Phone_status { get; set; }

            public string Information { get; set; }

            public string Company_name { get; set; }

            public object Company_file_id { get; set; }

            public string Address { get; set; }

            public string Status { get; set; }

            public string Is_contact { get; set; }

            public List<string> Additional_addresses { get; set; }
        }
    }
}
