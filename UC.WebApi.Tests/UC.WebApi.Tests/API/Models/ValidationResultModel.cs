using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Models
{
    public class ValidationResultModel<T>
    {
        public T Model { get; set; }
        public ICollection<ValidationResult> Results { get; set; }
    }
}
