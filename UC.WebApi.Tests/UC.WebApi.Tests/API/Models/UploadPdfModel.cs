using KnowledgeBase.WebApi.AutomatedTests.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    class UploadPdfModel
    {

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
        public bool Success { get; set; }

        [Required]
        public string Url { get; set; }
    }
}
