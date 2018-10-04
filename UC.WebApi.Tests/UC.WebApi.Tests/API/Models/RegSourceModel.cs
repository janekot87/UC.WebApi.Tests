
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace UC.WebApi.Tests.API.Models

    {
    class RegSourceModel
    {
        public class RootObject
        {
            [Required]
            public int results { get; set; }

            [Required]
            public List<Item> items { get; set; }

            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool success { get; set; }
        }

        public class Item
        {
            [Required]
            public string name { get; set; }

            [Required]
            public string source_key { get; set; }
        }

    }
}
