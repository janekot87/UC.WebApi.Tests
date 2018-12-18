using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    public class LeadModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            public string LeadId { get; set; }

            [Required]
            public string Status { get; set; }


            public List<Item> Recommendations { get; set; }
            public List<Item> SimilarCars { get; set; }
            public List<Item> RecentlyAdded { get; set; }
        }

        public class Item
        {
            public string Brand_path { get; set; }
            public string Model_path { get; set; }
            public string Variant_path { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public string Variant { get; set; }
            public string Fuel_type { get; set; }
            public string City_path { get; set; }
            public string City { get; set; }
            public string Price { get; set; }
            public string Guid { get; set; }
            public string Short_guid { get; set; }
            public string Thumb { get; set; }
            public string Km_driven { get; set; }
        }

    }
}
