using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;


namespace UC.WebApi.Tests.API.Tests
{
    class DealerLeadsModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            public string Results { get; set; }

            [Required]
            [EnsureOneElement(ErrorMessage = "At least one Item is required")]
            public List<Item> Items { get; set; }
        }
        public class Item
        {
            [Required]
            public string Request_id { get; set; }

            [Required]
            public string Classified_id { get; set; }

            [Required]
            public string Customer_name { get; set; }

            [Required]
            public string Customer_email { get; set; }

            public string Customer_city { get; set; }

            [Required]
            public string Dealer_name { get; set; }

            [Required]
            public string Created_at { get; set; }

            [Required]
            public string Customer_phone { get; set; }

            [Required]
            public string Phone_verified { get; set; }

            [Required]
            public string Brand { get; set; }

            [Required]
            public string Model { get; set; }

            [Required]
            public string Model_name { get; set; }

            [Required]
            public string Variant { get; set; }

            
            public string Dealer { get; set; }

            [Required]
            public string Dealer_id { get; set; }

            public string Partner { get; set; }

            [Required]
            public string City_name { get; set; }

            [Required]
            public string City { get; set; }

        
            public string Company_name { get; set; }

            public string Utm_source { get; set; }

            public string Utm_medium { get; set; }

            public string Utm_campaign { get; set; }

            public string Gclid { get; set; }

            public List<object> Parameters { get; set; }
        }

    }
}
