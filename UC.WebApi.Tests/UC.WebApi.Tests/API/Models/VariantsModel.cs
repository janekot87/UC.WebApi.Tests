using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

namespace UC.WebApi.Tests.API.Models
{
    class VariantsModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            public int Results { get; set; }

            [Required]
            [EnsureOneElement(ErrorMessage = "At least one Item is required")]
            public List<Item> Items { get; set; }
        }

        public class Item
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Model_name { get; set; }

            [Required]
            public string Brand_name { get; set; }

            [Required]
            public string Key { get; set; }

            [Required]
            public string Model_key { get; set; }

            [Required]
            public string Brand_key { get; set; }

            [Required]
            public string Brand_path { get; set; }

            [Required]
            public string Model_path { get; set; }
            
            public string Path { get; set; }
        }

    }
}
