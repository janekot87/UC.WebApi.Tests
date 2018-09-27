using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models

{
    class ReadClassifiedModel
    {
        public class RootObject
        {
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Success is not true")]
            public bool Success { get; set; }

            [Range(0, 15, ErrorMessage = "Invalid Results number")]
            public int Results { get; set; }

            [Required]
            public Items Items { get; set; }
        }
        public class Items
        {
            [Required]
            public string guid { get; set; }

            public string special_conditions { get; set; }

            public string description { get; set; }

            [Required]
            public string color { get; set; }

            [Required]
            public string status { get; set; }

            [Required]
            public string have_certificated { get; set; }

            [Required]
            public string engine { get; set; }

            [Required]
            public string owners { get; set; }

            [Required]
            public string price { get; set; }

            [Required]
            public string km_driven { get; set; }

            [Required]
            public string year { get; set; }

            public string step { get; set; }

            public string reason { get; set; }

            public string status_before_delete { get; set; }

            [Required]
            public string classified_phone { get; set; }

            [Required]
            public string address { get; set; }

            public string is_external { get; set; }

            [Required]
            public string short_guid { get; set; }

            [Required]
            public string user_name { get; set; }

            [Required]
            public string brand { get; set; }

            [Required]
            public string brand_key { get; set; }

            [Required]
            public string brand_path { get; set; }

            [Required]
            public string model { get; set; }

            [Required]
            public string model_key { get; set; }

            [Required]
            public string model_path { get; set; }

            [Required]
            public string variant { get; set; }

            [Required]
            public string variant_key { get; set; }

            [Required]
            public string variant_path { get; set; }

            [Required]
            public string city { get; set; }

            [Required]
            public string city_path { get; set; }

            public string certifier { get; set; }

            [Required]
            public string fuel_type { get; set; }

            [Required]
            public string transmission { get; set; }

            [Required]
            public string Created_at { get; set; }

            public List<string> images { get; set; }

            public List<string> Params { get; set; }

            public string Certificate { get; set; }

            [Required]
            public string Link { get; set; }
        }

    }

}
