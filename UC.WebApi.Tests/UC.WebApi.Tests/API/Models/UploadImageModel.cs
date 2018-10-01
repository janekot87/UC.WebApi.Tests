using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UC.WebApi.Tests.API.Models
{
    class UploadImageModel
    {

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
        public bool Success { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
