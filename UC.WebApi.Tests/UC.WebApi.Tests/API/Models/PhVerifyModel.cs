using System;
using System.ComponentModel.DataAnnotations;


namespace UC.WebApi.Tests.API.Models
{
    public class PhVerifyModel
    {
        
        
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            public int Code { get; set; }

            [Required]
            public String Message { get; set; }
        

    }
}
