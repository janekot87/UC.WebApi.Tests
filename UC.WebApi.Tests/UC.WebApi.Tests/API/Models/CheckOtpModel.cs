using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    public class CheckOtpModel
    {
        
        
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Required]
            public string Username { get; set; }
        
    }
}
