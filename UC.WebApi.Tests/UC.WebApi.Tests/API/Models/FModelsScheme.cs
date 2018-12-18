using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;


namespace UC.WebApi.Tests.API.Models
{
    public class FModelsScheme
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
            public string External_model_name { get; set; }
            public string External_brand_name { get; set; }
            public string Source { get; set; }
            public object Model_name { get; set; }
            public string Brand_name { get; set; }
            public object Years { get; set; }
        }
    }
}