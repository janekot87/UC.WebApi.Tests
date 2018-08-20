using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    public class RegisterRequestModel
    {
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
        public bool Success { get; set; }
    }
}
