using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

public class LeadListModel
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
        public string Request_id { get; set; }
        public string Classified_id { get; set; }
        public string Customer_name { get; set; }
        public string Customer_email { get; set; }
        public string Customer_city { get; set; }
        public string Dealer_name { get; set; }
        public string Created_at { get; set; }
        public string Customer_phone { get; set; }
        public string Phone_verified { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Model_name { get; set; }
        public string Variant { get; set; }
        public string Dealer { get; set; }
        public string Dealer_id { get; set; }
        public string Partner { get; set; }
        public string City_name { get; set; }
        public string City { get; set; }
        public string Company_name { get; set; }
        public string Utm_source { get; set; }
        public string Utm_medium { get; set; }
        public string Utm_campaign { get; set; }
        public string Gclid { get; set; }
        public List<object> Parameters { get; set; }
    }
}

