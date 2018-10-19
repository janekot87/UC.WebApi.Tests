using System;
using System.ComponentModel.DataAnnotations;


namespace UC.WebApi.Tests.API.Models
{
    public class ValidateNphNumberModel
    {
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
        public bool Success { get; set; }

        
        public object Description { get; set; }

        [Required]
        public int Error_code { get; set; }
    }
}
