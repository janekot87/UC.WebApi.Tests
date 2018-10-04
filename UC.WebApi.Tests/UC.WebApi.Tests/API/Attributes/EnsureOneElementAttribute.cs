using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace UC.WebApi.Tests.API.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list == null)
            {
                var dictionary = value as IDictionary;
                return dictionary?.Keys.Count > 0;
            }

            return list.Count > 0;
        }
    }
}
