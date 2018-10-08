using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;


namespace UC.WebApi.Tests.API.Models
{
    public class BrandsModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            [EnsureOneElement(ErrorMessage = "At least one Item is required")]
            public string Results { get; set; }

            [Required]
            public List<Item> Items { get; set; }
        }
        

           public class Item
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Key { get; set; }

            [Required]
            public string Path { get; set; }

            [Required]
            public string Logo { get; set; }
        }
    }
}
