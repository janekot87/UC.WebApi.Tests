﻿using KnowledgeBase.WebApi.AutomatedTests.Attributes;
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
        [ValidateUrl]
        public string Url { get; set; }
    }
}