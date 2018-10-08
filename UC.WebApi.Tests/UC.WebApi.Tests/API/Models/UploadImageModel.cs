using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Attributes;

namespace UC.WebApi.Tests.API.Models
{
    class UploadImageModel
    {

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
        public bool Success { get; set; }

        [Required]
        [ValidateUrl]
        public string Url { get; set; }
    }
}
