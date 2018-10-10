using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

namespace UC.WebApi.Tests.API.Models
{
    public class RecommendsModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            public List<Recommendation> Recommendations { get; set; }

            [Required]
            public List<RecentlyAdded> RecentlyAddedItems { get; set; }
        }
        public class RecentlyAdded
        {
            [Required]
            public string Brand_path { get; set; }

            [Required]
            public string Model_path { get; set; }

            [Required]
            public string Variant_path { get; set; }

            [Required]
            public string Brand { get; set; }

            [Required]
            public string Model { get; set; }

            [Required]
            public string Variant { get; set; }

            [Required]
            public string Fuel_type { get; set; }

            [Required]
            public string City_path { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string Price { get; set; }

            [Required]
            public string Guid { get; set; }

            [Required]
            public string Short_guid { get; set; }

            [Required]
            public string Thumb { get; set; }

            [Required]
            public string Km_driven { get; set; }
        }
        public class Recommendation
        {
            [Required]
            public string Brand_path { get; set; }

            [Required]
            public string Model_path { get; set; }

            [Required]
            public string Variant_path { get; set; }

            [Required]
            public string Brand { get; set; }

            [Required]
            public string Model { get; set; }

            [Required]
            public string Variant { get; set; }

            [Required]
            public string Fuel_type { get; set; }

            [Required]
            public string City_path { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string Price { get; set; }

            [Required]
            public string Guid { get; set; }

            [Required]
            public string Short_guid { get; set; }

            [Required]
            public string Thumb { get; set; }

            [Required]
            public string Km_driven { get; set; }
        }


    }
}
