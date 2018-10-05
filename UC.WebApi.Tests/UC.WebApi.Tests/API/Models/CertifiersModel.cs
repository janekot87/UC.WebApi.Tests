using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

namespace UC.WebApi.Tests.API.Models
{
    class CertifiersModel
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
            public string Name { get; set; }

            [Required]
            public string Key_value { get; set; }
        }
    }
}
