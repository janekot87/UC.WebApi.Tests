using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

namespace UC.WebApi.Tests.API.Models

    {
    class RegSourceModel
    {
        public class RootObject
        {
            [Required]
            public int Results { get; set; }

            [Required]
            [EnsureOneElement(ErrorMessage = "At least one Item is required")]
            public List<Item> Items { get; set; }

            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }
        }

        public class Item
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Source_key { get; set; }
        }

    }
}
