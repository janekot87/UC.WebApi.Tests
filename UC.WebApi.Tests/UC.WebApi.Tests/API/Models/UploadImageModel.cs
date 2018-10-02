using KnowledgeBase.WebApi.AutomatedTests.Attributes;
using System.ComponentModel.DataAnnotations;

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
