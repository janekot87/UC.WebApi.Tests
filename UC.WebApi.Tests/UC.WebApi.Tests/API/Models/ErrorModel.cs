using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    class ErrorModel
    {
        public class RootObject
        {
            [Required]
            public bool Success { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public int Error_code { get; set; }
        }

        public class AuthFail
        {
            [Required]
            public bool Success { get; set; }

            [Required]
            public string Message { get; set; }
        }
    }
}
