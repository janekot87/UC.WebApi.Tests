using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UC.WebApi.Tests.API.Models;

namespace UC.WebApi.Tests.API.Logic
{
    public class GlobalLogic
    {
        public static bool IsModelValid(object item, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            var context = new ValidationContext(item);
            var isValid = Validator.TryValidateObject(item, context, results, true);

            return isValid;
        }

        public static bool IsModelValid<T>(T item, out ValidationResultModel<T> resultModel)
            where T : new()
        {
            resultModel = new ValidationResultModel<T> { Model = item };

            ICollection<ValidationResult> results;
            var isValid = IsModelValid(item, out results);
            resultModel.Results = results;

            return isValid;

        }

        public static bool IsModelArrayValid<T>(IEnumerable<T> collection, out IList<ValidationResultModel<T>> resultModels)
            where T : new()
        {
            bool finalIsValid = true;

            resultModels = new List<ValidationResultModel<T>>();
            foreach (var item in collection)
            {
                ValidationResultModel<T> resultModel;
                var isValid = IsModelValid<T>(item, out resultModel);
                if (!isValid)
                {
                    resultModels.Add(resultModel);
                }

                finalIsValid &= isValid;
            }

            return finalIsValid;
        }


    }
}
